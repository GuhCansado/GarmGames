using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Serializable]
    public class CollectorNameVariable
    {
        private enum Type
        {
            LocalNameVariables,
            GlobalNameVariables
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private Type nameVariable = Type.LocalNameVariables;

        [SerializeField] private LocalNameVariables localNameVariables;
        [SerializeField] private GlobalNameVariables globalNameVariables;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Object Object => nameVariable == Type.GlobalNameVariables ? globalNameVariables : localNameVariables;

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public object Get(string variable)
        {
            switch (nameVariable)
            {
                case Type.LocalNameVariables:
                    if (localNameVariables != null)
                    {
                        return localNameVariables.Get(variable);
                    }
                    break;
                    
                case Type.GlobalNameVariables:
                    if (globalNameVariables != null)
                    {
                        return globalNameVariables.Get(variable);
                    }
                    break;
            }
            return null;
        }
        
        public void Set(string name, object value)
        {
            switch (nameVariable)
            {
                case Type.LocalNameVariables:
                    if (localNameVariables != null)
                    {
                        localNameVariables.Set(name, value);
                    }
                    break;
                    
                case Type.GlobalNameVariables:
                    if (globalNameVariables != null)
                    {
                        globalNameVariables.Set(name, value);
                    }
                    break;
            }
        }
        
        public bool Exists(string variable)
        {
            switch (nameVariable)
            {
                case Type.LocalNameVariables:
                    if (localNameVariables != null)
                    {
                        return localNameVariables.Exists(variable);
                    }
                    break;
                    
                case Type.GlobalNameVariables:
                    if (globalNameVariables != null)
                    {
                        return globalNameVariables.Exists(variable);
                    }
                    break;
            }
            return false;
        }

        // OVERRIDES: -----------------------------------------------------------------------------

        public override string ToString()
        {
            return nameVariable switch
            {
                Type.LocalNameVariables => localNameVariables != null
                    ? localNameVariables.gameObject.name
                    : "(none)",
                
                Type.GlobalNameVariables => globalNameVariables != null 
                    ? globalNameVariables.name 
                    : "(none)",
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public object[] ToObjectArray()
        {
            if(globalNameVariables == null && localNameVariables == null)
                return Array.Empty<object>();
            
            return nameVariable == Type.GlobalNameVariables ? globalNameVariables.ToObjectArray() : localNameVariables.ToObjectArray();
        }
    }
}