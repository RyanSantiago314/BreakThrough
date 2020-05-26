using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerVoice : MonoBehaviour
{
    RoundManager roundManager;

    public AudioSource Announcer;
    public AudioSource BGM;

    public AudioClip AchealisP1;
    public AudioClip AchealisP2;
    public AudioClip Begin;
    public AudioClip BoBB;
    public AudioClip Counter;
    public AudioClip DhaliaP1;
    public AudioClip DhaliaP2;
    public AudioClip DoubleKO;
    public AudioClip Duel1;
    public AudioClip Duel2;
    public AudioClip DuelExtra;
    public AudioClip DuelFinal;
    public AudioClip Hurry;
    public AudioClip KO;
    public AudioClip PerfectKO;
    public AudioClip Pierce;
    public AudioClip Shatter;
    public AudioClip SuddenDeath;
    public AudioClip SuperKO;
    public AudioClip TimeUp;
    public AudioClip Versus;
    public AudioClip DhaliaTheme;
    public AudioClip AchealisTheme;

    private bool startBGM = true;

    void Start()
    {
        roundManager = GetComponent<RoundManager>();
    }

    //Individual Voice Clip Functions
    public void PlayAchealisP1()
    {
        Announcer.PlayOneShot(AchealisP1, .8f);
    }

    public void PlayAchealisP2()
    {
        Announcer.PlayOneShot(AchealisP2, .8f);
    }

    public void PlayBegin()
    {
        Announcer.PlayOneShot(Begin, .8f);
    }

    public void PlayBoBB()
    {
        Announcer.PlayOneShot(BoBB, .8f);
    }

    public void PlayCounter()
    {
        Announcer.PlayOneShot(Counter, .8f);
    }

    public void PlayDhaliaP1()
    {
        Announcer.PlayOneShot(DhaliaP1, .8f);
    }

    public void PlayDhaliaP2()
    {
        Announcer.PlayOneShot(DhaliaP2, .8f);
    }

    public void PlayDoubleKO()
    {
        Announcer.PlayOneShot(DoubleKO, .8f);
    }

    public void PlayDuel1()
    {
        Announcer.PlayOneShot(Duel1, .8f);
    }

    public void PlayDuel2()
    {
        Announcer.PlayOneShot(Duel2, .8f);
    }

    public void PlayDuelExtra()
    {
        Announcer.PlayOneShot(DuelExtra, .8f);
    }

    public void PlayDuelFinal()
    {
        Announcer.PlayOneShot(DuelFinal, .8f);
    }

    public void PlayHurry()
    {
        Announcer.PlayOneShot(Hurry, .8f);
    }

    public void PlayKO()
    {
        Announcer.PlayOneShot(KO, .8f);
    }

    public void PlayPerfectKO()
    {
        Announcer.PlayOneShot(PerfectKO, .8f);
    }

    public void PlayPierce()
    {
        Announcer.PlayOneShot(Pierce, .8f);
    }

    public void PlayShatter()
    {
        Announcer.PlayOneShot(Shatter, .8f);
    }

    public void PlaySuddenDeath()
    {
        Announcer.PlayOneShot(SuddenDeath, .8f);
    }

    public void PlaySuperKO()
    {
        Announcer.PlayOneShot(SuperKO, .8f);
    }

    public void PlayTimeUp()
    {
        Announcer.PlayOneShot(TimeUp, .8f);
    }

    public void PlayVersus()
    {
        Announcer.PlayOneShot(Versus, .8f);
    }

    //Pre-Sorted Voice Clip Functions
    public void PlayDuel() {
        if (RoundManager.roundCount <= 1)
        {
            PlayDuel1();
        }
        else if (RoundManager.roundCount == 2)
        {
            PlayDuel2();
        }
        else if (RoundManager.roundCount == 3)
        {
            PlayDuelFinal();
        }
        else if (RoundManager.roundCount > 3)
        {
            PlayDuelExtra();
        }
    }

    public void PlayWinMethod()
    {
        switch (roundManager.centerText.text)
        {
            case "Time Up":
                PlayTimeUp();
                break;
            case "PERFECT":
                PlayPerfectKO();
                break;
            case "BreakDown":
                if (roundManager.P1Prop.HitDetect.Actions.superHit || roundManager.P2Prop.HitDetect.Actions.superHit)
                    PlaySuperKO();
                else
                    PlayKO();
                break;
            case "Double KO":
                PlayDoubleKO();
                break;
        }
    }

    public void PlayP1Name()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
        {
            switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character)
            {
                case "Dhalia":
                    PlayDhaliaP1();
                    break;
                case "Achealis":
                    PlayAchealisP1();
                    break;
            }
        }
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
        {
            switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
            {
                case "Dhalia":
                    PlayDhaliaP1();
                    break;
                case "Achealis":
                    PlayAchealisP1();
                    break;
            }
        }
    }

    public void PlayP2Name()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
        {
            switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character)
            {
                case "Dhalia":
                    PlayDhaliaP2();
                    break;
                case "Achealis":
                    PlayAchealisP2();
                    break;
            }
        }
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
        {
            switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
            {
                case "Dhalia":
                    PlayDhaliaP2();
                    break;
                case "Achealis":
                    PlayAchealisP2();
                    break;
            }
        }
    }

    //BGM (Figure out which theme to play here once music has been added)
    public void PlayBGM()
    {
        if (startBGM)
        {
            startBGM = false;
            BGM.clip = AchealisTheme;
            BGM.Play();
        }
    }

    public void PlayBGMTraining()
    {
        if (startBGM && (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial"))
        {
            startBGM = false;
            BGM.clip = AchealisTheme;
            BGM.Play();
        }
    }
}
