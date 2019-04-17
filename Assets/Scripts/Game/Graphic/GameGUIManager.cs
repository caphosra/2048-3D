using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        /// <summary>
        /// If the player is the client, you should make this value "true".
        /// </summary>
        public bool IsSwaped { get; set; } = false;

        public void Init()
        {
            playerProgressBar.fillAmount = 0.5f;
            playerScoreText.text = "10";
            enemyScoreText.text = "10";
        }

        public void Show(int playerScore, int enemyScore)
        {
            playerProgressBar.fillAmount = ((IsSwaped ? enemyScore : playerScore) / (playerScore + enemyScore));
            playerScoreText.text = (IsSwaped ? enemyScore : playerScore).ToString();
            enemyScoreText.text = (IsSwaped ? playerScore : enemyScore).ToString();
        }
    }
}
