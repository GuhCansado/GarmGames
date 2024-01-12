using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class FieldGetPhotonLocalModelsList : TFieldGetVariable
    {
        [SerializeField]
        protected PhotonLocalModelsList m_ModelsList;

        [SerializeReference]
        protected TListGetPick m_Select = new GetPickFirst();

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public FieldGetPhotonLocalModelsList(IdString typeID)
        {
            m_TypeID = typeID;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override object Get(Args args)
        {
            return m_ModelsList != null ? m_ModelsList.Get(m_Select, args) : null;
        }

        public override string ToString() => m_ModelsList != null
            ? $"{m_ModelsList.name}[{m_Select}]"
            : "(none)";
    }
}