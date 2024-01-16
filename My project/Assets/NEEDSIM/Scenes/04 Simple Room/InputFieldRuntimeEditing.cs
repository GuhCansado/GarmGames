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
    /// Helper to change need satisfaction rates of interactions and satisfaction change rates of needs at runtime in a UI. It provides a method to react to the user finishing his input to the InputField.
    /// </summary>
    public class InputFieldRuntimeEditing : MonoBehaviour
    {
        public enum TypeOfValue { NeedChangeRate, InteractionSatisfactionRate };
        public TypeOfValue valueType;

        public string needName { get; set; }
        public float value = 0.0f;
        public string interaction { get; set; }
        public string interactionNeed { get; set; }

        public UnityEngine.UI.InputField InputField;

        /// <summary>
        /// Update the value if the user has ended his input.
        /// </summary>
        /// <param name="newValue"></param>
        public void EndInput(string newValue)
        {
            if (newValue == "")
            {
                return; //The user has just been clicking around
            }

            bool didParseWork = false;
            didParseWork = float.TryParse(newValue, out value);

            if (value > 100)
            {
                value = 100; //Explicit clamp to the max meaningful value
            }
            else if (value < -100)
            {
                value = -100; //Explicit clamp to the min meaningful value
            }

            switch (valueType)
            {
                case TypeOfValue.NeedChangeRate:

                    if (didParseWork)
                    {
                        Simulation.Manager.Instance.Data.ChangePerSecond[needName] = value;
                    }
                    break;

                case TypeOfValue.InteractionSatisfactionRate:
                    if (didParseWork)
                    {
                        Simulation.Manager.Instance.Data.InteractionByNameDictionary[interaction].SatisfactionRates[interactionNeed] = value;
                    }
                    break;
            }

            InputField.text = value.ToString();

            if (!didParseWork)
            {
                InputField.text = "No float!";
            }
        }
    }
}
