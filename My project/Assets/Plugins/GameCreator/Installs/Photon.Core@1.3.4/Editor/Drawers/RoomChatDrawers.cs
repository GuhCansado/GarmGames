using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor
{
    /*[CustomPropertyDrawer(typeof(ChatUIComponents))]
    public class ChatUIComponentsDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Components";
    }*/
    
    [CustomPropertyDrawer(typeof(ChatUISettings))]
    public class ChatUISettingsDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Settings";
        
        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            var activateOnInput = new PropertyField(property.FindPropertyRelative("activateOnInput"));
            container.Add(activateOnInput);

            var inputField = new PropertyField(property.FindPropertyRelative("inputTrigger"));
            container.Add(inputField);
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(property.FindPropertyRelative("maxLines")));
            container.Add(new PropertyField(property.FindPropertyRelative("minVisibleLines")));
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(property.FindPropertyRelative("fadeOutStart")));
            container.Add(new PropertyField(property.FindPropertyRelative("fadeOutDuration")));
            container.Add(new PropertyField(property.FindPropertyRelative("backgroundFadeOutDuration")));
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(property.FindPropertyRelative("allowChatFading")));
            container.Add(new PropertyField(property.FindPropertyRelative("disablePlayerWhenTyping")));
            container.Add(new SpaceSmall());
            container.Add(new PropertyField(property.FindPropertyRelative("unseenMessages")));
            
        }
    }
    
    [CustomPropertyDrawer(typeof(ChatColors))]
    public class ChatColorsDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Colors";
    }
    
    [CustomPropertyDrawer(typeof(TextContent))]
    public class TextContentDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Text Content";
    }
    
    [CustomPropertyDrawer(typeof(ChatEvents))]
    public class ChatEventsDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Events";
        
        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            var onOpen = property.FindPropertyRelative("onOpen");
            var onClose = property.FindPropertyRelative("onClose");
            var onMessage = property.FindPropertyRelative("onSendMessage");
            var onReceiveMessage = property.FindPropertyRelative("onReceiveMessage");
            
            container.Add(new LabelTitle("On Open:"));
            container.Add(new SpaceSmallest());
            container.Add(new PropertyField(onOpen));
            
            container.Add(new SpaceSmall());
            container.Add(new LabelTitle("On Close:"));
            container.Add(new SpaceSmallest());
            container.Add(new PropertyField(onClose));
            
            container.Add(new SpaceSmall());
            container.Add(new LabelTitle("On Send Message:"));
            container.Add(new SpaceSmallest());
            container.Add(new PropertyField(onMessage));
            
            container.Add(new SpaceSmall());
            container.Add(new LabelTitle("On Receive Message:"));
            container.Add(new SpaceSmallest());
            container.Add(new PropertyField(onReceiveMessage));
        }
    }
    
    [CustomPropertyDrawer(typeof(FloatingText))]
    public class FloatingMessageDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Settings";
    }
}