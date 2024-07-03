using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeItem : Item, ItemAction
{

    public void Action(Transform _player)
    {
        Debug.Log("Freeze Item");
    }
}
