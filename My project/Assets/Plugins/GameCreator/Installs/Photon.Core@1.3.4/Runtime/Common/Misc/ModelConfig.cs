using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.UnityUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class ModelConfig : TPolymorphicItem<ModelConfig>
    {
        public PropertyGetSprite sprite = GetSpriteInstance.Create();
        public PropertyGetGameObject prefab = GetGameObjectInstance.Create();
        public Skeleton skeleton;
        public MaterialSoundsAsset materialSounds;
        public Vector3 offset;
        
        public override string ToString() => prefab == null ? "None" : prefab.ToString();
    }
}