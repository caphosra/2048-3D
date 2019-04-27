using System.Collections.Generic;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public interface IGameSyncer
    {
        /// <summary>
        /// 
        /// The status of the player
        /// 
        /// </summary>
        PlayerStatus PlayerStatus { get; set; }

        /// <summary>
        /// 
        /// The status of the game (When the value of it is changed, "IGameSyncer.OnGameStateChanged" will be caused.)
        /// 
        /// </summary>
        GameState State { get; set; }

        /// <summary>
        /// 
        /// Master Name
        /// 
        /// </summary>
        string MasterName { get; set; }
        
        /// <summary>
        /// 
        /// Client Name
        /// 
        /// </summary>
        string ClientName { get; set; }

        /// <summary>
        /// 
        /// It's caused when the value of "IGameSyncer.State" is changed.
        /// 
        /// </summary>
        event GameStateChangeHandler OnGameStateChanged;

        /// <summary>
        /// 
        /// It's caused when Both the master and the clinet has called "IGameSyncer.Ready".  
        /// 
        /// </summary>
        event GameSyncerHandler OnAllPlayerReady;

        /// <summary>
        /// 
        /// It's caused when the player's name or the opposite's is changed. 
        /// 
        /// </summary>
        event GameSyncerHandler OnNameChanged;

        /// <summary>
        /// 
        /// It's caused when the master called "IGameSyncer.EndGame".
        /// 
        /// </summary>
        event GameFinishHandler OnGameFinished;
        
        /// <summary>
        /// 
        /// There are the GameAction instances each of which will be executed.
        /// 
        /// </summary>
        Queue<GameAction> DoneActions { get; set; }

        /// <summary>
        /// 
        /// [Watcher Never Use] Report that the player is ready.
        /// 
        /// </summary>
        void Ready();

        /// <summary>
        /// [Watcher Never Use] Change your name.
        /// </summary>
        /// <param name="name">a new player name</param>
        void ChangeName(string name);

        /// <summary>
        /// Put "ActionType" and some parameters to the queue of "IGameSyncer.DoneActions".
        /// </summary>
        /// <param name="actionType">Action type</param>
        /// <param name="param">Parameters ziped</param>
        void InvokeAction(ActionType actionType, int param);

        /// <summary>
        /// [Master Only] Declare the end of the game.
        /// </summary>
        /// <param name="winner">Winner (If the game was draw, the value would be Winner.DRAW)</param>
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

        SPECIAL_CARD,
        CLEAR_SPECIAL,
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