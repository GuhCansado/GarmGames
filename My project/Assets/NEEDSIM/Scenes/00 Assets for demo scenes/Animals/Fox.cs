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
    /// Sets the fox to either run or walk
    /// </summary>
    [RequireComponent(typeof(NEEDSIM.NEEDSIMNode))]
    public class Fox : Animal
    {
        public bool isRunning { get; set; }
        [Tooltip("The speed the NavMeshAgent should have when the fox is running.")]
        public float runSpeed;
        [Tooltip("The speed the NavMeshAgent should have when the fox is walking.")]
        public float walkSpeed;

        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        public override void Start()
        {
            animator = gameObject.GetComponentInChildren<Animator>();
            navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            base.Start();
        }

        public override void Update()
        {
            if (isRunning)
            {
                navMeshAgent.speed = runSpeed;
            }
            else
            {
                navMeshAgent.speed = walkSpeed;
            }
            animator.SetBool("RunOrWalk", isRunning);

            base.Update();
        }
    }
}
