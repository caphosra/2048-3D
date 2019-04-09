using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class GameManager : MonoBehaviour
    {
        #region MonoBehaviour Instance (Edit on Inspector)

        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private GUIBoardManager boardGraphicManager;
        [SerializeField]
        private GameObject countDownGUI;
        [SerializeField]
        private Text countDownText;
        [SerializeField]
        private PhotonView photonRPCSender;

        #region Watcher Only

        [SerializeField]
        private GameObject subBoardObject;
        [SerializeField]
        private GUIBoardManager subBoardGraphicManager;
        [SerializeField]
        private Vector3 subCameraPosition;

        private BlockBoard subBoard = new BlockBoard();

        #endregion

        #endregion

        #region Game Logic Class Instance

        private BlockBoard board = new BlockBoard();

        #endregion

        PhotonManager photonManager;
        IGameSyncer gameSyncer;

        // Start is called before the first frame update
        void Start()
        {
            boardGraphicManager.ChangeGraphicAll(board);

            if (GameStartArgment.OnlineGame)
            {
                Debug.Log("Unload Matching Scene...");
                StartCoroutine(UnloadMatchingScene());

                photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
                gameSyncer = photonManager;

                if (gameSyncer.PlayerStatus.IsWatcher())
                {
                    subBoardObject.SetActive(true);
                    mainCamera.transform.position = subCameraPosition;
                }

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

            gameSyncer.State = GameState.GAME_START;

            gameSyncer.OnAllPlayerReady += () =>
            {
                StartCoroutine(CountDown());
            };
            gameSyncer.OnGameStateChanged += OnGameStateChangedCallback;

            // The player is ready
            gameSyncer.Ready();
        }

        // Update is called once per frame
        void Update()
        {
            switch (gameSyncer.State)
            {
                case GameState.GAME_NOW:
                    {
                        if (!gameSyncer.PlayerStatus.IsWatcher())
                        {
                            if (Input.GetKeyDown(KeyCode.W)) InvokeBlockMoved(MoveDirection.UP);
                            else if (Input.GetKeyDown(KeyCode.S)) InvokeBlockMoved(MoveDirection.DOWN);
                            else if (Input.GetKeyDown(KeyCode.A)) InvokeBlockMoved(MoveDirection.LEFT);
                            else if (Input.GetKeyDown(KeyCode.D)) InvokeBlockMoved(MoveDirection.RIGHT);
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

        void DoAction(GameAction action)
        {
            switch(action.ActionType)
            {
                case ActionType.BLOCK_MOVED:
                    {
                        if(gameSyncer.PlayerStatus.IsWatcher())
                        {
                            if(action.IsMaster)
                            {
                                board.Move((MoveDirection)action.Parameter, out _);
                                boardGraphicManager.ChangeGraphicAll(board);
                            }
                            else
                            {
                                subBoard.Move((MoveDirection)action.Parameter, out _);
                                subBoardGraphicManager.ChangeGraphicAll(subBoard);
                            }
                        }
                        else
                        {
                            if(gameSyncer.PlayerStatus.IsMaster() == action.IsMaster)
                            {
                                board.Move((MoveDirection)action.Parameter, out _);
                                boardGraphicManager.ChangeGraphicAll(board);
                            }
                        }
                    }
                    break;
                case ActionType.BLOCK_SPAWN:
                    {
                        if (gameSyncer.PlayerStatus.IsWatcher())
                        {
                            if (action.IsMaster)
                            {
                                int x = action.Parameter / 16;
                                int y = action.Parameter % 16;
                                board.SetValue(x, y, 2);
                                boardGraphicManager.ChangeGraphicAll(board);
                                boardGraphicManager.ShowBornParticleAt(x, y);
                            }
                            else
                            {
                                int x = action.Parameter / 16;
                                int y = action.Parameter % 16;
                                subBoard.SetValue(x, y, 2);
                                subBoardGraphicManager.ChangeGraphicAll(subBoard);
                                subBoardGraphicManager.ShowBornParticleAt(x, y);
                            }
                        }
                        else
                        {
                            if (gameSyncer.PlayerStatus.IsMaster() == action.IsMaster)
                            {
                                int x = action.Parameter / 16;
                                int y = action.Parameter % 16;
                                board.SetValue(x, y, 2);
                                boardGraphicManager.ChangeGraphicAll(board);
                                boardGraphicManager.ShowBornParticleAt(x, y);
                            }
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
            gameSyncer.State = GameState.GAME_NOW;
        }

        #endregion

        #region Support Funcitons

        private void InvokeBlockMoved(MoveDirection direction)
        {
            var clone = board.Clone() as BlockBoard;
            clone.Move(direction, out List<(int, int)> changed);
            if (changed.Count != 0 && !board.Full)
            {
                gameSyncer.InvokeAction(ActionType.BLOCK_MOVED, (int)direction);

                var random_spawn_pos = clone.RandomSpawn();
                var random_spawn_pos_zip = random_spawn_pos.x * 16 + random_spawn_pos.y;
                gameSyncer.InvokeAction(ActionType.BLOCK_SPAWN, random_spawn_pos_zip);
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

        #endregion
    }
}
