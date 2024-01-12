using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Disable Input Map")]
    [Description("Disables an Input Action asset with a Map value which stops reading user input")]

    [Category("Input/Disable Input Map")]

    [Parameter("Input Asset", "The Input Asset reference")]

    [Keywords("Deactivate", "Inactive")]
    [Image(typeof(IconBoltOutline), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionInputMapAssetDisable : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private InputMapFromAsset m_InputAsset = new InputMapFromAsset();

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => $"Disable {this.m_InputAsset}";

        // METHODS: -------------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            InputActionMap inputMap = this.m_InputAsset.InputMap;
            if (inputMap is { enabled: true }) inputMap.Disable();
            
            return DefaultResult;
        }
    }
}
