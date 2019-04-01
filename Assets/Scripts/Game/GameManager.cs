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
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviour Instance (Edit on Inspector)

        [SerializeField]
        private ParticleSpawner bornParticleSpawner;
        [SerializeField]
        private ParticleSpawner readyParticleSpawner;
        [SerializeField]
        private GameObject countDownGUI;
        [SerializeField]
        private Text countDownText;
        [SerializeField]
        private PhotonView photonRPCSender;

        #endregion

        #region MonoBehaviour Instance (Load by this code)

        private Dictionary<(int x, int y), BlockManager> blockManagers;

        #endregion

        #region Game Logic Class Instance

        private BlockBoard board = new BlockBoard();

        #endregion

        #region Other Variables

        PhotonManager photonManager;
        IGameStateManager stateManager;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            blockManagers = new Dictionary<(int x, int y), BlockManager>();
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    var blockManager = GameObject.Find($"Block{x}-{y}").GetComponent<BlockManager>();
                    blockManagers.Add((x, y), blockManager);
                }
            }
            board.RandomSpawn();
            ChangeGraphicAll();

            if (GameStartArgment.OnlineGame)
            {
                Debug.Log("Unload Matching Scene...");
                StartCoroutine(UnloadMatchingScene());

                photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
                stateManager = photonManager;

                if(stateManager.PlayerStatus == PlayerStatus.ONLINE_SERVER)
                {
                    photonRPCSender.RPC("CountDownStart", RpcTarget.AllViaServer);
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
                stateManager = new GameStateManagerOffline();
                stateManager.PlayerStatus = PlayerStatus.OFFLINE;
                stateManager.GameState = GameState.GAME_START;

                StartCoroutine(CountDown());
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (stateManager.GameState)
            {
                case GameState.GAME_NOW:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            board.Move(MoveDirection.UP, out List<(int, int)> changed);
                            if (changed.Count != 0 && !board.Full)
                            {
                                RandomSpawn();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.S))
                        {
                            board.Move(MoveDirection.DOWN, out List<(int, int)> changed);
                            if (changed.Count != 0 && !board.Full)
                            {
                                RandomSpawn();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.A))
                        {
                            board.Move(MoveDirection.LEFT, out List<(int, int)> changed);
                            if (changed.Count != 0 && !board.Full)
                            {
                                RandomSpawn();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.D))
                        {
                            board.Move(MoveDirection.RIGHT, out List<(int, int)> changed);
                            if (changed.Count != 0 && !board.Full)
                            {
                                RandomSpawn();
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
            stateManager.GameState = GameState.GAME_NOW;
        }

        #endregion

        #region Support Funcitons

        void RandomSpawn()
        {
            var pos = board.RandomSpawn();
            bornParticleSpawner.Spawn(CalcPosition(pos.x, pos.y) + new Vector3(0, 0.5f, 0));
            ChangeGraphicAll();
        }

        void ChangeGraphicAll()
        {
            readyParticleSpawner.Free();
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    blockManagers[(x, y)].ChangeMaterial(board[x, y]);
                    if (board[x, y] >= 64)
                    {
                        readyParticleSpawner.Spawn(CalcPosition(x, y) + new Vector3(0, -0.1f, 0));
                    }
                }
            }
        }

        const float BLOCKS_Y = 0.75f;
        const float BLOCK_SPACE = 4f;
        Vector3 CalcPosition(int x, int y)
        {
            return new Vector3((x - 1) * BLOCK_SPACE - 6f, BLOCKS_Y, (y - 1) * BLOCK_SPACE - 6f);
        }

        #endregion

        #region Photon RPC

        [PunRPC]
        private void CountDownStart(PhotonMessageInfo info)
        {
            StartCoroutine(CountDown());
        }

        #endregion
    }
}
