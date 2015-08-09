using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class Lobby : MonoBehaviour
    {
        public InputField RoomName;
        public Button[] Buttons;
  

        private bool _connected = false;
        private List<RoomInfo> _rooms = new List<RoomInfo>();

        public void Refresh()
        {
            if (!_connected) return;
            RoomInfo[] serverRooms = PhotonNetwork.GetRoomList();
            if (serverRooms == null) return;
            foreach (RoomInfo serverRoom in serverRooms)
            {
                if (!_rooms.Any(roomInfo => roomInfo.name.Equals(serverRoom.name)))
                {
                    _rooms.Add(serverRoom);
                    var room = serverRoom;
                    foreach (Button button in Buttons)
                    {
                        if (!button.IsActive())
                        {
                            button.transform.GetChild(0).GetComponent<Text>().text = room.name;
                            button.onClick.AddListener(() => JoinRoom(room.name));
                            button.gameObject.SetActive(true);
                            break;
                        }
                    }
                }

            }
        }

        public void CreateRoom()
        {
            if (!_connected) return;
            if (!RoomName.text.Equals("")) PhotonNetwork.CreateRoom(RoomName.text, new RoomOptions() {maxPlayers = 2}, TypedLobby.Default);

        }

        public void JoinRoom(string nameOfRoom)
        {
            PhotonNetwork.JoinRoom(nameOfRoom);
        }

        void Start()
        {
            Connect();
        }

        void Connect()
        {
            PhotonNetwork.ConnectUsingSettings("v1.0.0");
            Refresh();
        }

        void OnGUI()
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }

        void OnJoinedLobby()
        {
            _connected = true;
            Refresh();
        }

        void ExitLobby()
        {
            if(_connected)PhotonNetwork.Disconnect();
            Application.LoadLevel("MainMenu");
        }

        void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("GameMenu");
        }

        void OnDisconnectedFromPhoton()
        {
            _connected = false;
        }

        void OnPhotonJoinRoomFailed()
        {
            PhotonNetwork.Disconnect();
            Application.LoadLevel("MainMenu");
        }
    }
}