using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : StateMachineBehaviour
{

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool("walking")){
            string node = "N";
            switch(animator.GetInteger("direction")){ //play node based on direction
                case 0: //redundant but whatever
                    node = "N";
                    break;
                case 1:
                    node = "NE";
                    break;
                case 2:
                    node = "E";
                    break;
                case 3:
                    node = "SE";
                    break;
                case 4:
                    node = "S";
                    break;
                case 5:
                    node = "SW";
                    break;
                case 6:
                    node = "W";
                    break;
                case 7:
                    node = "NW";
                    break;
            }

            animator.Play("Base Layer." + node); //play node
        }
    }
}
