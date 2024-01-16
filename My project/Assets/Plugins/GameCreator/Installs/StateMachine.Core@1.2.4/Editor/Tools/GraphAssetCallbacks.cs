using NinjutsuGames.StateMachine.Editor;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class GraphAssetCallbacks
{
    [MenuItem("Assets/Create/Game Creator/State Machine", false, 10)]
    public static void CreateGraphProcessor()
    {
        var graph = ScriptableObject.CreateInstance<StateMachineAsset>();
        ProjectWindowUtil.CreateAsset(graph, "StateMachine.asset");
    }

    [OnOpenAsset(0)]
    public static bool OnBaseGraphOpened(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID) as StateMachineAsset;
        if (asset == null) return false;
        EditorWindow.GetWindow<StateMachineGraphWindow>().InitializeGraph(asset);
        return true;

    }
}