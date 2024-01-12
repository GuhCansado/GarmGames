using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Characters.IK;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Managers;
using NinjutsuGames.Photon.Runtime.Systems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Components
{
    [AddComponentMenu("Game Creator/Photon/Character Network")]
    [RequireComponent(typeof(PhotonView), typeof(Character)), DisallowMultipleComponent]
    [Icon(RuntimePaths.GIZMOS + "GizmoCharacter.png")]
    public class CharacterNetwork : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
    {
        [Serializable]
        public class Prop
        {
            public string boneName;
            public Vector3 position;
            public Quaternion rotation;
        }
        [SerializeField] private float smoothTime = 0.25f;
        [SerializeField] private float teleportIfDistance = 5;
        
        public static Action OnCharacterNetworkSpawned;
        public static Action OnCharacterNetworkDespawned;
        
        public bool IsNPC => photonView.IsRoomView;

        private const string LocalFormat = "[{0}] {1} - Local{2}";
        private const string RemoteFormat = "[{0}] {1} - Remote{2}";
        private const string Master = " [MasterClient]";
        private const string Clone = "(Clone)";
        
        private Character _character;
        private string _originalName;
        private Vector3 _networkPosition;
        private Vector3 _networkDirection;
        // private Quaternion _networkRotation;
        // private Vector3 _ragdollPosition;
        // private Quaternion _ragdollRotation;
        private bool _firstUpdate = true;
        private bool _firstResetVerticalSpeed = true;
        private double _lastPacketTime;
        private double _currentPacketTime;
        private Vector3 _targetPositionVelocity;
        private bool _isJumping;
        private float _lastTimeJump;
        private Vector3 _moveDirection;
        // private Vector3 _movePosition;
        private Quaternion _targetRotationVelocity;
        private int _lastTrackedIndex;
        private int _lastUnTrackedIndex;
        private Vector3 _mannequinPosition;
        private GameObject _trackingObject;
        private Transform _cachedTransform;
        private Transform _cachedRagdoll;
        private Vector3 _targetRotation;
        private readonly Dictionary<string, Prop> _propList = new();
        // private float _lastVerticalSpeed;
        private float _yPos;
        private bool _resetVerticalSpeed;
        private IUnitFacing _prevFacing;
        private IUnitPlayer _prevPlayer;
        private float _prevAngularSpeed = 1080;
        private string _currentModel;

        private float VerticalSpeed
        {
            get
            {
                return _character.Driver switch
                {
                    UnitDriverController controller => ReflectionUtilities.GetPrivateFieldValue<float>(controller, "m_VerticalSpeed"),
                    UnitDriverRigidbody => _character.Get<Rigidbody>().velocity.y,
                    UnitDriverNavmesh => -1,
                    _ => -1
                };
            }
        }

        #region Monobehaviour Callbacks

        private void Awake()
        {
            _cachedTransform = transform;
            _character = GetComponent<Character>();
            _originalName = gameObject.name.Replace(Clone, string.Empty);
            
            // _lastVerticalSpeed = _character.Driver.WorldMoveDirection.y;
            if (photonView.IsRoomView) return;
            photonView.Owner.TagObject = gameObject;
            _character.ChangeId(new IdString($"player-{photonView.Owner.ActorNumber}"));
                
            var variables = GetComponentsInChildren<LocalNameVariables>();
            var i = 0;
            foreach (var variable in variables)
            {
                variable.ChangeId(new IdString($"player-{photonView.Owner.ActorNumber}-variables-{i}"));
                i++;
            }
            PhotonNetworkManager.Instance.LastJoinedPlayer = photonView.Owner;
        }

        private void Start()
        {
            if (photonView.IsMine && !photonView.IsRoomView)
            {
                _character.IsPlayer = true;
            }
            else
            {
                _character.enabled = false;
                _character.enabled = true;
            }

            _prevAngularSpeed = _character.Motion.AngularSpeed;
            _prevFacing = _character.Kernel.Facing;
            _prevPlayer = _character.Kernel.Player;
            
            SetupKernel();
        }

        private void SetupKernel()
        {
            if (!photonView.IsMine || (photonView.IsMine && !_character.IsPlayer))
            {
                if(!photonView.IsMine && _character.Kernel.Facing is not UnitFacingPhotonDirection) _character.Kernel.ChangeFacing(_character, new UnitFacingPhotonDirection());
                
                if(IsNPC) return;
                if(_character.Kernel.Player is not UnitPlayerPhotonDirectional) _character.Kernel.ChangePlayer(_character, new UnitPlayerPhotonDirectional());
                _character.Motion.AngularSpeed = 9000;
            }
            else
            {
                if (_prevFacing != null) _character.Kernel.ChangeFacing(_character, _prevFacing as TUnitFacing);
                
                if(IsNPC) return;
                if (_prevPlayer != null) _character.Kernel.ChangePlayer(_character, _prevPlayer as TUnitPlayer);
                _character.Motion.AngularSpeed = _prevAngularSpeed;
            }
        }

        private void OnValidate()
        {
            if(Application.isPlaying) return;
            if(!_character) _character = GetComponent<Character>();
            _character.IsPlayer = false;
            #if UNITY_EDITOR
            SetupPhotonView();
            #endif
        }
        
        public override void OnEnable()
        {
            base.OnEnable();
            _character.EventBeforeUpdate += OnBeforeUpdate;
            _character.EventAfterUpdate += OnAfterUpdate;
            _character.Interaction.EventFocus += OnFocus;
            _character.Interaction.EventInteract += OnInteract;
            _character.Interaction.EventBlur += OnBlur;
            _character.EventJump += OnJump;
            _character.EventDie += OnDie;
            _character.EventRevive += OnRevive;
            _character.Props.EventAdd += OnAddProp;
            _character.Props.EventRemove += OnRemoveProp;
            _character.EventAfterChangeModel += OnChangeModel;

            CheckObservables();
#if UNITY_EDITOR
            SetupPhotonView();
#endif
            if (PhotonNetwork.UseRpcMonoBehaviourCache)
            {
                photonView.RefreshRpcMonoBehaviourCache();
            }

#if UNITY_EDITOR
            FormatName();
#endif
            
            OnCharacterNetworkSpawned?.Invoke();
        }

        public override void OnDisable()
        {
            if (Application.isPlaying && PhotonNetwork.InRoom && photonView && photonView.IsMine)
            {
                PhotonNetwork.RemoveRPCs(photonView);
            }
            base.OnDisable();
            
            _character.EventBeforeUpdate -= OnBeforeUpdate;
            _character.EventAfterUpdate -= OnAfterUpdate;
            _character.Interaction.EventFocus -= OnFocus;
            _character.Interaction.EventInteract -= OnInteract;
            _character.Interaction.EventBlur -= OnBlur;
            _character.EventJump -= OnJump;
            _character.EventDie -= OnDie;
            _character.EventRevive -= OnRevive;
            _character.EventAfterChangeModel -= OnChangeModel;
            
            OnCharacterNetworkDespawned?.Invoke();
        }

        private void LateUpdate()
        {
            if (photonView.IsMine)
            {
                if(_isJumping && Time.time >= _lastTimeJump)
                {
                    _isJumping = false;
                }
            }
            else
            {
                if (!(teleportIfDistance > 0) || _character.Dash.IsDashing) return;
                var dist = Vector3.Distance(transform.position, _networkPosition);
                if (!(dist >= teleportIfDistance)) return;
                // Debug.LogWarning($"Teleporting {name} because distance is {dist}");
                _character.Driver.SetPosition(_networkPosition);
                var rotation = Quaternion.LookRotation(_networkDirection, Vector3.up);
                _character.Driver.SetRotation(rotation);
                _cachedTransform.SetPositionAndRotation(_networkPosition, rotation);
                Physics.SyncTransforms();
            }
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                // UpdateCharacter();
            }
            else if (photonView.IsMine && !_isJumping) //Math.Abs(_character.Driver.WorldMoveDirection.y - _lastVerticalSpeed) > 0.025f
            {
                if(_character.Driver is UnitDriverController && VerticalSpeed == 0)
                {
                    _resetVerticalSpeed = true;
                }
            }
        }

        private void FixedUpdate()
        {
            if(photonView.IsMine) return;
            
            UpdateCharacter();
            UpdateTransform();
            // if(_character.IsPlayer) return;
            
            // ((UnitMotionController)_character.Motion).MoveDirection = _moveDirection;
            // ((UnitMotionController)_character.Motion).MovePosition = _movePosition;
        }

        #endregion

        #region Character Methods

        private void UpdateCharacter()
        {
            if(_firstUpdate) return;

            if (_character.Ragdoll.IsRagdoll || _character.IsDead)
            {
                // _targetPositionVelocity = Vector3.zero;
                _cachedTransform.SetPositionAndRotation(_networkPosition, Quaternion.LookRotation(_networkDirection, Vector3.up));
                Physics.SyncTransforms();
                if (!_cachedRagdoll) _cachedRagdoll = _character.Animim.Animator.transform;
                // if (_cachedRagdoll) _cachedRagdoll.SetPositionAndRotation(_ragdollPosition, _ragdollRotation);
            }
            else
            {
                ((UnitMotionController)_character.Motion).MoveDirection = _moveDirection; //if(IsNPC)
                // ((UnitMotionController)_character.Motion).MovePosition = _networkPosition;
                if (_networkDirection != Vector3.zero)
                {
                    if (_character.Kernel.Facing is UnitFacingPhotonDirection direction) direction.Direction = _networkDirection;
                    _cachedTransform.rotation = Quaternion.Lerp(_cachedTransform.rotation, Quaternion.LookRotation(_networkDirection, Vector3.up), Time.unscaledDeltaTime / 1f);
                }
            }
        }

        private void UpdateTransform()
        {
            var timeToReachGoal = _currentPacketTime - _lastPacketTime;
            if (!(timeToReachGoal > 0.0)) return;
            var position = _cachedTransform.position;
            _yPos = Mathf.Lerp(position.y, _networkPosition.y, Time.unscaledDeltaTime / 1f);
            // _yPos = Mathf.Clamp(_yPos, Mathf.Min(position.y, _position.y), Mathf.Max(position.y, _position.y));
            var newPos = Vector3.SmoothDamp(position, _networkPosition, ref _targetPositionVelocity, smoothTime);
            newPos.y = _yPos;
            position = newPos;
            _cachedTransform.position = position;
            // _cachedTransform.SetPositionAndRotation(position, _cachedTransform.rotation);
            Physics.SyncTransforms();
        }

        #endregion

        #region Character Events
        
        private void OnChangeModel()
        {
            if(!photonView.IsMine) return;
            if (!PhotonNetworkManager.RuntimeModels.ContainsKey(_character.Animim.Animator.gameObject.name)) return;
            _currentModel = _character.Animim.Animator.gameObject.name;
            photonView.RPC(nameof(RPC_ChangeModel), RpcTarget.Others, _currentModel);
        }
        
        private void OnDie()
        {
            if (photonView.IsMine) photonView.RPC(nameof(RPCDie), RpcTarget.Others);
        }
        
        private void OnRevive()
        {
            if (photonView.IsMine) photonView.RPC(nameof(RPCRevive), RpcTarget.Others);
            // else if(_cachedRagdoll) _cachedRagdoll.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        
        private void OnJump(float obj)
        {
            if (!photonView.IsMine) return;
            _lastTimeJump = Time.time + 0.1f;
            _isJumping = true;
        }
        
        private void OnInteract(Character character, IInteractive interactive)
        {
            /*if (photonView.IsMine)
            {
                if(!PhotonInteractiveRegistry.Instance) return;
                
                var obj = interactive?.Instance;
                InteractionTracker tracker = InteractionTracker.Require(obj);
                Debug.LogWarning($"OnInteract {obj}");
                _lastTrackedIndex = PhotonInteractiveRegistry.Instance.GetInstanceIndex(obj);
                photonView.RPC(nameof(RPCUnTrackObject), RpcTarget.OthersBuffered, _lastTrackedIndex);
            }*/
        }
        
        private void OnBlur(Character character, IInteractive interactive)
        {
            try
            {
                if (!photonView.IsMine) return;
                if (!PhotonInteractiveRegistry.Instance) return;
                if (character.Interaction.Target == null) return;
                if (character.Interaction.Target.Instance == null) return;
                if (interactive == null) return;

                var obj = character.Interaction.Target.Instance;
                if (!obj) return;
                _lastTrackedIndex = -1;
                _lastUnTrackedIndex = PhotonInteractiveRegistry.Instance.GetInstanceIndex(obj);
                if (_lastUnTrackedIndex != -1) photonView.RPC(nameof(RPCUnTrackObject), RpcTarget.Others, _lastUnTrackedIndex);
            }
            catch
            {
                // ignored
            }
        }
        
        private void OnFocus(Character character, IInteractive interactive)
        {
            try
            {
                if (!photonView.IsMine) return;
                if(!PhotonInteractiveRegistry.Instance) return;
                if (character.Interaction.Target == null) return;
                if (character.Interaction.Target.Instance == null) return;
                if (interactive == null) return;

                var obj = character.Interaction.Target.Instance;
                if (!obj) return;
                _lastUnTrackedIndex = -1;
                _lastTrackedIndex = PhotonInteractiveRegistry.Instance.GetInstanceIndex(obj);
                if(_lastTrackedIndex != -1) photonView.RPC(nameof(RPCTrackObject), RpcTarget.Others, _lastTrackedIndex);
            }
            catch
            {
                // ignored
            }
        }

        private void OnStopInteraction(Character character, IInteractive interactive)
        {
            Debug.LogWarning($"OnStopInteraction {interactive?.Instance}");
        }

        private void OnStartInteraction(Character character, IInteractive interactive)
        {
            Debug.LogWarning($"OnStartInteraction {interactive?.Instance}");
        }

        private void OnAfterUpdate()
        {
            
        }

        private void OnBeforeUpdate()
        {
           
        }
        
        private void OnRemoveProp(Transform attachment)
        {
            if(!attachment) return;
            if (!photonView.IsMine) return;
            var propName = attachment.name.Replace("(Clone)", string.Empty);
            if (_propList.ContainsKey(propName)) _propList.Remove(propName);

            photonView.RPC(nameof(RPC_RemoveProp), RpcTarget.Others, attachment.name);
        }

        /// <summary>
        /// Check if the character has attached a prop to a bone and if so, send it to the network.
        /// </summary>
        /// <param name="bone"></param>
        /// <param name="attachment"></param>
        private void OnAddProp(Transform bone, GameObject attachment)
        {
            if(!bone) return;
            if(!PhotonNetwork.InRoom) return;
            if (!photonView.IsMine) return;
            var propName = attachment.name.Replace("(Clone)", string.Empty);
            var localPosition = attachment.transform.localPosition;
            var localRotation = attachment.transform.localRotation;
            _propList.TryAdd(propName, new Prop() {boneName = bone.name, position = localPosition, rotation = localRotation});
            photonView.RPC(nameof(RPC_AddProp), RpcTarget.Others, bone.name, propName, localPosition, localRotation);
        }

        #endregion
        
        #region Editor Only Methods

#if UNITY_EDITOR
        private void SetupPhotonView()
        {
            if (photonView.ObservedComponents == null) photonView.ObservedComponents = new List<Component>();

            if (photonView.Synchronization == ViewSynchronization.Off) photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
            
            CheckObservables();
        }
        
        private void FormatName()
        {
            if (!PhotonNetwork.InRoom || !photonView) return;

            var player = photonView.Owner;
            var master = player?.IsMasterClient ?? false;

            gameObject.name = string.Format(photonView.IsMine ? LocalFormat : RemoteFormat, photonView.ViewID, _originalName, master ? Master : string.Empty);
        }
#endif

        #endregion

        #region Utils

        private void CheckObservables()
        {
            if(photonView.ObservedComponents == null) return;
            CharacterNetwork charNet = null;
            for (var i = 0; i < photonView.ObservedComponents.Count; i++)
            {
                if (photonView.ObservedComponents[i] is CharacterNetwork && i != 0)
                {
                    charNet = photonView.ObservedComponents[i] as CharacterNetwork;
                    photonView.ObservedComponents.RemoveAt(i);
                    break;
                }
            }

            if (charNet)
            {
                photonView.ObservedComponents.Insert(0, charNet);
            }
        }

        #endregion

        #region Photon RPCs
        
        /*[PunRPC]
        private void RPC_ResetVerticalSpeed()
        {
            Debug.LogWarning($"RPC_ResetVerticalSpeed");
            _character.Driver.ResetVerticalVelocity();
        }*/
        
        [PunRPC]
        private void RPC_ChangeModel(string model)
        {
            // Debug.LogWarning($"RPC_ChangeModel {model}");
            if (!PhotonNetworkManager.RuntimeModels.TryGetValue(model, out var config))
            {
                Debug.LogWarning($"Couldn't find model with name: '{model}'. Check if it's registered in a PhotonLocalModelsList or PhotonGlobalModelsList.");
                return;
            }
            var options = new Character.ChangeOptions
            {
                materials = config.materialSounds,
                offset = config.offset
            };
            _character.ChangeModel(config.prefab.Get(gameObject), options);
        }

        [PunRPC]
        private void RPC_RemoveProp(string boneName)
        {
            if (!Enum.TryParse(boneName, out HumanBodyBones bone))
            {
                Debug.LogWarning($"Couldn't parse {boneName} to HumanBodyBones in {gameObject.name}",gameObject);
                return;
            }
            _character.Props.RemoveAtBone(new Bone(bone));
        }
        
        [PunRPC]
        private void RPC_AddProp(string boneName, string propName, Vector3 position, Quaternion rotation)
        {
            if (!PhotonAttachments.Instance)
            {
                // Debug.LogWarning($"Couldn't find CharacterAttachments in {gameObject.name}");
                return;
            }
            // Debug.LogWarning($"RuntimeAttachments: {PhotonAttachments.Instance.RuntimeAttachments.Count}");
            if (!PhotonAttachments.Instance.RuntimeAttachments.ContainsKey(propName))
            {
                // Debug.Log($"Couldn't find prop {propName} in {gameObject.name}", gameObject);
                return;
            }
            var prop = PhotonAttachments.Instance.RuntimeAttachments[propName];
            if (!Enum.TryParse(boneName, out HumanBodyBones bone))
            {
                Debug.LogWarning($"Couldn't parse {boneName} to HumanBodyBones in {gameObject.name}",gameObject);
                return;
            }
            
            _character.Props.AttachPrefab(new Bone(bone), prop, position, rotation);
        }

        [PunRPC]
        private void RPCDie()
        {
            _character.IsDead = true;
        }
        
        [PunRPC]
        private void RPCRevive()
        {
            _character.IsDead = false;
        }
        
        [PunRPC]
        private void RPCTrackObject(int index)
        {
            if(!PhotonInteractiveRegistry.Instance) return;
            _trackingObject = PhotonInteractiveRegistry.Instance.GetInstance(index);
            if (!_trackingObject)
            {
                // Debug.LogWarning($"Character: {_character} couldn't find interactive object with index {index}. Check your Interactive Registry.");
                return;
            }
            var rig = _character.IK.GetRig<RigLookTo>();
            rig?.SetTarget(new LookToTransform(
                0, 
                _trackingObject.transform, 
                Vector3.zero
            ));
        }
        
        [PunRPC]
        private void RPCUnTrackObject(int index)
        {
            if(!PhotonInteractiveRegistry.Instance) return; 
            _trackingObject = PhotonInteractiveRegistry.Instance.GetInstance(index);
            if (!_trackingObject)
            {
                // Debug.LogWarning($"Character: {_character} couldn't find interactive object with index {index}. Check your Interactive Registry.");
                return;
            }
            IInteractive tracker = InteractionTracker.Require(_trackingObject);

            var rig = _character.IK.GetRig<RigLookTo>();
            rig?.RemoveTarget(new LookToTransform(
                0, 
                _trackingObject.transform, 
                Vector3.zero
            ));
            _trackingObject = null;
            tracker.Stop();
        }

        [PunRPC]
        private void SyncCharacter(int trackIndex, int untrackIndex, bool isDead, string currentModel, Hashtable props)
        {
            if(trackIndex > -1) RPCTrackObject(trackIndex);
            else if(untrackIndex > -1) RPCUnTrackObject(untrackIndex);
            _character.IsDead = isDead;
            if(!string.IsNullOrEmpty(currentModel)) RPC_ChangeModel(currentModel);
            foreach (var prop in props)
            {
                var propData = prop.Value as Hashtable;
                RPC_AddProp( propData["bone"].ToString(), prop.Key.ToString(), (Vector3)propData["position"], (Quaternion)propData["rotation"]);
            }
        }

        #endregion

        #region Photon Events

        public override void OnPlayerEnteredRoom(Player player)
        {
            if(!photonView.IsMine) return;
            var props = new Hashtable();
            foreach (var prop in _propList)
            {
                var location = new Hashtable
                {
                    {"position", prop.Value.position},
                    {"rotation", prop.Value.rotation},
                    {"bone", prop.Value.boneName},
                };
                props.Add(prop.Key, location);
            }
            photonView.RPC(nameof(SyncCharacter), player, _lastTrackedIndex, _lastUnTrackedIndex, _character.IsDead, _currentModel, props);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
#if UNITY_EDITOR
            if (PhotonNetwork.InRoom) FormatName();
#endif
            SetupKernel();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (!_cachedRagdoll) _cachedRagdoll = _character.Animim.Animator.transform;
                if(!IsNPC) stream.SendNext(_character.Player.InputDirection);
                stream.SendNext(_cachedTransform.position);
                // stream.SendNext(_cachedTransform.rotation);
                stream.SendNext(_character.Kernel.Facing.WorldFaceDirection);
                // stream.SendNext(_character.Motion.MovePosition);
                stream.SendNext(_character.Motion.MoveDirection); //if(IsNPC) 
                // stream.SendNext(_character.Ragdoll.IsRagdoll ? Vector3.zero : _character.Kernel.Player.InputDirection); 
                stream.SendNext(!_character.Ragdoll.IsRagdoll && _isJumping);
                if (_character.Ragdoll.IsRagdoll)
                {
                    stream.SendNext(_cachedRagdoll.position);
                    stream.SendNext(_cachedRagdoll.rotation);
                }
                // stream.SendNext(_character.Ragdoll.IsRagdoll ? _character.Animim.Animator.transform.position : Vector3.zero);
                // stream.SendNext(_character.Animim.SmoothTime);
                // stream.SendNext(_character.Animim.Animator.rootPosition);
                // stream.SendNext(_character.Animim.Animator.rootRotation);
                if(_resetVerticalSpeed && _firstResetVerticalSpeed)
                {
                    _firstResetVerticalSpeed = false;
                    _resetVerticalSpeed = false;
                }
                stream.SendNext(_resetVerticalSpeed);
                if(_resetVerticalSpeed) _resetVerticalSpeed = false;
            }
            else
            {
                if(!IsNPC)
                {
                    var input = (Vector3) stream.ReceiveNext();
                    (_character.Player as UnitPlayerPhotonDirectional)?.SetInput(input);
                }
                _networkPosition = (Vector3)stream.ReceiveNext();
                // _networkRotation = (Quaternion) stream.ReceiveNext();
                _networkDirection = (Vector3)stream.ReceiveNext();
                // _inputDirection = (Vector3)stream.ReceiveNext();
                
                // _movePosition = (Vector3)stream.ReceiveNext();
                _moveDirection = (Vector3)stream.ReceiveNext(); //if(IsNPC) 

                _isJumping = (bool)stream.ReceiveNext();
                // _mannequinPosition = (Vector3)stream.ReceiveNext();
                if(_isJumping) _character.Kernel.Motion.Jump();
                if (_character.Ragdoll.IsRagdoll)
                {
                    // _ragdollPosition = (Vector3)stream.ReceiveNext();
                    // _ragdollRotation = (Quaternion)stream.ReceiveNext();
                }
                if (_firstUpdate)
                {
                    _cachedTransform.SetPositionAndRotation(_networkPosition, Quaternion.LookRotation(Vector3.Scale(_networkDirection, Vector3Plane.NormalUp)));
                    Physics.SyncTransforms();
                    _firstUpdate = false;
                }
                else
                {
                    // UpdateCharacter();
                }
                _lastPacketTime = _currentPacketTime;
                _currentPacketTime = info.SentServerTime;

                // _character.Animim.SmoothTime = (float) stream.ReceiveNext();
                // _character.Animim.Animator.rootPosition = (Vector3) stream.ReceiveNext();
                // _character.Animim.Animator.rootRotation = (Quaternion) stream.ReceiveNext();
                // _positionAtLastPacket = transform.position;
                
                if((bool) stream.ReceiveNext())
                {
                    // Debug.LogWarning($"Restarting vertical velocity");
                    _character.Driver.ResetVerticalVelocity();
                }
            }
        }
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var list = GetComponentsInChildren<Trigger>();
            for(int i = 0, imax = list.Length; i<imax; i++)
            {
                var trigger = list[i];
                if (trigger.GetTriggerEvent() is not EventPhotonInstantiate) continue;
                var evt = (EventPhotonInstantiate)trigger.GetTriggerEvent();
                evt.OnPhotonInstantiate(info);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            if (_character.Motion is not UnitMotionController controller) return;
            controller.MoveDirection = Vector3.zero;
            controller.MovePosition = Vector3.zero;
        }

        #endregion
    }
}