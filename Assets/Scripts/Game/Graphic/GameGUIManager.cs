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

        public void Init(int masterScore, int clientScore)
        {
            if(masterScore != clientScore)
            {
                throw new System.ArgumentException("The value of MasterScore is different from ClientScore");
            }
            Show(masterScore, clientScore);
        }

        public void Show(int masterScore, int clientScore)
        {
            playerProgressBar.fillAmount = ((float)(IsSwaped ? clientScore : masterScore) / (masterScore + clientScore));
            playerScoreText.text = (IsSwaped ? clientScore : masterScore).ToString();
            enemyScoreText.text = (IsSwaped ? masterScore : clientScore).ToString();
        }
    }
}
