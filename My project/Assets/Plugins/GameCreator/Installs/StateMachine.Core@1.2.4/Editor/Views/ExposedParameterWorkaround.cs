using System;
using System.Collections.Generic;
using NinjutsuGames.StateMachine.Runtime;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Editor
{
    [Serializable]
    public class ExposedParameterWorkaround : ScriptableObject
    {
        [SerializeReference] public List<ExposedParameter> parameters = new List<ExposedParameter>();
        public StateMachineAsset graph;
    }
}