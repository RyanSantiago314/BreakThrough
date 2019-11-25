using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public void startLoad(int sceneIndex) {
    	StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex) {
    	AsyncOperation LoadBar = SceneManager.LoadSceneAsync(sceneIndex);
    	while (!LoadBar.isDone){
    		float progress = Mathf.Clamp01(LoadBar.progress / 0.9f);
    		Debug.Log(progress);
    		yield return null;
    	}
    }
}
