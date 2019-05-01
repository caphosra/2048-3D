using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class GameManager : MonoBehaviour
    {
        #region MonoBehaviour Instance (Edit on Inspector)

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private GUIBoardManager masterBoardGraphicManager;
        [SerializeField]
        private GUIBoardManager clientBoardGraphicManager;

        [SerializeField]
        private GameGUIManager gameGUIManager;
        [SerializeField]
        private SpecialCardManager specialCardManager;

        #region For Count Down

        [SerializeField]
        private GameObject countDownGUI;
        [SerializeField]
        private Text countDownText;

        #endregion

        #endregion

        #region Game Logic Class Instance

        const int DEFAULT_SCORE = 10;

        private int masterScore = DEFAULT_SCORE;
        private BlockBoard masterBoard = new BlockBoard();

        private int clientScore = DEFAULT_SCORE;
        private BlockBoard clientBoard = new BlockBoard();

        #endregion

        PhotonManager photonManager;
        IGameSyncer gameSyncer;

        void Start()
        {
            gameGUIManager.Init(masterScore, clientScore);

            if (GameStartArgment.OnlineGame)
            {
                Debug.Log("Unload Matching Scene...");
                StartCoroutine(UnloadMatchingScene());

                photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
                gameSyncer = photonManager;

                //
                // Debug code
                //
#if UNITY_EDITOR

                var master = PhotonNetwork.MasterClient;
                Debug.Log($"MasterNickName:{master.NickName}");
                var you = PhotonNetwork.NickName;
                Debug.Log($"YourNickName:{you}");
                Debug.Log($"You are \"{(PhotonNetwork.IsMasterClient ? "Master" : "Client")}\"");

#endif
            }
            else
            {
                gameSyncer = new GameSyncerOffline();
            }

            if(gameSyncer.PlayerStatus.IsClient())
            {
                var tmp = masterBoardGraphicManager;
                masterBoardGraphicManager = clientBoardGraphicManager;
                clientBoardGraphicManager = tmp;

                gameGUIManager.IsSwaped = true;
            }

            //
            // If the player is not a watcher, the blocks make themselves clickable.
            //
            if(!gameSyncer.PlayerStatus.IsWatcher())
            {
                if (gameSyncer.PlayerStatus.IsMaster())
                {
                    masterBoardGraphicManager.Clickable = true;
                    masterBoardGraphicManager.OnBlockClicked += OnBlockClickedCallback;
                }
                else
                {
                    clientBoardGraphicManager.Clickable = true;
                    clientBoardGraphicManager.OnBlockClicked += OnBlockClickedCallback;
                }

                gameSyncer.ChangeName(GameStartArgment.OnlineGame ? PhotonNetwork.NickName : "You");

                specialCardManager.GameSyncer = gameSyncer;
            }

            //
            // Reload the graphics of the blocks.
            //
            masterBoardGraphicManager.ChangeGraphicAll(masterBoard);
            clientBoardGraphicManager.ChangeGraphicAll(clientBoard);

            gameSyncer.State = GameState.GAME_START;

            gameSyncer.OnAllPlayerReady += () =>
            {
                OnChangeNameCallback();
                StartCoroutine(CountDown());
            };
            gameSyncer.OnGameStateChanged += OnGameStateChangedCallback;
            gameSyncer.OnNameChanged += OnChangeNameCallback;
            gameSyncer.OnGameFinished += OnGameFinished;

            //
            // The player is, now, ready.
            //
            gameSyncer.Ready();
        }

        void Update()
        {
            switch (gameSyncer.State)
            {
                case GameState.GAME_NOW:
                    {
                        if (!gameSyncer.PlayerStatus.IsWatcher())
                        {
                            if (specialCardManager.CurrentStatus != SpecialStatusType.WRONG_MOVE)
                            {
                                if (Input.GetKeyDown(KeyCode.W)) InvokeBlockMoved(MoveDirection.UP);
                                else if (Input.GetKeyDown(KeyCode.S)) InvokeBlockMoved(MoveDirection.DOWN);
                                else if (Input.GetKeyDown(KeyCode.A)) InvokeBlockMoved(MoveDirection.LEFT);
                                else if (Input.GetKeyDown(KeyCode.D)) InvokeBlockMoved(MoveDirection.RIGHT);
                            }
                            else
                            {
                                //
                                // A WorngMove Card is used.
                                //
                                if (Input.GetKeyDown(KeyCode.S)) InvokeBlockMoved(MoveDirection.UP);
                                else if (Input.GetKeyDown(KeyCode.W)) InvokeBlockMoved(MoveDirection.DOWN);
                                else if (Input.GetKeyDown(KeyCode.D)) InvokeBlockMoved(MoveDirection.LEFT);
                                else if (Input.GetKeyDown(KeyCode.A)) InvokeBlockMoved(MoveDirection.RIGHT);
                            }
                        }
                    }
                    break;
            }
            while(gameSyncer.DoneActions.Any())
            {
                var action = gameSyncer.DoneActions.Dequeue();
                DoAction(action);
            }
        }

        //
        // Execute the instance of the GameAction
        //
        void DoAction(GameAction action)
        {
            switch(action.ActionType)
            {
                case ActionType.BLOCK_MOVED:
                    {
                        if (action.IsMaster)
                        {
                            masterBoard.Move((MoveDirection)action.Parameter, out _);
                            masterBoardGraphicManager.ChangeGraphicAll(masterBoard);
                        }
                        else
                        {
                            clientBoard.Move((MoveDirection)action.Parameter, out _);
                            clientBoardGraphicManager.ChangeGraphicAll(clientBoard);
                        }
                    }
                    break;
                case ActionType.BLOCK_SPAWN:
                    {
                        int x = action.Parameter / 16;
                        int y = action.Parameter % 16;

                        if (action.IsMaster)
                        {
                            masterBoard.SetValue(x, y, 2);
                            masterBoardGraphicManager.ChangeGraphicAll(masterBoard);
                            masterBoardGraphicManager.ShowBornParticleAt(x, y);
                        }
                        else
                        {
                            clientBoard.SetValue(x, y, 2);
                            clientBoardGraphicManager.ChangeGraphicAll(clientBoard);
                            clientBoardGraphicManager.ShowBornParticleAt(x, y);
                        }
                    }
                    break;
                case ActionType.BLOCK_DELETE:
                    {
                        int x = action.Parameter / 16;
                        int y = action.Parameter % 16;

                        if (action.IsMaster)
                        {
                            masterBoard.SetValue(x, y, 0);
                            masterBoardGraphicManager.ChangeGraphicAll(masterBoard);
                        }
                        else
                        {
                            clientBoard.SetValue(x, y, 0);
                            clientBoardGraphicManager.ChangeGraphicAll(clientBoard);
                        }
                    }
                    break;
                case ActionType.ADD_SCORE:
                    {
                        if (action.IsMaster)
                        {
                            masterScore += action.Parameter;
                        }
                        else
                        {
                            clientScore += action.Parameter;
                        }
                        gameGUIManager.UpdateScoreText(masterScore, clientScore);
                    }
                    break;
                case ActionType.SPECIAL_CARD:
                    {
                        var specialStatusType = (SpecialStatusType)action.Parameter;
                        var runner = action.IsMaster;
                        var attackeeIsMaster = specialStatusType.IsAttack() ? !runner : runner;

                        if(attackeeIsMaster == true)
                        {
                            // TODO
                        }
                        else
                        {
                            // TODO   
                        }

                        if(!gameSyncer.PlayerStatus.IsWatcher())
                        {
                            if(gameSyncer.PlayerStatus.IsMaster() == attackeeIsMaster)
                            {
                                specialCardManager.OnAddSpecialStatus(specialStatusType);
                            }
                        }
                    }
                    break;
                case ActionType.CLEAR_SPECIAL:
                    {
                        //
                        // If you are master, your status will be changed by only client.
                        // And its opposite is also true.
                        //
                        if (action.IsMaster != gameSyncer.PlayerStatus.IsMaster())
                        {
                            specialCardManager.OnClearSpecialStatus();
                        }
                    }
                    break;
            }
        }

        #region Coroutine Functions

        IEnumerator UnloadMatchingScene()
        {
            yield return SceneManager.UnloadSceneAsync("OnlineMatching");
            Debug.Log("Unloaded Matching Scene Successfuly");
        }

        IEnumerator CountDown()
        {
            countDownGUI.SetActive(true);

            for(int i = 3; i >= 1; i--)
            {
                countDownText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            Destroy(countDownGUI);
            StartCoroutine(ReserveGameFinishEvent());
            gameSyncer.State = GameState.GAME_NOW;
        }

        #endregion

        #region Support Funcitons

        private void InvokeBlockMoved(MoveDirection direction)
        {
            var board = gameSyncer.PlayerStatus.IsMaster() ? masterBoard : clientBoard; 
            var clone = board.Clone() as BlockBoard;
            clone.Move(direction, out List<(int, int)> changed);
            if (changed.Count != 0 && !clone.Full)
            {
                gameSyncer.InvokeAction(ActionType.BLOCK_MOVED, (int)direction);

                var random_spawn_pos = clone.RandomSpawn();
                var random_spawn_pos_zip = random_spawn_pos.x * 16 + random_spawn_pos.y;
                gameSyncer.InvokeAction(ActionType.BLOCK_SPAWN, random_spawn_pos_zip);
            }
        }

        private IEnumerator ReserveGameFinishEvent()
        {
            for(int counter = 120; counter >= 1; counter--)
            {
                gameGUIManager.UpdateRemainingTimeText(counter);
                yield return new WaitForSeconds(1f);
            }

            gameGUIManager.UpdateRemainingTimeText(0);

            gameSyncer.State = GameState.GAME_FINISHING;

            if (gameSyncer.PlayerStatus.IsMaster())
            {
                if (masterScore > clientScore)
                {
                    gameSyncer.EndGame(Winner.MASTER_WIN);
                }
                else if (masterScore < clientScore)
                {
                    gameSyncer.EndGame(Winner.CLIENT_WIN);
                }
                else
                {
                    gameSyncer.EndGame(Winner.DRAW);
                }
            }
        }

        #endregion

        #region EventCallbacks

        private void OnGameStateChangedCallback(GameState state)
        {
            switch(state)
            {
                case GameState.GAME_NOW:
                    {
                        if (!gameSyncer.PlayerStatus.IsWatcher())
                        {
                            var random_spawn_pos_zip = Random.Range(1, 4 + 1) * 16 + Random.Range(1, 4 + 1);
                            gameSyncer.InvokeAction(ActionType.BLOCK_SPAWN, random_spawn_pos_zip);
                        }
                    }
                    break;
            }
        }

        private void OnChangeNameCallback()
        {
            gameGUIManager.SetPlayersName(gameSyncer.MasterName, gameSyncer.ClientName);
        }

        private int SCORE_64 = 5;
        private int SCORE_128 = 11;
        private int SCORE_256 = 23;
        private int SCORE_512 = 47;
        private void OnBlockClickedCallback(byte x, byte y)
        {
            var board = gameSyncer.PlayerStatus.IsMaster() ? masterBoard : clientBoard;

            var val = board.GetValue(x, y);
            if(val < 64)
            {
                return;
            }
            gameSyncer.InvokeAction(ActionType.BLOCK_DELETE, x * 16 + y);

            var score = 0;
            ISpecialCard card = null;
            switch(val)
            {
                case 64:
                    score = SCORE_64;
                    card = DataBase.UserDatabase.PlayersSpecialCards[64];
                    break;
                case 128:
                    score = SCORE_128;
                    card = DataBase.UserDatabase.PlayersSpecialCards[128];
                    break;
                case 256:
                    score = SCORE_256;
                    card = DataBase.UserDatabase.PlayersSpecialCards[256];
                    break;
                case int n when n > 512:
                    score = SCORE_512;
                    card = DataBase.UserDatabase.PlayersSpecialCards[512];
                    break;
            }
            gameSyncer.InvokeAction(ActionType.ADD_SCORE, score);
            specialCardManager.Run(card);
        }

        private void OnGameFinished(Winner winner)
        {
            gameSyncer.State = GameState.GAME_FINISHED;
            gameGUIManager.ShowResult(winner);
        }

        #endregion
    }
}
