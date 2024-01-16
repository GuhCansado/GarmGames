using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;

namespace NinjutsuGames.StateMachine.Editor
{

    [CustomEditor(typeof(StateMachineAsset), true)]
    public class GraphAssetInspector : GraphInspector
    {
        // protected override void CreateInspector()
        // {
        // }

        /*protected override void CreateInspector()
        {
            base.CreateInspector();

            /*m_Root.Add(new Button(() => EditorWindow.GetWindow<DefaultGraphWindow>().InitializeGraph(target as StateMachineAsset))
            {
                text = "Open base graph window"
            });
            m_Root.Add(new Button(() => EditorWindow.GetWindow<CustomContextMenuGraphWindow>().InitializeGraph(target as StateMachineAsset))
            {
                text = "Open custom context menu graph window"
            });
            m_Root.Add(new Button(() => EditorWindow.GetWindow<CustomToolbarGraphWindow>().InitializeGraph(target as StateMachineAsset))
            {
                text = "Open custom toolbar graph window"
            });
            m_Root.Add(new Button(() => EditorWindow.GetWindow<ExposedPropertiesGraphWindow>().InitializeGraph(target as StateMachineAsset))
            {
                text = "Open exposed properties graph window"
            });#1#
        }*/
    }
}