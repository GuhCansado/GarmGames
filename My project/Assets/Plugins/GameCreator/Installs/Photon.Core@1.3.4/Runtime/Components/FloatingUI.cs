using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NinjutsuGames.Photon.Runtime
{
    [AddComponentMenu("")]
    public class FloatingUI : MonoBehaviour
    {
        public Text Text => _text;
        public TextMeshProUGUI Tmp => _tmp;
        
        public bool IsTMP { get; private set; }

        private Text _text;
        private TextMeshProUGUI _tmp;

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
            _tmp = GetComponentInChildren<TextMeshProUGUI>();
            
            if (_text == null && _tmp == null)
            {
                Debug.LogError("FloatingTextUI needs a Text or TextMeshProUGUI component");
                return;
            }
            
            IsTMP = _tmp != null;
        }
    }
}