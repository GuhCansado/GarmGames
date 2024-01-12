using System;
using System.Collections.Generic;
using System.Text;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    public static class PhotonVariableParser
    {
        private const string ERR_BRACKETS = "Mis-matched bracket in expression: {0}";

        private const string EMPTY_SPACE = " ";

        private const char COLON = ':';
        private const char PIPE = '|';
        private const string COLONS = ":";
        private const string EXPRESSION = "[{0}]";
        private const string BRACKETS = "[]";
        private const string BRACKET_LEFT = "[";
        private const string BRACKET_RIGHT = "]";

        private const string FORMAT = "n0";
        private const string FORMAT_TIME = "{0:D2}:{1:D2}";
        private const string THIS = "this";
        private const string LOCAL = "local";
        private const string NAME = "Name";
        private const string SCORE = "Score";
        private const string ROOM_NAME = "RoomName";
        private const string ROOM_PROP = "Room:";
        private const string PLAYER_COUNT = "PlayerCount";
        private const string MAX_PLAYERS = "MaxPlayers";
        private const string ZERO = "0";
        private const string NOROOM = "None";
        private const string UNKNOWN = "Unknown";
        private const string TIME = "TIME";
        private const string TIME_START = "StartTime";
        private const string TIME_ELAPSED = "ElapsedTime";
        private const string TIME_REMAINING = "RemainingTime";
        private const string PING = "Ping";
        private static readonly Dictionary<int, PhotonView> CachedViews = new();

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static string Parse(string content, GameObject invoker, out bool needsMonoUpdate)
        {
            bool needsUpdate = false;
            List<string> tokens = GetTokens(content);
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];
                if (token == BRACKET_LEFT)
                {
                    string subExpr = GetSubExpression(tokens, ref tokenIndex);
                    operatorStack.Push(Parse(subExpr, invoker, out needsMonoUpdate));
                    continue;
                }

                if (token == BRACKET_RIGHT)
                {
                    throw new ArgumentException(string.Format(ERR_BRACKETS, content));
                }

                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();

                if (op.ToUpper().Contains(TIME))
                {
                    content = GetTime(op, content);
                    needsUpdate = true;
                    needsMonoUpdate = true;
                }
                else if (op.StartsWith(LOCAL, StringComparison.InvariantCultureIgnoreCase))
                {
                    content = GetPlayerData(PhotonNetwork.LocalPlayer, op, content);
                }
                else if (op.StartsWith(THIS, StringComparison.InvariantCultureIgnoreCase))
                {
                    PhotonView view = GetPhotonView(invoker);
                    content = GetPlayerData(!view ? null : view.Owner, op, content);
                }
                else if (op.StartsWith(ROOM_NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    content = content.Replace(string.Format(EXPRESSION, op), PhotonNetwork.CurrentRoom == null ? NOROOM : PhotonNetwork.CurrentRoom.Name);
                }
                else if (op.StartsWith(ROOM_PROP, StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] entries = op.Split(new char[] { COLON }, StringSplitOptions.RemoveEmptyEntries);
                    string roomProperty = entries[1];
                    content = content.Replace(string.Format(EXPRESSION, op), PhotonNetwork.CurrentRoom == null ? NOROOM : PhotonNetwork.CurrentRoom.GetProperty(roomProperty).ToString());
                }
                else if (op.StartsWith(PLAYER_COUNT, StringComparison.InvariantCultureIgnoreCase))
                {
                    content = content.Replace(string.Format(EXPRESSION, op), PhotonNetwork.CurrentRoom == null ? ZERO : PhotonNetwork.CurrentRoom.PlayerCount.ToString());
                }
                else if (op.StartsWith(MAX_PLAYERS, StringComparison.InvariantCultureIgnoreCase))
                {
                    content = content.Replace(string.Format(EXPRESSION, op), PhotonNetwork.CurrentRoom == null ? ZERO : PhotonNetwork.CurrentRoom.MaxPlayers.ToString());
                }
                else if (op.StartsWith(COLONS, StringComparison.InvariantCultureIgnoreCase) || op.Contains(COLONS))
                {
                    content = GetPlayerData(op, content);
                }
            }

            needsMonoUpdate = needsUpdate;

            return content;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private static PhotonView GetPhotonView(GameObject invoker)
        {
            int id = invoker.GetInstanceID();
            if (CachedViews.ContainsKey(id))
            {
                return CachedViews[id];
            }

            PhotonView view = invoker.GetComponentInChildren<PhotonView>();
            if (!view) view = invoker.GetComponentInParent<PhotonView>();

            CachedViews.Add(id, view);

            return view;
        }

        private static string GetTime(string op, string content)
        {
            double time = 0;
            if (op.StartsWith(TIME_START, StringComparison.InvariantCultureIgnoreCase))
            {
                time = PhotonNetwork.CurrentRoom.GetStartTime();
            }
            else if (op.StartsWith(TIME_ELAPSED, StringComparison.InvariantCultureIgnoreCase))
            {
                time = PhotonNetwork.CurrentRoom.GetElapsedTime();
            }
            else if (op.StartsWith(TIME_REMAINING, StringComparison.InvariantCultureIgnoreCase))
            {
                time = PhotonNetwork.CurrentRoom.GetRemainingTime();
            }

            TimeSpan ts = TimeSpan.FromSeconds(time);

            string[] entries = op.Split(new char[] { PIPE }, StringSplitOptions.RemoveEmptyEntries);

            if (entries.Length > 1)
            {
                string formatted = string.Format(FORMAT_TIME, ts.Minutes, ts.Seconds);
                string format = entries[1].ToString();
                string[] sc = format.Split(COLON);

                if (sc.Length == 1) formatted = string.Format(format, ts.Minutes, ts.Seconds);
                else if (sc.Length > 1) formatted = string.Format(format, ts.Hours, ts.Minutes, ts.Seconds);
                else if (sc.Length > 2) formatted = string.Format(format, ts.Days, ts.Hours, ts.Minutes, ts.Seconds);

                content = content.Replace(string.Format(EXPRESSION, op), formatted);
            }
            else
            {
                content = content.Replace(string.Format(EXPRESSION, op), string.Format(FORMAT_TIME, ts.Minutes, ts.Seconds));
            }

            return content;
        }

        private static string GetPlayerData(string op, string content)
        {
            if (!PhotonNetwork.InRoom) return content;

            string[] entries = op.Split(new char[] { COLON }, StringSplitOptions.RemoveEmptyEntries);

            if (entries.Length > 1)
            {
                int playerId = -1;
                if (int.TryParse(entries[0], out playerId))
                {
                    if (playerId != -1)
                    {
                        //Debug.LogWarningFormat("GetData id: {0} cachedPlayers: {1} players: {2}", playerId, NetworkManager.Instance.Players, PhotonNetwork.PlayerList.Length);
                        Player player = playerId > PhotonNetworkManager.Instance.Players.Length ? null : PhotonNetworkManager.Instance.Players[Mathf.Max(0, playerId - 1)];
                        if (playerId == 0) player = PhotonNetwork.MasterClient;
                        
                        content = GetPlayerData(player, op, content);
                        //Debug.Log("GetPlayerData playerId: " + playerId + " / op: "+ op + " / content: " + content + " / player: " + 
                        //    player+ " / InRoom: " + PhotonNetwork.InRoom+" / Players: "+ NetworkManager.Instance.Players.Length+" / index: "+ Mathf.Max(0, playerId - 1));
                    }
                }
            } 

            return content;
        }

        private static string GetPlayerData(Player player, string op, string content)
        {
            string[] entries = op.Split(new char[] { COLON }, StringSplitOptions.RemoveEmptyEntries);
            string entry = entries[1];

            if (entry.StartsWith(NAME, StringComparison.InvariantCultureIgnoreCase) || entry.ToUpper().Contains(NAME.ToUpper()))
            {
                content = content.Replace(string.Format(EXPRESSION, op), player == null ? UNKNOWN : player.NickName);
            }
            else if (entry.StartsWith(SCORE, StringComparison.InvariantCultureIgnoreCase) || entry.ToUpper().Contains(SCORE.ToUpper()))
            {
                content = content.Replace(string.Format(EXPRESSION, op), player == null ? UNKNOWN : player.GetScore().ToString(FORMAT));
            }
            else if (entry.StartsWith(PING, StringComparison.InvariantCultureIgnoreCase) || entry.ToUpper().Contains(PING.ToUpper()))
            {
                content = content.Replace(string.Format(EXPRESSION, op), player == null ? UNKNOWN : player.GetPing().ToString(FORMAT));
            }
            else
            {
                content = content.Replace(string.Format(EXPRESSION, op), player == null ? UNKNOWN : player.GetProperty(entry).ToString());
            }

            return content;
        }

        private static string GetSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == BRACKET_LEFT)
                {
                    parenlevels += 1;
                }

                if (tokens[index] == BRACKET_RIGHT)
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException(string.Format(ERR_BRACKETS, subExpr));
            }

            return subExpr.ToString();
        }

        private static List<string> GetTokens(string expression)
        {
            string literals = BRACKETS;
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(EMPTY_SPACE, string.Empty))
            {
                if (literals.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c.ToString());
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }
}