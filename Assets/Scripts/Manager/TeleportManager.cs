using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class TeleportManager : MonoBehaviour
    {
        [field: SerializeField] public TeleportController[] allSavePoints { get; private set; }
        private void Start()
        {
            allSavePoints=GetComponentsInChildren<TeleportController>();
            //CanvasManager.instance.SetMinimapTeleports(this);
            GameManager.instance.CheckForSavePoints(this);
        }
    }
}
