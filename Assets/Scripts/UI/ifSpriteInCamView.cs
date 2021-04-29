using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ifSpriteInCamView : MonoBehaviour
{
    public UnityEvent onTrue;
    public UnityEvent onFalse;
    SpriteRenderer sprite;
    bool isVisible;
    bool wasVisible;
    

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //calculate bounds of camera
        Vector3 center = Camera.main.transform.position;
        center.z = 0f; //set z to origin plane to ensure it hits something
        var vertExtent = Camera.main.orthographicSize*2f;    
        var horzExtent = vertExtent * Screen.width / Screen.height;
        Bounds camBounds = new Bounds(center,new Vector3(horzExtent,vertExtent,10f));
        //Debug.Log(sprite.bounds + ", " + camBounds + ", " + sprite.bounds.Intersects(camBounds));

        isVisible = sprite.bounds.Intersects(camBounds); //set isVisible to if it can be seen by the camera
        if(isVisible != wasVisible){ //if that's different from last frame
            if(isVisible){ //if it is visible, invoke onTrue events
                onTrue.Invoke();
                //Debug.Log("visible");
            }else{ //if it cannot be seen, invoke onFalse events
                onFalse.Invoke();
                //Debug.Log("invisible");
            }
        }
        wasVisible = isVisible; //sync the cache thing
    }
}
