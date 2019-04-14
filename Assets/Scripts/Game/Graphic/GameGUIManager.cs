using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGUIManager : MonoBehaviour
{
    [SerializeField]
    private Image playerProgressBar;
    [SerializeField]
    private Image enemyProgressBar;

    private float m_PlayerAmount = 0f;
    public float PlayerAmount
    {
        get => m_PlayerAmount;
        set
        {
            m_PlayerAmount = value;
            OnPlayerAmountChanged();
        }
    }

    private float m_EnemyAmount = 0f;
    public float EnemyAmount
    {
        get => m_EnemyAmount;
        set
        {
            m_EnemyAmount = value;
            OnEnemyAmountChanged();
        }
    }

    public void Swap()
    {
        var tmp = playerProgressBar;
        playerProgressBar = enemyProgressBar;
        enemyProgressBar = tmp;
    }

    private void OnPlayerAmountChanged()
    {
        playerProgressBar.fillAmount = m_PlayerAmount;
    }

    private void OnEnemyAmountChanged()
    {
        enemyProgressBar.fillAmount = m_EnemyAmount;
    }
}
