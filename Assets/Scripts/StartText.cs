using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartText : MonoBehaviour
{
	static public bool startReady;
    public TextMeshProUGUI startText;
    public GameObject thisText;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        startReady = false;
        timer = 5;
        startText.text = "Get Ready!";
        thisText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0) {
            timer -= Time.deltaTime;
        }
        if (timer <= 0) {
            startText.text = "Fight";
            thisText.SetActive(false);
        }
        if (timer <= 3.5f && timer > 2.5f) {
            startText.text = "3";
        }
        else if (timer <= 2.5f && timer > 1.5f) {
            startText.text = "2";
        }
        else if (timer <= 1.5f && timer > 0.5f) {
            startText.text = "1";
        }
        else if (timer <= 0.5f && timer > 0) {
            startText.text = "Fight!";
            startReady = true;
        }
    }
}
