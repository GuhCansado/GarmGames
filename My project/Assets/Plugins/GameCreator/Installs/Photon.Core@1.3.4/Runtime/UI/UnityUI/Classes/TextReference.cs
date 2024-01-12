using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class TextReference
    {
        private enum Type
        {
            Text = 0,
            TMP = 1
        }

        private const int MAX_CHARACTER_COUNT = 99999;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private Type m_Type = Type.Text;
        [SerializeField] private Text m_Text;
        [SerializeField] private TMP_Text m_TMP;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] private string m_Value;
        [NonSerialized] private int m_CharactersVisible = MAX_CHARACTER_COUNT;
        
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

        public int CharactersVisible
        {
            get => m_CharactersVisible;
            set
            {
                m_CharactersVisible = value;
                Refresh();
            }
        }
        
        public GameObject GameObject =>
            m_Type switch
            {
                Type.Text => m_Text.gameObject,
                Type.TMP => m_TMP.gameObject,
                _ => throw new ArgumentOutOfRangeException()
            };

        public Color Color
        {
            get => m_Type switch
            {
                Type.Text => m_Text.color,
                Type.TMP => m_TMP.color,
                _ => throw new ArgumentOutOfRangeException()
            };
            set
            {
                switch (m_Type)
                {
                    case Type.Text: m_Text.color = value; break;
                    case Type.TMP: m_TMP.color = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool AreAllCharactersVisible => m_Value.Length <= CharactersVisible;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public TextReference()
        {
            m_Value = Text;
        }

        public TextReference(Text text) : this()
        {
            m_Type = Type.Text;
            m_Text = text;
        }

        public TextReference(TMP_Text text)
        {
            m_Type = Type.TMP;
            m_TMP = text;
        }

        // TO STRING: -----------------------------------------------------------------------------

        public override string ToString()
        {
            return m_Type switch
            {
                Type.Text => m_Text != null ? m_Text.gameObject.name : "(none)",
                Type.TMP => m_TMP != null ? m_TMP.gameObject.name : "(none)",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Refresh()
        {
            switch (m_Type)
            {
                case Type.Text:
                    if (m_Text == null) return;
                    int count = Math.Min(m_Value.Length, CharactersVisible);
                    m_Text.text = m_Value[..count];
                    break;
                
                case Type.TMP:
                    if (m_TMP == null) return;
                    m_TMP.text = m_Value;
                    m_TMP.maxVisibleCharacters = CharactersVisible;
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}