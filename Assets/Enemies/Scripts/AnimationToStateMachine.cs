using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    //Attack State Reference
    public AttackState attackState;

    //Funtions
    //Function TriggerAttack
    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }
    //Function FinishAttack
    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
}
