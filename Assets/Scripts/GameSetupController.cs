using Photon.Pun;
using System.IO;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable; //For Custom Properties

public class GameSetupController : MonoBehaviour
{
	public PhotonView p1;
	public PhotonView p2;
	public GameObject P1Character;
    public GameObject P2Character;
    public GameObject HitMarker;
    private string P1Char;
    private string P2Char;

    void Start()
    {
		Hashtable properties = new Hashtable
		{
			{BREAKTHROUGH.PLAYER_LOADED_LEVEL, true}
		};
		PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    	CreatePlayer();
    }
    private void CreatePlayer()
    {
    	//Debug.Log(PhotonNetwork.PlayerList[0]);
		if (PhotonNetwork.IsMasterClient)
		{
			P1Character = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "OnlineDhalia"), new Vector3(1.3f, 1.15f, -3f), Quaternion.identity);

			GameObject.Find("Player1").GetComponent<FighterAgent>().myChar = P1Character.GetComponent<CharacterProperties>();
        	P1Character.GetComponent<MovementHandler>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        	P1Character.GetComponent<AttackHandlerDHA>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        	P1Character.transform.GetChild(2).GetComponent<HitDetector>().hitTrack = HitMarker.transform;
		}
		else
		{
			P2Character = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "OnlineDhalia"), new Vector3(-1.3f, 1.15f, -3f), Quaternion.identity);

			GameObject.Find("Player2").GetComponent<FighterAgent>().myChar = P2Character.GetComponent<CharacterProperties>();
        	P2Character.GetComponent<MovementHandler>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        	P2Character.GetComponent<AttackHandlerDHA>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        	P2Character.transform.GetChild(2).GetComponent<HitDetector>().hitTrack = HitMarker.transform;
		}
    }
}
