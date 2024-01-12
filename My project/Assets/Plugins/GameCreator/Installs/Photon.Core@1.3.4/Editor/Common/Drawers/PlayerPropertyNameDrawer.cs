using System;
using System.Collections.Generic;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.Photon.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(PlayerPropertyName))]
    public class PlayerPropertyNameDrawer : PropertyDrawer
    {
        [Serializable]
        private class Favorites
        {
            public string[] values = Array.Empty<string>();
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        // CONSTANTS: -----------------------------------------------------------------------------
        
        private const string USS_PATH = EditorPaths.COMMON + "Signals/StyleSheets/Signal";
        
        private const string NAME_ROOT = "GC-Signal";
        private const string NAME_FAVORITE = "GC-Signal-Favorite";
        private const string NAME_DROPDOWN = "GC-Signal-Dropdown";

        private const string TIP_FAVORITE = "Mark or unset as favorite";
        private const string TIP_DROPDOWN = "Choose from the favorite list";
        
        private static readonly IIcon ICON_FAVORITE_ON = new IconStarSolid(ColorTheme.Type.Yellow);
        private static readonly IIcon ICON_FAVORITE_OFF = new IconStarOutline(ColorTheme.Type.TextLight);
        private static readonly IIcon ICON_DROPDOWN = new IconArrowDropDown(ColorTheme.Type.TextLight);
        
        public const string KEY_SIGNALS_FAVORITES = "gc:playerproperty-favorites";

        // PAINT METHODS: -------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var sheets = StyleSheetUtils.Load(USS_PATH);
            foreach (var styleSheet in sheets)
            {
                root.styleSheets.Add(styleSheet);
            }

            var propertyString = property.FindPropertyRelative("m_String");
            var textField = new TextField { bindingPath = propertyString.propertyPath };
            
            var favorite = new Image
            {
                image = IsFavorite(propertyString.stringValue) 
                    ? ICON_FAVORITE_ON.Texture
                    : ICON_FAVORITE_OFF.Texture,
                name = NAME_FAVORITE,
                tooltip = TIP_FAVORITE,
                focusable = true
            };

            favorite.SetEnabled(!string.IsNullOrEmpty(propertyString.stringValue));
            textField.RegisterValueChangedCallback(changeEvent =>
            {
                favorite.SetEnabled(!string.IsNullOrEmpty(changeEvent.newValue));
                favorite.image = IsFavorite(changeEvent.newValue)
                    ? ICON_FAVORITE_ON.Texture
                    : ICON_FAVORITE_OFF.Texture;
            });

            favorite.RegisterCallback<ClickEvent>(_ =>
            {
                propertyString.serializedObject.Update();
                var text = propertyString.stringValue;
                if (string.IsNullOrEmpty(text)) return;
                
                if (IsFavorite(text))
                {
                    RemoveFavorite(text);
                    favorite.image = ICON_FAVORITE_OFF.Texture;
                }
                else
                {
                    AddFavorite(text);
                    favorite.image = ICON_FAVORITE_ON.Texture;
                }
            });
            
            var dropdown = new Image
            {
                image = ICON_DROPDOWN.Texture,
                name = NAME_DROPDOWN,
                tooltip = TIP_DROPDOWN,
                focusable = true
            };
            
            dropdown.AddManipulator(new MouseDropdownManipulator(context =>
            {
                var favoriteSignals = GetFavoriteSignals();
                if (favoriteSignals.Length == 0)
                {
                    context.menu.AppendAction(
                        "No favorite Player Property found", null, 
                        DropdownMenuAction.Status.Disabled
                    );
                    
                    return;
                }
                
                foreach (var favoriteSignal in favoriteSignals)
                {
                    context.menu.AppendAction(
                        favoriteSignal,
                        menuAction =>
                        {
                            propertyString.serializedObject.Update();
                            propertyString.stringValue = menuAction.name;
            
                            propertyString.serializedObject.ApplyModifiedProperties();
                            propertyString.serializedObject.Update();
                        },
                        menuAction => menuAction.name != propertyString.stringValue 
                            ? DropdownMenuAction.Status.Normal 
                            : DropdownMenuAction.Status.Checked);
                }
            }));
            
            var nameContainer = new VisualElement { name = NAME_ROOT };
            
            nameContainer.Add(new Label(property.displayName));
            nameContainer.Add(textField);
            nameContainer.Add(favorite);
            nameContainer.Add(dropdown);
            
            nameContainer.AddToClassList(AlignLabel.CLASS_UNITY_ALIGN_LABEL);
            AlignLabel.On(nameContainer);

            root.Add(nameContainer);
            root.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            
            return root;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private static string[] GetFavoriteSignals()
        {
            var json = EditorPrefs.GetString(KEY_SIGNALS_FAVORITES, string.Empty);
            if (string.IsNullOrEmpty(json)) return Array.Empty<string>();

            var favorites = JsonUtility.FromJson<Favorites>(json); 
            return favorites != null ? favorites.values : Array.Empty<string>();
        }

        private static bool IsFavorite(string text)
        {
            var favorites = GetFavoriteSignals();
            foreach (var favorite in favorites)
            {
                if (favorite == text) return true;
            }
            
            return false;
        }
        
        private static void AddFavorite(string text)
        {
            var values = new List<string>(GetFavoriteSignals()) { text };
            values.Sort();

            var favorites = new Favorites
            {
                values = values.ToArray()
            };
            
            var json = JsonUtility.ToJson(favorites);
            EditorPrefs.SetString(KEY_SIGNALS_FAVORITES, json);
        }

        private static void RemoveFavorite(string text)
        {
            var values = new List<string>(GetFavoriteSignals());
            values.Remove(text);

            var favorites = new Favorites
            {
                values = values.ToArray()
            };

            var json = JsonUtility.ToJson(favorites);
            EditorPrefs.SetString(KEY_SIGNALS_FAVORITES, json);
        }
    }
}