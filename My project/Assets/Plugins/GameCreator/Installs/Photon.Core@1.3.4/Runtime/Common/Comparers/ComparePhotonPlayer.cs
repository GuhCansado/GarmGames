using System;
using GameCreator.Runtime.Common;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Comparers
{
    [Serializable]
    public class ComparePhotonPlayer
    {
        private enum Comparison
        {
            Equals,
            Different
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] 
        private PropertyGetString m_Value = new();
        
        [SerializeField] 
        private Comparison m_Comparison = Comparison.Equals;
        
        [SerializeField] 
        private PropertyGetString m_CompareTo = new();

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ComparePhotonPlayer()
        { }
        
        public ComparePhotonPlayer(Player value) : this(GetStringString.Create)
        { }

        public ComparePhotonPlayer(PropertyGetString boolean) : this()
        {
            this.m_CompareTo = boolean;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(Player value, Args args)
        {
            var a = value;
            var b = this.m_CompareTo.Get(args);

            return this.m_Comparison switch
            {
                Comparison.Equals => a.Equals(b),
                Comparison.Different => !a.Equals(b),
                _ => throw new ArgumentOutOfRangeException($"Photon Player Comparison '{this.m_Comparison}' not found")
            };
        }

        // STRING: --------------------------------------------------------------------------------

        public override string ToString()
        {
            string operation = this.m_Comparison switch
            {
                Comparison.Equals => "=",
                Comparison.Different => "≠",
                _ => string.Empty
            };
            
            return $"{operation} {this.m_CompareTo}";
        }
    }
}