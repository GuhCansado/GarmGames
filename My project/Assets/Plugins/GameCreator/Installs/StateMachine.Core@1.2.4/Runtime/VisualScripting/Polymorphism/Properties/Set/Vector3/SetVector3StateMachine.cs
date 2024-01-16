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
    
    [Description("Sets the Vector3 value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable] [HideLabelsInEditor]
    public class SetVector3StateMachine : PropertyTypeSetVector3
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueVector3.TYPE_ID);

        public override void Set(Vector3 value, Args args) => m_Variable.Set(value, args);
        public override Vector3 Get(Args args) => (Vector3) m_Variable.Get(args);
        public static PropertySetVector3 Create => new(
            new SetVector3StateMachine()
        );
        
        public override string String => m_Variable.ToString();
    }
}