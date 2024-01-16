using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Runner Variable")]
    [Category("Variables/State Machine Runner Variable")]
    
    [Description("Sets the numeric value of a State Machine Runner Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]

    [Serializable] [HideLabelsInEditor]
    public class SetNumberStateMachineRunner : PropertyTypeSetNumber
    {
        [SerializeField]
        protected FieldSetStateMachineRunner m_Variable = new(ValueNumber.TYPE_ID);

        public override void Set(double value, Args args) => m_Variable.Set(value, args);
        public override double Get(Args args) => (double) m_Variable.Get( args);
        public static PropertySetNumber Create => new(
            new SetNumberStateMachineRunner()
        );
        
        public override string String => m_Variable.ToString();
    }
}