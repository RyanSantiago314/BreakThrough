using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable; //For Custom Properties

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Public Objects
    [Header("Login Panel")]
    public GameObject LoginPanel;

    public InputField InputName;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;
    public InputField RoomNameInput;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;
    public GameObject RoomListContent;
    public GameObject RoomEntryPrefab;

    [Header("In Room Panel")]
    public GameObject InsideRoomPanel;
    public Button ReadyButton;
    public GameObject ListEntryPrefab;
    #endregion

    #region Private Objects
    private GameObject tempData;
    private GameObject p2Data;
    private Dictionary<string, RoomInfo> cacheRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerEntries;
    #endregion

    #region Unity Calls
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //defaults
        cacheRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        InputName.text = "Player " + Random.Range(1000,10000);
    }
    #endregion

    #region PUN Calls
    //Set Active Menu Regions
    //OnConnectedToMaster
    public override void OnConnectedToMaster()
    {
        this.setActivePanel(SelectionPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomView();

        UpdateCacheList(roomList);
        UpdateRoomListView();
    }

    public override void OnLeftLobby()
    {
        cacheRoomList.Clear();

        ClearRoomView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //In case of failure default back to Selection Panel
        Debug.LogFormat("On Created Room Failed: " + message);
        setActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Default to Selection Panel again.
        Debug.LogFormat("On Join Room Failed: " + message);
        setActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //If we can't join a random room we create a Room.
        Debug.LogFormat("On Join Random Room Failed: " + message);

        string roomId = "Room " + Random.Range(1000, 10000);
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        Debug.LogFormat("Creating New Room");
        PhotonNetwork.CreateRoom(roomId, options, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat("Room Joined SUCCESS");
        //Switch UI to InsideRoom
        setActivePanel(InsideRoomPanel.name);

        //Check if Player List is Empty, then update
        if( playerEntries == null)
        {
            playerEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            //Update Players in UI
            //The entry object is the Player UI part
            GameObject entry = Instantiate(ListEntryPrefab);
            entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<ListEntry>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if(p.CustomProperties.TryGetValue(BREAKTHROUGH.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<ListEntry>().setPlayerReady((bool)isPlayerReady);
            }

            playerEntries.Add(p.ActorNumber, entry);
        }

        ReadyButton.gameObject.SetActive(CheckIfReady());
        GameObject pData = GameObject.Find("PlayerData");

        Hashtable property = new Hashtable
        {
            
            {BREAKTHROUGH.PLAYER_LOADED_LEVEL, false}, 
            {"Character", pData.GetComponent<SelectedCharacterManager>().P1Character},
            {"Color", pData.GetComponent<SelectedCharacterManager>().P1Color}
            
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(property);
        if(PhotonNetwork.IsMasterClient)
        {
            //else we are master client. 
            //Grabbing Second players CharManager Data that was set right above.
            p2Data = GameObject.Find("PlayerData");
            foreach(var player in PhotonNetwork.PlayerListOthers)
            {
                object p2char;
                if(player.CustomProperties.TryGetValue("Character", out p2char))
                {
                    p2Data.GetComponent<SelectedCharacterManager>().P2Character = (string)p2char;
                }
                object p2color;
                if(player.CustomProperties.TryGetValue("Color", out p2color))
                {
                    p2Data.GetComponent<SelectedCharacterManager>().P2Color = (int)p2color;
                }

            }

            
            p2Data.GetComponent<SelectedCharacterManager>().P2Side = "Right";
        }
        p2Data.GetComponent<SelectedCharacterManager>().gameMode = "PvP";

        if(PhotonNetwork.CountOfPlayers == 1)
        {
            var PlayerData = GameObject.Find("PlayerData");
            PlayerData.GetComponent<SelectedCharacterManager>().P2Side = "Right";
            PlayerData.GetComponent<SelectedCharacterManager>().P2Character = "Dhalia";
            PlayerData.GetComponent<SelectedCharacterManager>().P2Color = 2;
        }
        
    }

    public override void OnLeftRoom()
    {
        //If Leave Room, Go back to Selection
        setActivePanel(SelectionPanel.name);
        //Destroy all objects in list
        foreach (GameObject entry in playerEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        //Now Clear
        playerEntries.Clear();
        playerEntries = null;

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(ListEntryPrefab);
        entry.transform.SetParent(InsideRoomPanel.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<ListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerEntries.Add(newPlayer.ActorNumber, entry);
        ReadyButton.gameObject.SetActive(CheckIfReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerEntries[otherPlayer.ActorNumber].gameObject);
        playerEntries.Remove(otherPlayer.ActorNumber);

        ReadyButton.gameObject.SetActive(CheckIfReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            ReadyButton.gameObject.SetActive(CheckIfReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(playerEntries == null) //Refresh
        {
            playerEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if(playerEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady; 
            if(changedProps.TryGetValue(BREAKTHROUGH.PLAYER_READY, out isPlayerReady))//TRY GET VALUE PLAYER READY
            {
                entry.GetComponent<ListEntry>().setPlayerReady((bool) isPlayerReady);
            }
        }

        ReadyButton.gameObject.SetActive(CheckIfReady());
        
    }
    #endregion

    #region UI
    public void OnRoomListCancelButtonClicked()
    {
        if(PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        setActivePanel(SelectionPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = RoomNameInput.text;
        //Default if no chosen name
        roomName = (roomName.Equals(string.Empty)) ? "ROOM " + Random.Range(1000, 10000) : roomName;

        byte maxPlayers = 2;
        RoomOptions options = new RoomOptions {MaxPlayers = maxPlayers};

        PhotonNetwork.CreateRoom(roomName, options, null);
        
    }

    public void OnJoinRandomButtonClicked()
    {
        setActivePanel(JoinRandomRoomPanel.name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLoginButtonClicked()
    {
        string playerName = InputName.text;
        if(!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("PLAYER NAME IS INVALID");
        }
    }

    public void OnShowRoomButtonCLicked()
    {
        if( !PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        setActivePanel(RoomListPanel.name);
    }

    public void OnReadyButtonClicked()
    {
        // TODO: Set this up so it sends player to training room if only 1 player in Room.
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        // TODO: set the loadable level.
        PhotonNetwork.LoadLevel(3); //Training Room
    }
    #endregion

    #region Private Methods

    private bool CheckIfReady()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if(p.CustomProperties.TryGetValue(BREAKTHROUGH.PLAYER_READY, out isPlayerReady))
            {
                if(!(bool)isPlayerReady)
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

            
        }
        return true;
    }

    private void ClearRoomView()
    {
        foreach(GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }


    public void setActivePanel(string ActivePanel)
    {
        //basically goes down the list comparing the input name to what it can enable/disable
        LoginPanel.SetActive(ActivePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(ActivePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(ActivePanel.Equals(CreateRoomPanel.name));
        JoinRandomRoomPanel.SetActive(ActivePanel.Equals(JoinRandomRoomPanel.name));
        RoomListPanel.SetActive(ActivePanel.Equals(RoomListPanel.name));
        InsideRoomPanel.SetActive(ActivePanel.Equals(InsideRoomPanel.name));
    }

    private void UpdateCacheList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //Remove or hide rooms
            if(!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if(cacheRoomList.ContainsKey(info.Name))
                {
                    cacheRoomList.Remove(info.Name);
                }

                continue;
            }

            //Update the cache room list
            if (cacheRoomList.ContainsKey(info.Name))
            {
                cacheRoomList[info.Name] = info;
            }
            //Add new info
            else
            {
                cacheRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cacheRoomList.Values)
        {
            GameObject entry = Instantiate(RoomEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomList>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }
    
    #endregion

    #region Public Methods
    public void LocalPlayerPropertiesUpdated()
    {
        ReadyButton.gameObject.SetActive(CheckIfReady());
    }
    #endregion

}
