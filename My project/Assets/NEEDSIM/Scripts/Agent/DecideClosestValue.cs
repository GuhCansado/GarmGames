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
    public class DecideClosestValue : Action
    {
        public DecideClosestValue(NEEDSIMNode agent)
            : base(agent)
        { }

        public override string Name
        {
            get
            {
                return "DecideClosestValue";
            }
        }

        /// <summary>
        /// Try to get a slot based on utility of all available slots (relative to the current satisfaction level of the needs of the agent).
        /// </summary>
        /// <returns>Success if a slot has been distributed to the agent.</returns>
        public override Action.Result Run()
        {
            if (agent.Blackboard.currentState == Blackboard.AgentState.ExitNEEDSIMBehaviors)
            {
                //Actions should be interrupted until the agent state is dealt with.
                return Result.Failure;
            }

            //If previously a slot was allocated, try to consume it.
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

            //Try to allocate a slot to the agent that has the highest value regardless of state.
            //Simulation.Bidding.Result biddingResult = Simulation.Bidding.ValueOrientedBid(agent.AffordanceTreeNode);
            // Use the example callback
            Simulation.Bidding.Result biddingResult = Simulation.Bidding.ValueOrientedBid(agent.AffordanceTreeNode, preferShortestDistanceByAir);

            //Simulation.Bidding.BidParentGrandparentRoot(agent.AffordanceTreeNode);
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
