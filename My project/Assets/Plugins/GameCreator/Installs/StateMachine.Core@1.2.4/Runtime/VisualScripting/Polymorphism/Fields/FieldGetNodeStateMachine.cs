using System;
using GameCreator.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.StateMachine.Runtime.Variables
{
    [Serializable]
    public class FieldGetNodeStateMachine
    {
        [SerializeField] protected StateMachineAsset m_StateMachine;
        [SerializeField] protected string m_Name;
        [SerializeField] protected string m_GUID;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public StateMachineAsset GetStateMachine() => m_StateMachine;
        
        public string NodeName => m_Name;
        public string GUID => m_GUID;
        
        public T Get<T>()
        {
            var value = Get();
            return Convert.ChangeType(value, typeof(T)) is T typedValue ? typedValue : default;
        }

        public object Get()
        {
            return m_StateMachine != null ? m_StateMachine.GetNode(m_Name) : null;
        }

        public override string ToString()
        {
            return $"{(string.IsNullOrEmpty(m_Name) ? "(none)" : $"[{m_Name}]")} from {(m_StateMachine != null ? m_StateMachine.name : "(none)")}";
        }
    }
}