using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Editor.VisualScripting
{
    /*[CustomEditor(typeof(Trigger), true)]
    public class CustomTriggerEditor : TriggerEditor
    {
        UnityEditor.Editor defaultEditor;
        private VisualElement body2;
        
        private new void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector
            defaultEditor =  UnityEditor.Editor.CreateEditor(targets, typeof(TriggerEditor));
            // transform = target as Transform;
            Debug.Log($"CustomTriggerEditor OnEnable");
        }
 
        private new void OnDisable(){
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
                disableMethod.Invoke(defaultEditor,null);
            DestroyImmediate(defaultEditor);
            Debug.Log($"CustomTriggerEditor OnDisable");

        }

        private void Awake()
        {
            Debug.Log($"CustomTriggerEditor Awake");

        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            body2 = new VisualElement();
            body2.Add(new Toggle("Network Settings"));
            root.Add(body2);
            Debug.Log($"CustomTriggerEditor");
            return root;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Debug.Log($"OnInspectorGUI");

        }
    }*/
}