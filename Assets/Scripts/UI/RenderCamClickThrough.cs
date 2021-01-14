using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RenderCamClickThrough : MonoBehaviour
{
    public Camera portalCamera;
    public GameObject portal;
    Vector2 mouseP;

    public void OnMousePos(InputValue input){
        mouseP = input.Get<Vector2>();
    }

    public void OnMove(InputValue input){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouseP);
        
        // do we hit our portal plane?
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.collider.gameObject.name + ", " + portal.name);
            if (hit.collider.gameObject == portal){
                var localPoint = hit.textureCoord;
                Debug.Log(hit.textureCoord);
                // convert the hit texture coordinates into camera coordinates
                //portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight))
                RaycastHit2D portalHit = Physics2D.Raycast(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)), Vector2.zero, Mathf.Infinity);
                // test these camera coordinates in another raycast test
                if(portalHit.collider != null){
                    Debug.Log("hit something!");
                    Debug.Log(portalHit.collider.gameObject);
                }
            } 
        }
    }
}
