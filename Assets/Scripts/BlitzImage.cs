using System.Collections;
using System.Collections.Generic;
using Networking;
using UnityEngine;
using Photon.Pun;

public class BlitzImage : MonoBehaviour
{
    public SpriteRenderer AfterImage;
    public AudioSource blitz;

    AcceptInputs OpponentActions;
    
    //Networking
    private NetworkInstantiate netBool;
    private bool runOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            netBool = GameObject.Find("Player1").GetComponentInChildren<NetworkInstantiate>();
        }
        else
        {
            netBool = GameObject.Find("Player2").GetComponentInChildren<NetworkInstantiate>();
        }
        
        Init();
    }

    private void Init()
    {
        OpponentActions = transform.root.GetChild(0).GetComponent<MovementHandler>().HitDetect.OpponentDetector.Actions;
    }

    // Update is called once per frame
    void Update()
    {
        if (runOnce && netBool.allPlayersInstantiated)
        {
            runOnce = false;
            Init();
        }
        
        
        if (OpponentActions.blitzed > 30)
        {
            AfterImage.color = new Color(AfterImage.color.r, AfterImage.color.g, AfterImage.color.b, .7f);
        }
        else if (OpponentActions.blitzed > 0)
        {
            AfterImage.color = new Color(AfterImage.color.r, AfterImage.color.g, AfterImage.color.b, AfterImage.color.a - .02f);
        }
        else if (AfterImage.color.a > 0)
        {
            AfterImage.color = new Color(AfterImage.color.r, AfterImage.color.g, AfterImage.color.b, AfterImage.color.a - .05f);
        }
    }

    public void Play()
    {
        blitz.PlayOneShot(blitz.clip, 1);
    }
}
