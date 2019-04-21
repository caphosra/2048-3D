using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Capra314Cabra.Project_2048Ex
{
    class LobbyGUI : MonoBehaviour
    {
        public void FightOnline()
        {
            GameStartArgment.OnlineGame = true;
            SceneManager.LoadScene("Scenes/OnlineMatching");
        }

        public void FightOffline()
        {
            GameStartArgment.OnlineGame = false;
            SceneManager.LoadScene("Scenes/Game");
        }
    }
}
