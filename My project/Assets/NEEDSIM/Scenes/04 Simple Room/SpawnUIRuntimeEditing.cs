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
using System.Collections.Generic;

namespace NEEDSIMSampleSceneScripts
{
    /// <summary>
    /// Spawns a UI Element for each need and each satisfaction rate of an interaction.
    /// </summary>
    public class SpawnUIRuntimeEditing : MonoBehaviour
    {
        public GameObject UIElemetPrefab;

        public GameObject CanvasContainingObject;

        private GameObject[] NeedUIElements;
        private GameObject[] InteractionUIElements;

        private const string descriptionTag = "Description";
        private const string valueFieldTag = "Value";

        // Use this for initialization
        void Start()
        {
            NeedUIElements = new GameObject[Simulation.Manager.Instance.Data.NeedNames.Count];
           
            #region Determine length of InteractionUIElements[]
            int interactionUIElementsCount = 0;
            foreach (KeyValuePair<string, Simulation.Interaction> interaction in Simulation.Manager.Instance.Data.InteractionByNameDictionary)
            {
                foreach (KeyValuePair<string, float> satisfactionRate in interaction.Value.SatisfactionRates)
                {
                    if (satisfactionRate.Value != 0)
                    {
                        interactionUIElementsCount++;
                    }
                }
            }
            #endregion
            InteractionUIElements = new GameObject[interactionUIElementsCount];

            #region Spawn an InputFieldRuntimeEditing element for each need.
            int i = 0;
            foreach (string need in Simulation.Manager.Instance.Data.NeedNames)
            {
                NeedUIElements[i] = GameObject.Instantiate(UIElemetPrefab);
                NeedUIElements[i].transform.SetParent(CanvasContainingObject.transform, false);
                NeedUIElements[i].transform.Translate(0, -1 * NeedUIElements[i].GetComponent<RectTransform>().rect.height * i, 0);

                InputFieldRuntimeEditing inputField = NeedUIElements[i].GetComponent<InputFieldRuntimeEditing>();
                inputField.valueType = InputFieldRuntimeEditing.TypeOfValue.NeedChangeRate;

                UnityEngine.UI.Text[] textFields = NeedUIElements[i].GetComponentsInChildren<UnityEngine.UI.Text>();
                for (int j = 0; j < textFields.Length; j++)
                {
                    if (textFields[j].text == descriptionTag )
                    {
                        textFields[j].text = need + " change per second";
                        inputField.needName = need;
                    }
                    if (textFields[j].text == valueFieldTag)
                    {
                        textFields[j].text = Simulation.Manager.Instance.Data.ChangePerSecond[need].ToString();
                        inputField.value = Simulation.Manager.Instance.Data.ChangePerSecond[need];
                    }
                }
                i++;
            }
            #endregion

            #region Spawn an InputFieldRuntimeEditing for each satisfaction rate of each interaction
            i = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, Simulation.Interaction> interaction in Simulation.Manager.Instance.Data.InteractionByNameDictionary)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, float> satisfactionRate in interaction.Value.SatisfactionRates)
                {
                    if (satisfactionRate.Value != 0)
                    {
                        InteractionUIElements[i] = GameObject.Instantiate(UIElemetPrefab);
                        InteractionUIElements[i].transform.SetParent(CanvasContainingObject.transform, false);
                        InteractionUIElements[i].transform.Translate(InteractionUIElements[i].GetComponent<RectTransform>().rect.width + 35, -1 * InteractionUIElements[i].GetComponent<RectTransform>().rect.height * i, 0);

                        InputFieldRuntimeEditing inputField = InteractionUIElements[i].GetComponent<InputFieldRuntimeEditing>();
                        inputField.valueType = InputFieldRuntimeEditing.TypeOfValue.InteractionSatisfactionRate;

                        UnityEngine.UI.Text[] textFields = InteractionUIElements[i].GetComponentsInChildren<UnityEngine.UI.Text>();
                        for (int j = 0; j < textFields.Length; j++)
                        {
                            if (textFields[j].text == descriptionTag)
                            {
                                textFields[j].text = interaction.Key + " changes " + satisfactionRate.Key;
                                inputField.interaction = interaction.Key;
                                inputField.interactionNeed = satisfactionRate.Key;
                            }
                            if (textFields[j].text == valueFieldTag)
                            {
                                textFields[j].text = satisfactionRate.Value.ToString();
                                inputField.value = satisfactionRate.Value;
                            }
                        }
                        i++;
                    }
                }

            }
            #endregion
        }
    }
}
