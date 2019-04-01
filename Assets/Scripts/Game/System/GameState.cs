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
}
