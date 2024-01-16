using NinjutsuGames.StateMachine.Runtime;
using UnityEngine;
using UnityEditor;

namespace NinjutsuGames.StateMachine.Editor
{
    [ExecuteAlways]
    public class DeleteCallback : AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {
            var objects = AssetDatabase.LoadAllAssetsAtPath(path);

            foreach (var obj in objects)
            {
                if (obj is StateMachineAsset b)
                {
                    foreach (var graphWindow in Resources.FindObjectsOfTypeAll<BaseGraphWindow>())
                        graphWindow.OnGraphDeleted();

                    b.OnAssetDeleted();
                }
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}