using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime.Common
{
    public class IconStateMachineOverlayYellow : IconStateMachine
    {
        public IconStateMachineOverlayYellow(ColorTheme.Type color, IIcon overlay = null)
            : this(ColorTheme.Get(color), overlay)
        {
        }

        public IconStateMachineOverlayYellow(Color color, IIcon overlay = null) : base(color, overlay)
        {
        }

        protected override ColorTheme.Type OverlayColor => ColorTheme.Type.Yellow;
    }
}