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
    /// This is a spawning example script to maintain populations
    /// </summary>
    public class SimpleSpawn : MonoBehaviour
    {
        [Tooltip("The game object that will be spawned.")]
        public GameObject Prefab;
        [Tooltip("If this is true the population will always be maxed.")]
        public bool KeepAllAlive;
        [Tooltip("The length of this array is the max population number.")]
        public Vector3[] spawnLocations; 

        public int PopulationCount { get; private set; } //How many instances are alive.

        private GameObject[] spawnedObjects;

        void Awake()
        {
            spawnedObjects = new GameObject[spawnLocations.Length];
            for (int i = 0; i < spawnedObjects.Length; i++)
            {
                spawnedObjects[i] = null;
            }
        }

        void Update()
        {
            if (KeepAllAlive)
            {
                fillPopulation();
            }
        }

        /// <summary>
        /// Spawn an instance of the prefab for each spawning point at which the previously (if any) spawned instance is dead (null) 
        /// </summary>
        public void fillPopulation()
        {
            for (int i = 0; i < spawnedObjects.Length; i++)
            {
                if (spawnedObjects[i] == null)
                {
                    spawnedObjects[i] = GameObject.Instantiate(Prefab);
                    PopulationCount++;

                    NEEDSIM.NEEDSIMRoot.Instance.AddNEEDSIMNode(spawnedObjects[i].GetComponent<NEEDSIM.NEEDSIMNode>());

                    spawnedObjects[i].transform.position = spawnLocations[i];
                }
            }
        }

        /// <summary>
        /// Remove all agents from the slots they currently paticipate in and delete them.
        /// </summary>
        public void killAll()
        {
            for (int i = 0; i < spawnedObjects.Length; i++)
            {
                Simulation.Slot activeSlot = spawnedObjects[i].GetComponent<NEEDSIM.NEEDSIMNode>().Blackboard.activeSlot;

                if (activeSlot != null)
                {
                    activeSlot.AgentDeparture();
                }
                else { Debug.Log("Active slot was null"); }

                GameObject.Destroy(spawnedObjects[i]);
                PopulationCount--;
            }
        }
    }
}
