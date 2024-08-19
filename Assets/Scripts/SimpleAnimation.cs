using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SimpleAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private int index = 0;

    [SerializeField] bool randomizeStartFrame = false;
    [SerializeField] SpriteFrame[] frames;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();

        if (Application.isPlaying)
        {
            if (randomizeStartFrame)
            {
                index = Random.Range(0, frames.Length);
            }

            StartCoroutine(_LoopThroughSprites());
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying && frames.Length > 0)
        {
            spriteRend.sprite = frames[0].sprite;
        }
    }
#endif

    IEnumerator _LoopThroughSprites()
    {
        while (isActiveAndEnabled)
        {
            spriteRend.sprite = frames[index].sprite;

            yield return new WaitForSeconds(frames[index].duration);

            index++;
            index = Util.Helpers.CircularClamp(index, 0, frames.Length - 1);
        }
    }

    [Serializable]
    private class SpriteFrame
    {
        public Sprite sprite;
        public float duration = .2f;

        public SpriteFrame(Sprite sprite, float duration)
        {
            this.sprite = sprite;
            this.duration = duration;
        }
    }
}
