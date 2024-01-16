/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// For the slots delivered with the NEEDSIM Life simulation lake sprites slots are: 
// Slot 1: - 0.10 |   0.62  Look at:  0 | 1
// Slot 2:   0.46 | - 0.76
// Slot 3:   0.46 |   0.86

using UnityEngine;
using System.Collections;

namespace NEEDSIMSampleSceneScripts
{
    /// <summary>
    /// This example suggests an idea for how a variety of animations can be played at an interactive object.
    /// </summary>
    [RequireComponent(typeof(NEEDSIM.NEEDSIMNode))]
    public class Lake : MonoBehaviour
    {
        NEEDSIM.NEEDSIMNode needsimNode;
        public GameObject[] SpritesForSlots; //These sprites have tha animations for displaying water waves when an animal is drinking.
        private Animator[] animators;

        void Start()
        {
            needsimNode = gameObject.GetComponent<NEEDSIM.NEEDSIMNode>();

            animators = new Animator[SpritesForSlots.Length];
            for (int i = 0; i < SpritesForSlots.Length; i++)
            {
                animators[i] = SpritesForSlots[i].GetComponent<Animator>();
            }
        }

        void Update()
        {
            if (needsimNode.runningInteractions)
            {
                int counter = 0;
                foreach (Simulation.Slot slot in needsimNode.AffordanceTreeNode.Slots)
                {
                    //Play the animation if there is a character who is ready for drinking at the lake
                    bool playDrinkAnimation = (!(slot.currentInteraction == null) && slot.currentInteraction.Name == "Drink"
                            && slot.SlotState == Simulation.Slot.SlotStates.ReadyCharacter);

                    animators[counter].SetBool("Drink", playDrinkAnimation);

                    counter++;
                }
            }
            else
            {
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetBool("Drink", false);
                }
            }
        }
    }
}
