// (c) Copyright Cleverous 2022. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public abstract class VaultDataGroupColumn : VaultDashboardColumn
    {
        protected bool IsSubscribed;
        protected static List<Type> AllValidTypesCache;
        protected static List<IVaultDataGroupButton> AllButtonsCache;

        protected static Assembly VaultAssy;
        protected static AssemblyName VaultAssyName;

        public abstract VaultDataGroupFoldableButton SelectButtonByTitle(string title);
        public abstract void ScrollTo(VisualElement button);
        public abstract void Filter(string f);
        public abstract void FilterBySearchBar();
    }
}