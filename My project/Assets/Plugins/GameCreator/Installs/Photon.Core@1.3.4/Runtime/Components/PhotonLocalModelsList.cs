using System;
using System.Linq;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.Photon.Runtime.Managers;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    [Icon(RuntimePaths.GIZMOS + "GizmoStateLocomotion.png")]
    [Serializable]
    [AddComponentMenu("Game Creator/Photon/Local Models List")] 
    public class PhotonLocalModelsList : MonoBehaviour
    {
        [SerializeReference] private ModelsList models = new();

        public int Count => models.Models.Length;

        private void Start()
        {
            foreach (var config in models.Models.Where(config => config.prefab != null))
            {
                PhotonNetworkManager.RuntimeModels.TryAdd(config.prefab.Get(gameObject).name, config);
            }
        }
        
        public object Get(IListGetPick pick, Args args)
        {
            var index = pick?.GetIndex(Count, args) ?? -1;
            return Get(index);
        }
        
        public object Get(IListSetPick pick, Args args)
        {
            var index = pick?.GetIndex(Count, args) ?? -1;
            return Get(index);
        }
        
        public object Get(int index)
        {
            return models.Models[index];
        }
    }
}