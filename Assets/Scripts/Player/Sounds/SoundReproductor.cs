
using System;
using UnityEngine;


public class SoundReproductor : MonoBehaviour
{
    event Action<string, Vector3> playSound;

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
        playSound?.Invoke("Player_Jump", transform.position);
    }
    public void PlayLandSound()
    {
        playSound?.Invoke("Player_Land", transform.position);
    }
    public void PlayDashSound()
    {
        playSound?.Invoke("Player_Dash", transform.position);

    }
    public void PlayWalkSound()
    {
        playSound?.Invoke("Player_Walk", transform.position);

    }

    public void PlayClimbSound()
    {
        playSound?.Invoke("Player_Climb", transform.position);

    }
    public void PlayDeathSound()
    {
        playSound?.Invoke("Player_Death", transform.position);

    }
    public void PlayDamageSound()
    {
        playSound?.Invoke("Player_Damage", transform.position);

    }
}
