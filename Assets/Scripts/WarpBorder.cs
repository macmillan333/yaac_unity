using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBorder : MonoBehaviour
{
    public static Vector3 borderSize;

    // Start is called before the first frame update
    void Start()
    {
        borderSize = new Vector3(
            Camera.main.orthographicSize * Screen.width / Screen.height,
            1f,
            Camera.main.orthographicSize
            );
        transform.localScale = borderSize * 2;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
