#if UNITY_EDITOR
using GameCreator.Editor.Common;
using UnityEditor;
#endif

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GraphProcessor;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

namespace NinjutsuGames.StateMachine.Runtime
{
    public class GraphChanges
    {
        public SerializableEdge removedEdge;
        public SerializableEdge addedEdge;
        public BaseNode removedNode;
        public BaseNode addedNode;
        public BaseNode nodeChanged;
        public Group addedGroups;
        public Group removedGroups;
        public BaseStackNode addedStackNode;
        public BaseStackNode removedStackNode;
        public StickyNote addedStickyNotes;
        public StickyNote removedStickyNotes;
    }

    /// <summary>
    /// Compute order type used to determine the compute order integer on the nodes
    /// </summary>
    public enum ComputeOrderType
    {
        DepthFirst,
        BreadthFirst,
    }

    [Serializable]
    public class StateMachineAsset : TGlobalVariables, INameVariable, ISerializationCallbackReceiver
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeReference] private NameList m_NameList = new();

        // PROPERTIES: ----------------------------------------------------------------------------

        public NameList NameList => m_NameList;

        public string[] Names => m_NameList.Names;

        // GRAPH: ----------------------------------------------------------------------------

        private static readonly int maxComputeOrderDepth = 1000;

        /// <summary>Invalid compute order number of a node when it's inside a loop</summary>
        public static readonly int loopComputeOrder = -2;

        /// <summary>Invalid compute order number of a node can't process</summary>
        public static readonly int invalidComputeOrder = -1;

        /// <summary>
        /// List of all the nodes in the graph.
        /// </summary>
        /// <typeparam name="BaseNode"></typeparam>
        /// <returns></returns>
        [SerializeReference] public List<BaseNode> nodes = new();

        /// <summary>
        /// Dictionary to access node per GUID, faster than a search in a list
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="BaseNode"></typeparam>
        /// <returns></returns>
        [NonSerialized] public Dictionary<string, BaseNode> nodesPerGUID = new();

        /// <summary>
        /// Json list of edges
        /// </summary>
        /// <typeparam name="SerializableEdge"></typeparam>
        /// <returns></returns>
        [SerializeField] public List<SerializableEdge> edges = new();

        /// <summary>
        /// Dictionary of edges per GUID, faster than a search in a list
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="SerializableEdge"></typeparam>
        /// <returns></returns>
        [NonSerialized] public Dictionary<string, SerializableEdge> edgesPerGUID = new();

        /// <summary>
        /// All groups in the graph
        /// </summary>
        /// <typeparam name="Group"></typeparam>
        /// <returns></returns>
        [SerializeField]
        public List<Group> groups = new();

        /// <summary>
        /// All Stack Nodes in the graph
        /// </summary>
        /// <typeparam name="stackNodes"></typeparam>
        /// <returns></returns>
        [SerializeField, SerializeReference] // Polymorphic serialization
        public List<BaseStackNode> stackNodes = new();
        
        [SerializeReference] public List<string> networkNodes = new();

        /// <summary>
        /// All pinned elements in the graph
        /// </summary>
        /// <typeparam name="PinnedElement"></typeparam>
        /// <returns></returns>
        [SerializeField] public List<PinnedElement> pinnedElements = new();

        /// <summary>
        /// All exposed parameters in the graph
        /// </summary>
        /// <typeparam name="ExposedParameter"></typeparam>
        /// <returns></returns>
        [SerializeField, SerializeReference] public List<ExposedParameter> exposedParameters = new();

        [SerializeField, FormerlySerializedAs("exposedParameters")] // We keep this for upgrade
        private List<ExposedParameter> serializedParameterList = new();

        [SerializeField] public List<StickyNote> stickyNotes = new();

        [NonSerialized] private Dictionary<BaseNode, int> computeOrderDictionary = new();

        [NonSerialized] private Scene linkedScene;

        // Trick to keep the node inspector alive during the editor session
        [SerializeField] public UnityEngine.Object nodeInspectorReference;

        //graph visual properties
        public Vector3 position = Vector3.zero;
        public Vector3 scale = Vector3.one;

        /// <summary>
        /// Triggered when the graph is linked to an active scene.
        /// </summary>
        public event Action<Scene> onSceneLinked;

        /// <summary>
        /// Triggered when the graph is enabled
        /// </summary>
        public event Action onEnabled;

        /// <summary>
        /// Triggered when the graph is changed
        /// </summary>
        public event Action<GraphChanges> onGraphChanges;

        [NonSerialized] private bool _isEnabled = false;

        public bool isEnabled
        {
            get => _isEnabled;
            private set => _isEnabled = value;
        }

        public HashSet<BaseNode> graphOutputs { get; private set; } = new();
        
        /// <summary>
        /// Returns the active state machine asset in the editor
        /// </summary>
        public static StateMachineAsset Active { get; set; }

        protected virtual void OnEnable()
        {
            if (isEnabled)
                OnDisable();

            InitializeGraphElements();
            DestroyBrokenGraphElements();
            // UpdateComputeOrder();
            isEnabled = true;
            onEnabled?.Invoke();
        }

        private void InitializeGraphElements()
        {
            CleanupMissingReferences();
            
            // Sanitize the element lists (it's possible that nodes are null if their full class name have changed)
            // If you rename / change the assembly of a node or parameter, please use the MovedFrom() attribute to avoid breaking the graph.
            nodes.RemoveAll(n => n == null);
            exposedParameters.RemoveAll(e => e == null);

            foreach (var node in nodes.ToList())
            {
                nodesPerGUID[node.GUID] = node;
                node.Initialize(this);

                if (node is ActionsNode {networkingSettings: {networkSync: true}} && !networkNodes.Contains(node.GUID))
                {
                    networkNodes.Add(node.GUID);
                }
            }

            foreach (var edge in edges.ToList())
            {
                edge.Deserialize();
                edgesPerGUID[edge.GUID] = edge;

                // Sanity check for the edge:
                if (edge.inputPort == null || edge.outputPort == null)
                {
                    Disconnect(edge.GUID);
                    continue;
                }

                // Add the edge to the non-serialized port data
                edge.inputPort.owner.OnEdgeConnected(edge);
                edge.outputPort.owner.OnEdgeConnected(edge);
            }
            
            // Remove non existing nodes from the network nodes list
            networkNodes.RemoveAll(n => string.IsNullOrEmpty(n) || !nodesPerGUID.ContainsKey(n));
        }

        protected virtual void OnDisable()
        {
            isEnabled = false;
            foreach (var node in nodes)
                node.DisableInternal();
        }

        public virtual void OnAssetDeleted()
        {
        }

        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public BaseNode AddNode(BaseNode node)
        {
            nodesPerGUID[node.GUID] = node;

            nodes.Add(node);
            node.Initialize(this);

            onGraphChanges?.Invoke(new GraphChanges {addedNode = node});

            return node;
        }

        /// <summary>
        /// Removes a node from the graph
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(BaseNode node)
        {
            node.DisableInternal();
            node.DestroyInternal();

            nodesPerGUID.Remove(node.GUID);

            nodes.Remove(node);
            if(networkNodes.Contains(node.GUID)) networkNodes.Remove(node.GUID);

            onGraphChanges?.Invoke(new GraphChanges {removedNode = node});
        }

        /// <summary>
        /// Connect two ports with an edge
        /// </summary>
        /// <param name="inputPort">input port</param>
        /// <param name="outputPort">output port</param>
        /// <param name="DisconnectInputs">is the edge allowed to disconnect another edge</param>
        /// <returns>the connecting edge</returns>
        public SerializableEdge Connect(NodePort inputPort, NodePort outputPort, bool autoDisconnectInputs = true)
        {
            var edge = SerializableEdge.CreateNewEdge(this, inputPort, outputPort);

            //If the input port does not support multi-connection, we remove them
            if (autoDisconnectInputs && !inputPort.portData.acceptMultipleEdges)
            {
                foreach (var e in inputPort.GetEdges().ToList())
                {
                    // TODO: do not disconnect them if the connected port is the same than the old connected
                    Disconnect(e);
                }
            }

            // same for the output port:
            if (autoDisconnectInputs && !outputPort.portData.acceptMultipleEdges)
            {
                foreach (var e in outputPort.GetEdges().ToList())
                {
                    // TODO: do not disconnect them if the connected port is the same than the old connected
                    Disconnect(e);
                }
            }

            edges.Add(edge);

            // Add the edge to the list of connected edges in the nodes
            inputPort.owner.OnEdgeConnected(edge);
            outputPort.owner.OnEdgeConnected(edge);

            onGraphChanges?.Invoke(new GraphChanges {addedEdge = edge});

            return edge;
        }

        /// <summary>
        /// Disconnect two ports
        /// </summary>
        /// <param name="inputNode">input node</param>
        /// <param name="inputFieldName">input field name</param>
        /// <param name="outputNode">output node</param>
        /// <param name="outputFieldName">output field name</param>
        public void Disconnect(BaseNode inputNode, string inputFieldName, BaseNode outputNode, string outputFieldName)
        {
            edges.RemoveAll(r =>
            {
                var remove = r.inputNode == inputNode
                             && r.outputNode == outputNode
                             && r.outputFieldName == outputFieldName
                             && r.inputFieldName == inputFieldName;

                if (remove)
                {
                    r.inputNode?.OnEdgeDisconnected(r);
                    r.outputNode?.OnEdgeDisconnected(r);
                    onGraphChanges?.Invoke(new GraphChanges {removedEdge = r});
                }

                return remove;
            });
        }

        /// <summary>
        /// Disconnect an edge
        /// </summary>
        /// <param name="edge"></param>
        public void Disconnect(SerializableEdge edge) => Disconnect(edge.GUID);

        /// <summary>
        /// Disconnect an edge
        /// </summary>
        /// <param name="edgeGUID"></param>
        public void Disconnect(string edgeGUID)
        {
            var disconnectEvents = new List<(BaseNode, SerializableEdge)>();

            edges.RemoveAll(r =>
            {
                if (r.GUID != edgeGUID) return r.GUID == edgeGUID;
                disconnectEvents.Add((r.inputNode, r));
                disconnectEvents.Add((r.outputNode, r));
                onGraphChanges?.Invoke(new GraphChanges {removedEdge = r});

                return r.GUID == edgeGUID;
            });

            // Delay the edge disconnect event to avoid recursion
            foreach (var (node, edge) in disconnectEvents)
                node?.OnEdgeDisconnected(edge);
        }

        /// <summary>
        /// Add a group
        /// </summary>
        /// <param name="block"></param>
        public void AddGroup(Group block)
        {
            groups.Add(block);
            onGraphChanges?.Invoke(new GraphChanges {addedGroups = block});
        }

        /// <summary>
        /// Removes a group
        /// </summary>
        /// <param name="block"></param>
        public void RemoveGroup(Group block)
        {
            groups.Remove(block);
            onGraphChanges?.Invoke(new GraphChanges {removedGroups = block});
        }

        /// <summary>
        /// Add a StackNode
        /// </summary>
        /// <param name="stackNode"></param>
        public void AddStackNode(BaseStackNode stackNode)
        {
            stackNodes.Add(stackNode);
            onGraphChanges?.Invoke(new GraphChanges {addedStackNode = stackNode});
        }

        /// <summary>
        /// Remove a StackNode
        /// </summary>
        /// <param name="stackNode"></param>
        public void RemoveStackNode(BaseStackNode stackNode)
        {
            stackNodes.Remove(stackNode);
            onGraphChanges?.Invoke(new GraphChanges {removedStackNode = stackNode});
        }

        /// <summary>
        /// Add a sticky note 
        /// </summary>
        /// <param name="note"></param>
        public void AddStickyNote(StickyNote note)
        {
            stickyNotes.Add(note);
            onGraphChanges?.Invoke(new GraphChanges {addedStickyNotes = note});
        }

        /// <summary>
        /// Removes a sticky note 
        /// </summary>
        /// <param name="note"></param>
        public void RemoveStickyNote(StickyNote note)
        {
            stickyNotes.Remove(note);
            onGraphChanges?.Invoke(new GraphChanges {removedStickyNotes = note});
        }

        /// <summary>
        /// Invoke the onGraphChanges event, can be used as trigger to execute the graph when the content of a node is changed 
        /// </summary>
        /// <param name="node"></param>
        public void NotifyNodeChanged(BaseNode node) => onGraphChanges?.Invoke(new GraphChanges {nodeChanged = node});

        /// <summary>
        /// Open a pinned element of type viewType
        /// </summary>
        /// <param name="viewType">type of the pinned element</param>
        /// <returns>the pinned element</returns>
        public PinnedElement OpenPinned(Type viewType)
        {
            var pinned = pinnedElements.Find(p => p.editorType.type == viewType);

            if (pinned == null)
            {
                pinned = new PinnedElement(viewType);
                pinnedElements.Add(pinned);
            }
            else
                pinned.opened = true;

            return pinned;
        }

        /// <summary>
        /// Closes a pinned element of type viewType
        /// </summary>
        /// <param name="viewType">type of the pinned element</param>
        public void ClosePinned(Type viewType)
        {
            var pinned = pinnedElements.Find(p => p.editorType.type == viewType);

            pinned.opened = false;
        }

        public void OnBeforeSerialize()
        {
            // Cleanup broken elements
            stackNodes.RemoveAll(s => s == null);
            nodes.RemoveAll(n => n == null);

            CleanupMissingReferences();
        }

        

        // We can deserialize data here because it's called in a unity context
        // so we can load objects references
        public void Deserialize()
        {
            // Disable nodes correctly before removing them:
            if (nodes != null)
            {
                foreach (var node in nodes)
                    node.DisableInternal();
            }

            InitializeGraphElements();
        }

        public void OnAfterDeserialize()
        {
            // We can't deserialize data here because it's called in a non-unity context
            // so we can't load objects references
        }

        /// <summary>
        /// Update the compute order of the nodes in the graph
        /// </summary>
        /// <param name="type">Compute order type</param>
        public void UpdateComputeOrder(ComputeOrderType type = ComputeOrderType.DepthFirst)
        {
            if (nodes.Count == 0)
                return;

            // Find graph outputs (end nodes) and reset compute order
            graphOutputs.Clear();
            foreach (var node in nodes)
            {
                if (!node.GetOutputNodes().Any())
                {
                    graphOutputs.Add(node);
                }
                node.computeOrder = 0;
            }

            computeOrderDictionary.Clear();
            infiniteLoopTracker.Clear();

            switch (type)
            {
                default:
                case ComputeOrderType.DepthFirst:
                    UpdateComputeOrderDepthFirst();
                    break;
                case ComputeOrderType.BreadthFirst:
                    foreach (var node in nodes)
                        UpdateComputeOrderBreadthFirst(0, node);
                    break;
            }
        }
        
        /// <summary>
        /// Returns true if the node is running
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsNodeRunning(string nodeId, GameObject context = null)
        {
            if(!CheckNode(nodeId)) return false;
            var node = nodesPerGUID[nodeId] as BaseGameCreatorNode;
            return node.IsRunning(context);
        }
        
        /// <summary>
        /// Returns true if the node is enabled
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsNodeEnabled(string nodeId, GameObject context = null)
        {
            if(!CheckNode(nodeId)) return false;
            var node = nodesPerGUID[nodeId] as BaseGameCreatorNode;
            return node.enabledForExecution;
        }
        
        /// <summary>
        /// Run a node.
        /// If the context is null the action run in the State Machine asset.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context">State Machine Runner object</param>
        public void RunNode(string nodeId, GameObject context = null)
        {
            if(!CheckNode(nodeId)) return;
            nodesPerGUID[nodeId].OnProcess(context);
        }
        
        /// <summary>
        /// Stop a node
        /// If the context is null the action run in the State Machine asset.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context">State Machine Runner object</param>
        public void StopNode(string nodeId, GameObject context = null)
        {
            if(!CheckNode(nodeId)) return;
            var node = nodesPerGUID[nodeId] as BaseGameCreatorNode;
            node?.Stop(context);
        }
        
        /// <summary>
        /// Disable a node
        /// If the context is null the action run in the State Machine asset. 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context">State Machine Runner object</param>
        public void DisableNode(string nodeId, GameObject context = null)
        {
            if(!CheckNode(nodeId)) return;
            var node = nodesPerGUID[nodeId] as BaseGameCreatorNode;
            node.Disable(context);
        }
        
        /// <summary>
        /// Enable a node.
        /// If the context is null the action run in the State Machine asset. 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="context">State Machine Runner object</param>
        public void EnableNode(string nodeId, GameObject context = null)
        {
            if(!CheckNode(nodeId)) return;
            var node = nodesPerGUID[nodeId] as BaseGameCreatorNode;
            node.Enable(context);
        }

        /// <summary>
        /// Link the current graph to the scene in parameter, allowing the graph to pick and serialize objects from the scene.
        /// </summary>
        /// <param name="scene">Target scene to link</param>
        public void LinkToScene(Scene scene)
        {
            linkedScene = scene;
            onSceneLinked?.Invoke(scene);
        }

        /// <summary>
        /// Return true when the graph is linked to a scene, false otherwise.
        /// </summary>
        public bool IsLinkedToScene() => linkedScene.IsValid();

        /// <summary>
        /// Get the linked scene. If there is no linked scene, it returns an invalid scene
        /// </summary>
        public Scene GetLinkedScene() => linkedScene;

        /// <summary>
        /// Get the linked context. If there is no linked context, it returns null
        /// </summary>
        /// <returns></returns>
        // public List<StateMachineRunner> GetLinkedContexts() => linkedContexts;
        private HashSet<BaseNode> infiniteLoopTracker = new();
        
        private void CleanupMissingReferences()
        {
#if UNITY_EDITOR
            // If the graph contains managed references with missing types, we clear them
            if (SerializationUtility.HasManagedReferencesWithMissingTypes(this))
            {
                var missingTypes = SerializationUtility.GetManagedReferencesWithMissingTypes(this);
                foreach (var missingType in missingTypes)
                {
                    SerializationUtility.ClearManagedReferenceWithMissingType(this, missingType.referenceId);
                }
            }
#endif
        }
        private bool CheckNode(string nodeId)
        {
            if(!nodesPerGUID.ContainsKey(nodeId))
            {
                Debug.Log($"Couldn't find node with id: {nodeId} in graph: {name}. Make sure you are targeting the correct State Machine Runner.");
                return false;
            }

            return true;
        }

        private int UpdateComputeOrderBreadthFirst(int depth, BaseNode node)
        {
            var computeOrder = 0;

            if (depth > maxComputeOrderDepth)
            {
                Debug.LogError("Recursion error while updating compute order");
                return -1;
            }

            if (computeOrderDictionary.ContainsKey(node))
                return node.computeOrder;

            if (!infiniteLoopTracker.Add(node))
                return -1;

            if (!node.canProcess)
            {
                node.computeOrder = -1;
                computeOrderDictionary[node] = -1;
                return -1;
            }

            foreach (var dep in node.GetInputNodes())
            {
                var c = UpdateComputeOrderBreadthFirst(depth + 1, dep);

                if (c == -1)
                {
                    computeOrder = -1;
                    break;
                }

                computeOrder += c;
            }

            if (computeOrder != -1)
                computeOrder++;

            node.computeOrder = computeOrder;
            computeOrderDictionary[node] = computeOrder;

            return computeOrder;
        }

        private void UpdateComputeOrderDepthFirst()
        {
            var dfs = new Stack<BaseNode>();

            GraphUtils.FindCyclesInGraph(this, (n) => { PropagateComputeOrder(n, loopComputeOrder); });

            var computeOrder = 0;
            foreach (var node in GraphUtils.DepthFirstSort(this))
            {
                if (node.computeOrder == loopComputeOrder)
                    continue;
                if (!node.canProcess)
                    node.computeOrder = -1;
                else
                    node.computeOrder = computeOrder++;
            }
        }

        private void PropagateComputeOrder(BaseNode node, int computeOrder)
        {
            var deps = new Stack<BaseNode>();
            var loop = new HashSet<BaseNode>();

            deps.Push(node);
            while (deps.Count > 0)
            {
                var n = deps.Pop();
                n.computeOrder = computeOrder;

                if (!loop.Add(n))
                    continue;

                foreach (var dep in n.GetOutputNodes())
                    deps.Push(dep);
            }
        }

        private void DestroyBrokenGraphElements()
        {
            edges.RemoveAll(e => e.inputNode == null
                                 || e.outputNode == null
                                 || string.IsNullOrEmpty(e.outputFieldName)
                                 || string.IsNullOrEmpty(e.inputFieldName)
            );
            nodes.RemoveAll(n => n == null);
        }

        /// <summary>
        /// Tell if two types can be connected in the context of a graph
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool TypesAreConnectable(Type t1, Type t2)
        {
            if (t1 == null || t2 == null)
                return false;

            if (TypeAdapter.AreIncompatible(t1, t2))
                return false;

            //Check if there is custom adapters for this assignation
            if (CustomPortIO.IsAssignable(t1, t2))
                return true;

            //Check for type assignability
            if (t2.IsReallyAssignableFrom(t1))
                return true;

            // User defined type convertions
            if (TypeAdapter.AreAssignable(t1, t2))
                return true;

            return false;
        }
        
        // NODE PUBLIC METHODS: ------------------------------------------------------------------------

        public bool ExistsNode(string name)
        {
            return StateMachineVariablesManager.Instance.ExistsNode(this, name);
        }

        public object GetNode(string name)
        {
            return StateMachineVariablesManager.Instance.GetNode(this, name);
        }

        // GLOBAL VARIABLES PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Exists(string name)
        {
            return StateMachineVariablesManager.Instance.Exists(this, name);
        }

        public object Get(string name)
        {
            return ApplicationManager.IsExiting ? null : StateMachineVariablesManager.Instance.Get(this, name);
        }

        public void Set(string name, object value)
        {
            if (ApplicationManager.IsExiting) return;
            StateMachineVariablesManager.Instance.Set(this, name, value);
        }

        public void Register(Action<string> callback)
        {
            if (ApplicationManager.IsExiting) return;
            StateMachineVariablesManager.Instance.Register(this, callback);
        }

        public void Unregister(Action<string> callback)
        {
            if (ApplicationManager.IsExiting) return;
            StateMachineVariablesManager.Instance.Unregister(this, callback);
        }
        
        // LOCAL VARIABLES PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool Exists(string name, GameObject context)
        {
            var runner = context.Get<StateMachineRunner>();
            if (!runner)
            {
                return Exists(name);
            }
            return runner.Exists(name);
        }

        public object Get(string name, GameObject context)
        {
            var runner = context.Get<StateMachineRunner>();
            if (!runner)
            {
                return Get(name);
            }
            return runner.Get(name);
        }

        public void Set(string name, object value, GameObject context)
        {
            var runner = context.Get<StateMachineRunner>();
            if (!runner)
            {
                Set(name, value);
                return;
            }
            runner.Set(name, value);
        }

        public void Register(Action<string> callback, GameObject context)
        {
            if (ApplicationManager.IsExiting) return;
            var runner = context.Get<StateMachineRunner>();
            if (!runner)
            {
                Register(callback);
                return;
            }
            runner.Register(callback);
        }

        public void Unregister(Action<string> callback, GameObject context)
        {
            if (ApplicationManager.IsExiting) return;
            var runner = context.Get<StateMachineRunner>();
            if (!runner)
            {
                Unregister(callback);
                return;
            }
            runner.Unregister(callback);
        }

        // EDITOR METHODS: ------------------------------------------------------------------------

        public string Title(string name)
        {
            return StateMachineVariablesManager.Instance.Title(this, name);
        }

        public Texture Icon(string name)
        {
            return StateMachineVariablesManager.Instance.Icon(this, name);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Called when variables are changed in the editor. This is an Editor Only internal event.
        /// </summary>
        public static Action OnVariablesChanged;
        public void SyncVariablesInternal(NameList runtimeList)
        {
            // Add new variables from runner
            for (var i = 0; i < runtimeList.Names.Length; i++)
            {
                var nameVar = runtimeList.Names[i];
                var exists = NameList.Names.Any(v => v == nameVar);
                if (exists) continue;

                var value = runtimeList.Get(i);
                // Debug.Log($"Adding to State Machine {nameVar} with value {value} index {i}");
                OnVariablesChanged?.Invoke();

                var serializedObject = new SerializedObject(this);
                serializedObject.Update();

                var propertyList = serializedObject.FindProperty("m_NameList").FindPropertyRelative("m_Source");

                var index = propertyList.arraySize;
                propertyList.InsertArrayElementAtIndex(index);
                propertyList.GetArrayElementAtIndex(index).SetValue(value);

                SerializationUtils.ApplyUnregisteredSerialization(serializedObject);
            }
            
            // Remove variables that doesn't exist in runner
            for (var i = 0; i < NameList.Names.Length; i++)
            {
                var nameVar = NameList.Names[i];
                var exists = runtimeList.Names.Any(v => v == nameVar);
                if (exists) continue;
                OnVariablesChanged?.Invoke();
                // Debug.Log($"Removing to State Machine {nameVar} with index {i}");

                var serializedObject = new SerializedObject(this);
                serializedObject.Update();

                var propertyList = serializedObject.FindProperty("m_NameList").FindPropertyRelative("m_Source");

                propertyList.DeleteArrayElementAtIndex(i);
                SerializationUtils.ApplyUnregisteredSerialization(serializedObject);
            }
        }
#endif
    }
}