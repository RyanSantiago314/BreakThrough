using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;

    Transform Character1Sprite;
    Transform Character2Sprite;

    Light enviroLight;
    float intensity;

    // Start is called before the first frame update
    void Start()
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
        if (Character1Sprite.GetComponent<AcceptInputs>().blitzed > 0 || Character2Sprite.GetComponent<AcceptInputs>().blitzed > 0)
        {
            enviroLight.intensity = Mathf.Lerp(enviroLight.intensity, 0f, Time.deltaTime * 25);
        }
        else
            enviroLight.intensity = Mathf.Lerp(enviroLight.intensity, .75f, Time.deltaTime * 10);
    }
}
