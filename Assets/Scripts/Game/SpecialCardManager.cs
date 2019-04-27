using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class SpecialCardManager : MonoBehaviour
    {
        #region Attacker Side

        public IGameSyncer GameSyncer { get; set; }
        public bool Running { get; set; } = false;

        private Queue<ISpecialCard> cardQueue = new Queue<ISpecialCard>();

        public void Run(ISpecialCard card)
        {
            cardQueue.Enqueue(card);
        }

        private void Update()
        {
            if(!Running)
            {
                while(!Running && (cardQueue.Count != 0))
                {
                    var card = cardQueue.Dequeue();
                    Debug.Log(card.Name);
                    GameSyncer.InvokeAction(ActionType.SPECIAL_CARD, (int)card.SpecialStatusType);
                    if (card.Span != 0f)
                    {
                        Running = true;
                        StartCoroutine(WaitForFinishSpecialStatus(card.Span));
                    }
                }
            }
        }

        private IEnumerator WaitForFinishSpecialStatus(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            GameSyncer.InvokeAction(ActionType.CLEAR_SPECIAL, 0);
            Running = false;
        }

        #endregion

        #region Attackee Side

        public SpecialStatusType CurrentStatus { get; set; } = SpecialStatusType.NONE;

        public void OnAddSpecialStatus(SpecialStatusType type)
        {
            switch(type)
            {
                case SpecialStatusType.NONE:
                    CurrentStatus = SpecialStatusType.NONE;
                    break;
                case SpecialStatusType.WRONG_MOVE:
                    CurrentStatus = SpecialStatusType.WRONG_MOVE;
                    break;
            }
        }

        public void OnClearSpecialStatus()
        {
            CurrentStatus = SpecialStatusType.NONE;
        }

        #endregion
    }
}
