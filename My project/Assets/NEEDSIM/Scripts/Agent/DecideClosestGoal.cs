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

namespace NEEDSIM
{
    /// <summary>
    /// This is an alternative to the Action DecideGoal. It shows how to replace the default evaluation with a custom method by using a callback.
    /// 
    /// For goal oriented behaviors: Get a goal from the simulation, and try to get a slot where the goal can be satisfied.
    /// </summary>
    public class DecideClosestGoal : Action
    {
        public DecideClosestGoal(NEEDSIMNode agent)
            : base(agent)
        { }

        public override string Name
        {
            get
            {
                return "DecideClosestGoal";
            }
        }

        /// <summary>
        /// Get a goal from the simulation, and try to get a slot where the goal can be satisfied.
        /// </summary>
        /// <returns>Success if a slot has been distributed to the agent.</returns>
        public override Action.Result Run()
        {
            if (agent.Blackboard.currentState == Blackboard.AgentState.ExitNEEDSIMBehaviors)
            {
                //Actions should be interrupted until the agent state is dealt with.
                return Result.Failure;
            }

            //Get the goal to satisfy the need with the lowest satisfaction. You can replace the goals for your specific game.
            agent.AffordanceTreeNode.Goal = agent.AffordanceTreeNode.SatisfactionLevels.GoalToSatisfyLowestNeed();

            //If previously a slot was allocated to this agent, try to consume/use it.
            if (agent.Blackboard.currentState == Blackboard.AgentState.WaitingForSlot)
            {
                if (agent.AcceptSlot())
                {
                    return Result.Success;
                }
                else
                {
                    agent.Blackboard.currentState = Blackboard.AgentState.PonderingNextAction;
                }
            }

            //Try to allocate a slot to the agent that will satisfy the goal.
            // Simple solution
            // Simulation.Bidding.Result biddingResult = Simulation.Bidding.GoalOrientedBid(agent.AffordanceTreeNode);
            // You can instead use callbacks to adjust evaluataion of the slots to your game.
            Simulation.Bidding.Result biddingResult = Simulation.Bidding.GoalOrientedBid(agent.AffordanceTreeNode, preferShortestDistanceByAir);

            if (biddingResult == Simulation.Bidding.Result.Success)
            {
                agent.Blackboard.currentState = Blackboard.AgentState.WaitingForSlot;
                return Result.Running;
            }

            agent.Blackboard.currentState = Blackboard.AgentState.None;
            return Result.Failure;
        }

        public float preferShortestDistanceByAir(Simulation.AffordanceTreeNode caller, Simulation.Affordance good, string needName)
        {
            float distance = Vector3.Distance(caller.gameObject.transform.position, good.gameObject.transform.position);

            const float maxDistance = 3;

            return (maxDistance - distance);
        }
    }
}
