using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviourPunCallbacks
{
	private PhotonView myPhotonView;
	//Variables for character properties for both player 1 and 2
    private CharacterProperties PlayerProp1;
    private CharacterProperties PlayerProp2;
    //Menu object variables
    public GameObject p1menu;
    public GameObject p2menu;
    public GameObject roundOverMenu;
    public GameObject KOMenu;
    public GameObject overtimeMenu;
    private GameObject child1;
    private GameObject child2;
    private GameObject child3;
    private GameObject child4;
    private GameObject child5;
    public Button p1Replay;
    public Button p1Quit;
    public Button p2Replay;
    public Button p2Quit;
    public TextMeshProUGUI p1WinCount;
    public TextMeshProUGUI p2WinCount;
    public TextMeshProUGUI roundTimerText;
    //Global variables keeping track of each players win count
    static public int p1Win;
    static public int p2Win;

    static public bool dizzyKO;
    static public bool matchOver;
    static public bool lockInputs;

    //Various float timer variables
    private float endTimer;
    private float replayTimer;
    private float roundTimer;
    private float overtimeTimer;
    //Bool variable checking if replay is about to occur
    private bool replaying;
    //Bool variable deciding if timer should be running or not
    private bool timerStart;

    private bool isXbox;
    private string xboxInput;
    private string ps4Input;

	void Start()
	{	
		myPhotonView = GetComponent<PhotonView>();

		//Setting private character property variables to their appropriate player 1 and 2 child respectively
		PlayerProp1 = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
		PlayerProp2 = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();

		//Setting private menu child game obejcts to their appropriate menu children respectively
		child1 = p1menu.transform.GetChild(0).gameObject;
		child2 = p2menu.transform.GetChild(0).gameObject;
		child3 = roundOverMenu.transform.GetChild(0).gameObject;
		child4 = KOMenu.transform.GetChild(0).gameObject;
		child5 = overtimeMenu.transform.GetChild(0).gameObject;
		//Setting timers to an arbitrarily picked -2 for standby
		endTimer = -2;
		replayTimer = -2;
		overtimeTimer = -2;
		//Setting round timer to 99 (eventually will make it into a public variable for easier manipulation/access)
		roundTimer = 99;
		//Setting round timer text to be represented as a float with zero decimal places
		roundTimerText.text = roundTimer.ToString("F0");
		replaying = false;
		//If someone has won the game, reset wins for both players to 0 and reset armor to max
		if (p1Win == 2 || p2Win == 2)
		{
    		p1Win = 0;
    		p2Win = 0;
    		PlayerProp1.armor = 4;
    		PlayerProp2.armor = 4;
    		PlayerProp1.durability = 100;
    		PlayerProp2.durability = 100;
    	}
    	//Setting text variables to proper win text for each player
    	p1WinCount.text = p1Win.ToString();
    	p2WinCount.text = p2Win.ToString();
    	//Telling timer not to start yet
    	timerStart = false;
    	//Setting menu children to inactive
    	child1.SetActive(false);
    	child2.SetActive(false);
    	child3.SetActive(false);
    	child4.SetActive(false);
    	child5.SetActive(false);

    	//Setting color of the panel to transparent
 		GameObject.Find("Canvas/BlackScreen").GetComponent<Image>().color = new Color(0,0,0,0);

 		dizzyKO = false;
 		matchOver = false;
 		isXbox = false;

 		xboxInput = "Controller (Xbox One For Windows)";
     	ps4Input = "Wireless Controller";
	}

	void Update()
	{	
		//Decrementing end timer
		if (endTimer > 0)
		{
			timerStart = false;
			endTimer -= Time.deltaTime;
		}
		//Decrementing replay timer
		if (replayTimer > 0) replayTimer -= Time.deltaTime;
		if (overtimeTimer > 0) overtimeTimer -= Time.deltaTime;
		//If inputs are being allowed the game has started and so should the timer (this is a global variable)
		if (StartText.startReady) timerStart = true;
		//If round time is still greater than 0 and timer is allowed to be on, time ticks
		if (roundTimer > 0 && timerStart) roundTimer -= Time.deltaTime / 1.5f;
		//Setting round timer text to be represented as a float with zero decimal places 
		roundTimerText.text = roundTimer.ToString("F0");

		//If an input device is detected then establish what device it is in order to properly decipher inputs
    	if (Input.GetJoystickNames().Length > 0)
    	{
	    	if (Input.GetJoystickNames()[0] == xboxInput) isXbox = true;
	    	else if (Input.GetJoystickNames()[0] == ps4Input) isXbox = false;
	    	else if (Input.GetJoystickNames()[0] == "") isXbox = false;
    	}

		//If player 1 lost and player 2 has 2 wins, display player 2 wins screen
		if (PlayerProp1.currentHealth <= 0 && p2Win == 2)
		{
			child4.SetActive(true);
			//If end timer is on standby, set it at 3 and it will begin
			if (endTimer == -2) endTimer = 3;
			//If end timer is finished and its not on standby, display player 2 win screen
			if (endTimer <=  0 && endTimer > -2)
			{
				child4.SetActive(false);
				child2.SetActive(true);
				if (matchOver == false) p2Replay.Select();
				matchOver = true;
			}
			if (isXbox)
			{
				if (Input.GetAxis("Horizontal_P1") < 0) p2Quit.Select();
            	else if (Input.GetAxis("Horizontal_P1") > 0) p2Replay.Select();
			}
			else
			{
				if (Input.GetAxis("Vertical_P1") < 0) p2Quit.Select();
            	else if (Input.GetAxis("Vertical_P1") > 0) p2Replay.Select();
        	}

			//Global round count set to 0
			StartText.roundCount = 0;

        }
        //If player 2 lost and player 1 has 2 wins, display player 1 wins screen
        else if (PlayerProp2.currentHealth <= 0 && p1Win == 2)
        {
        	child4.SetActive(true);
        	//If end timer is on standby, set it at 3 and it will begin
        	if (endTimer == -2) endTimer = 3;
			//If end timer is finished and its not on standby, display player 1 win screen
            if (endTimer <=  0 && endTimer > -2)
            {
				child4.SetActive(false);
            	child1.SetActive(true);
            	if (matchOver == false) p1Replay.Select();
            	matchOver = true;
            }

            if (isXbox)
			{
				if (Input.GetAxis("Horizontal_P1") < 0) p1Quit.Select();
            	else if (Input.GetAxis("Horizontal_P1") > 0) p1Replay.Select();
			}
			else
			{
				if (Input.GetAxis("Vertical_P1") < 0) p1Quit.Select();
            	else if (Input.GetAxis("Vertical_P1") > 0) p1Replay.Select();
        	}

			//Global round count set to 0
			StartText.roundCount = 0;
        }
        //If the round timer runs out decide who wins
        if (roundTimer <= 0)
        {
        	if (overtimeTimer == -2) overtimeTimer = 1.5f;
        	if ((overtimeTimer > 0 || overtimeTimer == -2) && !replaying
        		&& ((int)((PlayerProp1.currentHealth / PlayerProp1.maxHealth) * 100) == 
        		(int)((PlayerProp2.currentHealth / PlayerProp2.maxHealth) * 100))) child5.SetActive(true);
        	else child5.SetActive(false);
        	
       		//Setting roundTimer to round 0
        	roundTimer = 0;
        	//Stopping timer
			timerStart = false;
			//Set dizzy KO to true
			dizzyKO = true;
			//If player 1 has more health, player 2 loses
			if (PlayerProp1.currentHealth > PlayerProp2.currentHealth) PlayerProp2.currentHealth = 0;
	        //If player 2 has more health, player 1 loses
	        else if (PlayerProp2.currentHealth > PlayerProp1.currentHealth) PlayerProp1.currentHealth = 0;
		}
		if (PlayerProp1.currentHealth <= 0 && PlayerProp2.currentHealth <= 0 && replaying == false && p1Win != 2 && p2Win != 2)
        {
        	if (p1Win == 1 && p2Win < 1)
        	{
        		++p1Win;
				replayTimer = 6;
				replaying = true;
				p1WinCount.text = p1Win.ToString();
				lockInputs = true;
        	}
        	else if (p2Win == 1 && p1Win < 1)
        	{
        		++p2Win;
				replayTimer = 6;
				replaying = true;
				p2WinCount.text = p2Win.ToString();
				lockInputs = true;
        	}
        	else if (p1Win == 0 && p2Win == 0)
        	{
        		++p1Win;
        		++p2Win;
				replayTimer = 6;
				replaying = true;
				p1WinCount.text = p2Win.ToString();
				p2WinCount.text = p2Win.ToString();
				lockInputs = true;
				child3.SetActive(true);
        	}
        	else if (p1Win == 1 && p2Win == 1)
        	{
        		PlayerProp1.currentHealth = PlayerProp1.maxHealth;
        		PlayerProp2.currentHealth = PlayerProp2.maxHealth;
        		roundTimer = 0;
        	}
        }
		//If player 1 loses then player 2 gets a win and reset round after 6 seconds
        else if (PlayerProp1.currentHealth <= 0 && replaying == false && p2Win != 2)
        {
			++p2Win;
			replayTimer = 6;
			replaying = true;
			p2WinCount.text = p2Win.ToString();
			lockInputs = true;
			if (p2Win != 2) child3.SetActive(true);
		}
		//If player 2 loses then player 1 gets a win and reset round after 6 seconds
		else if (PlayerProp2.currentHealth <= 0 && replaying == false && p1Win != 2)
		{
			++p1Win;
			replayTimer = 6;
			replaying = true;
			p1WinCount.text = p1Win.ToString();
			lockInputs = true;
			if (p1Win != 2) child3.SetActive(true);
		}
		//Sets screen black when round ends and new one starts
		if (replayTimer > 0 && replayTimer < 1 && p1Win != 2 && p2Win != 2) GoBlack();
		//When the 6 second replay timer is up restart the round
		if (replayTimer <= 0 && replayTimer > -2 && p1Win != 2 && p2Win != 2) ReplayGame();

		if (PhotonNetwork.IsMasterClient)
        {
            myPhotonView.RPC("RPC_SendRoundTimer", RpcTarget.Others, roundTimer);
        }
	}

	//Function that restarts a round
    void ReplayGame()
    {	
    	//Setting player health to max
    	PlayerProp1.currentHealth = PlayerProp1.maxHealth;
    	PlayerProp2.currentHealth = PlayerProp2.maxHealth;
    	//Setting players to starting location vectors
    	Vector3 p1Start = new Vector3(-1.3f, 1.10f, -3);
    	Vector3 p2Start = new Vector3(1.3f, 1.10f, -3);
    	GameObject.Find("Player1").transform.GetChild(0).transform.position = p1Start;
    	GameObject.Find("Player2").transform.GetChild(0).transform.position = p2Start;
        GameObject.Find("CameraPos").transform.GetChild(1).transform.position = GameObject.Find("CameraPos").transform.position;
        //Disabling player inputs
        lockInputs = false;
    	StartText.startReady = false;
    	//Setting replaying to false for some reason I cant remember
    	replaying = false;
    	matchOver = false;
    	//Running start again
    	Start();
    }

    //Function to load main menu scene
    void QuitToMenu()
    {
    	lockInputs = false;
    	StartText.startReady = false;
    	SceneManager.LoadSceneAsync(0);
    }

    //Function setting color of the panel to black
    void GoBlack()
 	{
 		child3.SetActive(false);
 		GameObject.Find("Canvas/BlackScreen").GetComponent<Image>().color = new Color(0,0,0,255);
 	}

 	[PunRPC]
    private void RPC_SendRoundTimer(float timerIn)
    {
    	Debug.Log("RPC_SendRoundTimer");
        roundTimer = timerIn;
    }
}