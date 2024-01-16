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
    /// This class stores the values that the NEEDSIMROOT will use for running the simulation
    /// </summary>
    [System.Serializable]
    public class NEEDSIMManager : MonoBehaviour
    {
        public bool buildAffordanceTreeFromScene = true; // Whether to build the Affordance Tree, the data structure for the simulation, upon awake.
        public bool LogSimulation = true; // Set this to false or strip out the code if you need every tiny bit of performance
        public bool PrintSimulationDebugLog = false; // Can be activated in the inspector via a button
        public string databaseName = Simulation.Strings.DefaultDatabaseName; //This database will be loaded
        public bool reportDetailedInformation = true; //This is not yet shown in the Inspector, as we did not want to clutter the UI.

        void Awake()
        {
            NEEDSIMRoot.Instance.processScene();
        }

        public static void PrintSimulationDebugLogToConsole()
        {
            NEEDSIMRoot.Instance.PrintSimulationDebugLogToConsole();
        }
    }

}
