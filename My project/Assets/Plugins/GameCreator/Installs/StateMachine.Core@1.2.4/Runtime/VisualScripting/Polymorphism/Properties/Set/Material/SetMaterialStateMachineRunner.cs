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

    [Description("Sets the Material value of a State Machine Runner Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]

    [Serializable]
    public class SetMaterialStateMachineRunner : PropertyTypeSetMaterial
    {
        [SerializeField]
        protected FieldSetStateMachineRunner m_Variable = new (ValueMaterial.TYPE_ID);

        public override void Set(Material value, Args args) => m_Variable.Set(value, args);
        public override Material Get(Args args) => m_Variable.Get(args) as Material;

        public static PropertySetMaterial Create => new(
            new SetMaterialStateMachineRunner()
        );

        public override string String => m_Variable.ToString();
    }
}
