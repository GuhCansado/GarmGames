using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Managers
{
    [Icon(RuntimePaths.GIZMOS + "GizmoHotspot.png")]
    [Serializable]
    [AddComponentMenu("Game Creator/Photon/Interactive Registry")]
    public class PhotonInteractiveRegistry : MonoBehaviour
    {
        public static PhotonInteractiveRegistry Instance { get; private set; }
        [SerializeField] private List<GameObject> interactiveInstances = new();
        
        private void Awake() 
        {
            Instance = this;
            // FindInteractiveInstances();
        }
        
        public void FindInteractiveInstances()
        {
            ClearNullInstances();
            var hotspots = FindObjectsOfType<Hotspot>();
            foreach (var hotspot in hotspots)
            {
                if(!interactiveInstances.Contains(hotspot.gameObject)) interactiveInstances.Add(hotspot.gameObject);
            }
        }

        private void OnValidate()
        {
            ClearNullInstances();
        }

        private void ClearNullInstances()
        {
            interactiveInstances.RemoveAll(item => item == null);
        }

        public int GetInstanceIndex(GameObject interactive)
        {
            return interactiveInstances.IndexOf(interactive);
        }
        
        public GameObject GetInstance(int index)
        {
            if (index == -1) return null;
            return index <= interactiveInstances.Count ? interactiveInstances[index] : null;
        }
    }
}