using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHASFX : MonoBehaviour
{
    public AudioSource SFX;
    public AudioClip Weak5B;
    public AudioClip Strong5B;
    public AudioClip ToasterStart;

    void WeakBreak()
    {
        SFX.PlayOneShot(Weak5B, .5f);
    }

    void StrongBreak()
    {
        SFX.PlayOneShot(Strong5B, .5f);
    }

    void StartToaster()
    {
        SFX.PlayOneShot(ToasterStart, .8f);
    }
}
