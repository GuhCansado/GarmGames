using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    public static class PhotonPlayerProperties
    {
        const string D = "Deaths";
        const string K = "Kills";
        const string A = "Assists";
        public const string PING = "ping";

        /// <summary>
        /// Predefined Kills property.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int Kills(this Player player)
        {
            return player.GetInt(K);
        }

        /// <summary>
        /// Predefined Deaths property.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int Deaths(this Player player)
        {
            return player.GetInt(D);
        }

        /// <summary>
        /// Predefined Assists property.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int Assists(this Player player)
        {
            return player.GetInt(A);
        }

        /// <summary>
        /// Returns Players ping.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int GetPing(this Player player)
        {
            return player.GetInt(PING);
        }

        /// <summary>
        /// Updates Player ping.
        /// </summary>
        /// <param name="player"></param>
        public static void SetPing(this Player player)
        {
            SetInt(player, PING, PhotonNetwork.GetPing());
        }

        /// <summary>
        /// Returns a Player property of type Bool.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static bool GetBool(this Player player, string propertyName, bool fallback = false)
        {
            return player.HasProperty(propertyName) ? player.GetProperty<bool>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Player property of type string.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static string GetString(this Player player, string propertyName, string fallback = "")
        {
            return player.HasProperty(propertyName) ? player.GetProperty<string>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Player property of type float.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static float GetFloat(this Player player, string propertyName, float fallback = 0)
        {
            return player.HasProperty(propertyName) ? player.GetProperty<float>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Player property of type int.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static int GetInt(this Player player, string propertyName, int fallback = 0)
        {
            return player.HasProperty(propertyName) ? player.GetProperty<int>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Player property.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static T GetProperty<T>(this Player player, string propertyName)
        {
            if (player.CustomProperties.TryGetValue(propertyName, out var prop))
            {
                return (T)Convert.ChangeType(prop, typeof(T));
            }

            return (T)Convert.ChangeType(null, typeof(T));
        }

        /// <summary>
        /// Returns a Player property.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty(this Player player, string propertyName)
        {
            return player.CustomProperties.TryGetValue(propertyName, out var prop) ? prop : prop;
        }

        /// <summary>
        /// Returns true if the give property is found
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this Player player, string propertyName)
        {
            return player != null && (player.CustomProperties?.ContainsKey(propertyName) ?? false);
        }

        /// <summary>
        /// Incremements an int property from the Player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webForward"></param>
        public static void AddInt(this Player player, string propertyName, int propertyValue, bool webForward = false)
        {
            if (!player.HasProperty(propertyName))
            {
                player.SetProperty(propertyName, propertyValue, webForward);
            }
            else
            {
                player.SetProperty(propertyName, propertyValue + player.GetProperty<int>(propertyName), webForward);
            }
        }

        /// <summary>
        /// Incremements an int property from the Player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webForward"></param>
        public static void AddFloat(this Player player, string propertyName, float propertyValue, bool webForward = false)
        {
            if (!player.HasProperty(propertyName))
            {
                player.SetProperty(propertyName, propertyValue, webForward);
            }
            else
            {
                player.SetProperty(propertyName, propertyValue + player.GetProperty<float>(propertyName), webForward);
            }
        }

        /// <summary>
        /// Clear all properties from this Player.
        /// </summary>
        /// <param name="player"></param>
        public static void ClearAllProperties(this Player player)
        {
            ExitGames.Client.Photon.Hashtable t = player.CustomProperties;
            List<object> keys = new List<object>(t.Keys);
            for (int i = 0, imax = keys.Count; i < imax; i++)
            {
                t[keys[i]] = null;
            }
            player.SetCustomProperties(t);
        }

        /// <summary>
        /// Defines a string property on this Player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webForward"></param>
        public static void SetString(this Player player, string propertyName, string propertyValue, bool webForward = false)
        {
            player.SetProperty(propertyName, propertyValue, webForward);
        }

        /// <summary>
        /// Defines a int property on this Player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webForward"></param>
        public static void SetInt(this Player player, string propertyName, int propertyValue, bool webForward = false)
        {
            player.SetProperty(propertyName, propertyValue, webForward);
        }

        /// <summary>
        /// Defines a float property on this Player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webForward"></param>
        public static void SetFloat(this Player player, string propertyName, float propertyValue, bool webForward = false)
        {
            player.SetProperty(propertyName, propertyValue, webForward);
        }

        /// <summary>
        /// Defines a bool property on this Player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webForward"></param>
        public static void SetBool(this Player player, string propertyName, bool propertyValue, bool webForward = false)
        {
            player.SetProperty(propertyName, propertyValue, webForward);
        }

        /// <summary>
        /// Defines a property on this Player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webForward"></param>
        private static void SetProperty<T>(this Player player, string propertyName, T propertyValue, bool webForward = false)
        {
            if (webForward)
            {
                var flags = new WebFlags(0)
                {
                    //flags.SendSync = false;
                    //flags.SendState = true;
                    HttpForward = true
                };
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { propertyName, propertyValue } }, null, flags);
            }
            else
            {
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { propertyName, propertyValue } });
            }
        }
    }
}