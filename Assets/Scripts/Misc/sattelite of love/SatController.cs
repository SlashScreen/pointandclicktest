using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatController : MonoBehaviour
{
    // Start is called before the first frame update

    public RenderCamClickThrough clickThrough;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)

        transform.position = new Vector2 (transform.position.x, clickThrough.mouseProjectedPos.y);
        //todo: collision and all that
    }
}
