// (c) Copyright Cleverous 2022. All rights reserved.

using System.Collections.Generic;
using Cleverous.VaultSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultFilterColumnInheritance : VaultDataGroupColumn
    {
        protected static ScrollView ScrollElement;
        public static Foldout CustomGroupsFoldout;
        public static Foldout DefaultGroupsFoldout;
        public List<VaultCustomDataGroup> CustomGroups = new List<VaultCustomDataGroup>();
        private List<IVaultDataGroupButton> m_filteringMustShow = new List<IVaultDataGroupButton>();

        public static Texture IconBoxGreen;
        public static Texture IconBoxBlue;
        public static Texture IconBoxWireframe;

        public override void Rebuild()
        { 
            RebuildPrep();
            RebuildCustomGroups();
            RebuildStaticGroups();
            VaultDashboard.SetCurrentGroup(VaultDashboard.CurrentSelectedGroup);
            FilterBySearchBar();
        }

        private void RebuildPrep()
        {
            Clear();
            if (IconBoxGreen == null)
            {
                IconBoxGreen = EditorGUIUtility.IconContent("BoxCollider Icon").image;
                IconBoxBlue = EditorGUIUtility.IconContent("d_Prefab Icon").image;
                IconBoxWireframe = EditorGUIUtility.IconContent("d_GameObject Icon").image;
            }
            if (!IsSubscribed)
            {
                VaultDashboard.OnSearchGroups += FilterBySearchBar;
            }
            IsSubscribed = true;
            AllButtonsCache = new List<IVaultDataGroupButton>();

            ScrollElement = new ScrollView();
            ScrollElement.style.flexGrow = 1;
            this.Add(ScrollElement);
        }
        private void RebuildCustomGroups()
        {
            CustomGroups = DatabaseBuilder.GetAllAssetsInProject<VaultCustomDataGroup>();
            CustomGroupsFoldout = new Foldout();
            CustomGroupsFoldout.text = "Custom Data Groups";
            CustomGroupsFoldout.contentContainer.style.marginLeft = -5;
            CustomGroups.Sort((x, y) => string.CompareOrdinal(x.Title, y.Title));

            ScrollElement.Add(CustomGroupsFoldout);

            foreach (VaultCustomDataGroup g in CustomGroups)
            {
                VaultDataGroupFoldableButton currentButton = new VaultDataGroupFoldableButton(g, IconBoxWireframe, true);
                CustomGroupsFoldout.Add(currentButton.MainElement);

                Editor ed = Editor.CreateEditor((VaultCustomDataGroup)currentButton.DataGroup);
                SerializedObject so = ed.serializedObject;
                Button button = currentButton.MainElement.Q<Button>();
                button.bindingPath = "m_title";
                button.BindProperty(so);
            }
        }
        private void RebuildStaticGroups()
        {
            DefaultGroupsFoldout = new Foldout();
            DefaultGroupsFoldout.text = "Class Hierarchy";
            DefaultGroupsFoldout.contentContainer.style.marginLeft = -5;
            DefaultGroupsFoldout.style.marginLeft = 5;

            ScrollElement.Add(DefaultGroupsFoldout);

            foreach (VaultStaticDataGroup group in Vault.Db.GetAllStaticGroups())
            {
                VaultDataGroupFoldableButton groupButton = new VaultDataGroupFoldableButton(group, group.SourceType.IsAbstract ? IconBoxBlue : IconBoxGreen, false);
                AllButtonsCache.Add(groupButton);
            }

            AllButtonsCache.Sort((x, y) => string.CompareOrdinal(x.Title, y.Title));

            foreach (IVaultDataGroupButton curButton in AllButtonsCache)
            {
                // put any first tier classes directly into the foldout.
                if (curButton.DataGroup.SourceType.BaseType == typeof(DataEntity) || 
                    curButton.DataGroup.SourceType == typeof(DataEntity))
                {
                    DefaultGroupsFoldout.Add(curButton.MainElement);
                }
                else
                {
                    // find parent class button
                    IVaultDataGroupButton targetParent = AllButtonsCache.Find(otherButton => otherButton.DataGroup.SourceType == curButton.DataGroup.SourceType.BaseType);
                    if (targetParent == null) return;

                    targetParent.InternalElement.Add(curButton.MainElement);
                    targetParent.SetShowFoldout(true);
                }
            }
        }

        public override VaultDataGroupFoldableButton SelectButtonByTitle(string title)
        {
            VaultDataGroupFoldableButton button = ScrollElement.Q<VaultDataGroupFoldableButton>(title);
            if (button == null) return null;

            ScrollTo(button);
            button.SetAsCurrent();
            return button;
        }
        public override void ScrollTo(VisualElement button)
        {
            ScrollElement.ScrollTo(button);
        }
        public override void Filter(string filter)
        {
            m_filteringMustShow.Clear();
            if (string.IsNullOrEmpty(filter))
            {
                foreach (IVaultDataGroupButton button in AllButtonsCache)
                {
                    button.SetIsHighlighted(false);
                }
            }
            else
            {
                foreach (IVaultDataGroupButton button in AllButtonsCache)
                {
                    // turn it off
                    button.SetIsHighlighted(false);

                    // if there's a name match, turn it back on
                    bool isNameMatch = button.DataGroup.SourceType.Name.ToLower().Contains(filter.ToLower());
                    if (!isNameMatch) continue;

                    button.SetIsHighlighted(true);
                    FilterUpHierarchy(button);
                }

                foreach (IVaultDataGroupButton button in m_filteringMustShow)
                {
                    button.SetIsHighlighted(true);
                }
            }
        }
        private void FilterUpHierarchy(IVaultDataGroupButton button)
        {
            IVaultDataGroupButton buttonParent = AllButtonsCache.Find(x => x.DataGroup.SourceType == button.DataGroup.SourceType.BaseType);
            if (buttonParent != null)
            {
                m_filteringMustShow.Add(buttonParent);
                FilterUpHierarchy(buttonParent);
            }
        }
        public override void FilterBySearchBar()
        {
            Filter(VaultDashboard.SearchFieldForGroup.value);
        }
    }
}