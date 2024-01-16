using UnityEngine;
using NinjutsuGames.StateMachine.Runtime;

namespace NinjutsuGames.StateMachine.Editor
{
    [NodeCustomEditor(typeof(StateMachineNode))]
    public class StateMachineNodeView : BaseGameCreatorNodeView
    {
        public override Texture2D DefaultIcon => ICON_STATE_MACHINE.Texture;
    }
}