using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Runner Variable")]
    [Category("Variables/State Machine Runner Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]
    [Description("Returns the boolean value of a State Machine Runner Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetBoolStateMachineRunner : PropertyTypeGetBool
    {
        [SerializeField]
        protected FieldGetStateMachineRunner m_Variable = new(ValueBool.TYPE_ID);

        public override bool Get(Args args) => m_Variable.Get<bool>(args);

        public override string String => m_Variable.ToString();
    }
}