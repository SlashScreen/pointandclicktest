using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    public float maxArmDistance = 11f;
    public Transform endOfArms;
    public Camera portalCamera;
    public GameObject portal;
    public Sprite openHand;
    public Sprite closeHand;
    public FixedJoint2D fixedMouse;
    public RelativeJoint2D fixedGrab;
    Vector2 pos;
    Rigidbody2D rb;
    GameObject grabbable;
    bool grabbed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnMousePos(InputValue input){

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(input.Get<Vector2>());
        
        // do we hit our portal plane?
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.collider.gameObject.name + ", " + portal.name);
            if (hit.collider.gameObject == portal){

                var localPoint = hit.textureCoord;
                Debug.Log(hit.textureCoord);
                // convert the hit texture coordinates into camera coordinates
                //portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight))
                //RaycastHit2D portalHit = Physics2D.Raycast(portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight)), Vector2.zero, Mathf.Infinity);
                pos = portalCamera.ScreenToWorldPoint(new Vector2(localPoint.x * portalCamera.pixelWidth, localPoint.y * portalCamera.pixelHeight));
            }
        }
    }

    private void FixedUpdate()
    {
        fixedMouse.connectedAnchor = pos;
        //rb.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Grabbable"){
            grabbable = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Grabbable"){
            grabbable = null;
        }
    }

    void OnPickUp(InputValue input){
        //Debug.Log(input.Get<float>());
        if (input.Get<float>() > 0f){ //if clicked
        GetComponent<SpriteRenderer>().sprite = closeHand;
            if(grabbable != null){
                grabbed = true;
                Debug.Log("Grabbed");
                fixedGrab.enabled = true;
                fixedGrab.connectedBody = grabbable.GetComponent<Rigidbody2D>();
            }
        }else{
            GetComponent<SpriteRenderer>().sprite = openHand;
            if(grabbed){
                grabbed = false;
                Debug.Log("Let go");
                fixedGrab.connectedBody = null;
                fixedGrab.enabled = false;
            }
        }
    }
}
