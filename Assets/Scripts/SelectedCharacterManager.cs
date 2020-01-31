using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectedCharacterManager : MonoBehaviour
{
    public string P1Character;
    public string P2Character;
    private static bool created = false;

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

    void start()
    {
        //set strings for characters
        P1Character = "Dhalia";
        P2Character = "Dhalia";
    }
}
