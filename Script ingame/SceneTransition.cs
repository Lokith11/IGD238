using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    [SerializeField] Animator animator;
    private string sceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeToScene(string sceneName)
    {
        this.sceneName = sceneName;
        StartCoroutine(LoadNextscene());
    }

    IEnumerator LoadNextscene()
    {
        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
        animator.SetTrigger("FadeOut");

    }
}
