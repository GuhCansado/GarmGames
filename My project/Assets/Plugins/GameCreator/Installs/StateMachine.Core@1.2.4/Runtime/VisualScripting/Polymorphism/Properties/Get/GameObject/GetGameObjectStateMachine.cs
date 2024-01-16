using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Common;

namespace NinjutsuGames.StateMachine.Runtime.Variables
{
    [Title("State Machine Variable")]
    [Category("Variables/State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the Game Object value of a State Machine Variable")]
    [Serializable]
    [HideLabelsInEditor]
    public class GetGameObjectStateMachine : PropertyTypeGetGameObject
    {
        [SerializeField] protected FieldGetStateMachine m_Variable = new(ValueGameObject.TYPE_ID);

        public override GameObject Get(Args args) 
        {
            return m_Variable.Get<GameObject>(args);
        }

        public override GameObject Get(GameObject gameObject)
        {
            return m_Variable.Get<GameObject>(new Args(gameObject));
        }

        public override string String => m_Variable.ToString();
    }
}