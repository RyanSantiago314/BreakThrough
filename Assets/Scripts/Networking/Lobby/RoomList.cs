using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class RoomList : MonoBehaviour
{
    [Header("UI References")]
    public Text RoomNameText;
    public Text RoomPlayersText;
    public Button JoinRoomButton;

    private string roomName;

    #region UNITY
    public void Start()
    {
        JoinRoomButton.onClick.AddListener(() =>
        {
            if(PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            PhotonNetwork.JoinRoom(roomName);
        });
    }
    #endregion

    #region Public Methods
    public void Initialize(string name, byte currentPlayers, byte maxPlayers)
    {
        //Max Players is not necessary, lets face it. we can't have more than 2 players ever.
        roomName = name;
        RoomNameText.text = name;
        RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
    }
    #endregion
}
