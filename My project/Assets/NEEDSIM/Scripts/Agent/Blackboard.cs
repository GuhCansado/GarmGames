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
    /// This is used to store some values, and make some methods available in a place where they can be edited without affecting the other classes.
    /// </summary>
    public class Blackboard
    {
        public enum AgentState { None, Paused, ParticipatingSlot, MovingToSlot, PonderingNextAction, WaitingForSlot, ExitNEEDSIMBehaviors }
        public AgentState currentState { get; set; }

        public Simulation.Slot activeSlot { get; set; } //The slot that the agent is currently participating in

        public string Species { get; private set; }

        public UnityEngine.AI.NavMeshAgent NavMeshAgent { get; set; } // Replace this if not using Unity NavMesh.
        public float smallDistance = 0.25f; //what agents consider to be close - used to refine the behavior of the NavMeshAgent

        public bool LastInteractionSatiesfiedUrgentNeed = false;

        public Blackboard(GameObject gameobject)
        {           
            NEEDSIMRoot.Instance.Blackboards.Add(this); //Register self with NEEDSIM Manager
            currentState = AgentState.None;
            Species = gameobject.GetComponent<NEEDSIMNode>().speciesName;
            NavMeshAgent = gameobject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        /// <summary>
        /// Depending on your game and your NavMesh you might have to change the conditions here.
        /// </summary>
        /// <returns>Whether the NavMeshAgent has arrived at the slot.</returns>
        public bool HasArrivedAtSlot()
        {
            bool result = false;

            if ((NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Whether the slot and the agent are as close as defined in the smallDistance value
        /// </summary>
        /// <param name="agentPosition"></param>
        /// <returns>true if agent is closer than small distance to target</returns>
        public bool slotToAgentDistanceSmall(Vector3 agentPosition)
        {
            Vector2 agentPosition2D = new Vector2(agentPosition.x, agentPosition.z);
            Vector2 slotPosition2D = new Vector2(activeSlot.Position.x, activeSlot.Position.z);

            if (Vector2.Distance(agentPosition2D, slotPosition2D) < smallDistance)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Do the necessary stept to follow up on accepting a slot. Set destination on nav mesh, and put agent into the correct state
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool AcceptSlot(Simulation.Slot slot)
        {
            NavMeshAgent.SetDestination(slot.Position); //This has to be replaced if not using the Unity NavMesh.
            currentState = Blackboard.AgentState.MovingToSlot;
            activeSlot = slot;

            return true;
        }

        public bool ResetNavMeshDestination()
        {
            if (activeSlot == null)
            {
                return false;
            }

            NavMeshAgent.SetDestination(activeSlot.Position); //This has to be replaced if not using the Unity NavMesh.

            return true;
        }

        public void ExitNEEDSIMBehaviors()
        {
            activeSlot.AgentDeparture();
            currentState = AgentState.ExitNEEDSIMBehaviors;
        }
    }
}
