using GameCreator.Editor.Common;
using NinjutsuGames.Photon.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(CollectorNameVariable))]
    public class CollectorNameVariableDrawer : PropertyDrawer
    {
        private const string PROP_LIST_VARIABLE = "nameVariable";
        private const string PROP_LOCAL_LIST = "localNameVariables";
        private const string PROP_GLOBAL_LIST = "globalNameVariables";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var head = new VisualElement();
            var body = new VisualElement();
            
            root.Add(head);
            root.Add(body);

            var listVariable = property.FindPropertyRelative(PROP_LIST_VARIABLE);
            var fieldListVariable = new PropertyField(listVariable, property.displayName);

            head.Add(fieldListVariable);
            
            fieldListVariable.RegisterValueChangeCallback(_ =>
            {
                UpdateBody(body, property);
            });

            UpdateBody(body, property);
            return root;
        }

        private void UpdateBody(VisualElement body, SerializedProperty property)
        {
            var listVariable = property.FindPropertyRelative(PROP_LIST_VARIABLE);
            body.Clear();
            
            switch (listVariable.enumValueIndex)
            {
                case 0 : // Local List Variable
                    var localList = property.FindPropertyRelative(PROP_LOCAL_LIST);
                    var fieldLocalList = new PropertyField(localList, " ");
                    fieldLocalList.Bind(property.serializedObject);
                    body.Add(fieldLocalList);
                    break;
                    
                case 1 : // Global List Variable
                    var globalList = property.FindPropertyRelative(PROP_GLOBAL_LIST);
                    var fieldGlobalList = new PropertyField(globalList, " ");
                    fieldGlobalList.Bind(property.serializedObject);
                    body.Add(fieldGlobalList);
                    break;
            }
        }
    }
}