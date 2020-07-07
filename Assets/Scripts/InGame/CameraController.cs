using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;

    public GameObject ScreenBound;

    Transform Character1;
    MovementHandler Char1Move;
    Transform Character2;
    MovementHandler Char2Move;

    float screenShake;
    float microScreenShake;
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
        zPosZoom = zPos + 1f;
        zPosZoomOut = zPos - .5f;

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");

        Character1 = Player1.transform.GetChild(0);
        Character2 = Player2.transform.GetChild(0);

        Char1Move = Character1.GetComponent<MovementHandler>();
        Char2Move = Character2.GetComponent<MovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Char1Move.Actions.grabbed || Char2Move.Actions.grabbed || Char1Move.Actions.grabZoom > 0 || Char2Move.Actions.grabZoom > 0 ||
           (Char2Move.HitDetect.hitStop > 0 && Character2.GetComponent<CharacterProperties>().currentHealth <= 0 && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice") ||
           (Char1Move.HitDetect.hitStop > 0 && Character1.GetComponent<CharacterProperties>().currentHealth <= 0) && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice" ||
            (Char2Move.HitDetect.hitStop > 0 && Char2Move.Actions.shattered) ||
            (Char1Move.HitDetect.hitStop > 0 && Char1Move.Actions.shattered) ||
            Char1Move.Actions.superFlash > (float)7/60 || Char2Move.Actions.superFlash > (float)7/60)
        {
            //zooming in "dynamic/cinematic" camera
            cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2, zPosZoom);
            if (Char1Move.Actions.superFlash > (float)7 / 60)
                cameraPos = new Vector3(Character1.position.x, Character1.position.y, zPosZoom);
            else if (Char2Move.Actions.superFlash > (float)7 / 60)
                cameraPos = new Vector3(Character2.position.x, Character2.position.y, zPosZoom);
            else if ((Char2Move.Actions.shattered || (Character2.GetComponent<CharacterProperties>().currentHealth <= 0) && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice"))
                cameraPos = new Vector3(Character2.position.x, Character2.position.y, zPosZoom);
            else if (Char1Move.Actions.shattered || (Character1.GetComponent<CharacterProperties>().currentHealth <= 0 && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice"))
                cameraPos = new Vector3(Character1.position.x, Character1.position.y, zPosZoom);

            smooth = 5;

            if (cameraPos.y < 1)
                cameraPos = new Vector3(cameraPos.x, 1f, cameraPos.z);
        }
        else
        {
            //gameplay camera
            if (Char1Move.hittingBound && Char2Move.hittingBound)
                cameraPos = new Vector3(transform.position.x, (Character1.position.y + Character2.position.y) / 2 + yOffset, transform.position.z);
            else
            {
                if (Mathf.Abs(Character1.position.x - Character2.position.x) > 3.5)
                    cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2 + yOffset, zPosZoomOut);
                else
                    cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2 + yOffset, zPos);
            }

            if (transform.position.x < (Character1.position.x + Character2.position.x) / 2 + .75f && transform.position.x > (Character1.position.x + Character2.position.x) / 2 - .75f)
                ScreenBound.transform.position = transform.position;

            smooth = 10;

            if (screenShake <= 0 && ((Char1Move.wallStickTimer < (float)42/60 && Char1Move.wallStickTimer >= (float)40 / 60) || (Char2Move.wallStickTimer < (float)42 / 60 && Char2Move.wallStickTimer >= (float)40 / 60) || (Char1Move.hittingWall && Char1Move.Actions.wallBounce && Char1Move.Actions.airborne) || (Char2Move.hittingWall && Char2Move.Actions.wallBounce && Char2Move.Actions.airborne)))
            {
                screenShake = .1f;
            }
            else if (Char2Move.Actions.screenShake || Char1Move.Actions.screenShake || (microScreenShake <= 0 && ((Char1Move.HitDetect.hitStop > 0 && (Char1Move.HitDetect.hitStun > 29 || Char1Move.Actions.superHit || Char1Move.HitDetect.hitStop > 13f/60f)) || (Char2Move.HitDetect.hitStop > 0 && (Char2Move.HitDetect.hitStun > 29 || Char2Move.Actions.superHit || Char2Move.HitDetect.hitStop > 13f / 60f)))))
            {
                microScreenShake = .1f;
                Char2Move.Actions.screenShake = false;
                Char1Move.Actions.screenShake = false;
            }

            if (screenShake > 0)
            {
                float randX = Random.Range(-1f, 1f);
                float randY = Random.Range(-1f, 1f);

                if (randX < 0)
                    randX -= .3f;
                else
                    randX += .3f;

                if (randY < 0)
                    randY -= .3f;
                else
                    randY += .3f;

                if (Mathf.Abs(Character1.position.x - Character2.position.x) > 3.5)
                    cameraPos = new Vector3(cameraPos.x + randX * 1.5f, cameraPos.y + randY * .5f, zPosZoomOut);
                else
                    cameraPos = new Vector3(cameraPos.x + randX * 1.5f, cameraPos.y + randY * .5f, zPos);
                
                smooth = 10;
            }
            else if (microScreenShake > 0)
            {
                float randX = Random.Range(-.5f, .5f);
                float randY = Random.Range(-.5f, .5f);

                if (randY < 0)
                    randY -= .05f;
                else
                    randY += .05f;

                if (Mathf.Abs(Character1.position.x - Character2.position.x) > 3.5)
                    cameraPos = new Vector3(cameraPos.x + randX * .3f, cameraPos.y + randY * .75f, zPosZoomOut);
                else
                    cameraPos = new Vector3(cameraPos.x + randX * .3f, cameraPos.y + randY * .75f, zPos);

                smooth = 10;
            }
            screenShake -= Time.deltaTime;
            microScreenShake -= Time.deltaTime;

            if (cameraPos.x < -7.8)
                cameraPos = new Vector3(-7.8f, cameraPos.y, cameraPos.z);
            else if (cameraPos.x > 7.8)
                cameraPos = new Vector3(7.8f, cameraPos.y, cameraPos.z);
            if (cameraPos.y < 1.3)
                cameraPos = new Vector3(cameraPos.x, 1.3f, cameraPos.z);
            else if (cameraPos.y > 6)
                cameraPos = new Vector3(cameraPos.x, 6f, cameraPos.z);
        }

        if (cameraPos.y < 1.45)
            cameraPos = new Vector3(cameraPos.x, 1.45f, cameraPos.z);
        else if (cameraPos.y > 5)
            cameraPos = new Vector3(cameraPos.x, 5f, cameraPos.z);

        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.smoothDeltaTime * smooth);


    }
}
