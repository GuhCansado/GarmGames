/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace NEEDSIMSampleSceneScripts
{
    /// <summary>
    /// This class shows bars for need satisfaction. A full bar equals full satisfaction, an empty bar means the need is not satisfied. If the need is currently being satisfied an outline will be added.
    /// </summary>
    public class NeedsUI : MonoBehaviour
    {
        [Tooltip("The canvas that is the parent of the sliders will be used to rotate this UI to the main cameras rotation.")]
        public Canvas Canvas;
        [Tooltip("These sliders will show how satisfied each need is.")]
        public Slider[] Slider;
        [Tooltip("The names of the needs have to match the needs in the database.")]
        public string[] NeedNames;
     
        private Quaternion targetRotation;

        private NEEDSIM.NEEDSIMNode NEEDSIMNode;

        private Outline[] outlines;

        void Start()
        {
            if (Slider.Length != NeedNames.Length)
            {
                Debug.LogError("NeedsUI is not correctly set up.");
            }

            outlines = new Outline[NeedNames.Length];
            NEEDSIMNode = gameObject.GetComponent<NEEDSIM.NEEDSIMNode>();
            targetRotation = Camera.main.transform.rotation;

            for (int i = 0; i < Slider.Length; i++)
            {
                outlines[i] = Slider[i].fillRect.gameObject.AddComponent<Outline>();
                outlines[i].effectDistance = new Vector2(0.2f, -0.2f);
            }
        }

        void Update()
        {
            //Rotate towards main camera
            Canvas.transform.rotation = targetRotation;

            for (int i = 0; i < NeedNames.Length; i++)
            {
                float needSatisfactionValue
                    = 1 - (NEEDSIMNode.AffordanceTreeNode.SatisfactionLevels.GetValue(NeedNames[i]) / 100);
                //Draw an outline around needs that currently are being satisfied.
                if (Slider[i].value > needSatisfactionValue)
                {
                    outlines[i].enabled = true; 
                }
                else
                {
                    outlines[i].enabled = false;
                }
                Slider[i].value = needSatisfactionValue;
            }
        }
    }
}

