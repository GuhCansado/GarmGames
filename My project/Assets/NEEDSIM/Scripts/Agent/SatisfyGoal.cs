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
    /// Participate in a slot to satisfy a goal.
    /// </summary>
    public class SatisfyGoal : Action
    {
        public SatisfyGoal(NEEDSIMNode agent)
            : base(agent)
        { }

        public override string Name
        {
            get
            {
                return "SatisfyGoal";
            }
        }

        /// <summary>
        /// Satisfying a goal at an AffordanceTree node.
        /// </summary>
        /// <returns>Success if need satsifaction goal was achieved, running whilst it is being satisfied. </returns>
        public override Action.Result Run()
        {
            if (agent.Blackboard.currentState == Blackboard.AgentState.ExitNEEDSIMBehaviors)
            {
                //Actions should be interrupted until the agent state is dealt with.
                return Result.Failure;
            }

            if (agent.Blackboard.activeSlot.SlotState == Simulation.Slot.SlotStates.Blocked)
            {
                return Result.Failure;
            }

            //Check whether the current interaction is still running. This is in a seperate block as it 
            // might be edited for some usage scenarios.
            bool interactionStillRunning = false;
            if (!agent.AffordanceTreeNode.Parent.Affordance.InteractionStartedThisFrame
                && agent.AffordanceTreeNode.Parent.Affordance.CurrentInteraction != null)
            {
                interactionStillRunning = true;
            }

            //If goal is achieved get ready for next action
            if (!interactionStillRunning
                && agent.AffordanceTreeNode.Goal.HasBeenAchieved)
            {
                //If you do not want agents to stay at the same slot until their type of goal is finished you can just go to the else case
                Simulation.Goal newGoal = agent.AffordanceTreeNode.SatisfactionLevels.GoalToSatisfyLowestNeed();
                if (newGoal.NeedToSatisfy == agent.AffordanceTreeNode.Goal.NeedToSatisfy)
                {
                    agent.AffordanceTreeNode.Goal = newGoal;
                }
                else
                {                 
                    agent.Blackboard.activeSlot.AgentDeparture();
                    agent.Blackboard.currentState = Blackboard.AgentState.PonderingNextAction;
                    return Result.Success;
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
