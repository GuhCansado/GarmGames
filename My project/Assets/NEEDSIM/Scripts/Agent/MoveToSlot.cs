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
    /// Moving to a slot. The best implementation for such a behavior might be different in your project, but this script offers a starting point.
    /// </summary>
    public class MoveToSlot : Action
    {
        public MoveToSlot(NEEDSIMNode agent)
            : base(agent)
        { }

        public override string Name
        {
            get
            {
                return "MoveToSlot";
            }
        }

        /// <summary>
        /// Go to the slot that has been given to the agent.
        /// </summary>
        /// <returns>Running as long as on the way. Success upon arrival.</returns>
        public override Action.Result Run()
        {
            if (agent.Blackboard.currentState == Blackboard.AgentState.ExitNEEDSIMBehaviors)
            {
                //Actions should be interrupted until the agent state is dealt with.
                return Result.Failure;
            }

            if (agent.Blackboard.activeSlot.SlotState == Simulation.Slot.SlotStates.Blocked)
            {
                agent.Blackboard.activeSlot.AgentDeparture();
                agent.Blackboard.currentState = Blackboard.AgentState.PonderingNextAction;
                return Action.Result.Failure;
            }

            if (agent.Blackboard.currentState == Blackboard.AgentState.MovingToSlot)
            {
                //Determining whether an agent has arrived at a slot is game specific. Here are two checks that might
                //be a helpful starting point, but you might have to adjust them to your navigation solution and game world.
                if (agent.Blackboard.HasArrivedAtSlot())
                {
                    if (!agent.Blackboard.slotToAgentDistanceSmall(agent.transform.position))
                    {
                        agent.Blackboard.activeSlot.AgentDeparture();
                        agent.Blackboard.currentState = Blackboard.AgentState.PonderingNextAction;
                        return Result.Failure;
                    }
                    if (agent.ArrivalAtSlot(agent.Blackboard.activeSlot))
                    {
                        return Action.Result.Success;
                    }
                    else 
                    {
                        agent.Blackboard.activeSlot.AgentDeparture();
                        agent.Blackboard.currentState = Blackboard.AgentState.PonderingNextAction;
                        return Action.Result.Failure;
                    }
                }
                return Action.Result.Running;
            }
            return Action.Result.Failure;
        }
    }
}
