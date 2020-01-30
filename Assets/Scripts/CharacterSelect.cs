using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour {
    public static int PlayerNum;
	public void CharacterSelectFunction (int sellectedNum) {
        PlayerNum = sellectedNum;
        Application.LoadLevel("TrainingStage");
	}
}
