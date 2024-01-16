using UnityEngine;
using System;

namespace NinjutsuGames.StateMachine.Runtime
{
    /// <summary>
    /// Element that overlays the graph like the blackboard
    /// </summary>
    [Serializable]
    public class PinnedElement
    {
        public static readonly Vector2 defaultSize = new(150, 200);

        public Rect position = new(Vector2.zero, defaultSize);
        public bool opened = true;
        public SerializableType editorType;

        public PinnedElement(Type editorType)
        {
            this.editorType = new SerializableType(editorType);
        }
    }
}