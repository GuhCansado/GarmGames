/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Simulation;

namespace NEEDSIM
{
    /// <summary>
    /// Every scene should have one root node for the AFFORDANCE TREE. This uses the settings of the NEEDSIM Manager and controls the simulation.
    /// </summary>
    public class NEEDSIMRoot : NEEDSIM3rdParty.Singleton<NEEDSIMRoot>
    {
        #region valuesFromUI
        private bool buildAffordanceTreeFromScene; // Whether to build the Affordance Tree, the data structure for the simulation, upon starting.
        private bool LogSimulation; // Set this to false or strip out the code if you need every tiny bit of performance
        private bool PrintSimulationDebugLog; // Can be activated in the inspector via a button
        public string databaseName { get; private set; } //This database will be loaded
        private bool reportDetailedInformation;
        #endregion

        public NEEDSIMManager configuration { get; private set; }

        public Simulation.SimulationData SimulationData { get; private set; }

        private string SimulationDebugLog { get; set; }

        private Simulation.AffordanceTreeNode root; // Attached to the root node should be all simulated objects
        public Simulation.AffordanceTreeNode Root { get { return root; } private set { root = value; } }

        public bool isSimulationInitialized { get; private set; }

        public List<NEEDSIM.Blackboard> Blackboards { get; private set; }

        private int nextUpdate = 0;

        /// <summary>
        /// guarantee this will be always a singleton only - can't use the constructor!
        /// </summary>
        protected NEEDSIMRoot()
        {
            WriteToSimulationDebugLog(Strings.SimulationManagerConstructed);
        }

        void Awake()
        {
            //This will make sure that the default database is loaded if no NEEDSIM Manager game object was added to the scene
            configuration = (NEEDSIMManager)FindObjectOfType(typeof(NEEDSIMManager));
            if (configuration == null)
            {
                processScene();
            }
        }

        public void processScene()
        {
            configuration = (NEEDSIMManager)FindObjectOfType(typeof(NEEDSIMManager));

            if (configuration == null)
            {
                configuration = gameObject.AddComponent<NEEDSIMManager>();
            }

            buildAffordanceTreeFromScene = configuration.buildAffordanceTreeFromScene;
            LogSimulation = configuration.LogSimulation;
            PrintSimulationDebugLog = configuration.PrintSimulationDebugLog;
            databaseName = configuration.databaseName;
            reportDetailedInformation = configuration.reportDetailedInformation;

            //Start simulation
            if (true)
            {
                Blackboards = new List<Blackboard>();

                isSimulationInitialized = StartSimulation();

                if (buildAffordanceTreeFromScene && isSimulationInitialized)
                {
                    BuildFlatAffordanceTreeFromScene();
                    buildAffordanceTreeFromScene = false;
                }
            }
        }
        
        /// <summary>
        /// Load Database and initialize simulation.
        /// </summary>
        /// <returns>True if both loading and initializing were successful.</returns>
        private bool StartSimulation()
        {
            if (!LoadDatabase())
            {
                Debug.LogError("Can not start simulation due to database loading problem.");
                PrintSimulationDebugLogToConsole();
                return false;
            }
            if (!GameDataManager.InitSimData(databaseName, true))
            {
                Debug.LogError("Can not start simulation due to bad database content.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Try to load database from Resource folder and write results into the debug log.
        /// Will try to load default database if specified database not found.
        /// </summary>
        /// <returns>Whether the desired database was loaded. False, if either none or default database was loaded.</returns>
        private bool LoadDatabase()
        {
            if (databaseName != "" && databaseName != null)
            {
                GameDataManager.Data = Resources.Load(databaseName) as DatabaseAsset;
                WriteToSimulationDebugLog("attached database was: " + GameDataManager.Data.DatabaseName);
                if (GameDataManager.Data != null)
                {
                    WriteToSimulationDebugLog("successfully loaded");
                }
                else
                {
                    WriteToSimulationDebugLog("simulation data source remains null");
                    return false;
                }
            }
            else
            {
                WriteToSimulationDebugLog("Attached Database null, loading default");
                GameDataManager.Data = Resources.Load(Strings.DefaultDatabaseName) as DatabaseAsset;

                return false;
            }
            return true;
        }

        /// <summary>
        /// Goes through the scene and adds all NEEDSIMNodes as children of the root.
        /// This is best if you don't have complexity issues to deal with. Otherwise building 
        /// the affordance tree with more depth  is adviced.
        /// </summary>
        public void BuildFlatAffordanceTreeFromScene()
        {
            WriteToSimulationDebugLog("Affordance Tree build from scene.");

            root = new AffordanceTreeNode(null, "Root", "", Vector3.zero);
            Simulation.Manager.Instance.Data.Root = root;

            NEEDSIMNode[] allSmartObjects = GameObject.FindObjectsOfType<NEEDSIM.NEEDSIMNode>();

            //Add Affordance Tree nodes to every NEEDSIM Node that was found.
            for (int i = 0; i < allSmartObjects.Length; i++)
            {
                AddNEEDSIMNode(allSmartObjects[i]);
            }
        }

        void Update()
        {
            PrintSimulationDebugLog = configuration.PrintSimulationDebugLog;
            if (PrintSimulationDebugLog)
            {
                PrintSimulationDebugLogToConsole();
                PrintSimulationDebugLog = false;
            }

            if (isSimulationInitialized)
            {
                if (root == null)
                {
                    Debug.LogError("Root is zero.");
                }
                else
                {
                    if (nextUpdate == 0)
                    {
                        Manager.Instance.UpdateAffordanceTree();
                        nextUpdate = 0; //If you do not want to update every frame you can put a higher value here. It might be necessary to adjust agent control accordingly.
                    }
                    else
                    {
                        nextUpdate--;
                    }
                }
            }
        }

        /// <summary>
        /// To use this please pass the node you want to use as root node to this method. It is assumed that in the scene hierarchy this node is the root node to all other NEEDSIMNodes. So if you have  a village with houses and objects in each house you should, in the scene view, make the village parent of the houses, and the houses object of the village, and each game object should have a NEEDSIMNode. You can leave the interactions set to none for the village and the houses if they are just used to calculate the abstraction for the Affordance Tree.
        /// The method BuildTreeBasedOnSceneHierarchy() that is called here will not work for intermediate objects in the hierarchy - there is only deeper search if a direct ancestor is a NEEDSIMNode.
        /// </summary>
        /// <param name="node"></param>
        public void BuildAffordanceTreeFromNode(NEEDSIMNode node)
        {
            node.AffordanceTreeNode = new AffordanceTreeNode(null, node.name, node.speciesName, node.gameObject.transform.position);
            root = node.AffordanceTreeNode;
            Simulation.Manager.Instance.Data.Root = root;
            node.BuildTreeBasedOnSceneHierarchy();  
        }

        public void AddNEEDSIMNode(NEEDSIMNode node)
        {
            AddNEEDSIMNode(node, root);
        }

        public void AddNEEDSIMNode(NEEDSIMNode node, NEEDSIMNode parent)
        {
            AddNEEDSIMNode(node, parent.AffordanceTreeNode);
        }

        public void AddNEEDSIMNode(NEEDSIMNode node, AffordanceTreeNode parent)
        {
            node.AffordanceTreeNode = new AffordanceTreeNode(parent, node.name, node.speciesName, node.gameObject.transform.position);
            // In case you do not move NEEDSIMNodes to different gameObjects at runtime you can set the game object here. Otherwise the simplest option is to move this line to NEEDSIMNode.Update()
            node.AffordanceTreeNode.gameObject = node.gameObject;
        }


        public void WriteToSimulationDebugLog(string newContent)
        {
            if (LogSimulation)
            {
                SimulationDebugLog = SimulationDebugLog + System.DateTime.Now.ToString("HH:mm:ss ") + newContent + " | ";
            }
        }

        public void PrintSimulationDebugLogToConsole()
        {
            if (isSimulationInitialized)
            {
                Debug.Log("Affordance Tree: " + root.Printer(reportDetailedInformation) + " - Total children of root: " + root.CountOfChildren());

                if (reportDetailedInformation)
                {
                    string simulationLog = "";
                    while (Simulation.GameDataManager.DebugMessages.Count > 0)
                    {
                        simulationLog += " | " + Simulation.GameDataManager.DebugMessages.Dequeue();
                    }
                    Debug.Log(simulationLog);
                }
            }
            Debug.Log(SimulationDebugLog);
        }
    }
}
