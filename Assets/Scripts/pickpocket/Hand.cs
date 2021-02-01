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
        /* Vector2 endPos = endOfArms.position;
        Vector2 dist = (pos - endPos);
        Vector2 finalPos = new Vector2();
        float sinMaxX = Mathf.Cos(Vector2.Angle(Vector2.right,pos)) * maxArmDistance;
        float sinMaxY = Mathf.Sin(Vector2.Angle(Vector2.right,pos)) * maxArmDistance;
        Debug.Log(dist + ", " + sinMaxX + ", " + sinMaxY);
        finalPos.x = Mathf.Clamp(dist.x+Camera.main.rect.xMin,0,sinMaxX);
        finalPos.y = Mathf.Clamp(dist.y-Camera.main.rect.yMax,0,sinMaxY); */
        rb.position = pos;
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
        Debug.Log(input.Get<float>());
        if (input.Get<float>() > 0f){
            if(grabbable != null){
                grabbed = true;
                Debug.Log("Grabbed");
                GetComponent<SpriteRenderer>().sprite = closeHand;
                GetComponent<RelativeJoint2D>().enabled = true;
                GetComponent<RelativeJoint2D>().connectedBody = grabbable.GetComponent<Rigidbody2D>();
            }
        }else{
            if(grabbed){
                grabbed = false;
                Debug.Log("Let go");
                GetComponent<SpriteRenderer>().sprite = openHand;
                GetComponent<RelativeJoint2D>().connectedBody = null;
                GetComponent<RelativeJoint2D>().enabled = false;
            }
        }
    }
}
