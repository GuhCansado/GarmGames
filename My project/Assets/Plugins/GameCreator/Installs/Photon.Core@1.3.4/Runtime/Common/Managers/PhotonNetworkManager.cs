using System.Collections;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

namespace NinjutsuGames.Photon.Runtime.Managers
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    [AddComponentMenu("")]
    public class PhotonNetworkManager : Singleton<PhotonNetworkManager>, IMatchmakingCallbacks, IInRoomCallbacks, IOnEventCallback
    {
        public static readonly Dictionary<string, ModelConfig> RuntimeModels = new();

        // private const string RESOURCE_PATH = "Photon/NetworkItemSyncer";
        // private const string MSG_ACTION_NOT_FOUND = "ActionsNetwork with id: {0} not found.";

        // public const byte EVENT_ACTION = 100;
        

        //private static double startTime = -1;
        /// <summary>
        /// Used in an edge-case when we wanted to set a start time but don't know it yet.
        /// </summary>
        private static bool _startRoundWhenTimeIsSynced;
        private float _lastTimeCheck;
        //private static PlayerNumbering PN;

        // PROPERTIES: ----------------------------------------------------------------------------

        public static List<Action> UpdateCalls = new();
        public static List<Action> PhotonCalls = new();
        private static double _clientStartTime;
        private float _lastLagCheck;
        private bool _skipLagCheck;
        private int _errors;
        private float _lastPingCheck;
        private float _timeAsMaster;

        private const float LAG_CHECK_DELAY = 5;

        /// <summary>
        /// Returns the time this client has been in the room. In Seconds.
        /// </summary>
        public static double CurrentClientTime => PhotonNetwork.Time - _clientStartTime;

        public PhotonCoreSettings CoreSettings => Settings.From<PhotonRepository>().Settings;

        /// <summary>
        /// Cached list of PhotonNetwork.PlayerList.
        /// </summary>
        public Player[] Players 
        {   
            get
            {
                if (_players.Length == 0)
                {
                    _players = PhotonNetwork.PlayerList;
                }
                return _players;
            }
        }

        private Player[] _players = Array.Empty<Player>();

        private bool CanSwitch => CoreSettings.switchMasterClient && PhotonNetwork.IsMasterClient && Time.timeSinceLevelLoad > 60 && Time.time > _lastLagCheck && !_skipLagCheck && _timeAsMaster >= 60;
        public Player LastJoinedPlayer { get; set; }
        private int RandomSeed { get; set; }

        private static readonly List<string> IgnoreErrors = new(2) { "AnimationEvent", "missing a component" };
        private float _lastRoomPingCheck;
        private bool initialized;

        protected override bool SurviveSceneLoads => true;


        // INITIALIZERS: --------------------------------------------------------------------------
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnSubsystemsInit()
        {
            RuntimeModels.Clear();
            UpdateCalls.Clear();
            PhotonCalls.Clear();
            Instance.WakeUp();
        }

        private void Awake()
        {
            RandomSeed = UnityEngine.Random.Range(10000, 99999);
            UnityEngine.Random.InitState(RandomSeed);
        }

        private void Init()
        {
            if(initialized) return;

            initialized = true;
            PhotonNetwork.PrefabPool = new DefaultPool();
            PhotonNetwork.UseRpcMonoBehaviourCache = CoreSettings.monobehaviourCache;
            PhotonNetwork.NickName = CoreSettings.defaultName + UnityEngine.Random.Range(0, 1000);
            PhotonNetwork.SendRate = CoreSettings.sendRate;
            PhotonNetwork.SerializationRate = CoreSettings.sendRateOnSerialize;

            if (Application.isPlaying)
            {
                if (PhotonNetwork.NetworkingClient != null) PhotonNetwork.AddCallbackTarget(this);
                Application.logMessageReceived += LogReceived;

                _lastLagCheck = Time.time + CoreSettings.lagCheck * LAG_CHECK_DELAY;

                var go = new GameObject("PlayerNumbering");
                go.AddComponent<PlayerNumbering>();
                DontDestroyOnLoad(go);
            }
        }

        private void LogReceived(string condition, string stackTrace, LogType type)
        {
            if (!PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient || !IsValidError(condition, stackTrace)) return;
            if (type is LogType.Error or LogType.Assert)
            {
                _errors++;
            }

            if (type == LogType.Exception || _errors >= CoreSettings.switchMasterErrors)
            {
                TrySwitchMasterClient();
            }
        }

        private bool IsValidError(string condition, string stackTrace)
        {
            for(int i = 0, imax = IgnoreErrors.Count; i<imax; i++)
            {
                if (condition.Contains(IgnoreErrors[i]) || stackTrace.Contains(IgnoreErrors[i])) return false;
            }
            return true;
        }
        
        private void OnApplicationQuit()
        {
            if (!Application.isPlaying) return;
            PhotonNetwork.RemoveCallbackTarget(this);
            Application.logMessageReceived -= LogReceived;
        }

        private void Update()
        {
            if (!PhotonNetwork.InRoom) return;
            if (CoreSettings == null) return;
            if (PhotonNetwork.IsMasterClient)
            {
                _timeAsMaster += Time.deltaTime;
                if (CoreSettings.updatePing && Time.time > _lastRoomPingCheck)
                {
                    _lastRoomPingCheck = Time.time + CoreSettings.updatePingEvery;
                    PhotonNetwork.CurrentRoom.SetPing();
                }
            }

            if (_startRoundWhenTimeIsSynced)
            {
                SetStartTime();   // the "time is known" check is done inside the method.
            }
                
            if (Time.time > _lastTimeCheck)
            {
                _lastTimeCheck = Time.time + 1f;
                for(int i = 0, imax = UpdateCalls.Count; i<imax; i++)
                {
                    UpdateCalls[i]();
                }
            }

            if (CoreSettings.updatePing && Time.time > _lastPingCheck)
            {
                _lastPingCheck = Time.time + CoreSettings.updatePingEvery;
                PhotonNetwork.LocalPlayer.SetPing();
            }

            if (!CanSwitch) return;
            _lastLagCheck = Time.time + CoreSettings.lagCheck;

            var p = PhotonNetwork.GetPing();
            if (p > CoreSettings.lagThreshold)
            {
                TrySwitchMasterClient();
            }
        }

        private void TrySwitchMasterClient()
        {
            #if UNITY_EDITOR
            Debug.LogWarning($"TrySwitchMasterClient - Nickname: {PhotonNetwork.NickName}");
            #endif
            if (Players == null || Players.Length == 1) return;

            var p = PhotonNetwork.GetPing();
            if (Players.Length <= 1 || (p <= CoreSettings.lagThreshold && _errors < CoreSettings.switchMasterErrors)) return;
            var n = GetNextMaster();
            if (Equals(n, PhotonNetwork.LocalPlayer))
            {
                _lastLagCheck = Time.time + CoreSettings.lagCheck * LAG_CHECK_DELAY;
                //continue;
            }
            else
            {
                if(!Equals(n, PhotonNetwork.MasterClient)) PhotonNetwork.SetMasterClient(n);
                _skipLagCheck = true;
            }
        }

        /// <summary>
        /// Returns the next best Master based on the Photon Player ping.
        /// </summary>
        /// <returns></returns>
        private Player GetNextMaster()
        {
            Array.Sort<Player>(Players, (x, y) => x.GetPing().CompareTo(y.GetPing()));
            return Players[0];
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            Init();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void SetStartTime()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            // in some cases, when you enter a room, the server time is not available immediately.
            // time should be 0.0f but to make sure we detect it correctly, check for a very low value.
            if (PhotonNetwork.Time < 0.0001f)
            {
                // we can only start the round when the time is available. let's check that in Update()
                _startRoundWhenTimeIsSynced = true;
                return;
            }
            _startRoundWhenTimeIsSynced = false;
            
            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.Time);
        }

        private void UpdatePhotonCalls()
        {
            for (int i = 0, imax = PhotonCalls.Count; i < imax; i++)
            {
                PhotonCalls[i]();
            }
        }
        
        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            UpdatePhotonCalls();
        }

        public void OnCreatedRoom()
        {
            _lastLagCheck = Time.time + CoreSettings.lagCheck * LAG_CHECK_DELAY;
            UpdatePhotonCalls();
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            UpdatePhotonCalls();
        }

        public void OnJoinedRoom()
        {
            LastJoinedPlayer = PhotonNetwork.LocalPlayer;

            _players = PhotonNetwork.PlayerList;
            _clientStartTime = PhotonNetwork.Time;
            _lastLagCheck = Time.time + CoreSettings.lagCheck * LAG_CHECK_DELAY;
            UpdatePhotonCalls();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            UpdatePhotonCalls();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            UpdatePhotonCalls();
        }

        public void OnLeftRoom()
        {
            _startRoundWhenTimeIsSynced = false;
            UpdatePhotonCalls();
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            LastJoinedPlayer = newPlayer;
            _players = PhotonNetwork.PlayerList;
            var options = new RaiseEventOptions
            {
                TargetActors = new int[] { newPlayer.ActorNumber }
            };
            if(PhotonNetwork.IsMasterClient) PhotonNetwork.RaiseEvent(199, RandomSeed, options, SendOptions.SendReliable);
            UpdatePhotonCalls();
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            _players = PhotonNetwork.PlayerList;
            UpdatePhotonCalls();
        }

        public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(PhotonPlayerProperties.PING)) return;

            UpdatePhotonCalls();
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PhotonPlayerProperties.PING)) return;

            UpdatePhotonCalls();
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            _lastLagCheck = Time.time + CoreSettings.lagCheck * LAG_CHECK_DELAY;
            _timeAsMaster = 0;
            _skipLagCheck = false;
            UpdatePhotonCalls();
        }

        public void OnEvent(EventData photonEvent)
        {
            if(photonEvent.Code != 199) return;
            if (photonEvent.Sender != PhotonNetwork.MasterClient.ActorNumber) return;
            RandomSeed = (int)photonEvent.CustomData;
            UnityEngine.Random.InitState(RandomSeed);
        }
    }
}