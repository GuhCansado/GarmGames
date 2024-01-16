using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Variable")]
    [Category("Variables/State Machine Variable")]

    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the Audio Clip value of a State Machine Variable")]

    [Serializable]
    public class GetAudioClipStateMachine : PropertyTypeGetAudio
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueAudioClip.TYPE_ID);

        public override AudioClip Get(Args args) => m_Variable.Get<AudioClip>(args);

        public override string String => m_Variable.ToString();
    }
}
