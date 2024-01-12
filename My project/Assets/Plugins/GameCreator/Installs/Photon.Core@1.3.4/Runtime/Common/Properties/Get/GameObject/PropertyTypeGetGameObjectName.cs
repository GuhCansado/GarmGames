using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Game Object Name")]

    [Serializable]
    public abstract class PropertyTypeGetGameObjectName : TPropertyTypeGet<string>
    {
        /*public virtual GameObject SceneReference => null;

        public virtual T Get<T>(Args args) where T : string
        {
            string gameObject = this.Get(args);
            return Get(args);
        }

        public virtual T Get<T>(string target) where T : Component
        {
            return this.Get<T>(new Args(target));
        }
        
        public virtual T Get<T>(Component component) where T : Component
        {
            return this.Get<T>(new Args(component));
        }*/
    }
}