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
        Bloom bloom;


        private void Awake()
        {
            instance = this;

            vol = GetComponent<Volume>();
            chromaticAberration=vol.profile.TryGet<ChromaticAberration>(out var ca) ? ca : null;
        }
        void test()
        { 
        }
    }
}
