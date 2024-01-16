using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Variable")]
    [Category("Variables/State Machine Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the boolean value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetBoolStateMachine : PropertyTypeGetBool
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueBool.TYPE_ID);

        public override bool Get(Args args) => m_Variable.Get<bool>(args);
        public override bool Get(GameObject gameObject) => m_Variable.Get<bool>(new Args(gameObject));

        public override string String => m_Variable.ToString();
    }
}