using System;
using System.Linq;
using System.Reflection;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using NinjutsuGames.StateMachine.Runtime;
using NinjutsuGames.StateMachine.Runtime.Common;

namespace NinjutsuGames.StateMachine.Editor
{
    public class BaseGameCreatorNodeView : BaseNodeView
    {
        protected static readonly IIcon ICON_INSTRUCTION = new IconInstructions(ColorTheme.Type.Blue);
        protected static readonly IIcon ICON_CONDITION = new IconConditions(ColorTheme.Type.Green);
        protected static readonly IIcon ICON_BRANCH = new IconBranch(ColorTheme.Type.Green);
        protected static readonly IIcon ICON_STATE_MACHINE = new IconStateMachine(ColorTheme.Type.Blue);
        protected static readonly IIcon ICON_EVENT = new IconTriggers(ColorTheme.Type.Yellow);
        protected static readonly IIcon ICON_NONE = new IconMarker(ColorTheme.Type.White);
        protected static readonly IIcon ICON_START = new IconArrowCircleRight(ColorTheme.Type.Purple);
        protected static readonly IIcon ICON_ARROW = new IconArrowRight(ColorTheme.Type.White);

        protected Image icon = new Image();

        public virtual Texture2D DefaultIcon => ICON_NONE.Texture;
        
        public virtual string DefaultIconName => null;

        public Image Icon => icon;

        private string lastChange;
        protected Label counter;

        public void SetIcon(Texture iconTexture)
        {
            icon.image = iconTexture;
        }

        protected Texture2D GetIcon(SerializedProperty property)
        {
            Type fieldType = TypeUtils.GetTypeFromProperty(property, true);
            ImageAttribute iconAttribute = fieldType?
                .GetCustomAttributes<ImageAttribute>()
                .FirstOrDefault();

            return iconAttribute != null ? iconAttribute.Image : Texture2D.whiteTexture;
        }

        public override void Enable()
        {
            EditorApplication.projectChanged -= UpdateIcon;
            EditorApplication.projectChanged += UpdateIcon;
            EditorApplication.playModeStateChanged -= PlayModeStateChange;
            EditorApplication.playModeStateChanged += PlayModeStateChange;
            UpdateExecutionStateView();
            var node = (BaseGameCreatorNode) this.nodeTarget;
            // if(EditorApplication.isPlaying)
            {
                node.eventStartRunning -= StartRunning;
                node.eventStopRunning -= StopRunning;
                
                node.eventStartRunning += StartRunning;
                node.eventStopRunning += StopRunning;
            }
            SetIcon(DefaultIcon);
            icon.AddToClassList("node-icon");
            titleContainer.Insert(0, icon);
            UpdateIcon();
            InjectCustomStyle();
        }

        public override void Disable()
        {
            base.Disable();
            EditorApplication.projectChanged -= UpdateIcon;
            EditorApplication.playModeStateChanged -= PlayModeStateChange;
        }

        private void PlayModeStateChange(PlayModeStateChange mode)
        {
            UpdateIcon();
        }

        private void StartRunning(GameObject context)
        {
            owner.eventQueue.AddEvent(() => Highlight(context));
        }

        private void StopRunning(GameObject context, bool runningResult)
        {
            owner.eventQueue.AddEvent(() => UnHighlight(context, runningResult));
        }

        protected void InjectCustomStyle()
        {
            var border = this.Q("node-border");
            var overflowStyle = border.style.overflow;
            overflowStyle.value = Overflow.Visible;
            border.style.overflow = overflowStyle;

            var selectionBorder = this.Q("selection-border");
            // selectionBorder.SendToBack();
        }

        protected Texture2D GetIcon(string typeName)
        {
            var fieldType = TypeUtils.GetTypeFromName(typeName);
            return fieldType?.GetCustomAttributes<ImageAttribute>().FirstOrDefault()?.Image; 
        }
        
        public virtual void UpdateIcon()
        {
            var nodeIcon = DefaultIcon;
            if (!string.IsNullOrEmpty(DefaultIconName))
            {
                var newIcon = GetIcon(DefaultIconName);
                if (newIcon != null) nodeIcon = newIcon;
            }
            SetIcon(nodeIcon);
        }

        public virtual void Update()
        {
            
        }
        
        protected bool HasChanged(object obj)
        {
            var json = JsonUtility.ToJson(obj);
            if (json.Equals(lastChange)) return false;
            lastChange = json;
            return true;
        }

        protected void AddCounter(int count)
        {
            counter = new Label();
            counter.AddToClassList("counter");
            counter.text = count.ToString();
            titleContainer.Add(counter);
        }
        
        protected void UpdateCounter(int count)
        {
            counter.text = count.ToString();
        }
    }
}