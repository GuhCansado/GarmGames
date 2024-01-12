using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Connect to Region")]
    [Description("Connects to the Photon Cloud region of choice.")]

    [Category("Photon/Connection/Connect to Region")]
    
    [Parameter("GameVersion", "Version number of your game. Setting this updates the AppVersion, which separates your playerbase in matchmaking.")]
    [Parameter("Region", "Connects to the Photon Cloud region of choice.")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Green, typeof(OverlayArrowUp))]

    [Keywords("Connect", "Network", "Photon", "Region")]
    [Serializable]
    public class InstructionPhotonConnectRegion : Instruction
    {
        public enum RegionCode
        {
            Asia,
            Australia,
            CanadaEast,
            Chinese,
            Europe,
            India,
            Japan,
            Russia,
            RussiaEast,
            SoutAmerica,
            SouthKorea,
            USAEast,
            USAWest
        }
    
        [SerializeField] private RegionCode region = RegionCode.USAEast;
        [SerializeField] private PropertyGetString gameVersion = GetStringString.Create;

        public override string Title => $"Photon Connect to Region: '{region}' with version: {gameVersion}";

        protected override Task Run(Args args)
        {
            if(!string.IsNullOrEmpty(gameVersion.Get(args))) PhotonNetwork.GameVersion = gameVersion.Get(args);
            return Task.FromResult(PhotonNetwork.ConnectToRegion(GetRegionCode(region)));
        }
        
        private string GetRegionCode(RegionCode region)
        {
            switch (region)
            {
                case RegionCode.Asia: return "asia";
                case RegionCode.Australia: return "au";
                case RegionCode.CanadaEast: return "cae";
                case RegionCode.Chinese: return "cn";
                case RegionCode.Europe: return "eu";
                case RegionCode.India: return "in";
                case RegionCode.Japan: return "jp";
                case RegionCode.Russia: return "ru";
                case RegionCode.RussiaEast: return "rue";
                case RegionCode.SoutAmerica: return "sa";
                case RegionCode.SouthKorea: return "kr";
                case RegionCode.USAEast: return "us";
                case RegionCode.USAWest: return "usw";
            }

            return "us";
        }
    }
}