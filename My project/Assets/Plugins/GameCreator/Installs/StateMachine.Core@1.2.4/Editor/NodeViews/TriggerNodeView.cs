using System;
using UnityEngine;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.StateMachine.Editor
{
    [NodeCustomEditor(typeof(TriggerNode))]
    public class TriggerNodeView : BaseGameCreatorNodeView
    {
        public override Texture2D DefaultIcon => ICON_EVENT.Texture;
        public override string DefaultIconName => ((TriggerNode)nodeTarget).triggerEvent == null ? string.Empty :((TriggerNode)nodeTarget).triggerEvent.GetType().Name;

        public event Action onUpdated;
        
        public override void Update()
        {
            var n = nodeTarget as TriggerNode;
            if (!HasChanged(n.triggerEvent)) return;

            UpdateIcon();
            onUpdated?.Invoke();

            if(!EditorApplication.isPlaying) return;
            var node = nodeTarget as TriggerNode;
            if(node.Context) node.OnProcess(node.Context);
        }
    }
}