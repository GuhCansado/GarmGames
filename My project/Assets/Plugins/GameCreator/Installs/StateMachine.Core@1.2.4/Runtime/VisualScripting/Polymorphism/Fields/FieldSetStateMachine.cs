using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime.Variables
{
    [Serializable]
    public class FieldSetStateMachine : TFieldSetVariable
    {
        [SerializeField] protected StateMachineAsset m_Variable;
        [SerializeField] protected IdPathString m_Name;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public FieldSetStateMachine(IdString typeID)
        {
            m_TypeID = typeID;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void Set(object value, Args args)
        {
            if (!m_Variable) return;
            m_Variable.Set(m_Name.String, value);
        }

        public override object Get(Args args)
        {
            return m_Variable ? m_Variable.Get(m_Name.String) : null;
        }

        public override string ToString()
        {
            return $"{(m_Variable != null ? m_Variable.name : "(none)")}{(string.IsNullOrEmpty(m_Name.String) ? string.Empty : $"[{m_Name.String}]")}";
        }
    }
}