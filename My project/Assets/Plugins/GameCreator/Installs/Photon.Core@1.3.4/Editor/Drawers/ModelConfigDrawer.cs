using GameCreator.Editor.Common;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor
{
    [CustomPropertyDrawer(typeof(ModelConfig))]

    public class ModelConfigDrawer : PropertyDrawer
    {
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var name = property.FindPropertyRelative("name");
            var sprite = property.FindPropertyRelative("sprite");
            var prefab = property.FindPropertyRelative("prefab");
            var skeleton = property.FindPropertyRelative("skeleton");
            var materialSounds = property.FindPropertyRelative("materialSounds");
            var offset = property.FindPropertyRelative("offset");
            
            root.Add(new PropertyField(name));

            var prefabField = new PropertyField(prefab, "");
            root.Add(prefabField);
            root.Add(new PropertyField(sprite));
            root.Add(new PropertyField(skeleton));
            root.Add(new PropertyField(materialSounds));
            root.Add(new PropertyField(offset));

            return root;
        }
    }
}