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
    /// Participating a slot. The respective behavior for value/urgency oriented behaviors.
    /// </summary>
    public class SatisfyUrgentNeed : Action
    {
        public SatisfyUrgentNeed(NEEDSIMNode agent)
            : base(agent)
        { }

        public override string Name
        {
            get
            {
                return "SatisfyUrgentNeed";
            }
        }

        /// <summary>
        /// Participating a slot. The respective behavior for value/urgency oriented behaviors.
        /// </summary>
        /// <returns>TODO</returns>
        public override Action.Result Run()
        {
            if (agent.Blackboard.currentState == Blackboard.AgentState.ExitNEEDSIMBehaviors)
            {
                //Actions should be interrupted until the agent state is dealt with.
                agent.Blackboard.activeSlot.AgentDeparture();
                return Result.Failure;
            }

            if (agent.Blackboard.activeSlot.SlotState == Simulation.Slot.SlotStates.Blocked)
            {
                agent.Blackboard.activeSlot.AgentDeparture();
                return Result.Failure;
            }

            //Check whether the current interaction is still running. This is in a seperate block as it 
            // might be edited for some usage scenarios.
            bool interactionStillRunning = false;
            if (!agent.AffordanceTreeNode.Parent.Affordance.InteractionStartedThisFrame
                && agent.AffordanceTreeNode.Parent.Affordance.CurrentInteraction != null)
            {
                interactionStillRunning = true;
                agent.Blackboard.LastInteractionSatiesfiedUrgentNeed = agent.AffordanceTreeNode.CurrentInteractionSatisfiesMostUrgentNeed;
            }

            if (!interactionStillRunning)
            {              
                //If the agent has a new most urgent need it is time to go satisfy it
                if (!agent.Blackboard.LastInteractionSatiesfiedUrgentNeed)
                {
                    agent.Blackboard.activeSlot.AgentDeparture();
                    agent.Blackboard.currentState = Blackboard.AgentState.PonderingNextAction;

                    return Result.Success;
                }
                //The following causes the agent to stay at the slot until another need becomes most urgent.
                else {
                    Simulation.Slot.Result result = 
                        agent.AffordanceTreeNode.Parent.Affordance.ProlongLastInteraction();
                    if (result != Simulation.Slot.Result.Success)
                    {
                        agent.AffordanceTreeNode.Parent.Affordance.StartRandomInteraction();
                    }
                    return Result.Running;                  
                }
            }

            //Participate in current interaction or start a new one
            if (agent.AffordanceTreeNode.Parent.Affordance.CurrentInteraction != null)
            {
                agent.AffordanceTreeNode.ApplyParentInteraction();
                agent.Blackboard.currentState = Blackboard.AgentState.ParticipatingSlot;
            }
            else
            {
                agent.AffordanceTreeNode.Parent.Affordance.StartRandomInteraction();
            }

            return Result.Running;
        }
    }
}
