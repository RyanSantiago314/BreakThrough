using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSFX : MonoBehaviour
{
    public AudioSource FX;
    int soundSelect;

    public AudioClip StrikeL;
    public AudioClip StrikeM;
    public AudioClip StrikeM2;
    public AudioClip StrikeH;
    public AudioClip StrikeH2;
    public AudioClip SlashL;
    public AudioClip SlashL2;
    public AudioClip SlashH;
    public AudioClip SlashH2;
    public AudioClip SlashH3;
    public AudioClip GuardL;
    public AudioClip GuardM;
    public AudioClip GuardH;
    public AudioClip Shatter;

    public void ShatterPlay()
    {
        FX.PlayOneShot(Shatter, .8f);
    }

    public void LightGuard()
    {
        FX.PlayOneShot(GuardL, .8f);
    }

    public void MediumGuard()
    {
        FX.PlayOneShot(GuardM, .8f);
    }

    public void HeavyGuard()
    {
        FX.PlayOneShot(GuardH, .8f);
    }

    public void LightStrike()
    {
        FX.PlayOneShot(StrikeL, .8f);
    }

    public void MediumStrike()
    {
        soundSelect = Random.Range(0, 2);
        switch (soundSelect)
        {
            case 1:
                FX.PlayOneShot(StrikeM2, .8f);
                break;
            case 0:
                FX.PlayOneShot(StrikeM, .8f);
                break;
            default:
                FX.PlayOneShot(StrikeM, .8f);
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
        soundSelect = Random.Range(0, 2);
        switch (soundSelect)
        {
            case 1:
                FX.PlayOneShot(SlashL2, .8f);
                break;
            case 0:
                FX.PlayOneShot(SlashL, .8f);
                break;
            default:
                FX.PlayOneShot(SlashL, .8f);
                break;
        }
    }

    public void HeavySlash()
    {
        soundSelect = Random.Range(0, 3);
        switch (soundSelect)
        {
            case 2:
                FX.PlayOneShot(SlashH3, .8f);
                break;
            case 1:
                FX.PlayOneShot(SlashH2, .8f);
                break;
            case 0:
                FX.PlayOneShot(SlashH, .8f);
                break;
            default:
                FX.PlayOneShot(SlashH, .8f);
                break;
        }
    }
}
