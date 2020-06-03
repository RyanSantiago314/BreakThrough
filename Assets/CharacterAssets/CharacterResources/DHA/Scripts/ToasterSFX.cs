using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterSFX : MonoBehaviour
{
    public AudioSource SFX;
    public AudioClip ToasterStart;
    public AudioClip ToasterFire;
    public AudioClip ToasterEnd;

    private void Update()
    {
        if (transform.position.y < 0)
            SFX.volume = 0;
        else
            SFX.volume = 1;
    }
    void FiringToaster()
    {
        SFX.PlayOneShot(ToasterFire, .8f);
    }

    
    void EndToaster()
    {
        SFX.PlayOneShot(ToasterEnd, .8f);
    }
}
