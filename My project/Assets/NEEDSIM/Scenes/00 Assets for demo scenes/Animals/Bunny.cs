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
    /// This example script shows how the bunny can deal with the 'EatBunny' interaction, that can be performed by a fox at the slot provided by a bunny.
    /// </summary>
    [RequireComponent(typeof(NEEDSIM.NEEDSIMNode))]
    public class Bunny : Animal
    {
        bool isBeingEaten = false;
        bool hasBeenBeingEaten = false; //i.e. is totally dead.

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();

            //if the bunny is running interactions it means he is being eaten.
            if (needsimNode.runningInteractions)
            {
                isBeingEaten = true;

                if (!hasBeenBeingEaten)
                {
                    animator.SetTrigger("Die");
                    //GetComponent<UnityEngine.AI.NavMeshAgent>().Stop();
					GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
                    needsimNode.isAgent = false;
                    hasBeenBeingEaten = true;
                }
            }

            //Once the interaction of eating the bunny is not running anymore the bunny can be destroyed.
            if (isBeingEaten && !needsimNode.runningInteractions)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
