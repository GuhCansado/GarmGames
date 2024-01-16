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

namespace NEEDSIMSampleSceneScripts
{
    /// <summary>
    /// This example suggests an idea for playing animations based on the states of NEEDSIMNodes.
    /// </summary>
    [RequireComponent(typeof(NEEDSIM.NEEDSIMNode))]
    public class Animal : MonoBehaviour
    {
        protected Animator animator;
        protected NEEDSIM.NEEDSIMNode needsimNode;

        public virtual void Start()
        {
            animator = GetComponentInChildren<Animator>();
            needsimNode = GetComponent<NEEDSIM.NEEDSIMNode>();
        }

        public virtual void Update()
        {
            if (needsimNode.AnimationsToPlay.Count > 0)
            {
                if (needsimNode.AnimationsToPlay.Peek() == NEEDSIM.NEEDSIMNode.AnimationOrders.MovementStartedByAgent)
                {
                    //Rotate agent into movement direction
                    gameObject.transform.LookAt(needsimNode.GetComponent<UnityEngine.AI.NavMeshAgent>().steeringTarget);
                    gameObject.transform.Rotate(90.0f, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
                }
                else if (needsimNode.AnimationsToPlay.Peek() == NEEDSIM.NEEDSIMNode.AnimationOrders.InteractionStartedByAgent)
                {
                    //Rotate agent towards LookAt 
                    gameObject.transform.LookAt(needsimNode.Blackboard.activeSlot.LookAt);
                }
                //This method will call the SetTrigger method on the animator, thus triggering correctly named transitions into animation states.
                needsimNode.TryConsumingAnimationOrder(animator);
            }
        }
    }
}
