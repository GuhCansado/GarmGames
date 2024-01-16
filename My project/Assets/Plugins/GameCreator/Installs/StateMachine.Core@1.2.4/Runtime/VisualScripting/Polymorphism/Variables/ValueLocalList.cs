using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Title("Local List Variables")]
    [Category("State Machine/Local List Variables")]
    
    [Serializable]
    public class ValueLocalList : TValue
    {
        public static readonly IdString TYPE_ID = new("local-list-variables");
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private LocalListVariables m_Value;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override IdString TypeID => TYPE_ID;
        public override Type Type => typeof(LocalListVariables);
        
        public override bool CanSave => false;

        public override TValue Copy => new ValueLocalList
        {
            m_Value = this.m_Value
        };
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public ValueLocalList() : base()
        { }

        public ValueLocalList(LocalListVariables value) : this()
        {
            this.m_Value = value;
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        protected override object Get()
        {
            return this.m_Value;
        }

        protected override void Set(object value)
        {
            this.m_Value = value is LocalListVariables cast ? cast : null;
        }
        
        public override string ToString()
        {
            return this.m_Value != null ? this.m_Value.name : "(none)";
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RuntimeInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueLocalList), CreateValue),
            typeof(LocalListVariables)
        );
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInit() => RegisterValueType(
            TYPE_ID, 
            new TypeData(typeof(ValueLocalList), CreateValue),
            typeof(LocalListVariables)
        );
        
        #endif

        private static ValueLocalList CreateValue(object value)
        {
            return new ValueLocalList(value as LocalListVariables);
        }
    }
}