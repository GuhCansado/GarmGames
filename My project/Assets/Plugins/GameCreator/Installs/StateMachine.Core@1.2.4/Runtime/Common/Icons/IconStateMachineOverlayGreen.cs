using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime.Common
{
    public class IconStateMachineOverlayGreen : IconStateMachine
    {
        public IconStateMachineOverlayGreen(ColorTheme.Type color, IIcon overlay = null)
            : this(ColorTheme.Get(color), overlay)
        {
        }

        public IconStateMachineOverlayGreen(Color color, IIcon overlay = null) : base(color, overlay)
        {
        }

        protected override ColorTheme.Type OverlayColor => ColorTheme.Type.Green;
    }
}