using System;
using System.Collections;
using System.Collections.Generic;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace NinjutsuGames.Photon.Runtime.Managers
{
    [AddComponentMenu("")]
    public class FloatingTextManager : Singleton<FloatingTextManager>
    {
        [Serializable]
        public class Message
        {
            public Transform instance;
            public Transform target;
            public Vector3 offset;
            public float duration;
            public float fadeOutTime = 0.5f;
            public bool forceExpire;
            public Text text;
            public TMP_Text tmpText;
            public float startTime;
            public CanvasGroup canvasGroup;
        }
        private const float CANVAS_WIDTH = 600f;
        private const float CANVAS_HEIGHT = 300f;

        private const float SIZE_X = 2f;
        private const float SIZE_Y = 1f;

        private const int PADDING = 20;
        private const int SPACING = 20;
        
        // TODO: Remove condition once 2022.3 LTS lands
        
#if UNITY_2022_2_OR_NEWER
        private const string FONT_NAME = "LegacyRuntime.ttf";
#else 
        private const string FONT_NAME = "Arial.ttf";
#endif

        private const int FONT_SIZE = 42;
        
        private static readonly Color COLOR_BACKGROUND = new Color(0f, 0f, 0f, 0.5f);
        private readonly List<Message> _messages = new();
        private readonly Dictionary<string, Message> _prevMessages = new();
        private GameObject _defaultPrefab;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnSubsystemsInit()
        {
            Instance.WakeUp();
        }

        private void Update()
        {
            for (var i = _messages.Count - 1; i >= 0; i--)
            {
                var msg = _messages[i];
                if (!msg.instance)
                {
                    RemoveEntry(msg);
                    continue;
                }
                if (!msg.instance.gameObject.activeInHierarchy) continue;   
                if (!msg.target)
                {
                    RemoveEntry(msg);
                    continue;
                }
                msg.instance.SetPositionAndRotation(msg.target.position + msg.offset,ShortcutMainCamera.Transform.rotation);
                if(!msg.forceExpire)
                {
                    if (msg.duration == 0) continue;
                    if (!(Time.time > msg.startTime)) continue;
                }
                // fade out canvas group and disable
                var canvasGroup = msg.canvasGroup;
                if(!canvasGroup)
                {
                    RemoveEntry(msg);
                    continue;
                }
                canvasGroup.alpha = 1f - (Time.time - msg.startTime) / msg.fadeOutTime;
                if(canvasGroup.alpha <= 0f)
                {
                    RemoveEntry(msg);
                }
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static void Show(string id, string message, Transform target, FloatingText settings)
        {
            Instance.Add(id, message, target, settings);
        }

        public static void RemoveEntry(Message message)
        {
            Instance.Remove(message);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void Remove(Message message)
        {
            message.instance.gameObject.SetActive(false);
            _messages.RemoveAt(_messages.IndexOf(message));
        }
        
        private Message Add(string id, string message, Transform target, FloatingText settings)
        {
            var instance = Instance.GetInstance(id, target, settings);
            
            instance.instance.gameObject.SetActive(true);

            if (instance.text)
            {
                instance.text.text = message;
                instance.text.color = settings.color;
            }

            if (!instance.tmpText) return instance;
            instance.tmpText.text = message;
            instance.tmpText.color = settings.color;

            return instance;
        }

        private Message GetInstance(string id, Transform target, FloatingText settings)
        {
            /*if(settings.clearPreviousEntry)
            {
                if (_prevMessages.ContainsKey(target))
                {
                    var prevMessage = _prevMessages[target];
                    if (prevMessage != null)
                    {
                        Remove(prevMessage);
                    }

                    _prevMessages.Remove(target);
                }
            }*/
            Message msg;
            // Debug.LogWarning($"AddText target: {target} id: {id}");
            /*if (_prevMessages.TryGetValue(id, out var prevMessage))
            {
                Debug.LogWarning($"PrevMessage: {prevMessage} id: {id}");
                Remove(prevMessage);
            }*/
            if(!_prevMessages.ContainsKey(id)) // && !singleInstance
            {
                GameObject message = default;
                Text messageText = default;
                TMP_Text messageTMPText = default;
                FloatingUI floating = default;
                if (settings.prefab)
                {
                    message = PoolManager.Instance.Pick(settings.prefab, target.position + target.transform.TransformDirection(settings.offset), ShortcutMainCamera.Transform.rotation, 10);
                    
                    floating = message.Get<FloatingUI>() ?? message.Add<FloatingUI>();
                    messageTMPText = floating.Tmp;
                    if(!floating.IsTMP) messageText = floating.Text;
                }
                else
                {
                    if (!_defaultPrefab)
                    {
                        _defaultPrefab = new GameObject("DefaultPrefab");
                        _defaultPrefab.transform.SetPositionAndRotation(
                            target.position + target.transform.TransformDirection(settings.offset),
                            ShortcutMainCamera.Transform.rotation
                        );
                    
                        var canvas = _defaultPrefab.AddComponent<Canvas>();
                        _defaultPrefab.AddComponent<CanvasScaler>();
                        _defaultPrefab.AddComponent<CanvasGroup>();

                        canvas.renderMode = RenderMode.WorldSpace;
                        canvas.worldCamera = ShortcutMainCamera.Get<Camera>();

                        var canvasTransform = _defaultPrefab.Get<RectTransform>();
                        canvasTransform.sizeDelta = new Vector2(CANVAS_WIDTH, CANVAS_HEIGHT);
                        canvasTransform.localScale = new Vector3(
                            SIZE_X / CANVAS_WIDTH,
                            SIZE_Y / CANVAS_HEIGHT,
                            1f
                        );

                        // var container = ConfigureContainer(canvasTransform);
                        var background = this.ConfigureBackground(canvasTransform);
                        ConfigureText(background);
                        _defaultPrefab.SetActive(false);
                    }
                    
                    message = PoolManager.Instance.Pick(_defaultPrefab, target.position + target.transform.TransformDirection(settings.offset), target.rotation, 10);
                    floating = message.Get<FloatingUI>() ?? message.Add<FloatingUI>();
                    messageText = floating.Text;
                }
                
                // message.hideFlags = HideFlags.HideAndDontSave;
            
                msg = new Message() {duration = settings.duration, instance = message.transform, offset = settings.offset, target = target, text = messageText, tmpText = messageTMPText, canvasGroup = message.Get<CanvasGroup>()};
                _prevMessages.TryAdd(id, msg);
            }
            else
            {
                msg = _prevMessages[id];
                msg.target = target;
                _messages.Remove(msg);
            }
            msg.instance.gameObject.SetActive(false);
            msg.instance.gameObject.SetActive(true);
            // _prevMessages.TryAdd(id, msg);
            _messages.Add(msg);

            // if(settings.clearPreviousEntry) _prevMessages.Add(target, msg);
            msg.fadeOutTime = settings.fadeOutTime;
            if(msg.canvasGroup) msg.canvasGroup.alpha = 1;
            msg.startTime = Time.time + settings.duration;
            return msg;
        }
        
        private RectTransform ConfigureContainer(RectTransform parent)
        {
            var gameObject = new GameObject("Container");

            var layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.padding = new RectOffset(PADDING, PADDING, PADDING, PADDING);
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childScaleWidth = true;
            layoutGroup.childScaleHeight = true;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.spacing = SPACING;

            var sizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            var rectTransform = gameObject.GetComponent<RectTransform>();
            RectTransformUtils.SetAndCenterToParent(rectTransform, parent);

            rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            return rectTransform;
        }

        private RectTransform ConfigureBackground(RectTransform parent)
        {
            var gameObject = new GameObject("Background");
            var image = gameObject.AddComponent<Image>();
            image.color = COLOR_BACKGROUND;

            var layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.padding = new RectOffset(PADDING, PADDING, PADDING, PADDING);
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childScaleWidth = true;
            layoutGroup.childScaleHeight = true;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;

            var sizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            var rectTransform = gameObject.GetComponent<RectTransform>();
            RectTransformUtils.SetAndCenterToParent(rectTransform, parent);

            rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            

            return rectTransform;
        }
        
        private Text ConfigureText(RectTransform parent)
        {
            var gameObject = new GameObject("Text");
            var messageText = gameObject.AddComponent<Text>();
            
            var font = (Font) Resources.GetBuiltinResource(typeof(Font), FONT_NAME);
            messageText.font = font;
            messageText.fontSize = FONT_SIZE;
            
            var textTransform = gameObject.GetComponent<RectTransform>();
            RectTransformUtils.SetAndCenterToParent(textTransform, parent);

            var shadow = gameObject.AddComponent<Shadow>();
            shadow.effectColor = COLOR_BACKGROUND;
            shadow.effectDistance = Vector2.one;
            
            return messageText;
        }
    }
}