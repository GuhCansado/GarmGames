using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Runner Variable")]
    [Category("Variables/State Machine Runner Variable")]

    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]
    [Description("Returns the Audio Clip value of a State Machine Runner Variable")]

    [Serializable]
    public class GetAudioClipStateMachineRunner : PropertyTypeGetAudio
    {
        [SerializeField]
        protected FieldGetStateMachineRunner m_Variable = new(ValueAudioClip.TYPE_ID);

        public override AudioClip Get(Args args) => m_Variable.Get<AudioClip>(args);

        public override string String => m_Variable.ToString();
    }
}
