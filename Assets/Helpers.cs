using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers 
{
    //SCRIPT de funciones generales

    static WaitForEndOfFrame waitForEndFrame;

    public static WaitForEndOfFrame GetWaitForEndOfFrame()
    { 
        return (waitForEndFrame!=null) ? waitForEndFrame : waitForEndFrame= new WaitForEndOfFrame();
    }



    static Dictionary<float, WaitForSeconds> waitDictionary= new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out var wait))   return wait;
        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }
}
