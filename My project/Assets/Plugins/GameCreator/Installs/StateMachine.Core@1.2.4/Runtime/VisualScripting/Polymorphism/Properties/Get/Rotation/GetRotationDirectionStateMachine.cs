using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Direction State Machine Variable")]
    [Category("Variables/Direction State Machine Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the direction vector value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationDirectionStateMachine : PropertyTypeGetRotation
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueVector3.TYPE_ID);

        public override Quaternion Get(Args args)
        {
            return Quaternion.LookRotation(m_Variable.Get<Vector3>(args));
        }

        public override string String => m_Variable.ToString();
    }
}