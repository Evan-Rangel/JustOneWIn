using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class HealthRecoverManager : MonoBehaviour
    {
        [SerializeField] Sprite fullChargeSprite, emptyChargeSprite;
        [SerializeField] Image[] charges;
        public static HealthRecoverManager instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
    }
}
