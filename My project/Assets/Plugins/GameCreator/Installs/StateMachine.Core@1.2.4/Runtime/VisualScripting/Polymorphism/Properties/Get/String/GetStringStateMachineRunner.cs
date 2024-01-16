using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Runner Variable")]
    [Category("Variables/State Machine Runner Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]
    [Description("Returns the string value of a State Machine Runner Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringStateMachineRunner : PropertyTypeGetString
    {
        [SerializeField]
        protected FieldGetStateMachineRunner m_Variable = new(ValueString.TYPE_ID);

        public override string Get(Args args)
        {
            return m_Variable.Get<string>(args);
        }

        public static PropertyGetString Create => new(
            new GetStringStateMachine()
        );

        public override string String => m_Variable.ToString();
    }
}