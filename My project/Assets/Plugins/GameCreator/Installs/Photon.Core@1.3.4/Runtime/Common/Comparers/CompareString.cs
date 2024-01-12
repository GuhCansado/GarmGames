using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Comparers
{
    [Serializable]
    public class CompareString
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

        public CompareString()
        { }
        
        public CompareString(string value) : this(GetStringString.Create)
        { }

        public CompareString(PropertyGetString boolean) : this()
        {
            this.m_CompareTo = boolean;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(string value, Args args)
        {
            string a = value;
            string b = this.m_CompareTo.Get(args);

            return this.m_Comparison switch
            {
                Comparison.Equals => a.Equals(b),
                Comparison.Different => !a.Equals(b),
                _ => throw new ArgumentOutOfRangeException($"String Comparison '{this.m_Comparison}' not found")
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