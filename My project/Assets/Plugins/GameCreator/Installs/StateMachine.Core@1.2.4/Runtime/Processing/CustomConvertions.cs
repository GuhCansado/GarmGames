using System;
using System.Collections.Generic;
using GraphProcessor;

namespace NinjutsuGames.StateMachine.Runtime
{
    public class CustomConvertions : ITypeAdapter
    {
        public static EntryPort EntryFromAction(ActionPortIn from) => new();
        public static EntryPort EntryFromCondition(ConditionsPort from) => new();
        public static EntryPort EntryFromSm(StateMachinePort from) => new();
        public static EntryPort EntryFromSm(BranchPortIn from) => new();

        public static ActionPortIn ActionFromEntry(EntryPort from) => new();
        public static ActionPortIn ActionFromConditions(ConditionsPortOutFail from) => new();
        public static ActionPortIn ActionFromConditions(ConditionsPortOutSuccess from) => new();
        public static ActionPortIn ActionFromConditions(BranchPortOut from) => new();
        
        public static ActionPortIn ActionFromOut(ActionPortOut from) => new();
        public static ActionPortOut ActionFromIn(ActionPortIn from) => new();
        public static ActionPortOut ActionOutFromConditions(ConditionsPort from) => new();
        public static ActionPortOut ActionOutFromSm(StateMachinePort from) => new();
        public static ActionPortOut ActionOutFromSm(BranchPortIn from) => new();
        
        public static ConditionsPort ConditionFromEntry(EntryPort from) => new();
        public static ConditionsPort ConditionFromActionOut(ActionPortOut from) => new();
        public static ConditionsPort ConditionsOutFromSm(StateMachinePort from) => new();
        public static ConditionsPort ConditionsOutFromSm(ConditionsPortOutFail from) => new();
        public static ConditionsPort ConditionsOutFromSm(ConditionsPortOutSuccess from) => new();
        public static ConditionsPort ConditionsOutFromSm(BranchPortIn from) => new();
        public static ConditionsPort ConditionsOutFromSm(BranchPortOut from) => new();
        
        public static ConditionsPortOutFail ConditionFromFail(ActionPortIn from) => new();
        public static ConditionsPortOutFail ConditionFromFail(StateMachinePort from) => new();
        public static ConditionsPortOutFail ConditionFromFail(ConditionsPort from) => new();
        public static ConditionsPortOutFail ConditionFromFail(BranchPortIn from) => new();
        
        public static ConditionsPortOutSuccess ConditionFromSuccess(ActionPortIn from) => new();
        public static ConditionsPortOutSuccess ConditionFromSuccess(StateMachinePort from) => new();
        public static ConditionsPortOutSuccess ConditionFromSuccess(ConditionsPort from) => new();
        public static ConditionsPortOutSuccess ConditionFromSuccess(BranchPortIn from) => new();

        
        public static TriggerPortIn TriggerFromOut(TriggerPortOut from) => new();
        public static TriggerPortIn TriggerFromOut(StateMachinePort from) => new();
        // public static TriggerPortIn TriggerFromOut(BranchPortIn from) => new();
        // public static TriggerPortIn TriggerFromOut(BranchPortOut from) => new();
        
        public static TriggerPortOut TriggerFromIn(TriggerPortIn from) => new();
        public static TriggerPortOut TriggerFromSm(StateMachinePort from) => new();
        public static TriggerPortOut TriggerFromSm(BranchPortIn from) => new();
        
        public static StateMachinePort SmFromTrigger(TriggerPortIn from) => new();
        public static StateMachinePort SmFromTrigger(TriggerPortOut from) => new();
        public static StateMachinePort SmFromActions(ActionPortOut from) => new();
        public static StateMachinePort SmFromConditions(ConditionsPort from) => new();
        public static StateMachinePort SmFromConditions(ConditionsPortOutFail from) => new();
        public static StateMachinePort SmFromConditions(ConditionsPortOutSuccess from) => new();
        public static StateMachinePort SmFromEntry(EntryPort from) => new();
        public static StateMachinePort SmFromEntry(BranchPortOut from) => new();
        
        // public static BranchPortIn BranchFrom(TriggerPortIn from) => new();
        public static BranchPortIn BranchFrom(TriggerPortOut from) => new();
        public static BranchPortIn BranchFrom(ActionPortOut from) => new();
        public static BranchPortIn BranchFrom(ConditionsPort from) => new();
        public static BranchPortIn BranchFrom(ConditionsPortOutFail from) => new();
        public static BranchPortIn BranchFrom(ConditionsPortOutSuccess from) => new();
        public static BranchPortIn BranchFrom(EntryPort from) => new();
        public static BranchPortIn BranchFrom(BranchPortOut from) => new();
        
        // public static BranchPortOut BranchTo(TriggerPortIn from) => new();
        public static BranchPortOut BranchTo(BranchPortIn from) => new();
        public static BranchPortOut BranchTo(ActionPortIn from) => new();
        public static BranchPortOut BranchTo(ConditionsPort from) => new();
        public static BranchPortOut BranchTo(StateMachinePort from) => new();

        public override IEnumerable<(Type, Type)> GetIncompatibleTypes()
        {
            yield return (typeof(ActionPortOut), typeof(object));
            yield return (typeof(RelayNode.PackedRelayData), typeof(object));
        }
    }
}