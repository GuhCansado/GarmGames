using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine List Variable")]
    [Category("Variables/State Machine List Variable")]
    
    [Image(typeof(IconStateMachineOverlayYellow), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Game Object value of a State Machine Runner List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectStateMachineList : PropertyTypeGetGameObject
    {
        [SerializeField] protected FieldGetStateMachineList m_Variable = new(ValueLocalList.TYPE_ID); 

        public override GameObject Get(Args args) => this.m_Variable.Get<GameObject>(args);

        public override string String => this.m_Variable.ToString();
    }
}