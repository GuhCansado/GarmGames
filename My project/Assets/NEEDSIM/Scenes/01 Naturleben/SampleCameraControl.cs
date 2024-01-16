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
    /// A very simple scrolling camera for NEEDSIM Life simulation example scenes.
    /// </summary>
    public class SampleCameraControl : MonoBehaviour
    {
        [Tooltip("Movement speed of scrolling.")]
        public float speed = 0.11f;
        [Tooltip("clamp camera scrolling to e.g. map size, horizontally.")]
        public Vector2 HorizontalMinMax;
        [Tooltip("clamp camera scrolling to e.g. map size, vertically.")]
        public Vector2 VerticalMinMax;

        void Update()
        {
            float horizontalSpeed = Input.GetAxis("Horizontal") * speed;
            float verticalSpeed = Input.GetAxis("Vertical") * speed;
            // Keep the camera within the horizontal bounds.
            if ((transform.position.x <= HorizontalMinMax.x && horizontalSpeed < 0)
            || (transform.position.x >= HorizontalMinMax.y && horizontalSpeed > 0))
            {
                horizontalSpeed = 0.0f;
            }
            //Keep the camera within the vertical bounds.
            if ((transform.position.z <= VerticalMinMax.x && verticalSpeed < 0)
                || (transform.position.z >= VerticalMinMax.y && verticalSpeed > 0))
            {
                verticalSpeed = 0.0f;
            }

            //Translate according to the coordinates in our 2D sample scenes.
            transform.Translate(horizontalSpeed, verticalSpeed, 0.0f);
        }
    }
}
