using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Runner Variable")]
    [Category("Variables/State Machine Runner Variable")]
    
    [Description("Sets the Sprite value of a State Machine Runner Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]

    [Serializable] [HideLabelsInEditor]
    public class SetSpriteStateMachineRunner : PropertyTypeSetSprite
    {
        [SerializeField]
        protected FieldSetStateMachineRunner m_Variable = new(ValueSprite.TYPE_ID);

        public override void Set(Sprite value, Args args) => m_Variable.Set(value, args);
        public override Sprite Get(Args args) => m_Variable.Get(args) as Sprite;
        public static PropertySetSprite Create => new(
            new SetSpriteStateMachineRunner()
        );
        
        public override string String => m_Variable.ToString();
    }
}