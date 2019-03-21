using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class Matching : MonoBehaviourPunCallbacks
    {
        AsyncOperation gameSceneAsync;

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

        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom("Hello");
            PhotonNetwork.NickName = "CAPRA";
            
            GameStartArgment.OnlineGame = true;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined the room");
            StartCoroutine(FinishAsyncLoad(gameSceneAsync));
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"FAILED JOIN THE ROOM : {returnCode}");
        }

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
    }
}
