using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class GameSyncerOffline : IGameSyncer
    {
        public PlayerStatus PlayerStatus { get; set; } = PlayerStatus.OFFLINE;

        private GameState m_State;
        public GameState State
        {
            get => m_State;
            set
            {
                m_State = value;
                OnGameStateChanged?.Invoke(m_State);
            }
        }
        public event GameStateChangeHandler OnGameStateChanged;

        public Queue<GameAction> DoneActions { get; set; } = new Queue<GameAction>();

        public GameSyncerOffline()
        {
            
        }

        public event GameSyncerHandler OnAllPlayerReady;
        public void Ready()
        {
            OnAllPlayerReady?.Invoke();
        }

        public void InvokeAction(ActionType actionType, int param)
        {
            DoneActions.Enqueue(new GameAction(true, actionType, param));
        }
    }
}
