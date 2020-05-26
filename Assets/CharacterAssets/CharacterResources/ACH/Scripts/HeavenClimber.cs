using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenClimber : MonoBehaviour
{
    public Animator anim;

    HitDetector HitDetect;
    Animator charAnim;
    bool followChar = false;
    bool flicker = false;
    float flickerTimer;
    float flickerIncrement;

    // Start is called before the first frame update
    void Start()
    {
        HitDetect = transform.root.GetChild(0).GetComponentInChildren<HitDetector>();
        charAnim = transform.root.GetChild(0).GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HitDetect.OpponentDetector.Actions.blitzed > 0 && HitDetect.hitStop <= 0)
            anim.SetFloat("AnimSpeed", .5f);
        else
            anim.SetFloat("AnimSpeed", charAnim.GetFloat("AnimSpeed"));

        if (followChar)
            transform.position = charAnim.transform.parent.transform.position;
    }

    void FlickerOn()
    {
        flicker = true;
    }

    void FlickerOff()
    {
        flicker = false;
    }

    void Follow()
    {
        followChar = true;
    }

    void Unfollow()
    {
        followChar = false;
    }

    void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
