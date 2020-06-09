using UnityEngine;
using System.Collections;

public class MusicLooper : MonoBehaviour
{
    //Loop Length/Point is input as seconds
    public float loopLength;
    public float loopPoint;
    public AudioSource audioSource;
    public AudioClip audioClip;

    //For Testing Looping
    public float startPoint;

    private void Start()
    {
        //Debug Tool
        //audioSource.timeSamples += Mathf.RoundToInt(startPoint * audioClip.frequency);
    }

    public void Update()
    {
        if (loopLength > 0 && loopPoint > 0)
        {
            if (audioSource.timeSamples > loopPoint * audioClip.frequency)
            {
                audioSource.timeSamples -= Mathf.RoundToInt(loopLength * audioClip.frequency);
            }
        }
    }
}