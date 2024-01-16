using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("On State Machine Variable Change")]
    [Category("Variables/On State Machine Variable Change")]
    [Description("Executed when the State Machine Variable is modified")]

    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable]
    public class EventOnVariableStateMachineChange : Event
    {
        [SerializeField]
        private DetectorStateMachine m_Variable = new();
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            m_Variable.StartListening(OnChange, null);
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            m_Variable.StopListening(OnChange, null);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void OnChange(string name)
        {
            _ = m_Trigger.Execute(Self);
        }
    }
}