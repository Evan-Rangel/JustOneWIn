using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    //---AnimationToStateMachine Vars---//
    #region References
    public AttackState attackState;
    #endregion

    #region Other Functions
    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }
    #endregion
}
