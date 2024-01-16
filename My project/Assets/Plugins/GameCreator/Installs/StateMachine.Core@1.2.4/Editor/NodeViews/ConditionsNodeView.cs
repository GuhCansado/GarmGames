using GameCreator.Runtime.Common;
using UnityEngine;
using NinjutsuGames.StateMachine.Runtime;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    [NodeCustomEditor(typeof(ConditionsNode))]
    public class ConditionsNodeView : BaseGameCreatorNodeView
    {
        public override Texture2D DefaultIcon => ICON_CONDITION.Texture;
        public override string DefaultIconName => ((ConditionsNode)nodeTarget).conditions.Length > 0 ? ((ConditionsNode)nodeTarget).conditions.Get(0).GetType().Name : null;

        protected static readonly IIcon ICON_SUCCESS = new IconArrowRight(ColorTheme.Type.Green);
        protected static readonly IIcon ICON_FAIL = new IconArrowRight(ColorTheme.Type.Red);
        
        public override void Enable()
        {
            base.Enable();
            
            var node = (ConditionsNode) nodeTarget;
            AddCounter(node.conditions.Length);
            
            var container = new VisualElement();
            container.name = "ResultContent";
            
            var successIcon = new Image();
            successIcon.image = ICON_SUCCESS.Texture;
            successIcon.name = "SuccessIcon";
            container.Add(successIcon);
            
            var failureIcon = new Image();
            failureIcon.image = ICON_FAIL.Texture;
            failureIcon.name = "FailureIcon";
            container.Add(failureIcon);
            
            titleContainer.Add(container);
        }
        
        public override void Update()
        {
            var n = nodeTarget as ConditionsNode;
            if (!HasChanged(n.conditions)) return;

            UpdateCounter(n.conditions.Length);
            UpdateIcon();
        }
    }
}