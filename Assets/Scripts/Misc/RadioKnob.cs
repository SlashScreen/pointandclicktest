using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioKnob : MonoBehaviour
{
    [System.Serializable]
    public struct station{
        public float frequency;
        public string node;
        public AudioClip chatter;
        public float listenTime;
        public bool looping;
    }

    public station[] stations;
    public AudioSource audioSource;
    public UnityEngine.UI.Text radiotext;
    PlayerMain main;
    Vector2 mPos;
    public float angle;
    float previousChannel;
    float timer = 0f;
    float rounded;
    float roun;
    bool onAStation;
    float timerTarget;
    string nodeToPlay;
    float mouseVal;
    float chann;
    RadioAudioEngine engine;

    private void Start()
    {
        main = GameObject.Find("Player").GetComponent<PlayerMain>();
        engine = GetComponent<RadioAudioEngine>();
        //engine.EngineStart();
    }

    public void OnMousePos(InputValue input){
        mPos = Camera.main.ScreenToWorldPoint(input.Get<Vector2>());
    }

    public void OnMouseHold(InputValue input){
        if (input.Get<float>() == 1f && GetComponent<Collider2D>().ClosestPoint(mPos) == mPos){ //if not clicking the knob
            mouseVal = input.Get<float>();
        }else if(input.Get<float>() == 0f){
            mouseVal = input.Get<float>();
        }else{
            return;
        }
    }

    private void Update()
    {
        

        if (main.yarn.inConversation){ //if Zero is talking, don't do anything
            return;
        }

        //this script is a disaster but it works
        if (mouseVal > 0f){
            angle = Vector2.SignedAngle(Vector2.right,(mPos - new Vector2 (transform.position.x,transform.position.y))); //get angle to mouse pos
            roun = Mathf.Round(angle*10)/10; //round to first digit
            float rounded;
            if (roun < 0){ //this maps the signed angles to 360
                rounded = 360 + roun;
            }else{
                rounded = roun;
            }

            rounded = Mathf.Round(rounded*10)/10; //round to first digit again
            chann = (Mathf.Round(Mathf.Lerp(80f,100f,(rounded/360))*10)/10); //calculate the channel
            
            radiotext.text = chann.ToString() + " kHz"; //display

            transform.Rotate(new Vector3(0,0,roun-previousChannel)); //rotate to mouse cursor

            foreach(station st in stations){
                if(st.frequency == chann){
                    onAStation = true;
                    timerTarget = st.listenTime; //set the timer target to match this channel
                    nodeToPlay = st.node; //set node to start

                    if (previousChannel != rounded){
                        previousChannel = roun;
                        audioSource.clip = st.chatter;
                        audioSource.loop = st.looping;
                        audioSource.Play();
                        Debug.Log("playing audio");
                    }
                    //NTS: Add like a volume monitor thing for deaf players

                    

                    return;
                }
            }
            audioSource.Stop();
            onAStation = false;
            previousChannel = roun;

            
            //todo play generic noise. generated? or random pitch of a constant frequency
            //todo: be able to skip over a frequency accidentally? only go 
        }

        //count up timer if player stays on channel
        if (onAStation && previousChannel == roun){
            timer += Time.deltaTime;
        }else{
            timer = 0f;
        }
        //if timer done, play the node
        //Debug.Log(timer +", "+timerTarget);
        if (onAStation && timer >= timerTarget){
            //Debug.Log(timer +","+timerTarget);
            main.d.dia.StartDialogue(nodeToPlay);
            onAStation = false; //should hopefully stop the timer 
        }
        //Chuck noise
        engine.updateChannel(chann,!onAStation);

    }
}
