using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Variable")]
    [Category("Variables/State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the decimal value of a State Machine Variable")]
    [Serializable]
    [HideLabelsInEditor]
    public class GetDecimalStateMachine : PropertyTypeGetDecimal
    {
        [SerializeField] protected FieldGetStateMachine m_Variable = new(ValueNumber.TYPE_ID);

        public override double Get(Args args) => m_Variable.Get<double>(args);
        public override double Get(GameObject gameObject) => m_Variable.Get<double>(new Args(gameObject));

        public static PropertyGetDecimal Create => new(
            new GetDecimalStateMachine()
        );

        public override string String => m_Variable.ToString();
    }
}