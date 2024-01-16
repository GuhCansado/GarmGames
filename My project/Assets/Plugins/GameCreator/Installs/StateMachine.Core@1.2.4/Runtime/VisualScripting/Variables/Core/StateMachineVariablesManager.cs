using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
// using GameCreator.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [AddComponentMenu("")]
    public class StateMachineVariablesManager : Singleton<StateMachineVariablesManager>, IGameSave
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnSubsystemsInit()
        {
            Instance.WakeUp();
        }
        
        // PROPERTIES: ----------------------------------------------------------------------------

        [field: NonSerialized] private Dictionary<IdString, List<BaseNode>> NodeValues;

        [field: NonSerialized] private Dictionary<IdString, NameVariableRuntime> Values { get; set; }

        [field: NonSerialized] private HashSet<IdString> SaveValues { get; set; }

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();
            
            NodeValues = new Dictionary<IdString, List<BaseNode>>();
            Values = new Dictionary<IdString, NameVariableRuntime>();
            SaveValues = new HashSet<IdString>();
            
            var nameVariables = StateMachineRepository.Get.StateMachineAssets.Assets;
            foreach (var entry in nameVariables)
            {
                if (entry == null) return;
                Instance.RequireInit(entry);   
            }

            _ = SaveLoadManager.Subscribe(this);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool ExistsNode(StateMachineAsset asset, string name)
        {
            return NodeValues.TryGetValue(asset.UniqueID, out var runtime);
        }
        
        public object GetNode(StateMachineAsset asset, string name)
        {
            return Values.TryGetValue(asset.UniqueID, out var runtime);
        }
        
        public bool Exists(StateMachineAsset asset, string name)
        {
            return Values.TryGetValue(
                asset.UniqueID,
                out var runtime
            ) && runtime.Exists(name);
        }
        
        public object Get(StateMachineAsset asset, string name)
        {
            return Values.TryGetValue(asset.UniqueID, out var runtime) 
                ? runtime.Get(name) 
                : null;
        }
        
        public string Title(StateMachineAsset asset, string name)
        {
            return Values.TryGetValue(asset.UniqueID, out var runtime) 
                ? runtime.Title(name) 
                : string.Empty;
        }
        
        public Texture Icon(StateMachineAsset asset, string name)
        {
            return Values.TryGetValue(asset.UniqueID, out var runtime) 
                ? runtime.Icon(name) 
                : null;
        }

        public void Set(StateMachineAsset asset, string name, object value)
        {
            if (!Values.TryGetValue(asset.UniqueID, out var runtime)) return;
            
            runtime.Set(name, value);
            if (asset.Save) this.SaveValues.Add(asset.UniqueID);
        }

        public void Register(StateMachineAsset asset, Action<string> callback)
        {
            if (Values.TryGetValue(asset.UniqueID, out var runtime))
            {
                runtime.EventChange += callback;
            }
        }
        
        public void Unregister(StateMachineAsset asset, Action<string> callback)
        {
            if (Values.TryGetValue(asset.UniqueID, out var runtime))
            {
                runtime.EventChange -= callback;
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RequireInit(StateMachineAsset asset)
        {
            if (Values.ContainsKey(asset.UniqueID)) return;
            
            var runtime = new NameVariableRuntime(asset.NameList);
            runtime.OnStartup();

            Values[asset.UniqueID] = runtime;
        }
        
        private void RequireInitNode(StateMachineAsset asset)
        {
            if (NodeValues.ContainsKey(asset.UniqueID)) return;
            
            NodeValues[asset.UniqueID] = asset.nodes;
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public string SaveID => "state-machine-name-variables";

        public LoadMode LoadMode => LoadMode.Greedy;
        public bool IsShared => false;

        public Type SaveType => typeof(SaveGroupNameVariables);

        public object GetSaveData(bool includeNonSavable)
        {
            var saveValues = new Dictionary<string, NameVariableRuntime>();
                        
            foreach (var entry in this.Values)
            {
                if (includeNonSavable)
                {
                    saveValues[entry.Key.String] = entry.Value;
                    continue;
                }

                var asset = StateMachineRepository.Get.StateMachineAssets.GetNameVariablesAsset(entry.Key);
                if (asset == null || !asset.Save) continue;
                
                saveValues[entry.Key.String] = entry.Value;
            }

            var saveData = new SaveGroupNameVariables(saveValues);
            return saveData;
        }

        public Task OnLoad(object value)
        {
            if (value is not SaveGroupNameVariables saveData) return Task.FromResult(false);
        
            var numGroups = saveData.Count();
            for (var i = 0; i < numGroups; ++i)
            {
                var uniqueID = new IdString(saveData.GetID(i));
                var candidates = saveData.GetData(i).Variables;

                if (!Values.TryGetValue(uniqueID, out var variables))
                {
                    continue;
                }
                
                foreach (var candidate in candidates)
                {
                    if (!variables.Exists(candidate.Name)) continue;
                    variables.Set(candidate.Name, candidate.Value);
                }
            }
            
            return Task.FromResult(true);
        }
    }
}