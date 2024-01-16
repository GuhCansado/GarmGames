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
    [Description("Returns the Vector3 value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetScaleStateMachine : PropertyTypeGetScale
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueVector3.TYPE_ID);

        public override Vector3 Get(Args args) => m_Variable.Get<Vector3>(args);

        public override string String => m_Variable.ToString();
    }
}