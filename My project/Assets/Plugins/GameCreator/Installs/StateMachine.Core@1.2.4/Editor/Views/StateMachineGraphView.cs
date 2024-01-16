using UnityEditor;

namespace NinjutsuGames.StateMachine.Editor
{

	public class StateMachineGraphView : BaseGraphView
	{
		// Nothing special to add for now
		public StateMachineGraphView(EditorWindow window) : base(window)
		{
		}

		/*public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			BuildStackNodeContextualMenu(evt);
			base.BuildContextualMenu(evt);
		}

		/// <summary>
		/// Add the New Stack entry to the context menu
		/// </summary>
		/// <param name="evt"></param>
		protected void BuildStackNodeContextualMenu(ContextualMenuPopulateEvent evt)
		{
			var position = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
			evt.menu.AppendAction("New Stack", e => AddStackNode(new BaseStackNode(position)), DropdownMenuAction.AlwaysEnabled);
		}*/
		
		/*public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			evt.menu.AppendSeparator();

			foreach (var nodeMenuItem in NodeProvider.GetNodeMenuEntries())
			{
				// if(nodeMenuItem.type != typeof(ActionsNode) || nodeMenuItem.type != typeof(ConditionsNode) ||nodeMenuItem.type != typeof(TriggerNode) || nodeMenuItem.type != typeof(BranchNode))
				// 	continue;
				
				var mousePos = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
				var nodePosition = mousePos;
				evt.menu.AppendAction("Create " + nodeMenuItem.path,
					e => CreateNodeOfType(nodeMenuItem.type, nodePosition),
					DropdownMenuAction.AlwaysEnabled
				);
			}

			base.BuildContextualMenu(evt);
		}

		private void CreateNodeOfType(Type type, Vector2 position)
		{
			RegisterCompleteObjectUndo("Added " + type + " node");
			AddNode(BaseNode.CreateFromType(type, position));
		}*/
	}
}