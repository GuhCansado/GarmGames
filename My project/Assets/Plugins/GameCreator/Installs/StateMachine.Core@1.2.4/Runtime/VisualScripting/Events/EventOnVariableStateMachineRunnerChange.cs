using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;
using UnityEngine.Serialization;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("On State Machine Runner Variable Change")]
    [Category("Variables/On State Machine Runner Variable Change")]
    [Description("Executed when the State Machine Runner Variable is modified")]

    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]

    [Serializable]
    public class EventOnVariableStateMachineRunnerChange : Event
    {
        [SerializeField] private DetectorStateMachine m_Variable = new();
        [SerializeField] private PropertyGetGameObject m_Runner = GetGameObjectTarget.Create();
        
        // INITIALIZERS: --------------------------------------------------------------------------x

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            m_Variable.StartListening(OnChange, m_Runner.Get(Self));
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            m_Variable.StopListening(OnChange, m_Runner.Get(Self));
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChange(string name)
        {
            _ = m_Trigger.Execute(Self);
        }
    }
}