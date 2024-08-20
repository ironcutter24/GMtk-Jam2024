using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] bool loadOnAwake = false;
    [SerializeField] bool loadNextScene = false;

    private void Awake()
    {
        if (loadOnAwake)
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (loadNextScene)
        {
            GameManager.Instance.LoadNextLevel();
        }
        else
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
