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
    [Description("Returns the Material value of a State Machine Variable")]

    [Serializable]
    public class GetMaterialStateMachine : PropertyTypeGetMaterial
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueMaterial.TYPE_ID);

        public override Material Get(Args args) => m_Variable.Get<Material>(args);

        public override string String => m_Variable.ToString();
    }
}
