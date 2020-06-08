using System.Collections;
using System.Collections.Generic;
using Networking;
using UnityEngine;

using Photon.Pun;

public class Lighting : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;

    Transform Character1Sprite;
    Transform Character2Sprite;

    Light enviroLight;
    float intensity;
    
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

    void Init()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");

        Character1Sprite = Player1.transform.GetChild(0).transform.GetChild(0);
        Character2Sprite = Player2.transform.GetChild(0).transform.GetChild(0);

        enviroLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

        if (runOnce && netBool.allPlayersInstantiated)
        {
            runOnce = false;
            Init();
            
        }
        
        if (Character1Sprite.GetComponent<AcceptInputs>().blitzed > 0 || Character2Sprite.GetComponent<AcceptInputs>().blitzed > 0 || 
            Character1Sprite.GetComponent<AcceptInputs>().superFlash > 0 || Character2Sprite.GetComponent<AcceptInputs>().superFlash > 0)
        {
            enviroLight.intensity = Mathf.Lerp(enviroLight.intensity, 0f, Time.deltaTime * 25);
        }
        else
            enviroLight.intensity = Mathf.Lerp(enviroLight.intensity, .75f, Time.deltaTime * 10);
    }
}
