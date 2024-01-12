using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Systems
{
    [Title("Towards Photon Direction")]
    [Image(typeof(IconArrowCircleRight), ColorTheme.Type.Green)]
    [Category("Towards Photon Direction")]
    [Description("NOTE: DO NOT USE this manually, this facing direction is set automatically at runtime when using Photon. Rotates the Character towards a specific world-space network direction.")]
    [Serializable]
    public class UnitFacingPhotonDirection : TUnitFacing
    {
        public Vector3 Direction { get; set; }

        public override Axonometry Axonometry { get; set; }
        protected override Vector3 GetDefaultDirection() => Vector3.Scale(Direction, Vector3Plane.NormalUp);

        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Towards Photon Direction";
    }
}