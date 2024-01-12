using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Comparers
{
    [Serializable]
    public class CompareBoolean
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
        private PropertyGetBool m_CompareTo = new(true);

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public CompareBoolean()
        { }
        
        public CompareBoolean(bool value) : this(GetBoolValue.Create(value))
        { }

        public CompareBoolean(PropertyGetBool boolean) : this()
        {
            m_CompareTo = boolean;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(bool value, Args args)
        {
            var b = m_CompareTo.Get(args);

            return m_Comparison switch
            {
                Comparison.Equals => value == b,
                Comparison.Different => value != b,
                _ => throw new ArgumentOutOfRangeException($"Boolean Comparison '{m_Comparison}' not found")
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