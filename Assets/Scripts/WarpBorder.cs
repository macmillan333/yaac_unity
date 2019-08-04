using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBorder : MonoBehaviour
{
    // Actually it's extent. Half of scale.
    public static Vector3 borderSize;
    
    void Awake()
    {
        borderSize = new Vector3(
            Camera.main.orthographicSize * Screen.width / Screen.height,
            1f,
            Camera.main.orthographicSize
            );
        transform.localScale = borderSize * 2;
    }
}
