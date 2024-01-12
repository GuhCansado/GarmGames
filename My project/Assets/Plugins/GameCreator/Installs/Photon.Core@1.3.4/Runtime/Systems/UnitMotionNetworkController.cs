using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NinjutsuGames.Photon.Runtime.Systems
{
    /*[Title("Photon Motion Controller")]
    [Image(typeof(IconChip), ColorTheme.Type.Green)]

    [Category("Photon Motion Controller")]
    [Description(
        "Motion system that defines how the Character responds to external stimulus and synchronize it with the Photon Network."
    )]

    [Serializable]
    public class UnitMotionNetworkController : UnitMotionController
    {
        [SerializeField] private MotionNetwork motionNetwork;
        private CharacterNetwork _characterNetwork;
        private PhotonView _photonView;

        public override void OnDispose(Character character)
        {
            base.OnDispose(character);
            Object.Destroy(_characterNetwork);
            Object.Destroy(_photonView);
        }

        public void SetMovePosition(Vector3 movePosition)
        {
            MovePosition = movePosition;
        }
        
        public void SetMoveDirection(Vector3 moveDirection)
        {
            MoveDirection = moveDirection;
        }

        public override void OnStartup(Character character)
        {
            base.OnStartup(character);
            
            _characterNetwork = character.GetComponent<CharacterNetwork>();
            if (!_characterNetwork)
            {
                _characterNetwork = character.gameObject.AddComponent<CharacterNetwork>();
            }

            // _characterNetwork.Setup(this);

            if (!_photonView)
            {
                _photonView = character.gameObject.GetComponent<PhotonView>();
            }
        }
    }*/
}