using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorMovement : MonoBehaviour {

    private int playerPaused;
    private float speed;
    public bool isPaused;
    public bool P1Selected;
    public bool P2Selected;

    public GameObject backMenuUI;
    public GameObject P1Cursor;
    public GameObject P2Cursor;
    public GameObject fightButton;

    public CursorDetection P1;
    public CursorDetection P2;


    void Update()
    {

        //Set cursor speed to be constant with resolution
        speed = Screen.width / 1.5f;

        //Manage Back Menu interations
        if (isPaused == false)
        {
            if (!P1.P1Selected)
            {
                //Manage P1Cursor movement
                float x = Input.GetAxis("Horizontal_P1");
                float y = Input.GetAxis("Vertical_P1");

                P1Cursor.transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

                P1Cursor.transform.position = new Vector3(Mathf.Clamp(P1Cursor.transform.position.x, Screen.width / 100, Screen.width),
                Mathf.Clamp(P1Cursor.transform.position.y, Screen.height / 20, Screen.height),
                P1Cursor.transform.position.z);
            }
            if (!P2.P2Selected)
            {
                //Manage P2Cursor movement
                float x2 = Input.GetAxis("Horizontal_P2");
                float y2 = Input.GetAxis("Vertical_P2");

                P2Cursor.transform.position += new Vector3(x2, y2, 0) * Time.deltaTime * speed;

                P2Cursor.transform.position = new Vector3(Mathf.Clamp(P2Cursor.transform.position.x, Screen.width / 100, Screen.width),
                Mathf.Clamp(P2Cursor.transform.position.y, Screen.height / 20, Screen.height),
                P2Cursor.transform.position.z);
            }
        }

        if (P1.P1Selected && P2.P2Selected)
        {
            fightButton.SetActive(true);
        }
        else
        {
            fightButton.SetActive(false);
        }

        //Manage Back Menu
        /* if (isPaused)
         {
             ActivateMenu();
             //Unpause the game (Only the player that paused can unpause)
             if (Input.GetButtonDown("Circle_P1") && playerPaused == 1)
             {
                 isPaused = !isPaused;
                 playerPaused = 0;
             }
             if (Input.GetButtonDown("Circle_P2") && playerPaused == 2)
             {
                 isPaused = !isPaused;
                 playerPaused = 0;
             }
         }
         else if (!isPaused)
         {
             DeactivateMenu();
             //Record which player paused
             if (Input.GetButtonDown("Circle_P1") && playerPaused == 0 && !P1.P1Selected)
             {
                 isPaused = !isPaused;
                 playerPaused = 1;
             }
             if (Input.GetButtonDown("Circle_P2") && playerPaused == 0 && !P2.P2Selected)
             {
                 isPaused = !isPaused;
                 playerPaused = 2;
             }
         }*/
     }
     

     public void ActivateMenu()
     {
         backMenuUI.SetActive(true);
     }

     public void DeactivateMenu()
     {
         backMenuUI.SetActive(false);
         isPaused = false;
         playerPaused = 0;
     }

    public void StartGame()
    {
        SceneManager.LoadScene(3);
    }

     public void QuitToMenu()
     {
         SceneManager.LoadScene(0);
     }
    
}
