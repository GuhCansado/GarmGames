using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    public class PhotonSettings : AssetRepository<PhotonRepository>
    {
        public override IIcon Icon => new IconComputer(ColorTheme.Type.TextLight);
        public override string Name => "Photon";
    }
}