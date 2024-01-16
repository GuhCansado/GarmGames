// (c) Copyright Cleverous 2022. All rights reserved.

using System;
using Cleverous.VaultSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Cleverous.VaultDashboard
{
    public class VaultDashboard : EditorWindow
    {
        // current values
        private const string UxmlAssetName = "vault_dashboard_uxml";

        public static DataEntity CurrentSelectedAsset
        {
            get
            {
                if (Instance.m_currentSelectedAsset != null)
                {
                    return Instance.m_currentSelectedAsset;
                }

                string currentGuid = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.CurrentAssetGuid);
                string currentPath = AssetDatabase.GUIDToAssetPath(currentGuid);
                DataEntity asset = AssetDatabase.LoadAssetAtPath<DataEntity>(currentPath);
                Instance.m_currentSelectedAsset = asset;
                return Instance.m_currentSelectedAsset;
            }
            set
            {
                string currentPath = AssetDatabase.GetAssetPath(value);
                GUID currentGuid = AssetDatabase.GUIDFromAssetPath(currentPath);
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.CurrentAssetGuid, currentGuid.ToString());
                Instance.m_currentSelectedAsset = value;
            }
        }
        private DataEntity m_currentSelectedAsset;

        public static IDataGroup CurrentSelectedGroup
        {
            get
            {
                if (Instance.m_currentGroupSelected != null)
                {
                    return Instance.m_currentGroupSelected;
                }

                string currentName = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.CurrentGroupName);
                VaultDataGroupFoldableButton button = GroupColumn.Q<VaultDataGroupFoldableButton>(currentName);
                if (button == null)
                {
                    // Debug.Log($"Failed to find a group button '{currentName}'.");
                    return null;
                }

                IDataGroup asset = button.DataGroup;
                if (asset == null)
                {
                    Debug.Log($"Failed to find group asset '{currentName}'.");
                }
                Instance.m_currentGroupSelected = asset;
                return Instance.m_currentGroupSelected;
            }
            set
            {
                string title = value == null
                    ? "NULL GROUP"
                    : value.Title;
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.CurrentGroupName, title);
                Instance.m_currentGroupSelected = value;
            }
        }
        private IDataGroup m_currentGroupSelected;

        // toolbar
        protected static Historizer Historizer;
        public static ToolbarSearchField SearchFieldForGroup; // TODO move these.
        public static ToolbarSearchField SearchFieldForAsset;// TODO move these.
        public static bool SearchTypeIsDirty => SearchFieldForGroup != null && SearchFieldForGroup.value != m_typeSearchCache;
        public static bool SearchAssetIsDirty => SearchFieldForAsset != null && SearchFieldForAsset.value != m_assetSearchCache;
        private static string m_assetSearchCache;
        private static string m_typeSearchCache;

        // columns
        public static VaultDataGroupColumn GroupColumn;
        public static VaultColumnOfAssets AssetColumn;
        public static VaultAssetInspector InspectorColumn;

        // action callbacks ////////////////

        // major changers
        public static Action OnCurrentAssetChanged;
        public static Action OnCurrentGroupChanged;

        // searches
        public static Action OnSearchAssets;
        public static Action OnSearchGroups;

        // assets
        public static Action OnDeleteAssetStart;
        public static Action OnDeleteAssetComplete;
        public static Action OnCreateNewAssetStart;
        public static Action OnCreateNewAssetComplete;
        public static Action OnCloneAssetStart;
        public static Action OnCloneAssetComplete;        

        // groups
        public static Action OnCreateGroupStart;
        public static Action OnCreateGroupComplete;

        // wrappers for views
        protected static VisualElement WrapperForGroupContent;
        protected static VisualElement WrapperForAssetList;
        protected static VisualElement WrapperForAssetContent;
        protected static VisualElement WrapperForInspector;

        private static ToolbarButton m_assetNewButton;
        private static ToolbarButton m_assetDeleteButton;
        private static ToolbarButton m_assetCloneButton;
        private static ToolbarButton m_assetRemoveFromGroupButton;
        private static ToolbarButton m_groupNewButton;
        private static ToolbarButton m_groupDelButton;
        private static Button m_idSetButton;
        private static IntegerField m_idSetField;
        
        public static VaultDashboard Instance;

        private static readonly StyleColor ButtonInactive = new StyleColor(Color.gray);
        private static readonly StyleColor ButtonActive = new StyleColor(Color.white);
        private static bool idValueIsDirty = true;

        [MenuItem("Tools/Cleverous/Vault Dashboard %#d", priority = 0)]
        public static void Open()
        {
            //Debug.Log("Open()");
            if (Instance != null)
            {
                FocusWindowIfItsOpen(typeof(VaultDashboard));
                return;
            }

            Instance = GetWindow<VaultDashboard>();
            Instance.titleContent.text = "Vault Dashboard";
            Instance.minSize = new Vector2(850, 200);
            Instance.Show();
            Instance.RebuildFull(); 
        }
        public void OnEnable()
        { 
            Instance = this;
        }
        public void Update()
        {
            if (SearchAssetIsDirty)
            {
                m_assetSearchCache = SearchFieldForAsset.value;
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SearchAssets, m_assetSearchCache);
                OnSearchAssets?.Invoke();
            }

            if (SearchTypeIsDirty)
            {
                m_typeSearchCache = SearchFieldForGroup.value;
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SearchGroups, m_typeSearchCache);
                OnSearchGroups?.Invoke();
            }

            if (idValueIsDirty)
            {
                SetIdStartingPoint(VaultEditorSettings.GetInt(VaultEditorSettings.VaultData.StartingKeyId));
                idValueIsDirty = false;
            }
        }
        
        [InitializeOnLoadMethod]
        private static void OnRecompile()
        {
            idValueIsDirty = true;
        }
        private void LoadUxmlTemplate()
        {
            Instance.rootVisualElement.Clear();

            // load uxml and elements
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>(UxmlAssetName);
            visualTree.CloneTree(rootVisualElement);

            // find important parts and reference them
            WrapperForGroupContent = rootVisualElement.Q<VisualElement>("GC_CONTENT");
            WrapperForAssetContent = rootVisualElement.Q<VisualElement>("AC_CONTENT");
            WrapperForAssetList = rootVisualElement.Q<VisualElement>("ASSET_COLUMN");
            WrapperForInspector = rootVisualElement.Q<VisualElement>("INSPECT_COLUMN");
            SearchFieldForGroup = rootVisualElement.Q<ToolbarSearchField>("GROUP_SEARCH");
            SearchFieldForAsset = rootVisualElement.Q<ToolbarSearchField>("ASSET_SEARCH");

            Historizer = new Historizer();
            rootVisualElement.Q<VisualElement>("TB_HISTORY").Add(Historizer);

            // init group column buttons
            m_groupNewButton = rootVisualElement.Q<ToolbarButton>("GC_NEW");
            m_groupDelButton = rootVisualElement.Q<ToolbarButton>("GC_DEL");
            rootVisualElement.Q<ToolbarButton>("GC_RELOAD").clicked += CallbackButtonRefresh;
            rootVisualElement.Q<ToolbarButton>("GC_HELP").clicked += CallbackButtonHelp;

            m_groupNewButton.clicked += CreateNewDataGroupCallback;
            m_groupDelButton.clicked += DeleteSelectedDataGroup;

            // init Asset Column Buttons
            m_assetNewButton = WrapperForAssetList.Q<ToolbarButton>("AC_NEW");
            m_assetDeleteButton = WrapperForAssetList.Q<ToolbarButton>("AC_DELETE");
            m_assetCloneButton = WrapperForAssetList.Q<ToolbarButton>("AC_CLONE");
            m_assetRemoveFromGroupButton = WrapperForAssetList.Q<ToolbarButton>("AC_GROUP_REMOVE");
             
            m_assetNewButton.clicked += CreateNewAssetCallback;
            m_assetDeleteButton.clicked += DeleteSelectedAsset;
            m_assetCloneButton.clicked += CloneSelectedAsset;
            m_assetRemoveFromGroupButton.clicked += RemoveAssetFromGroup;

            // init footer
            m_idSetButton = rootVisualElement.Q<Button>("ID_SET_BUTTON");
            m_idSetField = rootVisualElement.Q<IntegerField>("ID_SET_FIELD");

            m_idSetButton.clicked += SetIdCallback;
            m_idSetField.SetValueWithoutNotify(VaultEditorSettings.GetInt(VaultEditorSettings.VaultData.StartingKeyId));

            WrapperForGroupContent.Add(GroupColumn);
            WrapperForAssetContent.Add(AssetColumn);
            WrapperForInspector.Add(InspectorColumn);

            // init split pane draggers
            // BUG - basically we have to do this because there is no proper/defined initialization for the drag anchor position.
            SplitView mainSplit = rootVisualElement.Q<SplitView>("MAIN_SPLIT");
            SplitView columnSplit = rootVisualElement.Q<SplitView>("FILTERS_PICK_SPLIT");
            mainSplit.fixedPaneInitialDimension = 549;
            columnSplit.fixedPaneInitialDimension = 250;

            SetIdStartingPoint(VaultEditorSettings.GetInt(VaultEditorSettings.VaultData.StartingKeyId));
        }
        public void RebuildFull()
        {
            //Debug.Log($"Start RebuildFull() - Dashboard is {(Instance == null ? "null" : "valid")}");
            if (Instance == null) return;

            Instance.LoadUxmlTemplate();
            Rebuild(true);
        }
        public void Rebuild(bool fullRebuild = false)
        {
            //Debug.Log($"... Rebuild()");
            // search data
            SearchFieldForGroup.SetValueWithoutNotify(VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SearchGroups));
            SearchFieldForAsset.SetValueWithoutNotify(VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SearchAssets));
            m_typeSearchCache = SearchFieldForGroup.value;
            m_assetSearchCache = SearchFieldForAsset.value;

            // rebuild
            RebuildGroupColumn(fullRebuild);
            RebuildAssetColumn(fullRebuild);
            RebuildInspectorColumn(fullRebuild);
            SetCurrentGroup(CurrentSelectedGroup);

            OnCreateNewAssetComplete += OnCreatedNewAsset;
        }

        private void RebuildGroupColumn(bool fullRebuild = false)
        {
            if (fullRebuild || GroupColumn == null)
            {
                WrapperForGroupContent.Clear();
                GroupColumn = new VaultFilterColumnInheritance();
                WrapperForGroupContent.Add(GroupColumn);
            }
            GroupColumn.Rebuild();
        }
        private void RebuildAssetColumn(bool fullRebuild = false)
        {
            if (fullRebuild || AssetColumn == null)
            {
                WrapperForAssetContent.Clear();
                AssetColumn = new VaultColumnOfAssets();
                WrapperForAssetContent.Add(AssetColumn);
            }
            AssetColumn.Rebuild();
        }
        private void RebuildInspectorColumn(bool fullRebuild = false)
        {
            if (fullRebuild || InspectorColumn == null)
            {
                InspectorColumn?.RemoveFromHierarchy();
                InspectorColumn = new VaultAssetInspector();
                WrapperForInspector.Add(InspectorColumn);
            }
            InspectorColumn.Rebuild();
        }

        public static void SetCurrentGroup(IDataGroup group, bool isCustom = false)
        {
            if (group == null) return;

            m_assetRemoveFromGroupButton.SetEnabled(isCustom);
            m_assetRemoveFromGroupButton.style.unityBackgroundImageTintColor = isCustom ? ButtonActive : ButtonInactive;

            CurrentSelectedGroup = group;
            GroupColumn.SelectButtonByTitle(group.Title);
            SearchFieldForAsset.value = string.Empty;
            OnCurrentGroupChanged?.Invoke();
        }
        public static void SetCurrentInspectorAsset(DataEntity asset)
        {
            CurrentSelectedAsset = asset;
            OnCurrentAssetChanged?.Invoke();
        }
        public static void InspectAssetRemote(Object asset, Type t)
        {
            if (asset == null && t == null) return;
            if (t == null) return;

            if (Instance == null) Open();
            Instance.Focus();
            SearchFieldForAsset.SetValueWithoutNotify(string.Empty);

            VisualElement button = WrapperForGroupContent.Q<VisualElement>(t.Name);
            IVaultDataGroupButton buttonInterface = (IVaultDataGroupButton) button;
            if (buttonInterface != null)
            {
                buttonInterface.SetAsCurrent();
                GroupColumn.ScrollTo(button);
            }
            
            if (asset != null) AssetColumn.Pick((DataEntity)asset);
        }

        /// <summary>
        /// The Dashboard button calls this to create a new asset in the current group.
        /// </summary>
        private static void CreateNewAssetCallback()
        {
            if (CurrentSelectedGroup.SourceType.IsAbstract)
            {
                bool confirm = EditorUtility.DisplayDialog(
                    "Group Error",
                    "Selected Class is abstract! We can't create a new asset in abstract class groups. Choose a valid class and create a new Data Asset, then you can store it in a Custom Group.",
                    "Ok");
                if (confirm) return;
            }
            CreateNewAsset();
        }
        /// <summary>
        /// Create a new asset with the current group Type.
        /// </summary>
        /// <returns></returns>
        public static DataEntity CreateNewAsset()
        {
            OnCreateNewAssetStart?.Invoke();

            DataEntity newAsset = AssetColumn.NewAsset(CurrentSelectedGroup.SourceType);

            OnCreateNewAssetComplete?.Invoke();
            return newAsset;
        }
        /// <summary>
        /// Create a new asset with a specific Type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DataEntity CreateNewAsset(Type t)
        {
            Debug.Log($"Create new asset with specific Type: {t.Name}");
            OnCreateNewAssetStart?.Invoke();
            DataEntity newAsset = AssetColumn.NewAsset(t);
            OnCreateNewAssetComplete?.Invoke();
            return newAsset;
        }
        public static void CloneSelectedAsset()
        {
            OnCloneAssetStart?.Invoke();
            AssetColumn.CloneSelection();
            OnCloneAssetComplete?.Invoke();
        }
        public static void DeleteSelectedAsset()
        {
            OnDeleteAssetStart?.Invoke();
            AssetColumn.DeleteSelection();
            OnDeleteAssetComplete?.Invoke();
        }

        private static void OnCreatedNewAsset()
        {
            DatabaseBuilder.Reload();
        }

        public static void SetIdStartingPoint(int id)
        {
            Vault.Db.SetIdStartingValue(id);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.StartingKeyId, id);
            m_idSetField.value = id;
            EditorUtility.SetDirty(Vault.Db);
        }
        private static void SetIdCallback()
        {
            SetIdStartingPoint(m_idSetField.value);
        }

        public static void RemoveAssetFromGroup()
        {
            CurrentSelectedGroup.RemoveEntity(CurrentSelectedAsset.GetDbKey());
            AssetColumn.Rebuild();
        }
        public static void CreateNewDataGroupCallback()
        {
            CreateNewDataGroup();
        }
        public static void DeleteSelectedDataGroup()
        {
            if (CurrentSelectedGroup == null) return;
            VaultCustomDataGroup customGroup = (VaultCustomDataGroup) CurrentSelectedGroup;
            if (customGroup == null) return;

            bool confirm = EditorUtility.DisplayDialog(
                "Delete Custom Group",
                $"Are you sure you want to permanently delete '{CurrentSelectedGroup.Title}'?",
                "Delete",
                "Abort");
            if (!confirm) return;

            InspectAssetRemote(null, typeof(object));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(customGroup));
            CurrentSelectedGroup = null;
            Instance.Rebuild();
        }
        public static VaultCustomDataGroup CreateNewDataGroup()
        {
            OnCreateGroupStart?.Invoke();
            VaultCustomDataGroup result = (VaultCustomDataGroup)AssetColumn.NewAsset(typeof(VaultCustomDataGroup));
            GroupColumn.Rebuild();
            InspectAssetRemote(result, typeof(VaultCustomDataGroup));
            OnCreateGroupComplete?.Invoke();
            return null;
        }
        public void CallbackButtonRefresh()
        {
            RebuildFull();
        }
        public static void CallbackButtonHelp()
        {
            Application.OpenURL("https://lanefox.gitbook.io/vault/");
        }
    }
}