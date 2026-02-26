
using System;
using UnityEngine;


public class SoundReproductor : MonoBehaviour
{
    event Action<string, Transform> playSound;

    private void OnEnable()
    {
        playSound += AudioManager.instance.PlaySFXSound;
    }

    private void OnDisable()
    {
        playSound -= AudioManager.instance.PlaySFXSound;
    }
    public void PlayJumpSound()
    {
        playSound?.Invoke("Player_Jump", transform);
    }
    public void PlayLandSound()
    {
        playSound?.Invoke("Player_Land", transform);
    }
    public void PlayDashSound()
    {
        playSound?.Invoke("Player_Dash", transform);

    }
    public void PlayWalkSound()
    {
        playSound?.Invoke("Player_Walk", transform);

    }

    public void PlayClimbSound()
    {
        playSound?.Invoke("Player_Climb", transform);
            
    }
    public void PlayDeathSound()
    {
        playSound?.Invoke("Player_Death", transform);

    }
    public void PlayDamageSound()
    {
        playSound?.Invoke("Player_Damage", transform);

    }
}
