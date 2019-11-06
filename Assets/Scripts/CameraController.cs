using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;

    public BoxCollider2D leftBound;
    public BoxCollider2D rightBound;

    Transform Character1;
    Transform Character2;

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
           (Character2.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character2.GetComponent<CharacterProperties>().currentHealth <= 0) ||
           (Character1.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character1.GetComponent<CharacterProperties>().currentHealth <= 0) ||
            (Character2.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character2.GetComponent<MovementHandler>().Actions.shattered) ||
            (Character1.GetComponent<MovementHandler>().HitDetect.hitStop > 0 && Character1.GetComponent<MovementHandler>().Actions.shattered))
        {
            //zooming in "dynamic/cinematic" camera
            cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2, zPosZoom);

            if ((Character2.GetComponent<MovementHandler>().Actions.shattered || Character2.GetComponent<CharacterProperties>().currentHealth <= 0))
                cameraPos = new Vector3(Character2.position.x, Character2.position.y, zPosZoom);
            else if (Character1.GetComponent<MovementHandler>().Actions.shattered || Character1.GetComponent<CharacterProperties>().currentHealth <= 0)
                cameraPos = new Vector3(Character1.position.x, Character1.position.y, zPosZoom);

            leftBound.enabled = false;
            rightBound.enabled = false;

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
            leftBound.enabled = true;
            rightBound.enabled = true;

            smooth = 7;

            if (cameraPos.x < -8.5)
                cameraPos = new Vector3(-8.5f, cameraPos.y, cameraPos.z);
            else if (cameraPos.x > 8.5)
                cameraPos = new Vector3(8.5f, cameraPos.y, cameraPos.z);
            if (cameraPos.y < 1.5)
                cameraPos = new Vector3(cameraPos.x, 1.5f, cameraPos.z);
            else if (cameraPos.y > 4)
                cameraPos = new Vector3(cameraPos.x, 4f, cameraPos.z);
        }

        if (cameraPos.y < 1.45)
            cameraPos = new Vector3(cameraPos.x, 1.45f, cameraPos.z);
        else if (cameraPos.y > 4)
            cameraPos = new Vector3(cameraPos.x, 4f, cameraPos.z);

        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.smoothDeltaTime * smooth);

        
    }
}
