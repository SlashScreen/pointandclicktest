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
    public Vector2 mouseProjectedPos;
    public bool calcOnMove = false;
    EventSystem eventSystem;
    Vector2 mouseP;
    private void Start()
    {
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }

    public void OnMousePos(InputValue input){
        mouseP = input.Get<Vector2>();
        if (calcOnMove){
            //copied from below for the sattelite game. Normally off to save a little preformance
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseP);

            if (Physics.Raycast(ray, out hit)) {
            //Debug.Log(hit.collider.gameObject.name + ", " + portal.name);
            if (hit.collider.gameObject == portal){

                var localPoint = hit.textureCoord;
                //Debug.Log(hit.textureCoord);
                // convert the hit texture coordinates into camera coordinates
                //portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight))
                RaycastHit2D portalHit = Physics2D.Raycast(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)), Vector2.zero, Mathf.Infinity);
                //Debug.DrawRay(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)),Vector2.zero, Color.red, duration: 5f);
                //Debug.Log(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)));
                mouseProjectedPos = portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight));
            }
            }

        }
    }

    public void OnMove(InputValue input){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouseP);
        
        // do we hit our portal plane?
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.collider.gameObject.name + ", " + portal.name);
            if (hit.collider.gameObject == portal){

                var localPoint = hit.textureCoord;
                //Debug.Log(hit.textureCoord);
                // convert the hit texture coordinates into camera coordinates
                //portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight))
                RaycastHit2D portalHit = Physics2D.Raycast(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)), Vector2.zero, Mathf.Infinity);
                //Debug.DrawRay(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)),Vector2.zero, Color.red, duration: 5f);
                //Debug.Log(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)));
                mouseProjectedPos = portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight));

                PointerEventData m_PointerEventData = new PointerEventData(eventSystem);
                m_PointerEventData.position = new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight);
                List<RaycastResult> results = new List<RaycastResult>();
                gRay.Raycast(m_PointerEventData, results);

                //Debug.Log(portalHit.collider.gameObject.name);
                // test these camera coordinates in another raycast test
                foreach (RaycastResult result in results)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                    Debug.Log(result.gameObject.GetComponent<Button>());
                    if (result.gameObject.GetComponent<Button>() != null){
                        result.gameObject.GetComponent<Button>().onClick.Invoke();
                        Debug.Log("invoked");
                    }
                    if (result.gameObject.GetComponent<clickling>() != null){
                        result.gameObject.GetComponent<clickling>().Invoke();
                        Debug.Log("invoked");
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
