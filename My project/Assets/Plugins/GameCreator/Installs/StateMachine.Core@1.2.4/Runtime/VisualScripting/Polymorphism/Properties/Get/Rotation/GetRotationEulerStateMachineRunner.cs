using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Euler State Machine Runner Variable")]
    [Category("Variables/Euler State Machine Runner Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]
    [Description("Returns the euler rotation value of a State Machine Runner Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationEulerStateMachineRunner : PropertyTypeGetRotation
    {
        [SerializeField]
        protected FieldGetStateMachineRunner m_Variable = new(ValueVector3.TYPE_ID);

        public override Quaternion Get(Args args)
        {
            return Quaternion.Euler(m_Variable.Get<Vector3>(args));
        }

        public override string String => m_Variable.ToString();
    }
}