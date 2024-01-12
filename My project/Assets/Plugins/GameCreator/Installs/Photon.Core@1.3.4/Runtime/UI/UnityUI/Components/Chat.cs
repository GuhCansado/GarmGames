#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY)
#define MOBILE
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace NinjutsuGames.Photon.Runtime
{
    /*[Serializable]
    public class ChatUIComponents
    {
        public PropertyGetInstantiate prefab = new(){usePooling = true, duration = 0, hasDuration = false, size = 10};

        /// <summary>
        /// Input field used for chat input.
        /// </summary>
        public InputReference input;
        public Image background;

        /// <summary>
        /// Root object for chat window's history. This allows you to position the chat window's text.
        /// </summary>
        public ScrollRect container;
    }*/

    [Serializable]
    public class ChatUISettings
    {
        /// <summary>
        /// Whether the activate the chat input when Return key gets pressed.
        /// </summary>
        public bool activateOnInput = true;
        public InputPropertyButton inputTrigger = InputButtonKeyboardPress.Create(Key.Enter);

        public int maxLines = 1000;

        public int minVisibleLines = 0;

        /// <summary>
        /// Seconds that must elapse before a chat label starts to fade out.
        /// </summary>
        public float fadeOutStart = 10f;

        /// <summary>
        /// How long it takes for a chat label to fade out in seconds.
        /// </summary>
        public float fadeOutDuration = 3f;
        public float backgroundFadeOutDuration = 0.5f;

        /// <summary>
        /// Whether messages will fade out over time.
        /// </summary>
        public bool allowChatFading = true;

        
        public bool disablePlayerWhenTyping = true;
        public PropertySetNumber unseenMessages = new();
    }
    /// <summary>
    /// Generic chat window functionality.
    /// </summary>
    public abstract class Chat : MonoBehaviour
    {
        // [SerializeField] protected ChatUIComponents uiComponents = new();
        [SerializeField] protected PropertyGetInstantiate prefab = new(){usePooling = true, duration = 0, hasDuration = false, size = 10};

        /// <summary>
        /// Input field used for chat input.
        /// </summary>
        [SerializeField] protected InputReference input;
        [SerializeField] protected Image background;

        /// <summary>
        /// Root object for chat window's history. This allows you to position the chat window's text.
        /// </summary>
        [SerializeField] protected ScrollRect container;
        [SerializeField] protected ChatUISettings uiSettings = new();

        private const char SPACE_CHAR = '\n';
        
        //IsFocused has 1 frame delay.
        protected bool _selected;

        // public UnityEvent onOpen;
        // public UnityEvent onClose;

        private readonly WaitForEndOfFrame _wait = new();

        private class ChatEntry
        {
            public CanvasGroup Group;
            public Transform Transform;
            public Text Text;
            public TMP_Text TextTMP;
            public Color Color;
            public float Time;
            public int Lines = 0;
            public float Alpha = 0f;
            public bool IsExpired = false;
            public bool ShouldBeDestroyed = false;

            public GameObject GameObject;
            // public bool fadedIn = false;
        }

        private bool Selected => _selected || EventSystem.current.currentSelectedGameObject == input.GameObject;
        private readonly List<ChatEntry> _chatEntries = new();

        //private int mBackgroundHeight = -1;
        private bool _ignoreNextEnter = false;
        private Color _originalBgColor;
        private CanvasGroup _scrollBarCanvas;
        private float _uiTime;
        private Color _fadedOutBgColor;
        private bool _overInput;
        private bool _overContainer;
        private bool _selectedContainer;
        private int _unSeenCount;
        private static bool _wasPlayerControllable;

        /// <summary>
        /// For things you want to do after OnSubmitInternal method has ran.
        /// </summary>
        // public UnityEvent LateEndEdit = new UnityEvent();
        protected virtual void Awake()
        {
            uiSettings.inputTrigger.OnStartup();
            uiSettings.inputTrigger.RegisterPerform(OnTriggerInput);
            
            _originalBgColor = background.color;
            _fadedOutBgColor = background.color;
            _scrollBarCanvas = container.verticalScrollbar.GetComponent<CanvasGroup>();
            if(!_scrollBarCanvas)_scrollBarCanvas = container.verticalScrollbar.gameObject.AddComponent<CanvasGroup>();
            
            if(uiSettings.allowChatFading)
            {
                _fadedOutBgColor.a = 0;
                background.color = _fadedOutBgColor;

                _scrollBarCanvas.alpha = 0;
            }
            // prefab.SetActive(false);

            if (input != null)
            {/*
#if USE_TMP
                input.onSelect.AddListener(s => Select());
                input.onDeselect.AddListener(s => Deselect());
#else*/
                var eventTrigger = input.GameObject.GetComponent<EventTrigger>();
                if (!eventTrigger) eventTrigger = input.GameObject.AddComponent<EventTrigger>();
                
                var onSel = new EventTrigger.Entry();
                onSel.callback.AddListener(e => Select());
                onSel.eventID = EventTriggerType.Select;
                eventTrigger.triggers.Add(onSel);
                
                var onUnsel = new EventTrigger.Entry();
                onUnsel.callback.AddListener(e => Deselect());
                onUnsel.eventID = EventTriggerType.Deselect;
                eventTrigger.triggers.Add(onUnsel);
                
                var eventTrigger3 = input.GameObject.GetComponent<EventTrigger>();
                if (!eventTrigger3) eventTrigger3 = input.GameObject.AddComponent<EventTrigger>();
                
                var onHover2 = new EventTrigger.Entry();
                onHover2.callback.AddListener(e =>
                {
                    _overInput = true;
                    Select();
                });
                onHover2.eventID = EventTriggerType.PointerEnter;
                eventTrigger3.triggers.Add(onHover2);
                
                var onExit2 = new EventTrigger.Entry();
                onExit2.callback.AddListener(e =>
                {
                    _overInput = false;
                    Deselect();
                });
                onExit2.eventID = EventTriggerType.PointerExit;
                eventTrigger3.triggers.Add(onExit2);
                
                input.SubscribeOnValueChanged(OnValueChanged);
                input.SubscribeOnSubmit(OnSubmitInternal);
            }
            
            var eventTrigger2 = container.GetComponent<EventTrigger>();
            if (!eventTrigger2) eventTrigger2 = container.gameObject.AddComponent<EventTrigger>();
                
            var onHover = new EventTrigger.Entry();
            onHover.callback.AddListener(e =>
            {
                _overContainer = true;
                Select();
            });
            onHover.eventID = EventTriggerType.PointerEnter;
            eventTrigger2.triggers.Add(onHover);
                
            var onExit = new EventTrigger.Entry();
            onExit.callback.AddListener(e =>
            {
                _overContainer = false;
                Deselect();
            });
            onExit.eventID = EventTriggerType.PointerExit;
            eventTrigger2.triggers.Add(onExit);
            
            container.onValueChanged.AddListener(OnScroll);
            
            CheckPlayerControllable();
        }

        private void OnScroll(Vector2 arg0)
        {
            _selectedContainer = container.velocity != Vector2.zero;
            if(!_overContainer && !_selectedContainer && !_overInput) Deselect();
        }

        private void OnTriggerInput()
        {
            if (!uiSettings.activateOnInput) return; // && (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
            if (!_ignoreNextEnter)
            {
                input.Interactable = true;
                input.Select();
                input.ActivateInputField();

                EventSystem current;
                (current = EventSystem.current).SetSelectedGameObject(input.GameObject, null);
                input.OnPointerClick(new PointerEventData(current));
            }

            _ignoreNextEnter = false;
        }

        private void OnDestroy()
        {
            CheckPlayerControllable();
            
            uiSettings.inputTrigger.OnDispose();
            uiSettings.inputTrigger.ForgetPerform(OnTriggerInput);

            if (input == null) return;
            input.UnsubscribeOnValueChanged(OnValueChanged);
            input.UnsubscribeOnSubmit(OnSubmitInternal);
        }

        private void CheckPlayerControllable()
        {
            var player = ShortcutPlayer.Instance ? ShortcutPlayer.Instance.Get<Character>() : null;
            if (uiSettings.disablePlayerWhenTyping && player && !player.Player.IsControllable && _wasPlayerControllable)
            {
                player.Player.IsControllable = true;
            }
        }

        private void OnValueChanged(string input)
        {
            if (uiSettings.disablePlayerWhenTyping && 
                !string.IsNullOrEmpty(input) &&
                ShortcutPlayer.Instance &&
                ShortcutPlayer.Instance.Get<Character>().Player.IsControllable)
            {
                ShortcutPlayer.Instance.Get<Character>().Player.IsControllable = false;
            }
        }

        public void Select()
        {
            if(_selected) return;
            // Debug.Log($"Select");
            _uiTime = Time.time;

            _unSeenCount = 0;
            uiSettings.unseenMessages?.Set(_unSeenCount, gameObject);

            _selected = true;
            OnOpen();

            var player = ShortcutPlayer.Instance ? ShortcutPlayer.Instance.Get<Character>() : null;
            if (!uiSettings.disablePlayerWhenTyping || !player || !player.Player.IsControllable) return;
            _wasPlayerControllable = player.Player.IsControllable;
            player.Player.IsControllable = false;
        }

        public void Deselect()
        {
            if(!_selected) return;

            // Debug.LogWarning($"Deselect overInput: {_overInput} overContainer: {_overContainer} selectedContainer: {_selectedContainer} wasPlayerControllable: {_wasPlayerControllable} canSelect: {(!_overInput && !_overContainer && !_selectedContainer)}");

            if(_overInput || _overContainer || _selectedContainer) return;

            _selected = false;
            _selectedContainer = false;
            OnClose();
            CheckPlayerControllable();
        }

        /// <summary>
        /// Handle inputfield onEndEdit event.
        /// </summary>
        public void OnSubmitInternal(string content)
        {
            _ignoreNextEnter = true;
            input.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(content)) OnSubmit(content);

            input.DeactivateInputField();
            if (!EventSystem.current.alreadySelecting) EventSystem.current.SetSelectedGameObject(null, null);
        }

        private IEnumerator SetInputFieldNotInteractableAtEndOfFrame()
        {
            yield return _wait;
            input.Interactable = false;
        }

        public void ClearHistory()
        {
            for (int i = 0, imax = _chatEntries.Count; i < imax; i++)
            {
                var e = _chatEntries[i];
                RemoveEntry(i);
            }
        }
        
        protected virtual void OnOpen() {}
        protected virtual void OnClose() {}

        /// <summary>
        /// Custom submit logic for what happens on chat input submission.
        /// </summary>
        protected virtual void OnSubmit(string text)
        {
        }

        /// <summary>
        /// Add a new chat entry.
        /// </summary>
        private GameObject InternalAdd(string text, Color color, bool tintBackground)
        {
            var ent = new ChatEntry
            {
                Time = Time.time,
                Color = color
            };
            _chatEntries.Add(ent);

            var go = prefab.Get(gameObject);// Instantiate(prefab, container.content, false) as GameObject;
            go.transform.SetParent(container.content, false);
            go.SetActive(true);
            ent.GameObject = go;
            ent.Group = go.Get<CanvasGroup>() ?? go.Add<CanvasGroup>();
            ent.Transform = go.transform;

            ent.Text = go.Get<Text>();
            ent.TextTMP = go.Get<TMP_Text>();
            
            if (tintBackground)
            {
                go.Get<Image>().color = color;
            }
            else
            {
                if(ent.Text) ent.Text.color = color;
                if(ent.TextTMP) ent.TextTMP.color = color;
            }
            
            if(ent.Text) ent.Text.text = text;
            if(ent.TextTMP) ent.TextTMP.text = text;

            if(ent.Text) ent.Lines = ent.Text.text.Split(SPACE_CHAR).Length;
            if(ent.TextTMP) ent.Lines = ent.TextTMP.text.Split(SPACE_CHAR).Length;

            for (int i = _chatEntries.Count, lineOffset = 0; i > 0;)
            {
                var e = _chatEntries[--i];

                if (i + 1 == _chatEntries.Count)
                {
                    // It's the first entry. It doesn't need to be re-positioned.
                    lineOffset += e.Lines;
                }
                else
                {
                    // This is not a first entry. It should be tweened into its proper place.
                    if (lineOffset + e.Lines > uiSettings.maxLines && uiSettings.maxLines > 0)
                    {
                        e.IsExpired = true;
                        e.ShouldBeDestroyed = true;

                        if (e.Alpha == 0f)
                        {
                            RemoveEntry(i);
                            continue;
                        }
                    }

                    lineOffset += e.Lines;
                }
            }

            if (Selected) return go;
            _unSeenCount++;
            uiSettings.unseenMessages?.Set(_unSeenCount, gameObject);

            return go;
        }

        /// <summary>
        /// Update the "alpha" of each line and update the background size.
        /// </summary>
        protected virtual void Update()
        {
            if(!PhotonNetwork.InRoom) return;
            uiSettings.inputTrigger.OnUpdate();
            
            // if(!selected && EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.Equals(input.GameObject)) Select();
            
            
            if(uiSettings.allowChatFading)
            {
                float uiAlpha = 0;

                if (Selected)
                {
                    // Quickly fade in new entries
                    uiAlpha = Mathf.Clamp01(_scrollBarCanvas.alpha + Time.deltaTime * 5f);
                }
                else if (Time.time - (_uiTime + uiSettings.fadeOutStart) < uiSettings.backgroundFadeOutDuration)
                {
                    // Slowly fade out entries that have been visible for a while
                    uiAlpha = Mathf.Clamp01(_scrollBarCanvas.alpha - Time.deltaTime / uiSettings.backgroundFadeOutDuration);
                }
                else
                {
                    // Quickly fade out chat entries that should have faded by now,
                    // but likely didn't due to the input being selected.
                    uiAlpha = Mathf.Clamp01(_scrollBarCanvas.alpha - Time.deltaTime);
                }

                // originalBgColor.a = uiAlpha;
                background.color = Color.Lerp(_fadedOutBgColor, _originalBgColor, uiAlpha);

                _scrollBarCanvas.alpha = uiAlpha;
            }

            // int height = 0;

            for (var i = 0; i < _chatEntries.Count;)
            {
                var e = _chatEntries[i];
                float alpha = 0;

                if (e.IsExpired)
                {
                    // Quickly fade out expired chat entries
                    alpha = Mathf.Clamp01(e.Alpha - Time.deltaTime);
                }
                else if (Selected || Time.time - e.Time < uiSettings.fadeOutStart) //
                {
                    // Quickly fade in new entries
                    alpha = Mathf.Clamp01(e.Alpha + Time.deltaTime * 5f);
                }
                else if (Time.time - (e.Time + uiSettings.fadeOutStart) < uiSettings.fadeOutDuration)
                {
                    // Slowly fade out entries that have been visible for a while
                    alpha = Mathf.Clamp01(e.Alpha - Time.deltaTime / uiSettings.fadeOutDuration);
                }
                else
                {
                    // Quickly fade out chat entries that should have faded by now,
                    // but likely didn't due to the input being selected.
                    alpha = Mathf.Clamp01(e.Alpha - Time.deltaTime);
                }

                if (Math.Abs(e.Alpha - alpha) > 0f)
                {
                    e.Alpha = alpha;
                    e.Group.alpha = !uiSettings.allowChatFading || i > _chatEntries.Count - (uiSettings.minVisibleLines + 1) ? 1 : e.Alpha;

                    if ((int)alpha == 1)
                    {
                        // The chat entry has faded in fully
                        // e.fadedIn = true;
                    }
                    else if (alpha == 0f && e.ShouldBeDestroyed)
                    {
                        // This chat entry has expired and should be removed
                        RemoveEntry(i);
                        continue;
                    }
                }

                // If the line is visible, it should be counted
                ++i;
            }
        }

        private void RemoveEntry(int index)
        {
            var entry = _chatEntries[index].GameObject;
            if(prefab.usePooling) entry.SetActive(false);
            else Destroy(entry);
            
            _chatEntries.RemoveAt(index);
        }

        /// <summary>
        /// Add a new chat entry.
        /// </summary>
        protected virtual GameObject Add(string text, Color color, bool tintBackground, Player player)
        {
            return InternalAdd(text, color, tintBackground);
        }
    }
}