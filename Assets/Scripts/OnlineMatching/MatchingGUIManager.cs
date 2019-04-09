using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class MatchingGUIManager : MonoBehaviour
    {
        #region GUI Objects

        [SerializeField]
        private GameObject waitGUI;
        public GameObject WaitGUI { get => waitGUI; }

        [SerializeField]
        private GameObject matchingGUI;
        public GameObject MatchingGUI { get => matchingGUI; }

        [SerializeField]
        InputField nicknameField;
        [SerializeField]
        InputField roomField;

        #endregion
        
        const byte ROOM_MAX_PLAYER = 5;

        public void OnJoinRoomAsWatcherClicked()
        {
            var nickName = nicknameField.text;

            var isNotAllowedName = nickName.Contains("_");
            if(isNotAllowedName)
            {
                // Player inputed a name which is not allowed
                Debug.LogError($"The name, '{nickName}', is not allowed.");
                return;
            }

            nickName += "_";

            var roomName = roomField.text;

            PhotonNetwork.NickName = nickName;

            PhotonNetwork.JoinRoom(roomName);
        }

        public void OnJoinRoomAsPlayerClicked()
        {
            var nickName = nicknameField.text;

            var isNotAllowedName = nickName.Contains("_");
            if(isNotAllowedName)
            {
                // Player inputed a name which is not allowed
                Debug.LogError($"The name, '{nickName}', is not allowed.");
                return;
            }

            PhotonNetwork.NickName = nickName;

            var roomName = roomField.text;

            PhotonNetwork.JoinOrCreateRoom(roomName, null, null);
        }
    }
}
