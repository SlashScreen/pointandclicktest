using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRing : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Finished");
        Destroy(animator.gameObject);
    }
}
