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
    [Description("Returns the Sprite value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetSpriteStateMachine : PropertyTypeGetSprite
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueSprite.TYPE_ID);

        public override Sprite Get(Args args) => m_Variable.Get<Sprite>(args);
        public override Sprite Get(GameObject gameObject) => m_Variable.Get<Sprite>(new Args(gameObject));

        public override string String => m_Variable.ToString();
    }
}