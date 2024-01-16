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
    [Description("Returns the Color value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorStateMachine : PropertyTypeGetColor
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueColor.TYPE_ID);

        public override Color Get(Args args) => m_Variable.Get<Color>(args);
        public override Color Get(GameObject gameObject) => m_Variable.Get<Color>(new Args(gameObject));

        public override string String => m_Variable.ToString();
    }
}