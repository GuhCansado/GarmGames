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
    /// This is an example for how a chasing behavior could be implemented in NEEDSIM Life simulation. For a specific game better solutions might be desirable.
    /// </summary>
    public class ChaseSlot : Action
    {
        public ChaseSlot(NEEDSIMNode agent)
                : base(agent)
        {
            fox = agent.gameObject.GetComponent<NEEDSIMSampleSceneScripts.Fox>();
        }

        public override string Name
        {
            get
            {
                return "ChaseSlot";
            }
        }

        private NEEDSIMSampleSceneScripts.Fox fox;

        /// <summary>
        /// Go to the slot that has been given to the agent.
        /// </summary>
        /// <returns>Running as long as on the way. Success upon arrival.</returns>
        public override Action.Result Run()
        {         
            if (agent.AffordanceTreeNode.Goal.NeedToSatisfy == "Hunger")
            {
                fox.isRunning = true; //Let the fox run to his prey...
            }
            else
            {
                fox.isRunning = false; //... or walk to other need satisfactions.
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
                        agent.Blackboard.ResetNavMeshDestination();

                        return Action.Result.Running;
                    }

                    if (agent.ArrivalAtSlot(agent.Blackboard.activeSlot))
                    {
                        return Action.Result.Success;
                    }
                    else
                    {
                        return Action.Result.Failure;
                    }
                }
                return Action.Result.Running;
            }
            return Action.Result.Failure;
        }
    }
}
