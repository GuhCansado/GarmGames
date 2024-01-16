using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Serializable]
    public abstract class TDetectorNameVariable<T> where T : INameVariable
    {
        private enum Detection
        {
            Any,
            Name
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private T m_Variable;

        [SerializeField] private Detection m_When = Detection.Any;
        [SerializeField] private IdPathString m_Name;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        private int ListenersCount => this.EventOnChange?.GetInvocationList().Length ?? 0;
        
        // EVENTS: --------------------------------------------------------------------------------

        protected event Action<string> EventOnChange; 
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void StartListening(Action<string> callback, GameObject target)
        {
            if (this.m_Variable == null) return;
            if (this.ListenersCount == 0)
            {
                if(target == null) this.m_Variable.Register(this.OnChange);
                else this.m_Variable.Register(this.OnChange, target);
            }
            
            this.EventOnChange += callback;
        }

        public void StopListening(Action<string> callback, GameObject target)
        {
            if (this.m_Variable == null) return;
            if (this.ListenersCount == 1)
            {
                if(target == null) this.m_Variable.Unregister(this.OnChange);
                else this.m_Variable.Unregister(this.OnChange, target);
            }
            
            this.EventOnChange -= callback;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        protected void OnChange(string name)
        {
            if (this.m_When == Detection.Name)
            {
                string[] split = this.m_Name.String.Split('/');
                if (split[^1] != name) return;
            }
            
            this.EventOnChange?.Invoke(name);
        }
    }
}