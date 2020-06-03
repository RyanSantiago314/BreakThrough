using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenClimber : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer sprite;

    HitDetector HitDetect;
    Animator charAnim;
    bool followChar = false;
    bool flicker = false;
    bool fade = false;
    bool hitStopMatch;
    float flickerTimer;
    bool flickerDown;
    float flickerIncrement = .33f;

    // Start is called before the first frame update
    void Start()
    {
        HitDetect = transform.root.GetChild(0).GetComponentInChildren<HitDetector>();
        charAnim = transform.root.GetChild(0).GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HitDetect.OpponentDetector.Actions.blitzed > 0 && HitDetect.hitStop <= 0 && !followChar)
        {
            anim.SetFloat("AnimSpeed", .65f);
        }
        else if (hitStopMatch)
        {
            anim.SetFloat("AnimSpeed", charAnim.GetFloat("AnimSpeed"));
        }
        else
        {
            anim.SetFloat("AnimSpeed", 1);
        }

        if (anim.GetFloat("AnimSpeed") < 1)
            flickerIncrement = .05f;
        else
            flickerIncrement = .33f;

        if (followChar)
            transform.position = charAnim.transform.parent.transform.position;

        if (flicker)
        {
            sprite.color = Color.Lerp(Color.white, new Color(1, 180f / 255, 230f / 255, 1f), flickerTimer);
        }
        else if (fade && !flicker)
            sprite.color = Color.Lerp(Color.white, new Color(1, 180f / 255f, 230f / 255f, 0), flickerTimer);
        else
            sprite.color = Color.white;

        if (flickerTimer <= 0)
            flickerDown = false;
        else if (flickerTimer > 1)
            flickerDown = true;
        if (flicker)
        {
            if (flickerDown)
                flickerTimer -= flickerIncrement;
            else
                flickerTimer += flickerIncrement;
        }
        else if (!flicker && !followChar)
        {
            flickerTimer += flickerIncrement;
        }
    }

    void FlickerOn()
    {
        flicker = true;
        flickerTimer = 0;
    }

    void FlickerOff()
    {
        flicker = false;
        flickerTimer = 0;
    }

    void FadeOn()
    {
        fade = true;
    }

    void FadeOff()
    {
        fade = false;
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

    void MatchHitStop()
    {
        hitStopMatch = true;
    }

    void DesyncHitStop()
    {
        hitStopMatch = false;
    }
}
