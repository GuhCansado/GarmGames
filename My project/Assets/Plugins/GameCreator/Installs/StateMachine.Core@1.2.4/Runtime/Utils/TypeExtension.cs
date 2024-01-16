using System;
using System.Linq.Expressions;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    public static class TypeExtension
    {
        public static bool IsReallyAssignableFrom(this Type type, Type otherType)
        {
            if (type.IsAssignableFrom(otherType))
                return true;
            if (otherType.IsAssignableFrom(type))
                return true;

            try
            {
                var v = Expression.Variable(otherType);
                var expr = Expression.Convert(v, type);
                return expr.Method != null && expr.Method.Name != "op_Implicit";
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static T Clone<T>(this T target)
        {
            return (T) JsonUtility.FromJson(JsonUtility.ToJson(target), target.GetType());
        }
    }
}