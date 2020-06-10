using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSFX : MonoBehaviour
{
    public AudioSource FX;
    int soundSelect;
    HitDetector HitDetect;

    public AudioClip StrikeL;
    public AudioClip StrikeM;
    public AudioClip StrikeM2;
    public AudioClip StrikeH;
    public AudioClip StrikeH2;
    public AudioClip SlashL;
    public AudioClip SlashH;
    public AudioClip SlashSuper;
    public AudioClip GuardL;
    public AudioClip GuardM;
    public AudioClip GuardH;
    public AudioClip ArmorL;
    public AudioClip ArmorH;
    public AudioClip Shatter;

    void Start()
    {
        HitDetect = transform.root.GetChild(0).GetComponentInChildren<HitDetector>();
    }

    public void ShatterPlay()
    {
        FX.PlayOneShot(Shatter, .8f);
    }

    public void LightGuard()
    {
        FX.PlayOneShot(GuardL, .7f);
    }

    public void MediumGuard()
    {
        FX.PlayOneShot(GuardM, .7f);
    }

    public void HeavyGuard()
    {
        FX.PlayOneShot(GuardH, .7f);
    }

    public void LightStrike()
    {
        FX.PlayOneShot(StrikeL, .7f);
    }

    public void MediumStrike()
    {
        soundSelect = Random.Range(0, 2);
        switch (soundSelect)
        {
            case 1:
                FX.PlayOneShot(StrikeM2, .7f);
                break;
            case 0:
                FX.PlayOneShot(StrikeM, .7f);
                break;
            default:
                FX.PlayOneShot(StrikeM, .7f);
                break;
        }
    }

    public void HeavyStrike()
    {
        soundSelect = Random.Range(0, 2);
        switch (soundSelect)
        {
            case 1:
                FX.PlayOneShot(StrikeH2, .7f);
                break;
            case 0:
                FX.PlayOneShot(StrikeH, .7f);
                break;
            default:
                FX.PlayOneShot(StrikeH, .7f);
                break;
        }
    }

    public void LightSlash()
    {
                FX.PlayOneShot(SlashL, .7f);
    }

    public void HeavySlash()
    {
        if (HitDetect.OpponentDetector.Actions.superHit)
            FX.PlayOneShot(SlashSuper, .7f);
        else
            FX.PlayOneShot(SlashH, .7f);
    }

    public void ArmorHeavy()
    {
        FX.PlayOneShot(ArmorH, .7f);
    }

    public void ArmorLight()
    {
        FX.PlayOneShot(ArmorL, .7f);
    }
}
