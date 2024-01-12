using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Serializable]
    public struct RoomPropertyName : ISerializationCallbackReceiver
    {
        [SerializeField] private string m_String;
        [NonSerialized] private PropertyName m_Value;

        // PROPERTIES: ----------------------------------------------------------------------------

        public PropertyName Value
        {
            get
            {
                if (PropertyName.IsNullOrEmpty(this.m_Value))
                {
                    this.m_Value = new PropertyName(this.m_String);
                }
                
                return this.m_Value;
            }
        }

        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            return this.m_String;
        }

        // SERIALIZATION: -------------------------------------------------------------------------

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (AssemblyUtils.IsReloading) return;
            if (string.IsNullOrEmpty(this.m_String)) return;
            this.m_String = TextUtils.ProcessID(this.m_String);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        { }
    }
}