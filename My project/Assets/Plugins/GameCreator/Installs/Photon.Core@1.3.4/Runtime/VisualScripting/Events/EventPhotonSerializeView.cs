using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;
using Object = UnityEngine.Object;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Serialize View")]
    [Category("Photon/Core/On Serialize View")]
    [Description("Called by PUN several times per second, so that your script can write and read synchronization data for the PhotonView.")]

    [Image(typeof(IconRefresh), ColorTheme.Type.Green)]

    [Keywords("Player", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonSerializeView : Event
    {
        [SerializeField] private CollectorListVariable listVariables;
        [SerializeField] private CollectorNameVariable nameVariables;
        [SerializeField] private PropertySetBool isWriting = SetBoolNone.Create;
        
        private SerializeViewHelper _helper;
        private List<string> _cachedKeys = new();

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            if(!_helper) _helper = Self.GetComponent<SerializeViewHelper>();
            if(!_helper) _helper = Self.AddComponent<SerializeViewHelper>();
            _cachedKeys.Clear();
            var args = new Args(trigger.gameObject);
            for (int i = 0, imax = listVariables.GetCount(args); i < imax; i++)
            {
                var key = listVariables.Get(args)[i].ToString();
                _cachedKeys.Add(key);
            }
            _helper.OnSerialize += OnSerializeView;
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            if(_helper)
            {
                _helper.OnSerialize -= OnSerializeView;
                Object.Destroy(_helper);
            }
        }
        
        private void OnSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                for (int i = 0, imax = _cachedKeys.Count; i < imax; i++)
                {
                    var key = _cachedKeys[i];
                    stream.SendNext(nameVariables.Get(key));
                }
            }
            else
            {
                for (int i = 0, imax = _cachedKeys.Count; i < imax; i++)
                {
                    var key = _cachedKeys[i];
                    nameVariables.Set(key, stream.ReceiveNext());
                }
            }

            isWriting.Set(stream.IsWriting, Self);
            _ = m_Trigger.Execute(Self);
        }
    }
}
