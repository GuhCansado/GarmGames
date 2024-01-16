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
    /// This action demonstrates how interruption of typical NEEDSIM behaviors could look like.
    /// </summary>
    public class InterruptionFuchsalarm : Action
    {
        NEEDSIMSampleSceneScripts.FuchsalarmDemoScript scriptReference;
        bool movementStarted;

        public InterruptionFuchsalarm(NEEDSIMNode agent)
            : base(agent)
        {
            scriptReference = GameObject.FindObjectOfType<NEEDSIMSampleSceneScripts.FuchsalarmDemoScript>();
        }

        public override string Name
        {
            get
            {
                return "InterruptionFuchsalarm";
            }
        }

        /// <summary>
        /// Satisfying a goal at an AffordanceTree node.
        /// </summary>
        /// <returns>Success if need satsifaction goal was achieved, running whilst it is being satisfied. </returns>
        public override Action.Result Run()
        {
            if (scriptReference == null)
            {
                Debug.LogError("This action was specifically made to show interuptible behavior in the Nacht des Fuchses example scene.");
                return Result.Failure;
            }

            //If the fox has been survived it is time to go back to other plans
            if (!scriptReference.FoxAlive)
            {
                movementStarted = false;
                return Result.Success;
            }
            //if the agent is not yet running he/she should hurry to the safe zone
            if (!movementStarted)
            {
                agent.gameObject.GetComponentInChildren<Animator>().SetTrigger("Movement");

                agent.Blackboard.NavMeshAgent.SetDestination(scriptReference.SafeZone);
                movementStarted = true;
            }
           
            return Result.Running;
        }
    }
}
