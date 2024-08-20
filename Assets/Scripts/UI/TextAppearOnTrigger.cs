using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TextAppearOnTrigger : MonoBehaviour
{
    [SerializeField] SpriteRenderer balloon;
    [SerializeField] SpriteRenderer pointer;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] Color balloonColor;
    [SerializeField] Color tutorialColor;
    [Space]
    [SerializeField] bool isTutorial = false;
    [Space]
    [SerializeField, Multiline] string textMessage;

    private void Start()
    {
        if (!Application.isPlaying) return;

        balloon.gameObject.SetActive(false);
        textMeshPro.text = textMessage;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
        {
            textMeshPro.text = textMessage;

            balloon.color = isTutorial ? tutorialColor : balloonColor;
            pointer.color = balloonColor;
            pointer.gameObject.SetActive(!isTutorial);
        }
    }
#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        balloon.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        balloon.gameObject.SetActive(false);
    }
}
