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
    public class PhotonManager : MonoBehaviourPunCallbacks, IGameStateManager
    {
        #region GUI Objects

        [SerializeField]
        private GameObject waitGUI;
        [SerializeField]
        private GameObject matchingGUI;

        #endregion

        #region GUI Items

        [SerializeField]
        InputField nicknameField;
        [SerializeField]
        InputField roomField;

        #endregion

        public PlayerStatus PlayerStatus { get; set; } = PlayerStatus.OFFLINE;

        public GameState GameState { get; set; } = GameState.AWAKE;

        //
        // It's used only you are master.
        //
        private Player client;

        internal AsyncOperation gameSceneAsync;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            var result = PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connect " + result);
            gameSceneAsync = GameSceneAsync();
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Photon Callbacks

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
                matchingGUI.SetActive(false);
                waitGUI.SetActive(true);
                GameState = GameState.WAIT_CLIENT;
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
            Debug.LogError($"FAILED JOIN THE ROOM : {returnCode}");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                if(GetIsWatcher(newPlayer))
                {
                    Debug.Log($"NEW WATCHER : {newPlayer.NickName}");
                }
                else if(client == null)
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

        #region GUI Events

        public void OnCreateRoomButtonClicked()
        {
            var option = new RoomOptions();

            PhotonNetwork.CreateRoom(roomField.text, option);
            PhotonNetwork.NickName = nicknameField.text;
            
            GameStartArgment.OnlineGame = true;
        }

        public void OnJoinRoomButtonClicked()
        {
            var option = new RoomOptions();

            PhotonNetwork.JoinRoom(roomField.text);
            PhotonNetwork.NickName = nicknameField.text;
            
            GameStartArgment.OnlineGame = true;
        }

        public void OnRandomJoinRoomButtonClicked()
        {
            PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.NickName = nicknameField.text;
            
            GameStartArgment.OnlineGame = true;
        }

        #endregion

        #region My Callbacks

        public void OnMatchingSuccessful()
        {
            GameState = GameState.GAME_START;
            StartCoroutine(FinishAsyncLoad(gameSceneAsync));
        }

        #endregion

        public AsyncOperation GameSceneAsync()
        {
            var asyncOperation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;
            return asyncOperation;
        }

        public IEnumerator FinishAsyncLoad(AsyncOperation op)
        {
            op.allowSceneActivation = true;
            yield return op;
        }

        private bool GetIsWatcher(string playerName)
        {
            return Regex.IsMatch(playerName, ".*WATCHER");
        }

        private bool GetIsWatcher(Player player) => GetIsWatcher(player.NickName);
    }
}
