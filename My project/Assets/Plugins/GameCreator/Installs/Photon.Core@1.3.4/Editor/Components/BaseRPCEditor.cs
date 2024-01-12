using NinjutsuGames.Photon.Runtime.Components;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Components
{
    public abstract class BaseRPCEditor : UnityEditor.Editor
    {
        private static readonly StyleLength DefaultMarginTop = new(5);
        private const string STATE_DESCRIPTION = "If true when new player joins the room, it will run this RPC on the new player";
        private VisualScriptingNetwork _visualScriptingNetwork;

        // MEMBERS: -------------------------------------------------------------------------------
        
        // INITIALIZERS: --------------------------------------------------------------------------

        private void OnValidate()
        {
            var t = (BaseRPC) target;
            UnityEditor.EditorApplication.delayCall += Validate;
        }

        private void Validate()
        {
            UnityEditor.EditorApplication.delayCall -= Validate;
            var t = (BaseRPC) target;
            t.CheckNetwork();
        }

        private void OnDestroy()
        {
            var t = (BaseRPC) target;
            t.CleanUp(false);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            container.style.marginTop = DefaultMarginTop;

            container.Add(new PropertyField(serializedObject.FindProperty("rpcTarget")));

            var prop = new PropertyField(serializedObject.FindProperty("sendStateToNewPlayers"))
            {
                tooltip = STATE_DESCRIPTION
            };
            container.Add(prop);
            return container;
        }
    }
}