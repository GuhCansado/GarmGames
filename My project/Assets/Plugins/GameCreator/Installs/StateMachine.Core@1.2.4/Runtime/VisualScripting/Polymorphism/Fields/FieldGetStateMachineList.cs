using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime.Variables
{
    [Serializable]
    public class FieldGetStateMachineList : TFieldGetVariable
    {
        [SerializeField] protected PropertyGetGameObject m_Runner = GetGameObjectTarget.Create();
        [SerializeField] protected StateMachineAsset m_Variable;
        [SerializeField] protected IdPathString m_Name;
        [SerializeReference] protected TListGetPick m_Select = new GetPickFirst();

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public FieldGetStateMachineList(IdString typeID) //, IdString listID
        {
            m_TypeID = typeID;
            // m_List = new FieldGetLocalList(listID);
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
     
        public override object Get(Args args)
        {
            if (m_Variable == null) return null;
            var list = (LocalListVariables) m_Variable.Get(m_Name.String, m_Runner.Get(args));
            return list.Get(m_Select, args);
        }
        
        public override string ToString() => this.m_Variable != null
            ? $"{m_Name.String}[{this.m_Select}]"
            : "(none)";
    }
}