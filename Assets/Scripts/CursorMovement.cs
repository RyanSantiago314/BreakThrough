using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMovement : MonoBehaviour {

    private float speed;

	void Update () {
        //Set cursor speed to be constant with resolution
        speed = Screen.width/1.5f;

        //Manage P1Cursor movement
        if (this.name == "P1Cursor")
        {
            float x = Input.GetAxis("Horizontal_P1");
            float y = Input.GetAxis("Vertical_P1");

            transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, Screen.width/100, Screen.width),
            Mathf.Clamp(transform.position.y, Screen.height/20, Screen.height),
            transform.position.z);

        //Manage P2Cursor movement
        } else if (this.name == "P2Cursor")
        {
            float x = Input.GetAxis("Horizontal_P2");
            float y = Input.GetAxis("Vertical_P2");

            transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, Screen.width / 100, Screen.width),
            Mathf.Clamp(transform.position.y, Screen.height / 20, Screen.height),
            transform.position.z);
        }
        
    }
}
