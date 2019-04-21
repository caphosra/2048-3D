using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class GameSyncerOffline : IGameSyncer
    {
        public event GameSyncerHandler OnAllPlayerReady;
        public event GameStateChangeHandler OnGameStateChanged;
        public event GameSyncerHandler OnNameChanged;
        public event GameFinishHandler OnGameFinished;

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

        public string MasterName { get; set; } = "unknown";
        public string ClientName { get; set; } = "unknown";


        public Queue<GameAction> DoneActions { get; set; } = new Queue<GameAction>();

        public GameSyncerOffline()
        {
            
        }

        public void Ready()
        {
            OnAllPlayerReady?.Invoke();
        }

        public void ChangeName(string name)
        {
            MasterName = name;
        }

        public void InvokeAction(ActionType actionType, int param)
        {
            DoneActions.Enqueue(new GameAction(true, actionType, param));
        }

        public void EndGame(Winner winner)
        {
            OnGameFinished?.Invoke(winner);
        }
    }
}
