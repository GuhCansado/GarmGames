using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime.Common
{
    public class IconStateMachineOverlayRed : IconStateMachine
    {
        public IconStateMachineOverlayRed(ColorTheme.Type color, IIcon overlay = null)
            : this(ColorTheme.Get(color), overlay)
        {
        }

        public IconStateMachineOverlayRed(Color color, IIcon overlay = null) : base(color, overlay)
        {
        }

        protected override ColorTheme.Type OverlayColor => ColorTheme.Type.Red;
    }
}