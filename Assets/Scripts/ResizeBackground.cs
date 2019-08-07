using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBackground : MonoBehaviour
{
    private float alpha;

    // Start is called before the first frame update
    void Start()
    {
        alpha = GetComponent<SpriteRenderer>().color.a;

        Texture texture = GetComponent<SpriteRenderer>().sprite.texture;
        // 1 pixel = 1 unit on all sprites' import settings
        float width = texture.width;
        float height = texture.height;
        float targetWidth = WarpBorder.borderSize.x;
        float targetHeight = WarpBorder.borderSize.z;
        float scale = Mathf.Max(targetWidth / width, targetHeight / height) * 2f;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
