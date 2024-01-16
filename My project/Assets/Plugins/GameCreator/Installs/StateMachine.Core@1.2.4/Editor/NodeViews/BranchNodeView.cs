using UnityEngine;
using NinjutsuGames.StateMachine.Runtime;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    [NodeCustomEditor(typeof(BranchNode))]
    public class BranchNodeView : BaseGameCreatorNodeView
    {
        private Image conditionIcon;
        private Image actionIcon;
        public override Texture2D DefaultIcon => ICON_BRANCH.Texture;
        
        private Texture GetConditionIcon()
        {
            var node = (BranchNode) nodeTarget;
            var conditions = node.branch.GetConditionsList();
            return conditions.Length > 0 ? GetIcon(conditions.Get(0).GetType().Name) : ICON_CONDITION.Texture;
        }
        
        private Texture GetInstructionIcon()
        {
            var node = (BranchNode) nodeTarget;
            var instructions = node.branch.GetInstructionsList();
            return instructions.Length > 0 ? GetIcon(instructions.Get(0).GetType().Name) : ICON_INSTRUCTION.Texture;
        }

        public override void Enable()
        {
            base.Enable();
            
            var container = new VisualElement();
            container.name = "BranchContent";
            
            conditionIcon = new Image();
            conditionIcon.image = GetConditionIcon();
            conditionIcon.name = "ConditionIcon";
            container.Add(conditionIcon);
            
            var arrowIcon = new Image();
            arrowIcon.image = ICON_ARROW.Texture;
            arrowIcon.name = "ArrowIcon";
            container.Add(arrowIcon);
            
            actionIcon = new Image();
            actionIcon.image = GetInstructionIcon();
            actionIcon.name = "ActionIcon";
            container.Add(actionIcon);
            
            titleContainer.parent.Insert(1, container);
        }
        
        public override void Update()
        {
            var n = nodeTarget as BranchNode;
            if (!HasChanged(n.branch)) return;

            conditionIcon.image = GetConditionIcon();
            actionIcon.image = GetInstructionIcon();
            UpdateIcon();
        }
    }
}