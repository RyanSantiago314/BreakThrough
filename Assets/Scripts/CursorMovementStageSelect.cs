using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorMovementStageSelect : MonoBehaviour
{
    public float speed;
    private int stageNum;
    private int prevStageNum;
    public string currentStage;

    private bool isOverlap;

    public GameObject[] borders;
    public GameObject[] stagePreviews;
    public GameObject[] stageNames;

    public GameObject loadingScreen;
    public GameObject charSelect;
    public GameObject stageSelect;

    public CursorMovement cursordata;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        currentStage = collider.transform.parent.name;
        isOverlap = true;

        switch (currentStage)
        {
            case "TrainingStage":
                stageNum = 0;
                break;
        }
        stagePreviews[stageNum].SetActive(true);
        stageNames[0].SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        borders[stageNum].SetActive(false);
        stagePreviews[stageNum].SetActive(false);
        stageNames[stageNum].SetActive(false);
        currentStage = "";
        isOverlap = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Manage Cursor movement
        float x = Input.GetAxis("Horizontal_P1");
        float y = Input.GetAxis("Vertical_P1");

        //Enable P2 to also control cursor
        float x2 = Input.GetAxis("Horizontal_P2");
        float y2 = Input.GetAxis("Vertical_P2");

        transform.position += new Vector3(Mathf.Clamp(x+x2,-1,1), Mathf.Clamp(y+y2,-1,1), 0) * Time.deltaTime * speed;

        Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
        Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
        transform.position.z);

        //Manage Selection input
        if (isOverlap)
        {
            borders[stageNum].SetActive(true);
            if (Input.GetButtonDown("Cross_P1"))
            {
                GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().stage = currentStage;
                switch (currentStage)
                {
                    case "TrainingStage":
                        loadingScreen.SetActive(true);
                        SceneManager.LoadScene(3);
                        break;
                }
            }
        }

        if (Input.GetButtonDown("Circle_P1") || Input.GetButtonDown("Circle_P2"))
        {
            if (Input.GetButtonDown("Circle_P1"))
            {
                cursordata.P1Ready = false;
            } else if (Input.GetButtonDown("Circle_P2"))
            {
                cursordata.P2Ready = false;
            }
            stageSelect.SetActive(false);
            charSelect.SetActive(true);
            resetPosition();
            stageNum = 0;
        }
    }

    private void resetPosition() {
        transform.GetComponent<RectTransform>().localPosition = new Vector3(-392, -362, 0);
    }
}
