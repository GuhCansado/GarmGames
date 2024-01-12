using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Serializable]
    public class PropertyGetGameObjectName : TPropertyGet<PropertyTypeGetGameObjectName, string>
    {
        public PropertyGetGameObjectName() : base(new GetGameObjectNameInstance())
        { }

        public PropertyGetGameObjectName(PropertyTypeGetGameObjectName defaultType) : base(defaultType)
        { }

        // public T Get<T>(Args args) where T : string
        // {
        //     return m_Property.Get(args);
        // }
        //
        // public T Get<T>(GameObject target) where T : Component
        // {
        //     return this.m_Property.Get<T>(target);
        // }
        //
        // public T Get<T>(Component component) where T : Component
        // {
        //     return this.m_Property.Get<T>(component);
        // }
        
        // EDITOR: --------------------------------------------------------------------------------

        /// <summary>
        /// EDITOR ONLY: This is used by editor scripts that require an optional scene reference,
        /// if the value is not dynamic, but constant. For example, the GetGameObjectInstance.
        /// </summary>
        // public GameObject SceneReference => this.m_Property.SceneReference;
    }
}