/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NEEDSIM
{
    /// <summary>
    /// A simple behavior control solution. We tried to write this in a way that makes it easy to use our code samples in 
    /// Finite State Machines, Behavior Trees and Goal-oriented Action Planning. The idea is that you can run our simulation
    /// from within a different solution, for example in case you want to have agents with fighting capabilites.
    /// </summary>
    public class PlanDemo
    {
        Dictionary<string, Action> ActionLibrary;

        Stack<Action> Plan;

        private NEEDSIMNode.ExamplePlans selectedPlan;

        public PlanDemo(NEEDSIMNode agent)
        {
            ActionLibrary = new Dictionary<string, Action>();

            ActionLibrary.Add("DecideGoal", new DecideGoal(agent));
            ActionLibrary.Add("MoveToSlot", new MoveToSlot(agent));
            ActionLibrary.Add("SatisfyGoal", new SatisfyGoal(agent));
            ActionLibrary.Add("DecideValue", new DecideValue(agent));
            ActionLibrary.Add("SatisfyUrgentNeed", new SatisfyUrgentNeed(agent));
            ActionLibrary.Add("ChaseSlot", new ChaseSlot(agent));
            ActionLibrary.Add("InterruptionFuchsalarm", new InterruptionFuchsalarm(agent));
            ActionLibrary.Add("DecideClosestGoal", new DecideClosestGoal(agent));
            ActionLibrary.Add("DecideClosestValue", new DecideClosestValue(agent));

            selectedPlan = agent.selectedPlan;

            Plan = getNewPlan();
        }

        /// <summary>
        /// Update runs the plan.
        /// 
        /// If the currently running action returns, upon evaluation, Result.Running, we keep on running that action.
        /// If Result.Failure is returend we start a new sequence.
        /// If Result.Success is returned we go to the next step in the current sequence, or, if at the last step,
        /// start a new sequence.
        /// </summary>
        public void Update()
        {
            Action.Result result = Plan.Peek().Run();

            switch (result)
            {
                case Action.Result.Running:
                    break;
                case Action.Result.Failure:
                    Plan = getNewPlan();
                    break;
                case Action.Result.Success:
                    Plan.Pop();
                    if (Plan.Count == 0)
                        Plan = getNewPlan();
                    break;
            }
        }

        /// <summary>
        /// A simple sequence of actions is produced.
        /// </summary>
        /// <returns>A simple plan to decide on a goal to satisfy a need, go to a place that satisfies that need, and then
        /// interact with that place until satisfied.</returns>
        private Stack<Action> getNewPlan()
        {
            Stack<Action> plan = new Stack<Action>();

            switch (selectedPlan)
            {
                case (NEEDSIMNode.ExamplePlans.GoalOriented):
                    plan.Push(ActionLibrary["SatisfyGoal"]);
                    plan.Push(ActionLibrary["MoveToSlot"]);
                    plan.Push(ActionLibrary["DecideGoal"]);
                    break;
                case (NEEDSIMNode.ExamplePlans.ValueOriented):
                    plan.Push(ActionLibrary["SatisfyUrgentNeed"]);
                    plan.Push(ActionLibrary["MoveToSlot"]);
                    plan.Push(ActionLibrary["DecideValue"]);
                    break;
                case (NEEDSIMNode.ExamplePlans.GoalOrientedChase):
                    plan.Push(ActionLibrary["SatisfyGoal"]);
                    plan.Push(ActionLibrary["ChaseSlot"]);
                    plan.Push(ActionLibrary["DecideGoal"]);
                    break;
                case (NEEDSIMNode.ExamplePlans.InterruptionFuchsalarm):
                    plan.Push(ActionLibrary["SatisfyUrgentNeed"]);
                    plan.Push(ActionLibrary["MoveToSlot"]);
                    plan.Push(ActionLibrary["DecideValue"]);
                    plan.Push(ActionLibrary["InterruptionFuchsalarm"]);
                    break;
                case NEEDSIMNode.ExamplePlans.ClosestGoalByAir:
                    plan.Push(ActionLibrary["SatisfyGoal"]);
                    plan.Push(ActionLibrary["MoveToSlot"]);
                    plan.Push(ActionLibrary["DecideClosestGoal"]);
                    break;
                case (NEEDSIMNode.ExamplePlans.ClosestValueByAir):
                    plan.Push(ActionLibrary["SatisfyUrgentNeed"]);
                    plan.Push(ActionLibrary["MoveToSlot"]);
                    plan.Push(ActionLibrary["DecideClosestValue"]);
                    break;
                default:
                    Debug.LogWarning("Plan option not handled");
                    break;
            }
            return plan;
        }

        public string printCurrentAction()
        {
            return Plan.Peek().Name;
        }

    }
}
