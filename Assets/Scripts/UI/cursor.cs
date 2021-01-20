using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class cursor : MonoBehaviour
{
    public Texture2D neutral;
    public Texture2D canClick;
    public Texture2D click;
    Texture2D active;
    Vector2 cursorOffset;
    Texture2D cache;
    bool waiting = false;

    void Start(){
    active = neutral;
    //set the cursor origin to its centre. (default is upper left corner)
      cursorOffset = new Vector2(active.width/2, active.height/2);
     
      //Sets the cursor to the Crosshair sprite with given offset 
      //and automatic switching to hardware default if necessary
      Cursor.SetCursor(active, cursorOffset, CursorMode.Auto);
  }

  public void OnMousePos(InputValue input){
    if (waiting){
        return;
    }

    Vector3 mousePos2D = Camera.main.ScreenToWorldPoint(input.Get<Vector2>()); //gets mouse point in world space
    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity ,LayerMask.GetMask("Clickable")); //raycast to find clicked object. Only sees Clickable layer
    if (hit.collider != null){
        active = canClick;
    }else{
        active = neutral;
    }

    if (active != cache){
        Cursor.SetCursor(active, cursorOffset, CursorMode.Auto);
        cache = active;
    }
  }

  public void OnMove(InputValue input){
        active = click;
        Cursor.SetCursor(active, cursorOffset, CursorMode.Auto);
        StartCoroutine(wait());
  }

  IEnumerator wait(){
        waiting = true;
        yield return new WaitForSeconds(.2f);
        active = canClick;
        Cursor.SetCursor(active, cursorOffset, CursorMode.Auto);
        waiting = false;
  }
}
