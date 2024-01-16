using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    [CustomPropertyDrawer(typeof(NameList), true)]
    public class RunnerNameListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new RunnerNameListTool(property);
        }
    }
}