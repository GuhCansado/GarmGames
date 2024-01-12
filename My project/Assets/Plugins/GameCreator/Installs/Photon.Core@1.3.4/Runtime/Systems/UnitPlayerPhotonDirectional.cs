using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Systems
{
    [Title("Photon Directional")]
    [Image(typeof(IconGamepadCross), ColorTheme.Type.Blue)]
    
    [Category("Photon Directional")]
    [Description(
        "NOTE: DO NOT USE this manually, this is set automatically at runtime when using Photon. Moves the remote Player using a directional from network input."
    )]

    [Serializable]
    public class UnitPlayerPhotonDirectional : TUnitPlayer
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        // INITIALIZERS: --------------------------------------------------------------------------
        
        // OVERRIDERS: ----------------------------------------------------------------------------
     
        public override void OnDisable()
        {
            base.OnDisable();
            
            Character.Motion?.MoveToDirection(Vector3.zero, Space.World, 0);
        }
        
        public void SetInput(Vector3 input)
        {
            InputDirection = input;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        public override void OnUpdate()
        {
            base.OnUpdate();

            // var speed = Character.Motion?.LinearSpeed ?? 0f;
            // Character.Motion?.MoveToDirection(InputDirection * speed, Space.World, 0);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Photon Directional";
    }
}