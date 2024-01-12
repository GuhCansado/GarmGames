using System;
using System.Collections.Generic;
using System.Linq;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    [Icon(RuntimePaths.GIZMOS + "GizmoSkeleton.png")]
    [Serializable]
    [AddComponentMenu("Game Creator/Photon/Attachments")]
    public class PhotonAttachments : MonoBehaviour
    {
        public static PhotonAttachments Instance { get; private set; }

        public readonly Dictionary<string, GameObject> RuntimeAttachments = new();
        [SerializeField] private List<GameObject> attachments = new();

        private void Awake()
        {
            Instance = this;
            
            RuntimeAttachments.Clear();
            foreach (var attachment in attachments.Where(attachment => attachment != null))
            {
                RuntimeAttachments.Add(attachment.name, attachment);
            }
        }
    }
}