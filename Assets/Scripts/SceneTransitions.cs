using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    Animator SceneTransition;
    bool created = false;
    private int sceneNumber;
    AsyncOperation asyncLoadLevel;
    public GameObject loadingGraphic;
    static public bool lockinputs = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.transform.parent);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneTransition = this.GetComponent<Animator>();
    }

    public void LoadScene(int sceneIndex)
    {
        sceneNumber = sceneIndex;
        SceneTransition.SetTrigger("FadeIn");
        lockinputs = true;
    }

    IEnumerator TransitionScene(int sceneIndex)
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        loadingGraphic.SetActive(false);
        SceneTransition.SetTrigger("FadeOut");
        lockinputs = false;
    }

    public void BlackScreen()
    {
        loadingGraphic.SetActive(true);
        SceneTransition.SetTrigger("BlackScreen");
        StartCoroutine(TransitionScene(sceneNumber));
    }

    public void fadein()
    {
        SceneTransition.SetTrigger("FadeIn");
    }
}
