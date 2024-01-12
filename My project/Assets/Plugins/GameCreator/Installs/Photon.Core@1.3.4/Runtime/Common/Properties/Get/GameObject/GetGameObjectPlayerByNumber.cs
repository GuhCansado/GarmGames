using System;
using GameCreator.Runtime.Common;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Player by Number")]
    [Category("Photon/Player by Number")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Green, typeof(OverlayListVariable))]
    [Description("Reference to the Player gameObject by number.")]

    [Serializable]
    public class GetGameObjectPlayerByNumber : PropertyTypeGetGameObject
    {
        [SerializeField]
        private PropertyGetInteger playerNumber = new PropertyGetInteger(0);
        
        public override GameObject Get(Args args) => this.GetObject(args);

        private GameObject GetObject(Args args)
        {
            var id = playerNumber.Get(args);
            for (var i = 0; i < PlayerNumbering.SortedPlayers.Length; i++)
            {
                if (id != i) continue;
                var tag = PlayerNumbering.SortedPlayers[i].TagObject;
                if (tag == null) break;
                return tag as GameObject;
            }
            return null;
        }

        public static PropertyGetGameObject Create => new PropertyGetGameObject(
            new GetGameObjectPlayerByNumber()
        );

        public override string String => $"Player {playerNumber}";
    }
}