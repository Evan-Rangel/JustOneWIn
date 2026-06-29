using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class DoorsManager : MonoBehaviour
    {
        [field: SerializeField] public Door[] doors{ get; private set; }
        private void Awake()
        {
            GameManager.instance.currentDoorManager = this; 
        }
    }
}
