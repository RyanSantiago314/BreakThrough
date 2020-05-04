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

    private string p1Cross = "Cross_P1";
    private string p1Circle = "Circle_P1";
    private string p1Hor = "Horizontal_P1";
    private string p1Ver = "Vertical_P1";
    private string p2Cross = "Cross_P2";
    private string p2Circle = "Circle_P2";
    private string p2Hor = "Horizontal_P2";
    private string p2Ver = "Vertical_P2";

    private int p1Num = 0;
    private int p2Num = 1;

    public GameObject[] borders;
    public GameObject[] stagePreviews;
    public GameObject[] stageNames;

    public GameObject loadingScreen;
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
            case "DhaliaStage":
                stageNum = 1;
                break;
        }
        stagePreviews[stageNum].SetActive(true);
        stageNames[stageNum].SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        borders[stageNum].SetActive(false);
        stagePreviews[stageNum].SetActive(false);
        stageNames[stageNum].SetActive(false);
        currentStage = "";
        isOverlap = false;
    }

    private void Start()
    {
        p1Cross += UpdateControls(CheckXbox(p1Num));
        p2Cross += UpdateControls(CheckXbox(p2Num));
        p1Circle += UpdateControls(CheckXbox(p1Num));
        p2Circle += UpdateControls(CheckXbox(p2Num));
        p1Ver += UpdateControls(CheckXbox(p1Num));
        p1Hor += UpdateControls(CheckXbox(p1Num));
        p2Ver += UpdateControls(CheckXbox(p2Num));
        p2Hor += UpdateControls(CheckXbox(p2Num));
    }

    // Update is called once per frame
    void Update()
    {
        //Manage Cursor movement
        float x = Input.GetAxis(p1Hor);
        float y = Input.GetAxis(p1Ver);

        //Enable P2 to also control cursor
        float x2 = Input.GetAxis(p2Hor);
        float y2 = Input.GetAxis(p2Ver);

        transform.position += new Vector3(Mathf.Clamp(x+x2,-1,1), Mathf.Clamp(y+y2,-1,1), 0) * Time.deltaTime * speed;

        Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
        Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
        transform.position.z);

        //Manage Selection input
        if (isOverlap)
        {
            borders[stageNum].SetActive(true);
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "PvP")
            {
                if (Input.GetButtonDown(p1Cross) || Input.GetButtonDown(p2Cross))
                {
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().stage = currentStage;
                    switch (currentStage)
                    {
                        case "TrainingStage":
                            loadingScreen.SetActive(true);
                            SceneManager.LoadScene(2);
                            break;
                        case "DhaliaStage":
                            loadingScreen.SetActive(true);
                            SceneManager.LoadScene(3);
                            break;
                    }
                }
            } else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
            {
                if (Input.GetButtonDown(p1Cross))
                {
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().stage = currentStage;
                    switch (currentStage)
                    {
                        case "TrainingStage":
                            loadingScreen.SetActive(true);
                            SceneManager.LoadScene(2);
                            break;
                        case "DhaliaStage":
                            loadingScreen.SetActive(true);
                            SceneManager.LoadScene(3);
                            break;
                    }
                }
            }
            
        }

        if (Input.GetButtonDown(p1Circle) || Input.GetButtonDown(p2Circle))
        {
                stageSelect.SetActive(false);
                resetPosition();
                stageNum = 0;       
        }
    }

    private void resetPosition() {
        transform.GetComponent<RectTransform>().localPosition = new Vector3(-392, -362, 0);
    }

    private bool CheckXbox(int player)
    {
        if (Input.GetJoystickNames().Length > player)
        {
            if (Input.GetJoystickNames()[player].Contains("Xbox"))
            {
                return true;
            }
        }
        return false;
    }

    private string UpdateControls(bool xbox)
    {
        if (xbox)
            return "_Xbox";
        return "";
    }
}
