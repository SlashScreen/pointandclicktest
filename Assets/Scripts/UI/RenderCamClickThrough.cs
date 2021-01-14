using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RenderCamClickThrough : MonoBehaviour
{
    public Camera portalCamera;
    public GameObject portal;

    public void OnMove(InputValue input){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         
        // do we hit our portal plane?
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject == portal){
                var localPoint = hit.textureCoord;
                // convert the hit texture coordinates into camera coordinates
                Ray portalRay = portalCamera.ScreenPointToRay(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight));
                RaycastHit portalHit;
                // test these camera coordinates in another raycast test
                if(Physics.Raycast(portalRay, out portalHit)){
                    Debug.Log(portalHit.collider.gameObject);
                }
            } 
        }
    }
}
