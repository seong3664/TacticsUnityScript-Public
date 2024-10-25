using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_OutLine_highlighting : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color UiColor;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UiColor = Color.white;
    }
    private void OnEnable()
    {
        StartCoroutine(highlightNode());
    }
    IEnumerator highlightNode()
    {
        float a = 0.005f;
        while (true)
        {
            for (int i = 0; i < 150; i++)
            {
                UiColor.a = Mathf.Clamp(UiColor.a + a, 0f, 0.7f); 
                spriteRenderer.color = UiColor;
                yield return null;
            }
            a = -a; 
            yield return null;
        }
    }
    }
