using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialHit : MonoBehaviour
{
    public Text status;
    public AudioSource announcer;
    public AudioClip counter;
    public AudioClip pierce;
    public AudioClip shatter;

    void Counter()
    {
        status.text = "Counter";
        announcer.PlayOneShot(counter, .75f);
    }

    void Pierce()
    {
        status.text = "Pierce";
        announcer.PlayOneShot(pierce, .75f);
    }

    void Shatter()
    {
        status.text = "SHATTER";
        announcer.PlayOneShot(shatter, .8f);
    }
}
