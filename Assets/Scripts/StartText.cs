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
    public AudioSource one;
    public AudioSource two;
    public AudioSource three;
    public AudioSource go;
    public AudioSource ready;
    //bool that decides whether or not if round countdown is ready to begin
    private bool beginCountdown;
    //bool keeping track of when it is the first round of a match
    private bool isFirstRound;
    //Timer variable
    float timer;

    // Start is called before the first frame update
    void Start()
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
        startText.text = "Ready!";
        thisText.SetActive(true);
        //Countdown is now ready to begin
        beginCountdown = true;
    }

    // Update is called once per frame
    void Update()
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
                if (timer > 4.5 && timer < 4.6)
                {
                	ready.Play();
                }
                if (timer > 3.5 && timer < 3.6)
                {
                	three.Play();
                }
                else if (timer > 2.5 && timer < 2.6)
                {
                	two.Play();
                }
                else if (timer > 1.5 && timer < 1.6)
                {
                	one.Play();
                }
                else if (timer > 0.5 && timer < 0.6)
                {
                	go.Play();
                }
                if (timer <= 3.5f && timer > 2.5f)
                {
                    startText.text = "3";
                }
                else if (timer <= 2.5f && timer > 1.5f)
                {
                    startText.text = "2";
                    music.Play();
                }
                else if (timer <= 1.5f && timer > 0.5f)
                {
                    startText.text = "1";
                }
                else if (timer <= 0.5f && timer > 0)
                {
                    startText.text = "Go!";
                    startReady = true;
                    beginCountdown = false;
                    isFirstRound = false;
                }
            }
            //If its not first round do fast countdown
            else
            {
                if (timer > 2.5 && timer < 2.6)
                {
                    ready.Play();
                }
                else if (timer > 0.5 && timer < 0.6)
                {
                    go.Play();
                }
                else if (timer <= 0.5f && timer > 0)
                {
                    startText.text = "Go!";
                    startReady = true;
                    beginCountdown = false;
                }
            }
        }
    }
}
