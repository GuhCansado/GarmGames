using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Destroy")]
    [Description("Network-Destroy the GameObject, unless it is static or not under this client's control. Can only be called on the owner of the PhotonView or MasterClient.")]

    [Category("Photon/Core/Photon Destroy")]
    
    [Parameter("Target", "The target game object. This game object must have a photon view.")]
    [Parameter("Transfer Ownership", "If true, the ownership of the photon view will be transfered to the local player before destroying the game object.")]
    
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Red, typeof(OverlayMinus))]


    [Keywords("Connect", "Network", "Photon", "Destroy")]
    [Serializable]
    public class InstructionPhotonDestroy : Instruction, IPunOwnershipCallbacks
    {
        [SerializeField] private PropertyGetGameObject target = GetGameObjectNone.Create();
        [SerializeField] private PropertyGetBool transferOwnership = GetBoolValue.Create(false);
        private GameObject _destroyTarget;
        public override string Title => $"Photon Destroy {target}";

        private bool _isDestroyed;

        protected override async Task Run(Args args)
        {
            var p = target.Get(args).GetCachedPhotonView();
            if (!p)
            {
                Debug.LogError("Failed to 'network-remove' GameObject because has no PhotonView components: " + p.gameObject);
                return;
            }

            if (p.IsMine || PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(p);
            }
            else if(transferOwnership.Get(args))
            {
                /*if (p.OwnershipTransfer != OwnershipOption.Takeover)
                {
                    Debug.LogError("Cannot Destroy! Ownership must be set to Takeover in PhotonView: " + p.gameObject);
                    return;
                }*/
                _destroyTarget = p.gameObject;
                _isDestroyed = false;
                PhotonNetwork.AddCallbackTarget(this);

                await NextFrame();
                p.TransferOwnership(PhotonNetwork.LocalPlayer);
                await While(() => !_isDestroyed);
            }
            else
            {
                Debug.LogError("Cannot Destroy! You don't have the rights to destroy this: " + p.gameObject);
            }
        }
        
        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            if (targetView.IsMine)
            {
                PhotonNetwork.RemoveCallbackTarget(this);
                if(_destroyTarget) PhotonNetwork.Destroy(_destroyTarget);
                _isDestroyed = true;
            }
        }

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer) {}
        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest) {}
    }
}