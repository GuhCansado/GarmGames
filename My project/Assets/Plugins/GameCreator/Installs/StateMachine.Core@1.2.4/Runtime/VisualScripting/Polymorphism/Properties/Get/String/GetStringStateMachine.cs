using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Variable")]
    [Category("Variables/State Machine Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the string value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringStateMachine : PropertyTypeGetString
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueString.TYPE_ID);

        public override string Get(Args args)
        {
            return m_Variable.Get<string>(args);
        }
        public override string Get(GameObject gameObject)
        {
            return m_Variable.Get<string>(new Args(gameObject));
        }

        public static PropertyGetString Create => new(
            new GetStringStateMachine()
        );

        public override string String => m_Variable.ToString();
    }
}