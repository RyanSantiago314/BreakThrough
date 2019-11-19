using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private CharacterProperties PlayerProp1;
    private CharacterProperties PlayerProp2;
    public GameObject p1menu;
    public GameObject p2menu;
    private GameObject child1;
    private GameObject child2;
    float endTimer;

	void Start() {
		PlayerProp1 = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
		PlayerProp2 = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
		child1 = p1menu.transform.GetChild(0).gameObject;
		child2 = p2menu.transform.GetChild(0).gameObject;
		endTimer = -2;
	}

	void Update() {
		if (endTimer > 0) {
			endTimer -= Time.deltaTime;
		}
		if (PlayerProp1.currentHealth <= 0) {
			if (endTimer == -2) {
				endTimer = 3;
			}
			if (endTimer <=  0 && endTimer > -2) {
			child2.SetActive(true);
			}
        }
        if (PlayerProp2.currentHealth <= 0) {
        	if (endTimer == -2) {
				endTimer = 3;
			}
            if (endTimer <=  0 && endTimer > -2) {
			child1.SetActive(true);
			}
        }
	}

    public void ReplayGame() {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu() {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
