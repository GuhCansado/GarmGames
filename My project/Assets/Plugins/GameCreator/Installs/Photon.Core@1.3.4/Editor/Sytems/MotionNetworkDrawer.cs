using NinjutsuGames.Photon.Runtime.Components;
// using NinjutsuGames.Photon.Runtime.Systems;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Sytems
{
    using GameCreator.Editor.Common;
    using GameCreator.Runtime.Characters;
    using UnityEditor;

    /*[CustomPropertyDrawer(typeof(MotionNetwork))]
    public class MotionNetworkDrawer : TBoxDrawer
    {
        private Character _character;
        protected override string Name(SerializedProperty property) => "Network";

        protected override void CreatePropertyContent(VisualElement container, SerializedProperty property)
        {
            base.CreatePropertyContent(container, property);
            if (property.serializedObject.targetObject)
            {
                _character = property.serializedObject.targetObject as Character;
                if (!_character.GetComponent<CharacterNetwork>())
                {
                    _character.gameObject.AddComponent<CharacterNetwork>();
                }
            }
        }
    }*/
}