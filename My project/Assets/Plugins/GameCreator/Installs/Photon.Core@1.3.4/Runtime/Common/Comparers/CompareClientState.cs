using System;
using GameCreator.Runtime.Common;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Comparers
{
    [Serializable]
    public class CompareClientState
    {
        private enum Comparison
        {
            Equals,
            Different
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] 
        private Comparison m_Comparison = Comparison.Equals;
        
        [SerializeField] 
        private ClientState m_CompareTo = ClientState.Disconnected;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public CompareClientState()
        { }

        public CompareClientState(ClientState state) : this()
        {
            m_CompareTo = state;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(ClientState value)
        {
            var b = m_CompareTo;

            return m_Comparison switch
            {
                Comparison.Equals => value == b,
                Comparison.Different => value != b,
                _ => throw new ArgumentOutOfRangeException($"ClientState Comparison '{m_Comparison}' not found")
            };
        }

        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            var operation = m_Comparison switch
            {
                Comparison.Equals => "=",
                Comparison.Different => "≠",
                _ => string.Empty
            };
            
            return $"{operation} {m_CompareTo}";
        }
    }
}