using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Network Variable")]
    [Category("Photon/Network Variable")]

    [Image(typeof(IconHome), ColorTheme.Type.Green, typeof(OverlayListVariable))]
    [Description("Returns the value of the specified network property.")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonVariable : PropertyTypeGetString
    {
        [SerializeField] protected string content = "Room: [RoomName] Players: [PlayerCount]";
        private bool _needsMonoUpdate;
        private GameObject _gameObject;
        private string _result;

        public override string Get(Args args)
        {
            _gameObject = args.Self;
            UpdateText();
            return _result;
        }

        private void UpdateText()
        {
            _result = PhotonVariableParser.Parse(content, _gameObject, out _needsMonoUpdate);

            /*if (_needsMonoUpdate && !NetworkManager.UpdateCalls.Contains(UpdateText))
            {
                NetworkManager.UpdateCalls.Add(UpdateText);
            }

            if (!_needsMonoUpdate && !NetworkManager.PhotonCalls.Contains(UpdateText))
            {
                NetworkManager.PhotonCalls.Add(UpdateText);
            }*/
        }

        public override string String
        {
            get
            {
                if(!Application.isPlaying) return !string.IsNullOrEmpty(content) ? content : "(none)";
                return !string.IsNullOrEmpty(_result) ? _result : "(none)";
            }
        }
    }
}