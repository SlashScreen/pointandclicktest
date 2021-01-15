using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RenderCamClickThrough : MonoBehaviour
{
    public Camera portalCamera;
    public GameObject portal;
    public GraphicRaycaster gRay;
    EventSystem eventSystem;
    Vector2 mouseP;

    public void OnMousePos(InputValue input){
        mouseP = input.Get<Vector2>();
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
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
                
                PointerEventData m_PointerEventData = new PointerEventData(eventSystem);
                m_PointerEventData.position = new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight);
                List<RaycastResult> results = new List<RaycastResult>();
                gRay.Raycast(m_PointerEventData, results);

                // test these camera coordinates in another raycast test
                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                    if (result.gameObject.GetComponent<Button>() != null){
                        result.gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                }

                if(portalHit.collider != null){
                    Debug.Log("hit something!");
                    Debug.Log(portalHit.collider.gameObject);
                    //copy n pasted sum sh*t from stack overflow
                    
                }
            } 
        }
    }
}
