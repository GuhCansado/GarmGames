using GameCreator.Editor.Common;
using UnityEngine;
using NinjutsuGames.StateMachine.Runtime;

namespace NinjutsuGames.StateMachine.Editor
{
    [NodeCustomEditor(typeof(StartNode))]
    public class StartNodeView : BaseGameCreatorNodeView
    {
        public override Texture2D DefaultIcon => ICON_START.Texture;
        
        public const string INFO_MESSAGE = "This is the entry point of the state machine. " +
                                           "It will be executed when the state machine is started.";

        /*public override void Enable()
        {
            base.Enable();
            
            var node = (StartNode) this.nodeTarget;
            var description = new InfoMessage(INFO_MESSAGE);
            controlsContainer.Add(description);
        }*/
    }
}