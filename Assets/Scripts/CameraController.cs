using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;

    public GameObject ScreenBound;

    Transform Character1;
    Transform Character2;

    float screenShake;
    float zPos;
    float zPosZoom;
    float zPosZoomOut;

    float yOffset = .2f;

    Vector3 cameraPos;
    float smooth = 4;


    // Start is called before the first frame update
    void Start()
    {
        zPos = transform.position.z;
        zPosZoom = zPos + 1;
        zPosZoomOut = zPos - .65f;

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");

        Character1 = Player1.transform.GetChild(0);
        Character2 = Player2.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Character1.GetComponent<MovementHandler>().Actions.grabbed || Character2.GetComponent<MovementHandler>().Actions.grabbed ||
           (Character2.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character2.GetComponent<CharacterProperties>().currentHealth <= 0 && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice") ||
           (Character1.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character1.GetComponent<CharacterProperties>().currentHealth <= 0) && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice" ||
            (Character2.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character2.GetComponent<MovementHandler>().Actions.shattered) ||
            (Character1.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character1.GetComponent<MovementHandler>().Actions.shattered) ||
            Character1.GetComponent<MovementHandler>().Actions.superFlash > 10 || Character2.GetComponent<MovementHandler>().Actions.superFlash > 10)
        {
            //zooming in "dynamic/cinematic" camera
            cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2, zPosZoom);

            if ((Character2.GetComponent<MovementHandler>().Actions.shattered || (Character2.GetComponent<CharacterProperties>().currentHealth <= 0) && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice") || 
                Character2.GetComponent<MovementHandler>().Actions.superFlash > 10)
                cameraPos = new Vector3(Character2.position.x, Character2.position.y, zPosZoom);
            else if (Character1.GetComponent<MovementHandler>().Actions.shattered || (Character1.GetComponent<CharacterProperties>().currentHealth <= 0 && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice") ||
                    Character1.GetComponent<MovementHandler>().Actions.superFlash > 10)
                cameraPos = new Vector3(Character1.position.x, Character1.position.y, zPosZoom);

            smooth = 5;

            if (cameraPos.y < 1)
                cameraPos = new Vector3(cameraPos.x, 1f, cameraPos.z);
        }
        else
        {
            //gameplay camera
            if (Mathf.Abs(Character1.position.x - Character2.position.x) > 3.5)
                cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2 + yOffset, zPosZoomOut);
            else
                cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2 + yOffset, zPos);

            if (transform.position.x < (Character1.position.x + Character2.position.x) / 2 + .75f && transform.position.x > (Character1.position.x + Character2.position.x) / 2 - .75f)
                ScreenBound.transform.position = transform.position;

            smooth = 10;

            if (Character1.GetComponent<MovementHandler>().wallStickTimer == 35 || Character2.GetComponent<MovementHandler>().wallStickTimer == 35)
            {
                screenShake = .1f;
            }

            if (screenShake > 0)
            {
                float randX = Random.Range(-1f, 1f);
                float randY = Random.Range(-1f, 1f);
                if (cameraPos.x < -8.5)
                    randX = Random.Range(0f, 1f);
                else if (cameraPos.x > 8.5)
                    randX = Random.Range(-1f, 0f);

                cameraPos = new Vector3(cameraPos.x + randX * 1.5f, cameraPos.y + randY * .3f, zPos);
                smooth = 10;
            }
            screenShake -= Time.deltaTime;

            if (cameraPos.x < -8.5)
                cameraPos = new Vector3(-8.5f, cameraPos.y, cameraPos.z);
            else if (cameraPos.x > 8.5)
                cameraPos = new Vector3(8.5f, cameraPos.y, cameraPos.z);
            if (cameraPos.y < 1.5)
                cameraPos = new Vector3(cameraPos.x, 1.5f, cameraPos.z);
            else if (cameraPos.y > 5)
                cameraPos = new Vector3(cameraPos.x, 5f, cameraPos.z);
        }

        if (cameraPos.y < 1.45)
            cameraPos = new Vector3(cameraPos.x, 1.45f, cameraPos.z);
        else if (cameraPos.y > 5)
            cameraPos = new Vector3(cameraPos.x, 5f, cameraPos.z);

        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.smoothDeltaTime * smooth);


    }
}
