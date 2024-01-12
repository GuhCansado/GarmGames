using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class TextContent
    {
        public PropertyGetString playersMessageFormat = new("[{0}]: {1}");
        public bool enableNotificationMessages = true;
        public PropertyGetString joinedRoomMessage = new("Joined room {0}.");
        public PropertyGetString playerJoinedMessage = new("{0} Joined!");
        public PropertyGetString playerLeftMessage = new("{0} Left!");
    }
    [Serializable]
    public class ChatEvents
    {
        public InstructionList onOpen = new();
        public InstructionList onClose = new();
        public InstructionList onSendMessage = new();
        public InstructionList onReceiveMessage = new();
    }

    [Serializable]
    public class FloatingText
    {
        public GameObject prefab;
        public Vector3 offset = new(0, 1f, 0);
        public float duration = 6;
        public Color color = Color.white;
        public float fadeOutTime = 0.5f;
    }

    [Serializable]
    public class ChatColors
    {
        public Color playerColor = Color.cyan;
        public Color othersColor = Color.white;
        /*public Gradient otherColor = new() {alphaKeys = new[]{ new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }, colorKeys = new[]
        {
            new GradientColorKey(Color.red, 0), new GradientColorKey(Color.yellow, 0.25f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.blue, 0.75f), new GradientColorKey(Color.magenta, 1f)
        }};*/
        public Color serverColor = Color.yellow;
    }

    /// <summary>
    /// Networked chat logic. Takes care of sending and receiving of chat messages.
    /// </summary>

    [AddComponentMenu("Game Creator/Photon/Room Chat")]
    public class RoomChat : Chat, IOnEventCallback, IInRoomCallbacks, IMatchmakingCallbacks
    {
        public static RoomChat Instance { get; private set; }

        [SerializeField] private ChatColors colors = new();
        [SerializeField] private TextContent notifications = new();
        [SerializeField] private ChatEvents events = new();
        // [SerializeField] private FloatingText floatingText = new();       
        
        public Dictionary<Player, string> LastMessages { get; private set; } = new();
        public Player LastPlayer { get; private set; }

        /// <summary>
        /// If you want the chat window to only be shown in multiplayer games, set this to 'true'.
        /// </summary>
        // private bool destroyIfOffline = true;
        private readonly int chatEventCode = 198;
        private const string S_SPLIT = "|s|";
        // private Color[] _otherPlayerColors;
        
        private readonly HashSet<Color> usedColors = new HashSet<Color>();
        private GameObject templateOnReceive;


        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        /// <summary>
        /// We want to listen to input field's events.
        /// </summary>

        private void Start()
        {
            /*if (destroyIfOffline && !PhotonNetwork.InRoom || PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
                return;
            }*/
            if (PhotonNetwork.NetworkingClient != null) PhotonNetwork.AddCallbackTarget(this);
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            // SetupColors();
            if(notifications.enableNotificationMessages) Add(string.Format(notifications.joinedRoomMessage.Get(Args.EMPTY), PhotonNetwork.CurrentRoom.Name), colors.serverColor, false, null);

            input.GameObject.SetActive(true);
            
        }

        public void OnLeftRoom()
        {
            input.GameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (PhotonNetwork.NetworkingClient != null) PhotonNetwork.RemoveCallbackTarget(this);
        }

        /// <summary>
        /// Send the chat message to everyone else.
        /// </summary>

        protected override void OnSubmit(string text)
        {
            //OnRaiseEvent((byte)chatEventCode, text, PhotonNetwork.LocalPlayer.ActorNumber);

            Send(text);
        }

        /*private Color GetUniqueColor()
        {
            Color newColor;
            do
            {
                var randomTime = Random.Range(0f, 1f);
                newColor = colors.otherColor.Evaluate(randomTime);
            }
            while (usedColors.Contains(newColor)); // && usedColors.Count < colorLimit

            usedColors.Add(newColor);
            return newColor;
        }

        private void SetupColors()
        {
            _otherPlayerColors = new Color[PhotonNetwork.CurrentRoom.MaxPlayers];
            for (var i = 0; i < _otherPlayerColors.Length; i++)
            {
                _otherPlayerColors[i] = colors.otherColor.Evaluate(Random.Range(0f, 1f));
            }
        }*/
        /// <summary>
        /// True when input field is focused.
        /// </summary>
        public static bool IsOpen()
        {
            return Instance && Instance._selected; //mInst.input.isFocused;
        }

        public static void Send(string text)
        {
            //mInst.OnRaiseEvent((byte)mInst.chatEventCode, text, PhotonNetwork.LocalPlayer.ActorNumber);

            var options = RaiseEventOptions.Default;
            options.Receivers = ReceiverGroup.All;
            PhotonNetwork.RaiseEvent((byte)Instance.chatEventCode, text, options, SendOptions.SendReliable);
            
            _ = Instance.events.onSendMessage.Run(new Args(Instance.gameObject));
        }

        /// <summary>
        /// Add a new chat entry.
        /// </summary>
        /// <param name="text"></param>
        public static void Add(string text)
        {
            if(Instance == null)
            {
                Debug.LogWarning("Can't add chat messages there is no RoomChat instance found.");
                return;
            }
            Add(text, Instance.colors.serverColor);
        }

        /// <summary>
        /// Add a new chat entry.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void Add(string text, Color color)
        {
            if (Instance) Instance.Add(text, Instance.colors.serverColor, false, null);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != chatEventCode) return;
            var player = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
            if (player == null) return;
            var color = colors.playerColor;
            var message = (string)photonEvent.CustomData;
            var originalMessage = message;

            if (message.Contains(S_SPLIT))
            {
                message = message.Replace(S_SPLIT, string.Empty);
                color = colors.serverColor;
            }
            else
            {
                // If the message was not sent by the player, color it differently and play a sound
                if (!Equals(player, PhotonNetwork.LocalPlayer))
                {
                    color = colors.othersColor;//_otherPlayerColors[player.GetPlayerNumber()];
                }

                // Embed the player's name into the message
                message = string.Format(notifications.playersMessageFormat.Get(Args.EMPTY), player.NickName, message);// $"[{player.NickName}]: {message}";
                
                if(LastMessages.ContainsKey(player)) LastMessages[player] = originalMessage;
                else LastMessages.Add(player, originalMessage);
                LastPlayer = player;
            
                // if(floatingText.enabled) FloatingMessage(originalMessage, floatingText.messageColor, player);

                var target = player.TagObject as GameObject;
                if(!target) target = gameObject;
                var args = new Args(gameObject, target);
                _ = events.onReceiveMessage.Run(args);
                
                /*if (templateOnReceive == null)
                {
                    templateOnReceive = RunInstructionsList.CreateTemplate(events.onReceiveMessage);
                }

                _ = RunInstructionsList.Run(
                    args.Clone, templateOnReceive, 
                    new RunnerConfig
                    {
                        Name = $"On Receive"/*,
                        Location = new RunnerLocationPosition(
                            args.Self != null ? args.Self.transform.position : Vector3.zero, 
                            args.Self != null ? args.Self.transform.rotation : Quaternion.identity
                        )#1#
                    }
                );*/
            }
           
            Add(message, color, false, player);
        }
        
        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(notifications.enableNotificationMessages) Add(string.Format(notifications.playerJoinedMessage.Get(Args.EMPTY), newPlayer.NickName), colors.serverColor, false, null);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (notifications.enableNotificationMessages) Add(string.Format(notifications.playerLeftMessage.Get(Args.EMPTY), otherPlayer.NickName), colors.serverColor, false, null);
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {           
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        /*private void FloatingMessage(string message, Color color, Player player)
        {
            if(player?.TagObject != null)
            {
                // if(player.IsLocal) return;

                var go = player.TagObject as GameObject;

                FloatingTextManager.Show(floatingText.prefab, message, color, go.transform, floatingText.messageOffset, floatingText.messageDuration, floatingText.fadeOutTime);
            }
        }*/

        protected override void OnOpen()
        {
            _ = events.onOpen.Run(new Args(gameObject));
        }
        
        protected override void OnClose()
        {
            _ = events.onClose.Run(new Args(gameObject));
        }
    }
}