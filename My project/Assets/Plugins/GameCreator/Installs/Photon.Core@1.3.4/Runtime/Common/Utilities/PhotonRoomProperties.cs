using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace NinjutsuGames.Photon.Runtime
{
    public static class PhotonRoomProperties
    {
        private const string TIME_START = "start-time";
        private const string TIME_DURATION = "duration-time";
        private const string PING = "ping";

        /// <summary>
        /// Returns room Start Time.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static double GetElapsedTime(this Room room)
        {
            return room.HasProperty(TIME_START) ? PhotonNetwork.Time - room.GetStartTime() : 0;
        }

        /// <summary>
        /// Returns room Start Time.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static double GetRemainingTime(this Room room)
        {
            float secsPerRound = room.HasProperty(TIME_DURATION) ? room.GetDurationTime() : 0;
            return room.HasProperty(TIME_DURATION) ? secsPerRound - (room.GetElapsedTime() % secsPerRound) : room.GetElapsedTime();
        }

        /// <summary>
        /// Returns room Start Time.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static double GetStartTime(this Room room)
        {
            return room.HasProperty(TIME_START) ? room.GetProperty<double>(TIME_START) : -1;
        }

        /// <summary>
        /// Returns room Duration Time.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static int GetDurationTime(this Room room)
        {
            return room.HasProperty(TIME_START) ? room.GetProperty<int>(TIME_DURATION) : -1;
        }

        /// <summary>
        /// Defines the Start Time Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="time"></param>
        public static void SetStartTime(this Room room, double time)
        {
            room.SetProperty(TIME_START, time, false);
        }

        /// <summary>
        /// Defines the duration time of this Room (in seconds).
        /// </summary>
        /// <param name="room"></param>
        /// <param name="timeInSeconds"></param>
        public static void SetDurationTime(this Room room, int timeInSeconds)
        {
            room.SetProperty(TIME_DURATION, timeInSeconds, false);
        }
        
        public static void SetPing(this Room room)
        {
            int averagePing = 0;
            foreach (var player in room.Players)
            {
                averagePing += player.Value.GetPing();
            }
            SetInt(room, PING, averagePing / room.PlayerCount, false);
        }

        /// <summary>
        /// Returns a Room property of type Bool.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static bool GetBool(this Room room, string propertyName, bool fallback = false)
        {
            return room.HasProperty(propertyName) ? room.GetProperty<bool>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Room property of type string.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static string GetString(this Room room, string propertyName, string fallback = "")
        {
            return room.HasProperty(propertyName) ? room.GetProperty<string>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Room property of type float.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static float GetFloat(this Room room, string propertyName, float fallback = 0)
        {
            return room.HasProperty(propertyName) ? room.GetProperty<float>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Room property of type double.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static double GetDouble(this Room room, string propertyName, double fallback = 0)
        {
            return room.HasProperty(propertyName) ? room.GetProperty<double>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Room property of type int.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static int GetInt(this Room room, string propertyName, int fallback = 0)
        {
            return room.HasProperty(propertyName) ? room.GetProperty<int>(propertyName) : fallback;
        }

        /// <summary>
        /// Returns a Room property.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetProperty<T>(this Room room, string propertyName)
        {
            if (room.CustomProperties.TryGetValue(propertyName, out var prop))
            {
                return (T)Convert.ChangeType(prop, typeof(T));
            }

            return (T)Convert.ChangeType(null, typeof(T));
        }

        /// <summary>
        /// Returns a Room property.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetProperty(this Room room, string propertyName)
        {
            if (room.CustomProperties.TryGetValue(propertyName, out var prop))
            {
                return prop;
            }

            return null;
        }

        /// <summary>
        /// Returns true if the give property is found
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this Room room, string propertyName)
        {
            return room != null && (room.CustomProperties?.ContainsKey(propertyName) ?? false);
        }

        /// <summary>
        /// Incremements an int property from the Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void AddInt(this Room room, string propertyName, int propertyValue, bool webFoward)
        {
            if (!room.HasProperty(propertyName))
            {
                room.SetProperty(propertyName, propertyValue, webFoward);
            }
            else
            {
                room.SetProperty(propertyName, propertyValue + room.GetProperty<int>(propertyName), webFoward);
            }
        }

        /// <summary>
        /// Incremements an int property from the Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void AddFloat(this Room room, string propertyName, float propertyValue, bool webFoward)
        {
            if (!room.HasProperty(propertyName))
            {
                room.SetProperty(propertyName, propertyValue, webFoward);
            }
            else
            {
                room.SetProperty(propertyName, propertyValue + room.GetProperty<float>(propertyName), webFoward);
            }
        }

        /// <summary>
        /// Clear all properties from this Room.
        /// </summary>
        /// <param name="room"></param>
        public static void ClearAllProperties(this Room room)
        {
            Hashtable t = room.CustomProperties;
            List<object> keys = new List<object>(t.Keys);
            for (int i = 0, imax = keys.Count; i < imax; i++)
            {
                t[keys[i]] = null;
            }
            room.SetCustomProperties(t);
        }

        /// <summary>
        /// Defines a string property on this Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void SetString(this Room room, string propertyName, string propertyValue, bool webFoward)
        {
            room.SetProperty(propertyName, propertyValue, webFoward);
        }

        /// <summary>
        /// Defines a int property on this Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void SetInt(this Room room, string propertyName, int propertyValue, bool webFoward)
        {
            room.SetProperty(propertyName, propertyValue, webFoward);
        }

        /// <summary>
        /// Defines a float property on this Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void SetFloat(this Room room, string propertyName, float propertyValue, bool webFoward)
        {
            room.SetProperty(propertyName, propertyValue, webFoward);
        }

        /// <summary>
        /// Defines a double property on this Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void SetDouble(this Room room, string propertyName, double propertyValue, bool webFoward)
        {
            room.SetProperty(propertyName, propertyValue, webFoward);
        }

        /// <summary>
        /// Defines a bool property on this Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void SetBool(this Room room, string propertyName, bool propertyValue, bool webFoward)
        {
            room.SetProperty(propertyName, propertyValue, webFoward);
        }

        /// <summary>
        /// Defines a property on this Room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="webFoward"></param>
        public static void SetProperty<T>(this Room room, string propertyName, T propertyValue, bool webFoward)
        {
            Hashtable startTimeProp = new Hashtable();  // only use ExitGames.Client.Photon.Hashtable for Photon
            startTimeProp[propertyName] = propertyValue;
            PhotonNetwork.CurrentRoom.SetCustomProperties(startTimeProp);
            //room.SetCustomProperties(new Hashtable() { { propertyName, propertyValue } }, null, webFoward);
        }
    }
}