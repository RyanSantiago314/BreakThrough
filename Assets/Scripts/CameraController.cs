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

    Vector3 cameraPos;
    float smooth = 5;


    // Start is called before the first frame update
    void Start()
    {
        zPos = transform.position.z;
        zPosZoom = transform.position.z + 1;

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");

        Character1 = Player1.transform.GetChild(0);
        Character2 = Player2.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            cameraPos = new Vector3(Character1.position.x, Character1.position.y, zPosZoom);
            smooth = 5;
            leftBound.enabled = false;
            rightBound.enabled = false;
        }
        else
        {
            cameraPos = new Vector3((Character1.position.x + Character2.position.x) / 2, (Character1.position.y + Character2.position.y) / 2, zPos);
            smooth = 7;
            leftBound.enabled = true;
            rightBound.enabled = true;
        }

        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.fixedDeltaTime * smooth);

        if (transform.position.x < -7.6)
            transform.position = new Vector3(-7.6f, transform.position.y, transform.position.z);
        if (transform.position.x > 7.6)
            transform.position = new Vector3(7.6f, transform.position.y, transform.position.z);
        if(transform.position.y < 1.45)
            transform.position = new Vector3(transform.position.x, 1.45f, transform.position.z);
        if (transform.position.y > 4)
            transform.position = new Vector3(transform.position.x, 4f, transform.position.z);
    }
}
