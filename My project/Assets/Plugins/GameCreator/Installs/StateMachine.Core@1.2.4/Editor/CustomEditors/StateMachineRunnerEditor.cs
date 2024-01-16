using System;
using System.Linq;
using GameCreator.Editor.Common;
using GameCreator.Editor.Variables;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    public class NameListView : TListView<Runtime.NameVariableRuntime>
    {
        private const string USS_PATH = EditorPaths.VARIABLES + "StyleSheets/NameList";

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override string USSPath => USS_PATH;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public NameListView(Runtime.NameVariableRuntime runtime) : base(runtime)
        {
            runtime.EventChange += OnChange;
        }

        private void OnChange(string name)
        {
            Refresh();
        }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        protected override void Refresh()
        {
            base.Refresh();
            if (m_Runtime?.GetEnumerator() == null) return;

            foreach (var variable in m_Runtime)
            {
                Add(new NameVariableView(variable));
            }
        }
    }

    [CustomPropertyDrawer(typeof(Runtime.NameVariableRuntime))]
    public class NameVariablesRuntimeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var list = property.FindPropertyRelative("m_List");
            var runtime = property.GetValue<Runtime.NameVariableRuntime>();

            var target = property.serializedObject.targetObject;
            var isPrefab = PrefabUtility.IsPartOfPrefabAsset(target);
            switch (EditorApplication.isPlayingOrWillChangePlaymode && !isPrefab)
            {
                case true:
                    root.Add(new NameListView(runtime));
                    break;

                case false:
                    root.Add(new RunnerNameListTool(list));
                    break;
            }

            return root;
        }
    }

    [CustomEditor(typeof(StateMachineRunner))]
    public class StateMachineRunnerEditor : UnityEditor.Editor
    {
        private static readonly Length ERROR_MARGIN = new Length(10, LengthUnit.Pixel);
        private const string ERR_DUPLICATE_ID = "Another Variable component has the same ID";

        // MEMBERS: -------------------------------------------------------------------------------

        private ErrorMessage m_Error;
        private StateMachineRunner runner;
        private RunnerNameListTool fieldRunnerList;
        private SerializedProperty propertyList;
        private StateMachineAsset _lastAsset;
        
        public static StateMachineRunnerEditor Instance { get; private set; }
        public static Action OnListChanged;

        // PAINT METHOD: --------------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            Instance = this;
            var root = new VisualElement
            {
                style =
                {
                    marginTop = new StyleLength(5)
                }
            };

            var graph = serializedObject.FindProperty("stateMachineAsset");
            var runtime = serializedObject.FindProperty("m_Runtime");
            var saveUniqueID = serializedObject.FindProperty("m_SaveUniqueID");

            var fieldGraph = new PropertyField(graph);

            runner = target as StateMachineRunner;
            fieldRunnerList = new RunnerNameListTool(runtime.FindPropertyRelative("m_List"));
            propertyList = runtime.FindPropertyRelative("m_List").FindPropertyRelative("m_Source");
            
            _lastAsset = runner.stateMachineAsset;

            var fieldRuntime = new PropertyField(runtime);
            var fieldSaveUniqueID = new PropertyField(saveUniqueID);
            m_Error = new ErrorMessage(ERR_DUPLICATE_ID)
            {
                style = {marginTop = ERROR_MARGIN}
            };
            root.Add(fieldGraph);
            root.Add(new SpaceSmall());
            // root.Add(fieldRunnerList);
            SyncVariables(true);

            switch (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                case true:
                    root.Add(fieldRuntime);
                    break;

                case false:
                    SyncVariables();
                    root.Add(fieldRunnerList);
                    fieldRunnerList.EventChangeSize -= OnChanged;
                    fieldRunnerList.EventChangeSize += OnChanged;
                    GraphInspector.OnListChanged += OnAssetVariableChange;
                    BlackboardView.OnListChanged += OnAssetVariableChange;
                    break;
            }
            
            root.Add(m_Error);
            root.Add(fieldSaveUniqueID);

            RefreshErrorID();
            
            fieldGraph.RegisterValueChangeCallback(OnGraphChange);
            fieldSaveUniqueID.RegisterValueChangeCallback(_ => { RefreshErrorID(); });
            return root;
        }

        private void OnDisable()
        {
            Instance = null;
            GraphInspector.OnListChanged -= OnAssetVariableChange;
            BlackboardView.OnListChanged -= OnAssetVariableChange;
        }

        private void OnAssetVariableChange()
        {
            if(fieldRunnerList == null) return;
            SyncVariables(true);
        }

        private void OnChanged(int obj)
        {
            EditorApplication.delayCall += SyncStateMachineVariables;
            OnListChanged?.Invoke();
        }

        public void SyncStateMachineVariables()
        {
            // Debug.Log($"SyncStateMachineVariables");
            runner.stateMachineAsset.SyncVariablesInternal(runner.Runtime.List);
            EditorApplication.delayCall -= SyncStateMachineVariables;
        }

        private void OnGraphChange(SerializedPropertyChangeEvent evt)
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            if (runner.stateMachineAsset == _lastAsset) return;

            serializedObject.Update();
            fieldRunnerList.PropertyList.ClearArray();
            fieldRunnerList.Clear();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            if (runner.stateMachineAsset != _lastAsset)
            {
                _lastAsset = runner.stateMachineAsset;
                SyncVariables();
            }

            fieldRunnerList.EventChangeSize -= OnChanged;
            fieldRunnerList.EventChangeSize += OnChanged;
        }

        private void SyncVariables(bool remove = false)
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode) return;
            if(fieldRunnerList == null) return;
            if(runner.stateMachineAsset == null) return;
            
            runner = target as StateMachineRunner;
            _lastAsset = runner.stateMachineAsset;
            
            // Copy missing variables from the State Machine
            for (var i = 0; i < runner.stateMachineAsset.NameList.Names.Length; i++)
            {
                var nameVar = runner.stateMachineAsset.NameList.Names[i];
                var exists = runner.Runtime.List.Names.Any(v => v == nameVar);
                if(exists)
                {
                    if(i >= runner.Runtime.List.Names.Length) continue;
                    if(i >= runner.stateMachineAsset.NameList.Names.Length) continue;
                    var assetIndex = runner.stateMachineAsset.NameList.Names.ToList().IndexOf(nameVar);
                    var runnerIndex = runner.Runtime.List.Names.ToList().IndexOf(nameVar);
                    if (runner.stateMachineAsset.NameList.Get(assetIndex).Type != runner.Runtime.List.Get(runnerIndex).Type)
                    {
                        // Debug.LogWarning($"Variable {nameVar} has different type in State Machine and Runtime. From: {runner.Runtime.List.Get(runnerIndex).Type} to: {runner.stateMachineAsset.NameList.Get(assetIndex).Type}");
                        fieldRunnerList.DeleteItem(runnerIndex);
                        // runner.stateMachineAsset.NameList.Get(assetIndex).Value = runner.Runtime.List.Get(runnerIndex).Value;
                        // runner.Runtime.List.Get(runnerIndex).Value = runner.stateMachineAsset.NameList.Get(assetIndex).Value;
                    }
                    continue;
                }
                var value = runner.stateMachineAsset.NameList.Get(i);
                fieldRunnerList.InsertItem(propertyList.arraySize, value.Copy);
            }

            // Remove variables that doesn't exist in the State Machine
            if (remove)
            {
                var removeList = runner.Runtime.List.Names.Where(v => !runner.stateMachineAsset.NameList.Names.Contains(v));
                for (var i = 0; i < runner.Runtime.List.Names.Length; i++)
                {
                    var nameVar = runner.Runtime.List.Names[i];
                    if(removeList.Contains(nameVar))
                    {
                        // Debug.Log($"Removing variable that doesn't exist in State Machine {nameVar} with index {i}");
                        fieldRunnerList.DeleteItem(i);
                    }
                }
            }
            fieldRunnerList?.Refresh();
            serializedObject.Update();
        }

        private void RefreshErrorID()
        {
            serializedObject.Update();
            m_Error.style.display = DisplayStyle.None;

            if (PrefabUtility.IsPartOfPrefabAsset(target)) return;

            var saveUniqueID = serializedObject.FindProperty("m_SaveUniqueID");

            var itemID = saveUniqueID
                .FindPropertyRelative(SaveUniqueIDDrawer.PROP_UNIQUE_ID)
                .FindPropertyRelative(UniqueIDDrawer.SERIALIZED_ID)
                .FindPropertyRelative(IdStringDrawer.NAME_STRING)
                .stringValue;

            var variables = FindObjectsOfType<TLocalVariables>(true);
            foreach (var variable in variables)
            {
                if (variable.SaveID != itemID || variable == target) continue;
                m_Error.style.display = DisplayStyle.Flex;

                return;
            }
        }
    }
}