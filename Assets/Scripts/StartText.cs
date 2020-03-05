using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartText : MonoBehaviour
{  
    //Global bool controlling whether or not user input is allowed
	static public bool startReady;
    //Global int that keeps track of the round count
    static public int roundCount;
    //Text and music objects
    public TextMeshProUGUI startText;
    public GameObject thisText;
    private AudioSource music;
    public AudioSource BoBB;
    public AudioSource begin;
    public AudioSource ready;
    public AudioSource ready2;
    public AudioSource ready3;
    //bool that decides whether or not if round countdown is ready to begin
    private bool beginCountdown;
    //bool keeping track of when it is the first round of a match
    private bool isFirstRound;
    //Timer variable
    float timer;

    void Awake()
    {
        roundCount = 0; 
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
        {
            //If it is the first round set first round bool to true
            if (roundCount < 1)
            {
                isFirstRound = true;
            }
            else
            {
                isFirstRound = false;
                roundCount++;
            }
            //If it is the first round set the timer for countdown to 5 seconds and round count to 0 total rounds
            if (isFirstRound)
            {
                timer = 5;
                roundCount = 1;
                music = GetComponent<AudioSource>();
            }
            //If not the first round set the round start countdown to 3 seconds
            else
            {
                timer = 3;
            }
            //Set text to ready and activate it
            if (isFirstRound) startText.text = "Break or Be Broken";
            else if (roundCount == 2) startText.text = "Duel 2";
            else if (roundCount >= 3) startText.text = "Final Duel";

            if (!GameOver.matchOver && !PauseMenu.pauseQuit) thisText.SetActive(true);
            //Countdown is now ready to begin
            beginCountdown = true;
        }
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            music = GetComponent<AudioSource>();
            startText.text = "";
            startReady = true;
            music.Play();
        }
        if (isFirstRound) BoBB.Play();
        else if (roundCount == 2) startText.text = "Duel 2";
        else if (roundCount >= 3) startText.text = "Final Duel";
        //BoBB.time = 0.2f;
        //BoBB.Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(BoBB.time);
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
        {
            //Adjusts timer every update frame
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            //If round ends restart script
            if (startReady == false && timer == -2 && beginCountdown == false)
            {
                Start();
            }
            if (timer <= 0)
            {
                thisText.SetActive(false);
            }
            //If timer is finished set it to -2 for standby
            if (timer < 0)
            {
                timer = -2;
            }
            //Checking to see if countdown is ready
            if (beginCountdown)
            {
                //If its the first round do long countdown
                if (isFirstRound)
                {
                    //Exact timing parameters for each text pop up and corresponding sound
                    if (timer > 2f && timer < 3f)
                    {
                        if (roundCount == 1) ready.Play();
                        else if (roundCount == 2) ready2.Play();
                        else if (roundCount >= 3) ready3.Play();
                        
                    }
                    else if (timer > 4f && timer <= 6f)
                    {
                        //BoBB.Play();
                    }
                    else if (timer > 0f && timer < 1f)
                    {
                        begin.Play();
                    }
                    if (timer <= 3f && timer > 0.5f)
                    {
                        if (roundCount == 1) startText.text = "Duel 1";
                        else if (roundCount == 2) startText.text = "Duel 2";
                        else if (roundCount >= 3) startText.text = "Final Duel";

                        if (roundCount == 1) music.Play();
                    }
                    else if (timer <= 5f && timer > 3f)
                    {
                        startText.text = "Break or Be Broken";
                        
                    }
                    else if (timer <= 0.5f && timer > 0)
                    {
                        startText.text = "Begin";
                        startReady = true;
                        beginCountdown = false;
                        isFirstRound = false;
                    }
                }
                //If its not first round do fast countdown
                else
                {
                    if (timer > 2f && timer < 3f)
                    {
                        if (roundCount == 2) ready2.Play();
                        else if (roundCount >= 3) ready3.Play();
                    }
                    else if (timer > 0.5f && timer < 0.6f)
                    {
                        begin.Play();
                    }
                    else if (timer <= 0.5f && timer > 0)
                    {
                        startText.text = "Begin";
                        startReady = true;
                        beginCountdown = false;
                    }
                }
            }
        }       
    }
}
