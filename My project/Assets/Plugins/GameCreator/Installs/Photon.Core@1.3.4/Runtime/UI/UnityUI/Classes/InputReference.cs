using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class InputReference
    {
        private enum Type
        {
            Unity = 0,
            TMP = 1
        }

        // private const int MAX_CHARACTER_COUNT = 99999;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Type m_Type = Type.Unity;
        [SerializeField] private InputField m_Text;
        [SerializeField] private TMP_InputField m_TMP;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private string m_Value;
        // [NonSerialized] private int m_CharactersVisible = MAX_CHARACTER_COUNT;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public string Text
        {
            get => m_Value;
            set
            {
                m_Value = value;
                Refresh();
            }
        }
        
        public GameObject GameObject => m_Type switch
        {
            Type.Unity => m_Text.gameObject,
            Type.TMP => m_TMP.gameObject,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        public bool Interactable
        {
            get =>
                m_Type switch
                {
                    Type.Unity => m_Text.interactable,
                    Type.TMP => m_TMP.interactable,
                    _ => throw new ArgumentOutOfRangeException()
                };
            set
            {
                switch (m_Type)
                {
                    case Type.Unity:
                        m_Text.interactable = value;
                        break;
                    case Type.TMP:
                        m_TMP.interactable = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Component component => m_Type switch
        {
            Type.Unity => m_Text,
            Type.TMP => m_TMP,
            _ => throw new ArgumentOutOfRangeException()
        };

        public Color Color
        {
            get => m_Type switch
            {
                Type.Unity => m_Text.textComponent.color,
                Type.TMP => m_TMP.textComponent.color,
                _ => throw new ArgumentOutOfRangeException()
            };
            set
            {
                switch (m_Type)
                {
                    case Type.Unity: m_Text.textComponent.color = value; break;
                    case Type.TMP: m_TMP.textComponent.color = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        // public bool AreAllCharactersVisible => this.m_Value.Length <= this.CharactersVisible;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public InputReference()
        {
            m_Value = Text;
        }

        public InputReference(InputField text) : this()
        {
            m_Type = Type.Unity;
            m_Text = text;
        }

        public InputReference(TMP_InputField text)
        {
            m_Type = Type.TMP;
            m_TMP = text;
        }

        public void SubscribeOnValueChanged(UnityAction<string> onValueChanged)
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.onValueChanged.AddListener(onValueChanged); break;
                case Type.TMP: m_TMP.onValueChanged.AddListener(onValueChanged); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        public void UnsubscribeOnValueChanged(UnityAction<string> onValueChanged)
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.onValueChanged.RemoveListener(onValueChanged); break;
                case Type.TMP: m_TMP.onValueChanged.RemoveListener(onValueChanged); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        public void SubscribeOnSubmit(UnityAction<string> onSubmit)
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.onSubmit.AddListener(onSubmit); break;
                case Type.TMP: m_TMP.onSubmit.AddListener(onSubmit); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        public void UnsubscribeOnSubmit(UnityAction<string> onSubmit)
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.onSubmit.RemoveListener(onSubmit); break;
                case Type.TMP: m_TMP.onSubmit.RemoveListener(onSubmit); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        public void SubscribeOnEndEdit(UnityAction<string> onEndEdit)
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.onEndEdit.AddListener(onEndEdit); break;
                case Type.TMP: m_TMP.onEndEdit.AddListener(onEndEdit); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        
        public void UnsubscribeOnEndEdit(UnityAction<string> onEndEdit)
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.onEndEdit.RemoveListener(onEndEdit); break;
                case Type.TMP: m_TMP.onEndEdit.RemoveListener(onEndEdit); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        // TO STRING: -----------------------------------------------------------------------------

        public override string ToString()
        {
            return m_Type switch
            {
                Type.Unity => m_Text != null ? m_Text.gameObject.name : "(none)",
                Type.TMP => m_TMP != null ? m_TMP.gameObject.name : "(none)",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Refresh()
        {
            switch (m_Type)
            {
                case Type.Unity:
                    if (m_Text == null) return;
                    // int count = Math.Min(this.m_Value.Length, this.CharactersVisible);
                    m_Text.text = m_Value;
                    break;
                
                case Type.TMP:
                    if (m_TMP == null) return;
                    m_TMP.text = m_Value;
                    // this.m_TMP.maxVisibleCharacters = this.CharactersVisible;
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void DeactivateInputField()
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.DeactivateInputField(); break;
                case Type.TMP: m_TMP.DeactivateInputField(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void ActivateInputField()
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.ActivateInputField(); break;
                case Type.TMP: m_TMP.ActivateInputField(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void Select()
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.Select(); break;
                case Type.TMP: m_TMP.Select(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            switch (m_Type)
            {
                case Type.Unity: m_Text.OnPointerClick(pointerEventData); break;
                case Type.TMP: m_TMP.OnPointerClick(pointerEventData); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}