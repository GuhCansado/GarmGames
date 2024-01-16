using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Direction State Machine Runner Variable")]
    [Category("Variables/Direction State Machine Runner Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]
    [Description("Returns the direction vector value of a State Machine Runner Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationDirectionStateMachineRunner : PropertyTypeGetRotation
    {
        [SerializeField]
        protected FieldGetStateMachineRunner m_Variable = new(ValueVector3.TYPE_ID);

        public override Quaternion Get(Args args)
        {
            return Quaternion.LookRotation(m_Variable.Get<Vector3>(args));
        }

        public override string String => m_Variable.ToString();
    }
}