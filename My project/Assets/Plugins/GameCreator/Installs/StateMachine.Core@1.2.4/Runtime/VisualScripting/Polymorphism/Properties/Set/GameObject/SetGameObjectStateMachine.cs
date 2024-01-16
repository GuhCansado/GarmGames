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
    
    [Description("Sets the Game Object value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable] [HideLabelsInEditor]
    public class SetGameObjectStateMachine : PropertyTypeSetGameObject
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueGameObject.TYPE_ID);

        public override void Set(GameObject value, Args args) => m_Variable.Set(value, args);
        public override GameObject Get(Args args) => m_Variable.Get(args) as GameObject;
        
        public static PropertySetGameObject Create => new(
            new SetGameObjectStateMachine()
        );
        
        public override string String => m_Variable.ToString();
    }
}