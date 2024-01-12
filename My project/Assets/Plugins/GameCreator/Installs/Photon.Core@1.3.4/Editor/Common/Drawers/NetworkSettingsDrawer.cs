using GameCreator.Editor.Common;
using NinjutsuGames.Photon.Runtime;
using UnityEditor;

namespace NinjutsuGames.Photon.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(PhotonCoreSettings))]
    public class NetworkSettingsDrawer : TTitleDrawer
    {
        protected override string Title => "Network Settings";
    }
}