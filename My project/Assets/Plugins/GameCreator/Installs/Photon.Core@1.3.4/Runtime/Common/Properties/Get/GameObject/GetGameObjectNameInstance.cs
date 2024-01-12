using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Game Object Name")]
    [Category("Game Objects/Game Object Name")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    [Description("A path name from a Prefab in resources folder.")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectNameInstance : PropertyTypeGetGameObjectName
    {
#if UNITY_EDITOR
        [SerializeField] private GameObject prefab;
#endif
        [SerializeField] private string prefabName;

        public override string Get(Args args) => prefabName;
        public override string Get(GameObject gameObject) => prefabName;
        public override string String => prefabName;

        public GetGameObjectNameInstance() : base()
        { }

        // public GetGameObjectNameInstance(GameObject gameObject) : this()
        // {
        //     this.m_GameObject = gameObject;
        // }

        public static PropertyGetGameObjectName Create()
        {
            GetGameObjectNameInstance instance = new GetGameObjectNameInstance();
            return new PropertyGetGameObjectName(instance);
        }
        
        // public static PropertyGetGameObjectName Create(GameObject gameObject)
        // {
        //     GetGameObjectNameInstance instance = new GetGameObjectNameInstance
        //     {
        //         m_GameObject = gameObject
        //     };
        //     
        //     return new PropertyGetGameObjectName(instance);
        // }
        //
        // public static PropertyGetGameObjectName Create(Transform transform)
        // {
        //     return Create(transform != null ? transform.gameObject : null);
        // }

        // public override string String => this.m_GameObject != null
        //     ? this.m_GameObject.name
        //     : "(none)";

        // public override GameObject SceneReference => this.m_GameObject;
    }
}