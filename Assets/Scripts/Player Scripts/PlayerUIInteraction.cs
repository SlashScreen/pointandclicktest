using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerUIInteraction : MonoBehaviour
{
    public Vector3 UIPosition; //options wheel position
    optionspanel opt;
    public GameObject clickedObject = null; //an interactive object that is clicked
    public bool inMenu = false;

    PlayerMain main;
    InGameDropdown dropdown;

        //Explanation for clickedObject:
    //if an object that is clickable is clicked
    //store a reference to the object
    //set goingToObject to be true
    //then generate a path and walk to the object, and ensure the player cannot click away
    //if at the object, only then start dialog and reset everything
    
    private void Start() //get components 
    {
        main = GetComponent<PlayerMain>();
        dropdown = GameObject.Find("Menu").GetComponent<InGameDropdown>();
        opt = GameObject.Find("optionspanel").GetComponent<optionspanel>();
    }

    public void OnMove(InputValue input){
        if (main.yarn.inConversation || main.control.goingToTalkPoint || inMenu){
            //nothing should happen if youre in a conversation or in a menu
            return;
        }

        if(!main.control.goingToObject) //when clicked and not actively going to an object
        {
            UIPosition = main.control.mouseP; //set point where mouse clicked in world space
            Vector3 mousePos2D = Camera.main.ScreenToWorldPoint(main.control.mouseP); //gets mouse point in world space
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity ,LayerMask.GetMask("Clickable")); //raycast to find clicked object. Only sees Clickable layer

            //mousePos2D.z = GameObject.FindGameObjectWithTag("BG").transform.position.z - Camera.main.transform.position.z;
            //Ray ray = Camera.main.ScreenPointToRay(mousePos2D);
            //RaycastHit hit;


            if (EventSystem.current.IsPointerOverGameObject()){
                //if any UI clicked, exit out of script
                return;
            }

            if (hit.collider != null) { //if something is clicked on layer clickable
                Debug.Log(hit.collider.gameObject.tag);
                

                if (hit.collider.gameObject.tag == "Minigame"){ //if minigame
                    Debug.Log("Hit minigame surface");
                    return;
                }
                
                if(hit.collider.gameObject.tag == "Clickable" || hit.collider.gameObject.tag == "Wall"){ //if clicked object has tag "Clickable" or "Wall"
                    //"Clickable" *tag* is interactive objects, thats not totally clear

                    if (hit.collider.gameObject.GetComponent<InteractiveObject>() || hit.collider.gameObject.GetComponent<NPCscript>()){ 
                        //If it is an interactive object or NPC
                        clickedObject = hit.collider.gameObject; //set clicked object

                        if(clickedObject.GetComponent<InteractiveObject>()){ //gets talk points (points in space that the player goes to to interact with object)
                            if (clickedObject.GetComponent<InteractiveObject>().talkPoint != null){ //if it has a talk point
                                main.control.targetPosition = clickedObject.GetComponent<InteractiveObject>().talkPoint.position;
                            }else{
                                main.control.targetPosition = transform.position;
                            }
                        }else{
                            if (clickedObject.GetComponent<NPCscript>().talkPoint != null){ //if it has a talk point
                                main.control.targetPosition = clickedObject.GetComponent<NPCscript>().talkPoint.position;
                            }else{
                                main.control.targetPosition = transform.position;
                            }
                        }

                        if (main.d.gameObject.GetComponent<Yarn.VariableStorage>().GetValue("$selectedInventory").AsNumber != 0){ //if item in hand from inventory
                            //Start use script once movement complete
                            StartCoroutine(main.control.MovePlayer(new string[] {main.control.targetPosition.x.ToString(),main.control.targetPosition.y.ToString(),main.control.targetPosition.z.ToString()},main.yarn.StartUseDialog));
                    
                        }else{//if no item in hand, set up wheel
                            opt.setPosition(UIPosition); //teleport wheel
                            if(clickedObject.GetComponent<InteractiveObject>()){
                                opt.setButtons(clickedObject.GetComponent<InteractiveObject>()); //init wheel for Object. NTS: Could make as one function using a generic?...
                            }else{
                                opt.setButtons_NPC(clickedObject.GetComponent<NPCscript>()); //init wheel for NPC
                            }
                            opt.Show(); //show wheel
                        }
                    }

                }else if (hit.collider.gameObject.tag == "Player" && main.d.gameObject.GetComponent<Yarn.VariableStorage>().GetValue("$selectedInventory").AsNumber != 0){  //if clicked self and something in hand
                    main.d.dia.StartDialogue("player.use"); //start use dialogue. yes it's hardcoded but i guess it doesnt matter         
                }else{
                    WrapUpMouseClick();
                }
            }else{
                //mmm yes I know its the same code
                WrapUpMouseClick();
            }
        }
    }

    void WrapUpMouseClick(){ //find path when not interactive item clicked
        main.d.gameObject.GetComponent<Yarn.VariableStorage>().SetValue("$selectedInventory",0); //reset hand
        if (opt.hidden && dropdown.hidden){
            main.control.targetPosition = Camera.main.ScreenToWorldPoint(main.control.mouseP); //set point where mouse clicked in world space
            main.control.GenPath(main.control.targetPosition); //generate path to mouse point if ground is clicked
        }else{
            opt.Hide();
            dropdown.Cancel();
        }
    }
}
