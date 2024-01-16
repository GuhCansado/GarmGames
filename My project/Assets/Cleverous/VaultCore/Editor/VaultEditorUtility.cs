// (c) Copyright Cleverous 2022. All rights reserved.

using UnityEditor;

namespace Cleverous.VaultDashboard
{
    public static class VaultEditorUtility
    {
        private const string VaultLocatorFileGuid = "cb441f7bdab6b61459aa19cfd0138c36";
        private const string VaultItemPath = "VaultCoreStorage/";
         
        public static string GetPathToVaultCoreRootFolder()
        {
            string result = AssetDatabase.GUIDToAssetPath(VaultLocatorFileGuid);
            result = result.Replace("VaultLocatorFile.cs", "");
            return result;
        }        
        public static string GetPathToVaultCoreStorageFolder()
        {
            string result = GetPathToVaultCoreRootFolder() + VaultItemPath;
            return result;
        }
    }
}