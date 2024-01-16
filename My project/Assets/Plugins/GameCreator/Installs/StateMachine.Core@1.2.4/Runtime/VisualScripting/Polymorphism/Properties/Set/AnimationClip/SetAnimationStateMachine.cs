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

    [Description("Sets the Animation Clip value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable]
    public class SetAnimationStateMachine : PropertyTypeSetAnimation
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueAnimClip.TYPE_ID);

        public override void Set(AnimationClip value, Args args) => m_Variable.Set(value, args);
        public override AnimationClip Get(Args args) => m_Variable.Get(args) as AnimationClip;

        public static PropertySetAnimation Create => new(
            new SetAnimationStateMachine()
        );

        public override string String => m_Variable.ToString();
    }
}
