using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextAppearOnTrigger : MonoBehaviour
{
    [SerializeField] GameObject textBallon;
    // Start is called before the first frame update
    void Start()
    {
        textBallon.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {
            
                textBallon.SetActive(true);
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.Instance.State == GameManager.GameState.Play)
        {

            Destroy(textBallon);

        }
    }
}
