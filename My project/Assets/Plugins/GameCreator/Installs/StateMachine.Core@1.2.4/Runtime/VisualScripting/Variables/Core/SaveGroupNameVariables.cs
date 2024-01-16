using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    // [HelpURL("https://docs.gamecreator.io/gamecreator/variables/local-name-variables")]
    // [AddComponentMenu("Game Creator/Variables/Local Name Variables")]
    // [Icon(RuntimePaths.GIZMOS + "GizmoLocalNameVariables.png")]

    /*[Serializable]
    public class LocalNameVariables : TLocalVariables, INameVariable
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] public NameVariableRuntime m_Runtime = new();

        // PROPERTIES: ----------------------------------------------------------------------------

        internal NameVariableRuntime Runtime => m_Runtime;

        // EVENTS: --------------------------------------------------------------------------------

        private event Action<string> EventChange;

        // INITIALIZERS: --------------------------------------------------------------------------

        protected override void Awake()
        {
            m_Runtime.OnStartup();
            m_Runtime.EventChange += OnRuntimeChange;

            base.Awake();
        }

        public static LocalNameVariables Create(GameObject target, NameVariableRuntime variables)
        {
            LocalNameVariables instance = target.Add<LocalNameVariables>();
            instance.m_Runtime = variables;
            instance.m_Runtime.OnStartup();

            instance.m_Runtime.EventChange += instance.OnRuntimeChange;
            return instance;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Exists(string name)
        {
            return m_Runtime.Exists(name);
        }

        public object Get(string name)
        {
            return m_Runtime.Get(name);
        }

        public void Set(string name, object value)
        {
            m_Runtime.Set(name, value);
        }

        public void Register(Action<string> callback)
        {
            EventChange += callback;
        }

        public void Unregister(Action<string> callback)
        {
            EventChange -= callback;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        public void OnRuntimeChange(string name)
        {
            EventChange?.Invoke(name);
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public override Type SaveType => typeof(SaveSingleNameVariables);

        public override object SaveData => m_SaveUniqueID.SaveValue
            ? new SaveSingleNameVariables(m_Runtime)
            : null;

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
    }*/

    [Serializable]
    public class NameVariableRuntime : TVariableRuntime<NameVariable>
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeReference] private NameList m_List = new();

        // PROPERTIES: ----------------------------------------------------------------------------

        public NameList List => m_List;
        public Dictionary<string, NameVariable> Variables { get; set; }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<string> EventChange;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public NameVariableRuntime()
        {
            Variables = new Dictionary<string, NameVariable>();
        }

        public NameVariableRuntime(NameList nameList) : this()
        {
            m_List = nameList;
        }

        public NameVariableRuntime(params NameVariable[] nameList) : this()
        {
            m_List = new NameList(nameList);
        }

        // INITIALIZERS: --------------------------------------------------------------------------

        public override void OnStartup()
        {
            Variables = new Dictionary<string, NameVariable>();

            for (var i = 0; i < m_List.Length; ++i)
            {
                var variable = m_List.Get(i);
                if (variable == null) continue;

                if (Variables.ContainsKey(variable.Name)) continue;
                Variables.Add(variable.Name, variable.Copy as NameVariable);
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Exists(string name)
        {
            return Variables.ContainsKey(name);
        }

        public object Get(string name)
        {
            var variable = AccessRuntimeVariable(name);
            return variable?.Value;
        }

        public string Title(string name)
        {
            var variable = AccessRuntimeVariable(name);
            return variable?.Title;
        }

        public Texture Icon(string name)
        {
            var variable = AccessRuntimeVariable(name);
            return variable?.Icon;
        }

        public void Set(string name, object value)
        {
            var variable = AccessRuntimeVariable(name);

            if (variable == null) return;

            variable.Value = value;
            EventChange?.Invoke(name);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private NameVariable AccessRuntimeVariable(string name)
        {
            var keys = name.Split('/', 2, StringSplitOptions.RemoveEmptyEntries);

            var variable = Variables.TryGetValue(keys[0], out var entry) ? entry : null;

            if (keys.Length == 1) return variable;
            if (variable?.Value is not GameObject gameObject) return null;

            var variables = gameObject.Get<StateMachineRunner>();
            return variables != null ? variables.Runtime.AccessRuntimeVariable(keys[1]) : null;
        }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        public override IEnumerator<NameVariable> GetEnumerator()
        {
            return Variables.Values.GetEnumerator();
        }
    }

    [Serializable]
    public class SaveSingleNameVariables
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private List<NameVariable> m_Variables;

        // PROPERTIES: ----------------------------------------------------------------------------

        public List<NameVariable> Variables => m_Variables;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SaveSingleNameVariables(NameVariableRuntime runtime)
        {
            m_Variables = new List<NameVariable>();
            foreach (var entry in runtime.Variables)
            {
                m_Variables.Add(entry.Value.Copy as NameVariable);
            }
        }
    }

    [Serializable]
    public class SaveGroupNameVariables
    {
        [Serializable]
        private class Group
        {
            [SerializeField] private string m_ID;
            [SerializeField] private SaveSingleNameVariables m_Data;

            public string ID => m_ID;
            public SaveSingleNameVariables Data => m_Data;

            public Group(string id, NameVariableRuntime runtime)
            {
                m_ID = id;
                m_Data = new SaveSingleNameVariables(runtime);
            }
        }

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private List<Group> m_Groups;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public SaveGroupNameVariables(Dictionary<string, NameVariableRuntime> runtime)
        {
            m_Groups = new List<Group>();

            foreach (var entry in runtime)
            {
                m_Groups.Add(new Group(entry.Key, entry.Value));
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public int Count()
        {
            return m_Groups?.Count ?? 0;
        }

        public string GetID(int index)
        {
            return m_Groups?[index].ID ?? string.Empty;
        }

        public SaveSingleNameVariables GetData(int index)
        {
            return m_Groups?[index].Data;
        }
    }
}