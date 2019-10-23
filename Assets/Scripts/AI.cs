using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    float timer;
    private MaxInput MaxInput;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        MaxInput = GetComponent<MaxInput>();
        if (!MaxInput.AI)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MaxInput.ClearInput("Player2");
        timer += Time.deltaTime;

        if (timer > 2)
        {
            MaxInput.Circle("Player2");
            Debug.Log("Stabbing");
            timer = 0;
        }

        MaxInput.moveLeft("Player2");
    }
}
