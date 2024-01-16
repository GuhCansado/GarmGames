using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Common;

namespace NinjutsuGames.StateMachine.Runtime.Variables
{
    [Title("State Machine Runner Variable")]
    [Category("Variables/State Machine Runner Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]
    [Description("Returns the Game Object value of a State Machine Runner Variable")]
    [Serializable]
    [HideLabelsInEditor]
    public class GetGameObjectStateMachineRunner : PropertyTypeGetGameObject
    {
        [SerializeField] protected FieldGetStateMachineRunner m_Variable = new(ValueGameObject.TYPE_ID);

        public override GameObject Get(Args args) 
        {
            return m_Variable.Get<GameObject>(args);
        }

        public override string String => m_Variable.ToString();
    }
}