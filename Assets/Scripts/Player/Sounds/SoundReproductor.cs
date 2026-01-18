
using UnityEngine;


public class SoundReproductor : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] jumpSound;
    [SerializeField] private AudioClip[] landSound;
    [SerializeField] private AudioClip[] dashSound;
    [SerializeField] private AudioClip[] walkSound;
    [SerializeField] private AudioClip[] climbSound;
    [SerializeField] private AudioClip[] deathSound;
    [SerializeField] private AudioClip[] damageSound;
    public void PlayJumpSound()
    {
        AudioManager.instance.PlayOneShotSFX(jumpSound[Random.Range(0, jumpSound.Length)]);
    }
    public void PlayLandSound()
    {
        AudioManager.instance.PlayOneShotSFX(landSound[Random.Range(0,landSound.Length)]);
    }
    public void PlayDashSound()
    {
        AudioManager.instance.PlayOneShotSFX(dashSound[Random.Range(0, dashSound.Length)]);
    }
    public void PlayWalkSound()
    {
        AudioManager.instance.PlayOneShotSFX(walkSound[Random.Range(0, walkSound.Length)]);
    }
  
    public void PlayClimbSound()
    {
        AudioManager.instance.PlayOneShotSFX(climbSound[Random.Range(0, climbSound.Length)]);
    }
    public void PlayDeathSound()
    {
        AudioManager.instance.PlayOneShotSFX(deathSound[Random.Range(0, deathSound.Length)]);
    }
    public void PlayDamageSound()
    {
        AudioManager.instance.PlayOneShotSFX(damageSound[Random.Range(0, damageSound.Length)]);
    }
}
