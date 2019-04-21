using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class GameGUIManager : MonoBehaviour
    {
        [SerializeField]
        private Image playerProgressBar;

        [SerializeField]
        private Text playerScoreText;
        [SerializeField]
        private Text enemyScoreText;
        [SerializeField]
        private Text remainingTimeText;

        [SerializeField]
        GameObject resultGUI;
        [SerializeField]
        private Transform WinText;
        [SerializeField]
        private Transform LoseText;
        [SerializeField]
        private Transform DrawText;

        [SerializeField]
        private Text playerNameText;
        [SerializeField]
        private Text enemyNameText;

        /// <summary>
        /// If the player is the client, you should make this value "true".
        /// </summary>
        public bool IsSwaped { get; set; } = false;

        public void Init(int masterScore, int clientScore)
        {
            if(masterScore != clientScore)
            {
                throw new System.ArgumentException("The value of MasterScore is different from ClientScore");
            }
            UpdateScoreText(masterScore, clientScore);
        }

        public void UpdateScoreText(int masterScore, int clientScore)
        {
            playerProgressBar.fillAmount = ((float)(IsSwaped ? clientScore : masterScore) / (masterScore + clientScore));
            playerScoreText.text = (IsSwaped ? clientScore : masterScore).ToString();
            enemyScoreText.text = (IsSwaped ? masterScore : clientScore).ToString();
        }

        public void UpdateRemainingTimeText(int remainingTime)
        {
            remainingTimeText.text = remainingTime.ToString();
        }

        public void ShowResult(Winner winner)
        {
            resultGUI.SetActive(true);

            if (winner == Winner.DRAW)
            {
                WinText.gameObject.SetActive(false);
                LoseText.gameObject.SetActive(false);
            }
            else
            {
                DrawText.gameObject.SetActive(false);

                var swap = IsSwaped ^ (winner == Winner.CLIENT_WIN);
                if (swap)
                {
                    var tmp = WinText.transform.position;
                    WinText.transform.position = LoseText.transform.position;
                    LoseText.transform.position = tmp;
                }
            }
        }

        public void SetPlayersName(string masterName, string clientName)
        {
            playerNameText.text = IsSwaped ? clientName : masterName;
            enemyNameText.text = IsSwaped ? masterName : clientName;
        }

        public void BackToLobby()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
