using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class AudioRepr : MonoBehaviour
    {
        public AudioSource audioSource; // Arrastra aquí un AudioSource desde el inspector
        public AudioClip soundClip; // Arrastra aquí el clip de sonido

        public void PlaySound()
        {
            if (audioSource != null && soundClip != null)
            {
                audioSource.PlayOneShot(soundClip);
            }
            else
            {
                Debug.LogWarning("AudioSource o AudioClip no asignado.");
            }
        }
    }
}
