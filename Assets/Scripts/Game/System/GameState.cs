using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public enum GameState
    {
        AWAKE,
        WAIT_CLIENT,
        GAME_START,
        GAME_NOW,
        GAME_FINISHED,
        GAME_SERVER_WIN,
        GAME_SERVER_LOSE
    }

    public enum PlayerStatus
    {
        OFFLINE,
        
        ONLINE_SERVER,
        ONLINE_CLIENT,
        ONLINE_WATCHER
    }

    public static class PlayerStatusEx
    {
        public static bool IsMaster(this PlayerStatus status)
        {
            switch(status)
            {
                case PlayerStatus.OFFLINE:
                case PlayerStatus.ONLINE_SERVER:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsClient(this PlayerStatus status)
        {
            switch (status)
            {
                case PlayerStatus.ONLINE_CLIENT:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsWatcher(this PlayerStatus status)
        {
            switch (status)
            {
                case PlayerStatus.ONLINE_WATCHER:
                    return true;
                default:
                    return false;
            }
        }
    }
}
