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
    
    [Description("Sets the Game Object value of a State Machine Runner Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]

    [Serializable] [HideLabelsInEditor]
    public class SetGameObjectStateMachineRunner : PropertyTypeSetGameObject
    {
        [SerializeField]
        protected FieldSetStateMachineRunner m_Variable = new(ValueGameObject.TYPE_ID);

        public override void Set(GameObject value, Args args) => m_Variable.Set(value, args);
        public override GameObject Get(Args args) => m_Variable.Get(args) as GameObject;
        public static PropertySetGameObject Create => new(
            new SetGameObjectStateMachineRunner()
        );
        
        public override string String => m_Variable.ToString();
    }
}