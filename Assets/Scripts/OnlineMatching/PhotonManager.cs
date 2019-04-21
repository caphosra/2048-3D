using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class PhotonManager : MonoBehaviourPunCallbacks, IGameSyncer
    {
        [SerializeField]
        private MatchingGUIManager guiManager;

        public PlayerStatus PlayerStatus { get; set; } = PlayerStatus.OFFLINE;

        private GameState m_State = GameState.AWAKE;
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

        //
        // It's used only you are master.
        //
        private Player client;

        // Game scene (Async Load)
        internal AsyncOperation gameSceneAsync;

        void Awake()
        {
            GameStartArgment.OnlineGame = true;
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Initalize Photon
            var result = PhotonNetwork.ConnectUsingSettings();

            Debug.Log("Connect " + result);

            gameSceneAsync = GameSceneAsync();
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Before Game Start

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined the room");
            
            if(GetIsWatcher(PhotonNetwork.NickName))
            {
                PlayerStatus = PlayerStatus.ONLINE_WATCHER;
            }
            else if(PhotonNetwork.IsMasterClient)
            {
                PlayerStatus = PlayerStatus.ONLINE_SERVER;
                guiManager.MatchingGUI.SetActive(false);
                guiManager.WaitGUI.SetActive(true);
                State = GameState.WAIT_CLIENT;
            }
            else
            {
                PlayerStatus = PlayerStatus.ONLINE_CLIENT;
                OnMatchingSuccessful();
            }

            Debug.Log($"You are {System.Enum.GetName(typeof(PlayerStatus), PlayerStatus)}");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"FAILED JOIN THE ROOM : {message}");
        }

        private void OnMatchingSuccessful()
        {
            StartCoroutine(FinishAsyncLoad(gameSceneAsync));
        }

        #endregion

        #region On Player Join or Left

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (GetIsWatcher(newPlayer.NickName))
                {
                    Debug.Log($"NEW WATCHER : {newPlayer.NickName}");
                }
                else if (client == null)
                {
                    Debug.Log($"MATHCING SUCCESS ! : {newPlayer.NickName}");
                    client = newPlayer;

                    OnMatchingSuccessful();
                }
                else
                {
                    Debug.Log($"{newPlayer.NickName} wanted to become a client but be rejected.");

                    // If fighters are over, master will kick the client.
                    PhotonNetwork.CloseConnection(newPlayer);
                }
            }
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("OnlineMatching");
            Destroy(gameObject);
        }

        #endregion

        #region Load Game Scene

        private AsyncOperation GameSceneAsync()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;
            return asyncOperation;
        }

        private IEnumerator FinishAsyncLoad(AsyncOperation op)
        {
            op.allowSceneActivation = true;
            yield return op;
        }

        #endregion
    
        #region On Ready

        private ReadyStatus readyStatus = ReadyStatus.NONE;
        public event GameSyncerHandler OnAllPlayerReady;

        [PunRPC]
        private void OnReceiveAllPlayerReady()
        {
            OnAllPlayerReady?.Invoke();
        }

        [PunRPC]
        private void OnReceiveClientReady()
        {
            readyStatus |= ReadyStatus.CLIENT_READY;
            Debug.Log("Client Ready");
            CheckAllPlayerReady();
        }

        private void CheckAllPlayerReady()
        {
            if (readyStatus == (ReadyStatus.MASTER_READY | ReadyStatus.CLIENT_READY))
            {
                Debug.Log("Master & Client Ready !");
                photonView.RPC("OnReceiveAllPlayerReady", RpcTarget.AllViaServer);
            }
        }

        public void Ready()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                readyStatus |= ReadyStatus.MASTER_READY;
                Debug.Log("Master Ready");
                CheckAllPlayerReady();
            }
            else
            {
                photonView.RPC("OnReceiveClientReady", RpcTarget.MasterClient);
            }
        }

        [System.Flags]
        private enum ReadyStatus
        {
            NONE = 0b00,
            MASTER_READY = 0b01,
            CLIENT_READY = 0b10
        }

        #endregion

        #region Sync Blocks

        public Queue<GameAction> DoneActions { get; set; } = new Queue<GameAction>();

        [PunRPC]
        private void OnReceivedDoneAction(bool isMaster, byte actionType, int param)
        {
            DoneActions.Enqueue(new GameAction(isMaster, (ActionType)actionType, param));
        }

        public void InvokeAction(ActionType actionType, int param)
        {
            bool isMaster = PlayerStatus == PlayerStatus.ONLINE_SERVER;
            photonView.RPC("OnReceivedDoneAction", RpcTarget.AllBufferedViaServer, isMaster, (byte)actionType, param);
        }

        #endregion

        #region Game Finished

        public event GameFinishHandler OnGameFinished;

        [PunRPC]
        private void OnReceivedGameFinished(int winner)
        {
            OnGameFinished?.Invoke((Winner)winner);
        }

        public void EndGame(Winner winner)
        {
            if(PlayerStatus.IsMaster())
            {
                photonView.RPC("OnReceivedGameFinished", RpcTarget.AllBufferedViaServer, (int)winner);
            }
        }

        #endregion

        public bool GetIsWatcher(string name)
        {
            return name.Contains("_");
        }
    }
}
