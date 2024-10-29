using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockBackable
{
    #region Interface Functions
    void KnockBack(Vector2 angle, float strength, int direction);
    #endregion
}
