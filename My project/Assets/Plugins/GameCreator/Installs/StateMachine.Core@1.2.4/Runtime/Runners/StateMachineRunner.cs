using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [AddComponentMenu("Game Creator/State Machine/State Machine Runner")]
    public class StateMachineRunner : TLocalVariables, INameVariable
    {
        public StateMachineAsset stateMachineAsset;
        private BaseGraphProcessor processor;
        
        
        // MEMBERS: -------------------------------------------------------------------------------
    
        [SerializeReference] private NameVariableRuntime m_Runtime = new();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public IdString UniqueId => m_SaveUniqueID.Get;

        public NameVariableRuntime Runtime => m_Runtime;
        
        // EVENTS: --------------------------------------------------------------------------------
        
        private event Action<string> EventChange;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void Awake()
        {
            m_Runtime.OnStartup();
            m_Runtime.EventChange += OnRuntimeChange;
            
            base.Awake();
            
            if (stateMachineAsset != null) processor = new StateMachineGraphProcessor(stateMachineAsset, gameObject);
            // stateMachineAsset.LinkToObject(this);
        }
        
        private void Start()
        {
            processor?.Run();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CacheUtils.Prune();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        /// <summary>
        /// Returns true if the variable exists 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return m_Runtime.Exists(name);
        }
        
        /// <summary>
        /// Returns the value of the variable 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Get(string name)
        {
            return m_Runtime.Get(name);
        }

        /// <summary>
        /// Sets the value of the variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            m_Runtime.Set(name, value);
        }

        /// <summary>
        /// Registers a callback to be invoked when the variable changes
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="target"></param>
        public void Register(Action<string> callback, GameObject target)
        {
            EventChange += callback;
        }

        /// <summary>
        /// Unregisters a callback to be invoked when the variable changes
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="target"></param>
        public void Unregister(Action<string> callback, GameObject target)
        {
            EventChange -= callback;
        }

        /// <summary>
        /// Registers a callback to be invoked when the variable changes
        /// </summary>
        /// <param name="callback"></param>
        public void Register(Action<string> callback)
        {
            EventChange += callback;
        }
        
        /// <summary>
        /// Unregisters a callback to be invoked when the variable changes
        /// </summary>
        /// <param name="callback"></param>
        public void Unregister(Action<string> callback)
        {
            EventChange -= callback;
        }
        
        /// <summary>
        /// Runs a node.
        /// If the context is null the action run in the State Machine asset. 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context"></param>
        public void RunNode(string nodeId, GameObject context)
        {
            processor.RunNode(nodeId, context);
        }
        
        /// <summary>
        /// Stops a node.
        /// If the context is null the action run in the State Machine asset. 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context"></param>
        public void StopNode(string nodeId, GameObject context)
        {
            processor.StopNode(nodeId, context);
        }

        /// <summary>
        /// Disables a node.
        /// If the context is null the action run in the State Machine asset. 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context"></param>
        public void DisableNode(string nodeId, GameObject context)
        {
            processor.DisableNode(nodeId, context);
        }

        /// <summary>
        /// Enables a node.
        /// If the context is null the action run in the State Machine asset. 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context"></param>
        public void EnableNode(string nodeId, GameObject context)
        {
            processor.EnableNode(nodeId, context);
        }
        
        public bool IsNodeEnabled(string nodeId, GameObject context)
        {
            return processor.IsNodeEnabled(nodeId, context);
        }
        
        public bool IsNodeRunning(string nodeId, GameObject context)
        {
            return processor.IsNodeRunning(nodeId, context);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void OnRuntimeChange(string name)
        {
            EventChange?.Invoke(name);
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public override Type SaveType => typeof(SaveSingleNameVariables);

        public override object GetSaveData(bool includeNonSavable)
        {
            return m_SaveUniqueID.SaveValue
                ? new SaveSingleNameVariables(m_Runtime)
                : null;   
        }

        public override Task OnLoad(object value)
        {
            SaveSingleNameVariables saveData = value as SaveSingleNameVariables;
            if (saveData != null && m_SaveUniqueID.SaveValue)
            {
                NameVariable[] candidates = saveData.Variables.ToArray();
                foreach (NameVariable candidate in candidates)
                {
                    if (!m_Runtime.Exists(candidate.Name)) continue;
                    m_Runtime.Set(candidate.Name, candidate.Value);
                }
            }
            
            return Task.FromResult(saveData != null || !m_SaveUniqueID.SaveValue);
        }
    }
}