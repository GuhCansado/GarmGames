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
    
    [Description("Sets the Sprite value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable] [HideLabelsInEditor]
    public class SetSpriteStateMachine : PropertyTypeSetSprite
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueSprite.TYPE_ID);

        public override void Set(Sprite value, Args args) => m_Variable.Set(value, args);
        public override Sprite Get(Args args) => m_Variable.Get(args) as Sprite;
        
        public static PropertySetSprite Create => new(
            new SetSpriteStateMachine()
        );
        
        public override string String => m_Variable.ToString();
    }
}