using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime.Variables
{
    [Serializable]
    public class FieldGetStateMachineRunner : TFieldGetVariable
    {
        [SerializeField] protected PropertyGetGameObject m_Runner = GetGameObjectTarget.Create();
        [SerializeField] protected StateMachineAsset m_Variable;
        [SerializeField] protected IdPathString m_Name;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public FieldGetStateMachineRunner(IdString typeID)
        {
            m_TypeID = typeID;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override object Get(Args args)
        {
            return m_Variable ? m_Variable.Get(m_Name.String, m_Runner.Get(args)) : null;
        }

        public override string ToString()
        {
            return $"{(m_Variable != null ? m_Variable.name : "(none)")}{(string.IsNullOrEmpty(m_Name.String) ? string.Empty : $"[{m_Name.String}]")}";
        }
    }
}