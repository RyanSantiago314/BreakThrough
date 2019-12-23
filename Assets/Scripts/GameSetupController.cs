using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
	public PhotonView p1;
	public PhotonView p2;
    void Start()
    {
    	//CreatePlayer();
    }
    private void CreatePlayer()
    {
    	Debug.Log("Creating Player");
  //   	if(PhotonNetwork.IsMasterClient)
		// {
		// 	Debug.Log("Creating Player1");
		// 	PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player2"), new Vector3(1.3f, 1.15f, -3f), Quaternion.identity);
		// }
		// else
		// {
		// 	Debug.Log("Creating Player2");
		// 	PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player2"), new Vector3(-1.3f, 1.15f, -3f), Quaternion.identity);
		// }
		p1.TransferOwnership(PhotonNetwork.PlayerList[0]);
		p2.TransferOwnership(PhotonNetwork.PlayerList[1]);
    }
}
