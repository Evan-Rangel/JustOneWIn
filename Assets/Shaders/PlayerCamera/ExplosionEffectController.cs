using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class ExplosionEffectController : MonoBehaviour
    {
        public float timeValue;//{ private set;  get; }
        [SerializeField, Range(0, 20)] float timeMax;
        [SerializeField, Range(0, 100)] float effectSpeed;
       
        private void OnEnable()
        {
            PlayerCamera_S.Instance.AddToPool(this);
            timeValue = 0;
        }
        private void OnDisable()
        {
            PlayerCamera_S.Instance.RemoveFromPool(this);
        }

        private void Update()
        {
            if (timeValue >= timeMax)
            {
                gameObject.SetActive(false);
            }
            timeValue += Time.deltaTime* effectSpeed;
            return;
        }
      
    }
}
