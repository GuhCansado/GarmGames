using System;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    public interface INameVariable
    {
        void Register(Action<string> callback, GameObject target);
        void Unregister(Action<string> callback, GameObject target);
        
        void Register(Action<string> callback);
        void Unregister(Action<string> callback);
    }
}