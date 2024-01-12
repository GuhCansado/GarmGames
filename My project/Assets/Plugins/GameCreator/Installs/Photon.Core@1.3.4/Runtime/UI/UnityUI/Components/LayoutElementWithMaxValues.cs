namespace NinjutsuGames.Photon.Runtime
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.UI;
#endif

    [RequireComponent(typeof(RectTransform))]
    [System.Serializable]
    public class LayoutElementWithMaxValues : LayoutElement
    {
        public float maxHeight;
        public float maxWidth;

        public bool useMaxWidth;
        public bool useMaxHeight;

        private bool _ignoreOnGettingPreferredSize;

        public override int layoutPriority
        {
            get => _ignoreOnGettingPreferredSize ? -1 : base.layoutPriority;
            set => base.layoutPriority = value;
        }

        public override float preferredHeight
        {
            get
            {
                if (useMaxHeight)
                {
                    var defaultIgnoreValue = _ignoreOnGettingPreferredSize;
                    _ignoreOnGettingPreferredSize = true;

                    var baseValue = LayoutUtility.GetPreferredHeight(transform as RectTransform);

                    _ignoreOnGettingPreferredSize = defaultIgnoreValue;

                    return baseValue > maxHeight ? maxHeight : baseValue;
                }
                else
                    return base.preferredHeight;
            }
            set => base.preferredHeight = value;
        }

        public override float preferredWidth
        {
            get
            {
                if (useMaxWidth)
                {
                    var defaultIgnoreValue = _ignoreOnGettingPreferredSize;
                    _ignoreOnGettingPreferredSize = true;

                    var baseValue = LayoutUtility.GetPreferredWidth(transform as RectTransform);

                    _ignoreOnGettingPreferredSize = defaultIgnoreValue;

                    return baseValue > maxWidth ? maxWidth : baseValue;
                }
                else
                    return base.preferredWidth;
            }
            set => base.preferredWidth = value;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LayoutElementWithMaxValues), true)]
    [CanEditMultipleObjects]
    public class LayoutMaxSizeEditor : LayoutElementEditor
    {
        private LayoutElementWithMaxValues _layoutMax;

        private SerializedProperty _maxHeightProperty;
        private SerializedProperty _maxWidthProperty;

        private SerializedProperty _useMaxHeightProperty;
        private SerializedProperty _useMaxWidthProperty;

        private RectTransform _myRectTransform;

        protected override void OnEnable()
        {
            base.OnEnable();

            _layoutMax = target as LayoutElementWithMaxValues;
            _myRectTransform = _layoutMax.transform as RectTransform;

            _maxHeightProperty = serializedObject.FindProperty(nameof(_layoutMax.maxHeight));
            _maxWidthProperty = serializedObject.FindProperty(nameof(_layoutMax.maxWidth));

            _useMaxHeightProperty = serializedObject.FindProperty(nameof(_layoutMax.useMaxHeight));
            _useMaxWidthProperty = serializedObject.FindProperty(nameof(_layoutMax.useMaxWidth));
        }

        public override void OnInspectorGUI()
        {
            Draw(_maxWidthProperty, _useMaxWidthProperty);
            Draw(_maxHeightProperty, _useMaxHeightProperty);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }

        private void Draw(SerializedProperty property, SerializedProperty useProperty)
        {
            var position = EditorGUILayout.GetControlRect();

            var label = EditorGUI.BeginProperty(position, null, property);

            var fieldPosition = EditorGUI.PrefixLabel(position, label);

            var toggleRect = fieldPosition;
            toggleRect.width = 16;

            var floatFieldRect = fieldPosition;
            floatFieldRect.xMin += 16;


            var use = EditorGUI.Toggle(toggleRect, useProperty.boolValue);
            useProperty.boolValue = use;

            if (use)
            {
                EditorGUIUtility.labelWidth = 4;
                property.floatValue = EditorGUI.FloatField(floatFieldRect, new GUIContent(" "), property.floatValue);
                EditorGUIUtility.labelWidth = 0;
            }


            EditorGUI.EndProperty();
        }
    }

#endif
}