using System.Collections.Generic;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public interface IGameSyncer
    {
        PlayerStatus PlayerStatus { get; set; }
        GameState State{ get; set; }

        event GameStateChangeHandler OnGameStateChanged;
        event GameSyncerHandler OnAllPlayerReady;

        event GameFinishHandler OnGameFinished;
        
        Queue<GameAction> DoneActions { get; set; }

        void Ready();
        void InvokeAction(ActionType actionType, int param);
        void EndGame(Winner winner);
    }

    public struct GameAction
    {
        public bool IsMaster { get; set; }

        public ActionType ActionType { get; set; }
        public int Parameter { get; set; }

        public GameAction(bool isMaster, ActionType type, int param)
        {
            IsMaster = isMaster;
            ActionType = type;
            Parameter = param;
        }
    }

    public enum ActionType
    {
        BLOCK_MOVED,
        BLOCK_SPAWN,
        BLOCK_DELETE,

        ADD_SCORE,
    }

    public enum Winner
    {
        DRAW,

        MASTER_WIN,
        CLIENT_WIN
    }

    public delegate void GameSyncerHandler(); 
    public delegate void GameStateChangeHandler(GameState state);
    public delegate void GameBlockMoveHandler(bool isMaster, MoveDirection direction);
    public delegate void GameSpawnBlockHandler(bool isMaster, byte x, byte y);
    public delegate void GameFinishHandler(Winner winner);
}