// (c) Copyright Cleverous 2022. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cleverous.VaultSystem;
using UnityEditor; 
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard 
{
    public class VaultColumnOfAssets : VaultDashboardColumn
    {
        //public static DataEntity InDrag;
        public Action<DataEntity> OnPick;
        public ListView ListElement;
        private List<DataEntity> m_searchFilteredList;
        private bool m_isSearchFiltering;

        public List<DataEntity> CurrentSelections;
        public VaultColumnOfAssets()
        {
            this.Rebuild();
        }

        public override void Rebuild()
        {
            Clear();

            //Debug.Log($"<color=orange>Asset Column rebuild - Group: {VaultDashboard.CurrentSelectedGroup.Title}, Asset: {VaultDashboard.CurrentSelectedAsset.Title}</color>");

            this.style.flexGrow = 1;
            this.name = "Asset List Wrapper";
            this.viewDataKey = "asset_list_wrapper";

            ListElement = new ListView(
                VaultDashboard.CurrentSelectedGroup == null ? new List<DataEntity>() : VaultDashboard.CurrentSelectedGroup.Content, 
                32, 
                ListMakeItem, 
                ListBindItem);

            ListElement.name = "Asset List View";
            ListElement.viewDataKey = "asset_list";
            ListElement.style.flexGrow = 1;
            ListElement.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
            ListElement.selectionType = SelectionType.Multiple;

#if UNITY_2020_3_OR_NEWER
            ListElement.onSelectionChange += SelectAssetsInternal;
            ListElement.onItemsChosen += ChooseAssetInternal;
#else
            ListElement.onSelectionChanged += SelectAssetsInternal;
            ListElement.onItemChosen += ChooseAssetInternal;
#endif
            // plug into events for updates.
            VaultDashboard.OnSearchAssets += CallbackListBySearch;
            VaultDashboard.OnCurrentGroupChanged += CallbackListByType;

            Add(ListElement);
            ListAssetsByGroup();

            if (!string.IsNullOrEmpty(VaultDashboard.SearchFieldForAsset.value))
            {
                ListAssetsBySearch();
            }

            GetSelectionPersistence();
            if (m_isSearchFiltering) CallbackListBySearch();

            Pick(VaultDashboard.CurrentSelectedAsset);
        }

        private void CallbackListBySearch() {ListAssetsBySearch(true);}
        private void CallbackListByType() {ListAssetsByGroup(true);}

        public void ListAssetsByGroup(bool scrollToTop = false)
        {
            if (VaultDashboard.CurrentSelectedGroup == null) return;

            //Debug.Log($"Match: Cache: <color=red>{m_typeCache}</color>, Target: <color=lime>{VaultDashboard.CurrentTypeFilter}</color>");
            m_isSearchFiltering = false;

            if (VaultDashboard.CurrentSelectedGroup.Content != null && 
                VaultDashboard.CurrentSelectedGroup.Content.Count > 1)
            {
                VaultDashboard.CurrentSelectedGroup.Sanitize();
                VaultDashboard.CurrentSelectedGroup.Content.Sort((x, y) => string.CompareOrdinal(x.Title, y.Title));
            }

            ListElement.itemsSource = VaultDashboard.CurrentSelectedGroup.Content;
            ListElement.Rebuild();

            if (scrollToTop) ListElement.ScrollToItem(0);
        }
        public void ListAssetsBySearch(bool scrollToTop = false)
        {
            if (VaultDashboard.CurrentSelectedGroup.Content.Count == 0) ListAssetsByGroup();

            //Debug.Log($"<color=yellow>Searching {m_allAssetsOfFilteredType.Count} assets.</color>");

            m_isSearchFiltering = true;
            m_searchFilteredList = VaultDashboard.CurrentSelectedGroup.Content.FindAll(SearchMatchesItem);
            m_searchFilteredList.Sort((x, y) => string.CompareOrdinal(x.Title, y.Title));

            //Debug.Log($"<color=green>Found {m_searchFilteredList.Count} matches.</color>"); 

            ListElement.itemsSource = m_searchFilteredList;
            ListElement.Rebuild();
            if (scrollToTop) ListElement.ScrollToItem(0);
        }
        
        /// <summary>
        /// ONLY for use when you want something external to change the list selection.
        /// This will change the list index and subsequently trigger the internal method
        /// to fire the global changed event so everything else catches up.
        /// </summary>
        public void Pick(DataEntity asset)
        {
            // fail out
            if (asset == null) return;
            //if (!VaultDashboard.CurrentSelectedGroup.Data.Contains(asset)) Rebuild(); // todo fails and causes stack alloc overload?

            // set index
            int index = ListElement.itemsSource.IndexOf(asset);
            ListElement.selectedIndex = index;
            ListElement.ScrollToItem(index);
            OnPick?.Invoke(asset); 
        }

        private static bool SearchMatchesItem(DataEntity entity)
        {
            bool result = entity.Title.ToLower().Contains(VaultDashboard.SearchFieldForAsset.value.ToLower());
            return result;
        }
        /// <summary>
        /// ONLY for use when the list has chosen something.
        /// </summary>
        /// <param name="obj"></param>
        private void ChooseAssetInternal(object obj)
        {
            // fail
            DataEntity entity;
            if (ListElement.selectedIndex < 0) return;
            if (obj == null) return;
            if (obj is IList)
            {
                entity = ((List<object>) obj)[0] as DataEntity;
                if (entity == VaultDashboard.CurrentSelectedAsset) return;
            }
            else
            {
                entity = (DataEntity) obj;
                if (entity == VaultDashboard.CurrentSelectedAsset) return;
            }

            // set index in prefs
            int index = ListElement.itemsSource.IndexOf(entity);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.CurrentAssetGuid, ListElement.selectedIndex);

            // broadcast change
            VaultDashboard.SetCurrentInspectorAsset(entity);
        }
#if UNITY_2020_3_OR_NEWER
        private void SelectAssetsInternal(IEnumerable<object> input)
        {
            List<object> objs = (List<object>)input;
#else
        private void SelectAssetsInternal(List<object> objs)
        {
#endif
            CurrentSelections = objs.ConvertAll(asset => (DataEntity)asset);
            StringBuilder sb = new StringBuilder();
            foreach (DataEntity assetFile in CurrentSelections)
            {
                sb.Append(AssetDatabase.GetAssetPath(assetFile) + "|");
            }

            VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SelectedAssetGuids, sb.ToString());
            if (objs.Count > 0) ChooseAssetInternal(objs[0]);
        }
        private void GetSelectionPersistence()
        {
            string selected = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SelectedAssetGuids);
            if (string.IsNullOrEmpty(selected)) return;

            CurrentSelections = new List<DataEntity>();
            string[] split = selected.Split('|');
            foreach (string path in split)
            {
                if (path == string.Empty || path.Contains('|')) continue;
                CurrentSelections.Add(AssetDatabase.LoadAssetAtPath<DataEntity>(path));
            }
            VaultDashboard.CurrentSelectedAsset = CurrentSelections[0];
        }
        
        private void ListBindItem(VisualElement element, int listIndex)
        {
            // find the serialized property
            Editor ed = Editor.CreateEditor(m_isSearchFiltering ? m_searchFilteredList[listIndex] : VaultDashboard.CurrentSelectedGroup.Content[listIndex]);
            SerializedObject so = ed.serializedObject;

            SerializedProperty propTitle = so.FindProperty("m_title");
            Sprite sprite = ((DataEntity) so.targetObject).GetDataIcon;

            //element.RegisterCallback<PointerDownEvent>(evt => InDrag = (DataEntity)ListElement.itemsSource[listIndex]);

            // images are not bindable
            ((Image)element.ElementAt(0)).image = sprite != null
                ? AssetPreview.GetAssetPreview(sprite)
                : EditorGUIUtility.IconContent("d_ToolHandleLocal@2x").image;

            // bind the label to the serialized target target property title
            ((Label) element.ElementAt(1)).BindProperty(propTitle);
        }
        private static VisualElement ListMakeItem()
        {
            VisualElement selectableItem = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                    flexBasis = 1,
                    flexShrink = 1,
                    flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap)
                }
            };

            //selectableItem.Add(new Label {name = "Prefix", text = "error", style = {unityFontStyleAndWeight = FontStyle.Bold}});
            Image icon = new Image();
            icon.style.flexGrow = 0;
            icon.style.flexShrink = 0;
            icon.style.paddingLeft = 5;
            icon.style.height = 32;
            icon.style.width = 32;
            icon.scaleMode = ScaleMode.ScaleAndCrop;

            Label label = new Label {name = "Asset Title", text = "loading..."};
            label.style.paddingLeft = 5;

            selectableItem.Add(icon);
            selectableItem.Add(label);
            selectableItem.AddManipulator(new VaultDashboardDragManipulator(selectableItem, EditorWindow.focusedWindow.rootVisualElement));

            return selectableItem;
        }

        /// <summary>
        /// Creates a new asset of the provided type, then focuses the dashboard on it.
        /// </summary>
        /// <param name="t">Type to create. Must derive from DataEntity.</param>
        /// <returns>The newly created asset object</returns>
        public DataEntity NewAsset(Type t) 
        {
            if (t == null)
            {
                Debug.LogError("Type for new asset cannot be null.");
                return null;
            }
            if (t.IsAbstract)
            {
                Debug.LogError("Cannot create instances of abstract classes.");
                return null;
            }

            const string prefix = "Data-";
            const string suffix = ".asset";
            string timeHash = Math.Abs(DateTime.Now.GetHashCode()).ToString();
            string filename = $"{prefix}{t.Name}-{timeHash}{suffix}";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{VaultEditorUtility.GetPathToVaultCoreStorageFolder()}{filename}");

            ScriptableObject asset = ScriptableObject.CreateInstance(t);
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            DataEntity real = (DataEntity)AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathAndName);

            Vault.Db.Add(real, true);
            VaultStaticDataGroup group = Vault.Db.GetStaticGroup(t);
            group.AddEntity(real);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VaultDashboard.SearchFieldForAsset.SetValueWithoutNotify(string.Empty);

            VaultDashboard.SetCurrentGroup(group);
            Pick(real);

            return real;
        }

        public void CloneSelection()
        {
            if (VaultDashboard.CurrentSelectedGroup == null) return;
            if (VaultDashboard.CurrentSelectedAsset == null) return;

            const string prefix = "Data-";
            const string suffix = ".asset";
            string timeHash = Math.Abs(DateTime.Now.GetHashCode()).ToString();
            string filename = $"{prefix}{VaultDashboard.CurrentSelectedAsset.GetType().Name}-{timeHash}{suffix}";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{VaultEditorUtility.GetPathToVaultCoreStorageFolder()}{filename}");

            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(VaultDashboard.CurrentSelectedAsset), assetPathAndName);
            DataEntity newEntity = AssetDatabase.LoadAssetAtPath<DataEntity>(assetPathAndName);
            newEntity.Title += " (CLONED)";
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            DataEntity real = (DataEntity)AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathAndName);
            VaultDashboard.SearchFieldForAsset.SetValueWithoutNotify(string.Empty);
            
            Rebuild();

            int i = Mathf.Clamp(ListElement.itemsSource.IndexOf(real), 0, ListElement.itemsSource.Count);
            ListElement.ScrollToItem(i);
            ListElement.selectedIndex = i;
        }
        public void DeleteSelection()
        {
            StringBuilder sb = new StringBuilder();
            if (CurrentSelections.Count == 0) return;
            if (VaultDashboard.CurrentSelectedAsset == null) return;

            foreach (DataEntity asset in CurrentSelections)
            {
                if (asset == null) continue;
                sb.Append(asset.Title + "\n");
            }
            bool confirm = EditorUtility.DisplayDialog("Deletion warning!", $"Delete assets from the disk?\n\n{sb}", "Yes", "Cancel");
            if (!confirm) return;

            foreach (DataEntity asset in CurrentSelections)
            {
                Vault.Db.Remove(asset.GetDbKey());
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            }
            VaultDashboard.CurrentSelectedAsset = null;

            Rebuild();
        }
    }
} 