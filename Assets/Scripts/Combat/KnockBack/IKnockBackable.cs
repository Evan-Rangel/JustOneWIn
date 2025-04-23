using System.Collections;
using System.Collections.Generic;
using Avocado.Combat.KnockBack;
using UnityEngine;

public interface IKnockBackable
{
    void KnockBack(KnockBackData data);
}