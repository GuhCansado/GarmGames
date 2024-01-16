using GameCreator.Editor.Common;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    [CustomPropertyDrawer(typeof(StateMachineAssets))]
    public class StateMachineAssetsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var buttonRefresh = new Button(StateMachineAssetsPostProcessor.RefreshVariables)
            {
                text = "Refresh",
                style = { height = 25 }
            };

            root.Add(new SpaceSmall());
            root.Add(buttonRefresh);
            root.Add(new SpaceSmall());

            var nameVariables = property.FindPropertyRelative("m_StateMachineAssets");
            var boxNameVariables = new ContentBox("State Machine Assets", true);

            PaintAssets(nameVariables, boxNameVariables);
            root.Add(boxNameVariables);
            
            StateMachineAssetsPostProcessor.EventRefresh += () =>
            {
                PaintAssets(nameVariables, boxNameVariables);
            };

            return root;
        }

        private void PaintAssets(SerializedProperty property, ContentBox box)
        {
            box.Content.Clear();

            property.serializedObject.Update();

            var itemsCount = property.arraySize;
            for (var i = 0; i < itemsCount; ++i)
            {
                var item = property.GetArrayElementAtIndex(i);
                var itemField = new ObjectField
                {
                    label = string.Empty,
                    value = item.objectReferenceValue
                };

                itemField.SetEnabled(false);
                box.Content.Add(itemField);
                
                if (i < itemsCount - 1)
                {
                    box.Content.Add(new SpaceSmaller());
                }
            }
        }
    }
}