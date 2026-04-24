using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class LaserManager : MonoBehaviour
    {
        [SerializeField] LasersNode[] lasersStartNodes;

        private void Start()
        {
        Invoke("StartLasers", 1);
        }
        public void StartLasers()
        {
            for (int i = 0; i < lasersStartNodes.Length; i++)
            {
                lasersStartNodes[i].ActivateLasers();
            }
        }
    }
}
