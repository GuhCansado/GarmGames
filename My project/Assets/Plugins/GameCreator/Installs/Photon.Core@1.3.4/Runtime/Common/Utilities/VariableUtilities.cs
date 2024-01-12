using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Variables;
using NinjutsuGames.Photon.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    public static class VariableUtilities
    {
        public static Hashtable ToHashtable(this GlobalNameVariables variables)
        {
            int max = variables.Names.Length;
            Hashtable table = new Hashtable();
            for (int i = 0; i < max; i++)
            {
                var name = variables.Names[i];
                var data = variables.Get(name);
                if (data.IsAllowedType())
                {
                    table.Add(name, data);
                }
                else
                {
                    Debug.LogWarning($"Data type: {data.GetType()} is not allowed in Photon.");
                }
            }
            return table;
        }
        
        public static void SetHashtableValues(this GlobalNameVariables variables, Hashtable hashtable)
        {
            foreach (var entry in hashtable)
            {
                if (variables.Exists(entry.Key.ToString()))
                {
                    variables.Set(entry.Key.ToString(), entry.Value);
                }
                // else
                // {
                //     Debug.LogWarning($"Could not found key: {entry.Key} in LocalNameVariables: {variables}", variables);
                // }
            }
        }
        
        public static void SetHashtableValues(this LocalNameVariables variables, Hashtable hashtable)
        {
            foreach (var entry in hashtable)
            {
                if (variables.Exists(entry.Key.ToString()))
                {
                    variables.Set(entry.Key.ToString(), entry.Value);
                }
                // else
                // {
                //     Debug.LogWarning($"Could not found key: {entry.Key} in LocalNameVariables: {variables}", variables);
                // }
            }
        }
        
        public static void SetHashtableValues(this CollectorNameVariable variables, Hashtable hashtable)
        {
            foreach (var entry in hashtable)
            {
                if (variables.Exists(entry.Key.ToString()))
                {
                    variables.Set(entry.Key.ToString(), entry.Value);
                }
                // else
                // {
                //     Debug.LogWarning($"Could not found key: {entry.Key} in CollectorNameVariable: {variables.Object}", variables.Object); 
                // }
            }
        }
        
        /*public static object[] ToObjectArray(this CollectorNameVariable variables)
        {
            int max = variables.Names.Length;
            var table = new List<object>();
            for (int i = 0; i < max; i++)
            {
                var name = variables.Names[i];
                var data = variables.Get(name);
                if (data.IsType<int>() || data.IsType<float>() || data.IsType<bool>()  || data.IsType<Color>() || data.IsType<string>() || data.IsType<Vector3>())
                {
                    table.Add(data);
                }
                else
                {
                    Debug.LogWarning($"Data type: {data.GetType()} is not allowed in Photon.");
                }
            }
            return table.ToArray();
        }*/
        
        public static bool IsAllowedType(this object data)
        {
            return data.IsType<int>() || data.IsType<double>() || data.IsType<float>() || data.IsType<bool>()  || data.IsType<Color>() || data.IsType<string>() || data.IsType<Vector3>();
        }
        
        public static object[] ToObjectArray(this GlobalNameVariables variables)
        {
            int max = variables.Names.Length;
            var table = new List<object>();
            for (int i = 0; i < max; i++)
            {
                var name = variables.Names[i];
                var data = variables.Get(name);
                if (data.IsAllowedType())
                {
                    table.Add(name);
                    table.Add(data);
                }
                else
                {
                    Debug.LogWarning($"Data type: {data.GetType()} is not allowed in Photon.");
                }
            }
            return table.ToArray();
        }
        
        public static object[] ToObjectArray(this LocalNameVariables variables)
        {
            var table = new List<object>();
            var vars = variables.GetRuntimeVariables();
            var enumerator = vars.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var data = enumerator.Current;
                if (data.Value.IsAllowedType())
                {
                    table.Add(data.Name);
                    table.Add(data.Value);
                }
                else
                {
                    Debug.LogWarning($"Data type: {data.GetType()} is not allowed in Photon.");
                }
            }
            return table.ToArray();
        }
        
        public static string[] ToAllowedKeys(this GlobalNameVariables variables)
        {
            List<string> keys = new List<string>();
            int max = variables.Names.Length;
            for (int i = 0; i < max; i++)
            {
                var name = variables.Names[i];
                var data = variables.Get(name);
                if (data.IsAllowedType())
                {
                    keys.Add(name);
                }
                else
                {
                    Debug.LogWarning($"Data type: {data.GetType()} is not allowed in Photon.");
                }
            }
            return keys.ToArray();
        }
    }
}