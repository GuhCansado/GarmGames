using System;
using System.Collections.Generic;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NinjutsuGames.StateMachine.Runtime
{
    public static class CacheUtils
    {
        private class Cache
        {
            public readonly object reference;
            public readonly Dictionary<Type, MonoBehaviour> components;
            public readonly Dictionary<Type, object> datas;

            public Cache(object reference)
            {
                components = new Dictionary<Type, MonoBehaviour>();
                datas = new Dictionary<Type, object>();
                this.reference = reference;
            }
        }
        
        // VARIABLES: -----------------------------------------------------------------------------
        
        private static readonly Dictionary<int, Cache> CACHE = new();
        
        // INIT: ----------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemsInit()
        {
            CACHE.Clear();
            SceneManager.sceneUnloaded += scene => Prune();
        }
        
        // GET: -----------------------------------------------------------------------------------

        /// <summary>
        /// Returns the requested component (null if it does not exist) and caches its value
        /// so retrieving the same value afterwards is faster. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetCached<T>(this GameObject context, int instanceID) where T : MonoBehaviour
        {
            if (!context) return null;
            // var instanceID = context.GetInstanceID();

            if (!CACHE.TryGetValue(instanceID, out var cache))
            {
                cache = new Cache(context);
                CACHE[instanceID] = cache;
            }

            if (!cache.components.TryGetValue(typeof(T), out var component))
            {
                component = context.AddComponent<T>();
                if (component != null)
                {
                    cache.components[typeof(T)] = component;
                    CACHE[instanceID] = cache;
                }
            }
            return component as T;
        }
        
        /// <summary>
        /// Returns requested data (null if it does not exist) and caches its value so retrieving the same value afterwards is faster.
        /// Notice this won't cache the data if the game is running in the editor to allow for live editing.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="instanceID"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCachedData<T>(this T context, int instanceID) where T : class
        {
#if UNITY_EDITOR
            return context.Clone();
#else
            if (context == null) return null;
            if (!CACHE.TryGetValue(instanceID, out var cache))
            {
                cache = new Cache(context);
                CACHE[instanceID] = cache;
            }

            if (!cache.datas.TryGetValue(typeof(T), out var data))
            {
                data = context.Clone();
                if (data != null)
                {
                    cache.datas[typeof(T)] = data;
                    CACHE[instanceID] = cache;
                }
            }
            return data as T;
#endif
        }
        
        // PRUNE: ---------------------------------------------------------------------------------

        /// <summary>
        /// Iterates through the whole cache database and flushes all those elements which
        /// reference to null. This is an expensive operation and should only be done when the
        /// game is expected to be unresponsive, such as loading a scene. By default, when a scene
        /// is unloaded, this method will be executed.
        /// </summary>
        public static void Prune()
        {
            var removeKeys = new List<int>();
            
            foreach (var entry in CACHE)
            {
                if (entry.Value.reference == null)
                {
                    removeKeys.Add(entry.Key);
                }
            }

            foreach (var removeKey in removeKeys)
            {
                CACHE.Remove(removeKey);
            }
        }
    }
}