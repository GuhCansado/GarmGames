/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace NEEDSIM
{
    /// <summary>
    /// We hope we wrote our example actions in a way that they can be integrated into your
    /// Finite State Machine, Behavior Tree, or Planner. We provide a sample use of our sample
    /// actions in the PlanDemo.cs class.
    /// </summary>
    public abstract class Action
    {
        protected NEEDSIMNode agent;

        public enum Result { Failure = 0, Success, Running }

        public abstract string Name { get; }

        public Action(NEEDSIMNode agent)
        {
            this.agent = agent;
        }

        public abstract Result Run();
    }
}
