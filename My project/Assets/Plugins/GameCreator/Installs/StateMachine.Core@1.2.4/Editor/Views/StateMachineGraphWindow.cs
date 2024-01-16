
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{

	public class StateMachineGraphWindow : BaseGraphWindow
	{
		private StateMachineAsset tmpGraph;
		private CustomToolbarView toolbarView;
		private MiniMapView minimap;
		
		private Label titleLabel;
		private Label pathLabel;

		protected override void OnDestroy()
		{
			graphView?.Dispose();
			DestroyImmediate(tmpGraph);
		}

		protected override void InitializeWindow(StateMachineAsset graph)
		{
			titleContent = new GUIContent("State Machine");

			if (graphView == null)
			{
				graphView = new StateMachineGraphView(this);
				graphView.initialized += AddInfo;
				minimap = new MiniMapView(graphView)
				{
					anchored = true,
					style =
					{
						backgroundColor = Color.clear,
						borderBottomColor = Color.clear,
						borderLeftColor = Color.clear,
						borderRightColor = Color.clear,
						borderTopColor = Color.clear,
					}
				};
				minimap.SetPosition(new Rect(1, 20, 200, 100));
				minimap.UpdatePresenterPosition();
				graphView.Add(minimap);
				
				toolbarView = new CustomToolbarView(graphView, minimap);
				graphView.Add(toolbarView);
				
				var grid = new GridBackground();
				graphView.Insert(0, grid);
				grid.StretchToParentSize();
			}

			rootView.Add(graphView);
		}
		
		private void AddInfo()
		{
			if(titleLabel == null)
			{
				var root = new VisualElement();
				root.name = "StateMachineInfo";
				titleLabel = new Label();
				titleLabel.AddToClassList("StateMachineTitle");
				pathLabel = new Label();
				pathLabel.AddToClassList("StateMachinePath");
				root.Add(titleLabel);
				root.Add(pathLabel);
				graphView.Insert(1, root);
				root.pickingMode = PickingMode.Ignore;
				root.StretchToParentSize();
			}
			titleLabel.text = StateMachineAsset.Active ? StateMachineAsset.Active.name : "State Machine";
			pathLabel.text = StateMachineAsset.Active ? AssetDatabase.GetAssetPath(StateMachineAsset.Active) : string.Empty;
		}

		protected override void InitializeGraphView(BaseGraphView view)
		{
			toolbarView.UpdateButtonStatus();
		}
	}
}
