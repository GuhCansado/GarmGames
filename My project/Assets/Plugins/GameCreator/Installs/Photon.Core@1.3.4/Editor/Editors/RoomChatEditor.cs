using GameCreator.Editor.Common;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor
{
    [CustomEditor(typeof(RoomChat))]
    public class RoomChatEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            // var uiComponentsProperty = serializedObject.FindProperty("uiComponents");
            var uiSettingsProperty = serializedObject.FindProperty("uiSettings");
            var notificationsProperty = serializedObject.FindProperty("notifications");
            var eventsProperty = serializedObject.FindProperty("events");
            var colorsProperty = serializedObject.FindProperty("colors");
            
            // root.Add(new PropertyField(uiComponentsProperty));
            root.Add(new PropertyField(serializedObject.FindProperty("prefab")));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(serializedObject.FindProperty("input")));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(serializedObject.FindProperty("background")));
            root.Add(new PropertyField(serializedObject.FindProperty("container")));
            root.Add(new SpaceSmall());
            root.Add(new PropertyField(uiSettingsProperty));
            root.Add(new PropertyField(colorsProperty));
            root.Add(new PropertyField(notificationsProperty));
            root.Add(new PropertyField(eventsProperty));

            
            return root;
        }
    }
}