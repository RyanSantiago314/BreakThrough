using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartText : MonoBehaviour
{
	static public bool startReady;
    public TextMeshProUGUI startText;
    public GameObject thisText;
    private AudioSource music;
    public AudioSource one;
    public AudioSource two;
    public AudioSource three;
    public AudioSource go;
    public AudioSource ready;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 5;
        startText.text = "Ready!";
        thisText.SetActive(true);
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0) {
            timer -= Time.deltaTime;
        }
        if (timer <= 0) {
            thisText.SetActive(false);
        }
        if (timer > 4.5 && timer < 4.6) {
        	ready.Play();
        }
        if (timer > 3.5 && timer < 3.6) {
        	three.Play();
        }
        else if (timer > 2.5 && timer < 2.6) {
        	two.Play();
        }
        else if (timer > 1.5 && timer < 1.6) {
        	one.Play();
        }
        else if (timer > 0.5 && timer < 0.6) {
        	go.Play();
        }
        if (timer <= 3.5f && timer > 2.5f) {
            startText.text = "3";
        }
        else if (timer <= 2.5f && timer > 1.5f) {
            startText.text = "2";
            music.Play();
        }
        else if (timer <= 1.5f && timer > 0.5f) {
            startText.text = "1";
        }
        else if (timer <= 0.5f && timer > 0) {
            startText.text = "Go!";
            startReady = true;
        }
    }
}
