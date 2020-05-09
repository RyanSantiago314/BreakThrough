using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectedCharacterManager : MonoBehaviour
{
    public string gameMode;
    public string stage;
    public string P1Character;
    public string P2Character;
    public int P1Color;
    public int P2Color;
    public string P1Side;
    public string P2Side;
    public float CPUDifficulty = 50f;

    public bool online = false;

    private static bool created = false;
    private bool reset = false;
    private bool resetDifficulty = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelect" && reset == false)
        {
            stage = "";
            P1Character = "";
            P2Character = "";
            P1Color = 0;
            P2Color = 0;
            reset = true;
        }
        else if (SceneManager.GetActiveScene().name != "CharacterSelect")
        {
            reset = false;
        }
        //Debug.Log(CPUDifficulty);
    }
}
