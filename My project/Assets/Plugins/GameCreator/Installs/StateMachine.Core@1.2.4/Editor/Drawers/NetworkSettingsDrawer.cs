using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    [CustomPropertyDrawer(typeof(NetworkingSettings), false)]
    public class NetworkSettingsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement
            {
                name = "NetworkSettings",
                style =
                {
                    display = DisplayStyle.None
                }
            };
            return root;
        }
    }
}