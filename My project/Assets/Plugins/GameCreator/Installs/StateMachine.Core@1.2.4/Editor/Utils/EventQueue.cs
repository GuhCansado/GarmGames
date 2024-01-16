using UnityEditor;

namespace NinjutsuGames.StateMachine.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class EventQueue
    {
        private readonly Queue<Action> _eventQueue = new();
        private EditorCoroutine _currentCoroutine;
        private readonly float _delayBetweenEvents;

        public EventQueue(float delayBetweenEvents = 0.035f)
        {
            _delayBetweenEvents = delayBetweenEvents;
        }
        public void AddEvent(Action eventAction)
        {
            _eventQueue.Enqueue(eventAction);
            
            // If there's no coroutine running, start one
            _currentCoroutine ??= EditorCoroutine.StartCoroutine(RunEvents());
        }

        public IEnumerator RunEvents()
        {
            while (_eventQueue.Count > 0)
            {
                Action currentEvent = _eventQueue.Dequeue();
                currentEvent();

                // Wait for the specified delay
                float startTime = (float)EditorApplication.timeSinceStartup;
                while ((float)EditorApplication.timeSinceStartup - startTime < _delayBetweenEvents)
                {
                    yield return null;
                }
            }
            
            // Coroutine finished, set it to null
            _currentCoroutine = null;
        }
    }

}