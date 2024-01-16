using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Euler State Machine Variable")]
    [Category("Variables/Euler State Machine Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the euler rotation value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationEulerStateMachine : PropertyTypeGetRotation
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueVector3.TYPE_ID);

        public override Quaternion Get(Args args)
        {
            return Quaternion.Euler(m_Variable.Get<Vector3>(args));
        }

        public override string String => m_Variable.ToString();
    }
}