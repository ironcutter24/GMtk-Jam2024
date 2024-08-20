using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : GeneralObject
{
    [SerializeField] string sceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
            if (collision.CompareTag("Player"))
            {
                    SceneManager.LoadScene(sceneName);
            }
        
    }

    protected override void ResetState()
    {
        // Noting to reset
    } 
}
