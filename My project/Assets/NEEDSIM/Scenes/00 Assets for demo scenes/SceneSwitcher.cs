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
using UnityEngine.SceneManagement;

namespace NEEDSIMSampleSceneScripts
{
    /// <summary>
    /// public methods to switch scenes via a button click. You have to add the scenes to your build settings to use the prefab that uses this script.
    /// </summary>
    public class SceneSwitcher : MonoBehaviour
    {
        string[] levelNames = { "01 Naturleben", "02 Hasenjagd", "03 Fuchsalarm", "04 Simple Room", "05 Spawn Beds", "06 Simple Time"  };

        public void loadLevel00()
        {
            SceneManager.LoadScene(levelNames[0]);
        }

        public void loadLevel01()
        {
            SceneManager.LoadScene(levelNames[1]);
        }

        public void loadLevel02()
        {
            SceneManager.LoadScene(levelNames[2]);
        }

        public void loadLevel03()
        {
            SceneManager.LoadScene(levelNames[3]);
        }

        public void loadLevel04()
        {
            SceneManager.LoadScene(levelNames[4]);
        }

        public void loadLevel05()
        {
            SceneManager.LoadScene(levelNames[5]);
        }
    }
}
