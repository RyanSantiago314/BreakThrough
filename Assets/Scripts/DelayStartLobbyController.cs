using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;


public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private GameObject delayStartButton;
	[SerializeField]
	private GameObject delayCancelButton;
	[SerializeField]
	private int roomSize;

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		delayStartButton.SetActive(true);
	}

	public void loadLobby()
	{
		SceneManager.LoadScene("Lobby");
	}

	public void DelayStart()
	{
		delayStartButton.SetActive(false);
		delayCancelButton.SetActive(true);
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.LogFormat(message + "return Code: " + returnCode);
		CreateRoom();
	}

	void CreateRoom()
	{
		Debug.LogFormat("CreateRoom()");
		int randomRoomNumber = Random.Range(0, 10000);
		RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
		PhotonNetwork.CreateRoom("Room " + randomRoomNumber, roomOps);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.LogFormat(message + " Return Code: " + returnCode);
		CreateRoom();
	}

	public void DelayCancel()
	{
		delayCancelButton.SetActive(false);
		delayStartButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
}
