using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSpriteControl : MonoBehaviour
{
    public SpriteRenderer parentRenderer;
    SpriteRenderer rend;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        rend.sprite = parentRenderer.sprite;
        transform.localScale = new Vector2(parentRenderer.transform.localScale.x,transform.localScale.y);
    }
}
