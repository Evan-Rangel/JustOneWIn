using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Avocado
{
    public class GlobalVolumeController : MonoBehaviour
    {
       Volume vol;
        public static GlobalVolumeController instance;
        ChromaticAberration chromaticAberration;
        [SerializeField] float chromaticAberrationMinValue;
        Bloom bloom;


        private void Awake()
        {
            instance = this;

            vol = GetComponent<Volume>();
            chromaticAberration=vol.profile.TryGet<ChromaticAberration>(out var ca) ? ca : null;
        }
      
        private void Update()
        {
            if (chromaticAberration != null&& chromaticAberration.intensity.value> chromaticAberrationMinValue)
            {  
                chromaticAberration.intensity.value -= Time.deltaTime*0.3f;
                
            }

        }
       public void SetChromaticAberration(float _value)
        {
            if (chromaticAberration != null)
            {
                chromaticAberration.intensity.value = _value;
            }
        }
    }
}
