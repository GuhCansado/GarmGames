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
    /// This script shows how the behaviors of the bunnies are interrupted when the fox is spawned.
    /// </summary>
    public class FuchsalarmDemoScript : MonoBehaviour
    {
        [Tooltip("Where the bunnies will run to.")]
        public Vector3 SafeZone;
        [Tooltip("The button will be made inactive by this script when a fox is alive.")]
        public GameObject SpawnFoxButton;

        public bool FoxAlive { get; private set; }

        private int LastPopulationCount; //This will be used to determine whether a bunny died.
        private SimpleSpawn FoxSpawner;

        void Awake()
        {
            FoxSpawner = GetComponent<SimpleSpawn>();
            FoxAlive = false;
        }

        void Start()
        {
            LastPopulationCount = FindObjectsOfType<Bunny>().Length;
        }

        void Update()
        {
            if (FoxAlive)
            {
                //The button should only work whilst there are no foxes, as the max population is 1.
                SpawnFoxButton.GetComponent<UnityEngine.UI.Button>().interactable = false;

                //If a bunny was killed it is time for the fox to disappear again.
                if (LastPopulationCount > FindObjectsOfType<Bunny>().Length)
                {
                    FoxAlive = false;
                    FoxSpawner.killAll();

                    for (int i = 0; i < NEEDSIM.NEEDSIMRoot.Instance.Blackboards.Count; i++)
                    {
                        if (NEEDSIM.NEEDSIMRoot.Instance.Blackboards[i].Species == "Bunny")
                        {
                            NEEDSIM.NEEDSIMRoot.Instance.Blackboards[i].currentState = NEEDSIM.Blackboard.AgentState.None;
                        }
                    }
                    LastPopulationCount = FindObjectsOfType<Bunny>().Length;
                }
            }
            else
            {
                SpawnFoxButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            }
        }

        /// <summary>
        /// Call this to create a fox and interrupt all the bunnies.
        /// </summary>
        public void SpawnTheFox()
        {
            if (FoxSpawner.PopulationCount < 1)
            {
                FoxAlive = true;
                FoxSpawner.fillPopulation();

                // Let all the bunnys know that they should exit their current behavior (due to an emergency).
                for (int i = 0; i < NEEDSIM.NEEDSIMRoot.Instance.Blackboards.Count; i++)
                {
                    if (NEEDSIM.NEEDSIMRoot.Instance.Blackboards[i].Species == "Bunny")
                    {
                        NEEDSIM.NEEDSIMRoot.Instance.Blackboards[i].ExitNEEDSIMBehaviors();
                    }
                }
            }
        }
    }
}
