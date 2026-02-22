
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
        AudioManager.instance.PlaySFXSound("Player_Jump", transform.position);
        // AudioManager.instance.PlayOneShotSFX(jumpSound[Random.Range(0, jumpSound.Length)]);
    }
    public void PlayLandSound()
    {
        AudioManager.instance.PlaySFXSound("Player_Land", transform.position);

        // AudioManager.instance.PlayOneShotSFX(landSound[Random.Range(0,landSound.Length)]);
    }
    public void PlayDashSound()
    {
        AudioManager.instance.PlaySFXSound("Player_Dash", transform.position);

        // AudioManager.instance.PlayOneShotSFX(dashSound[Random.Range(0, dashSound.Length)]);
    }
    public void PlayWalkSound()
    {
        AudioManager.instance.PlaySFXSound("Player_Walk", transform.position);

        // AudioManager.instance.PlayOneShotSFX(walkSound[Random.Range(0, walkSound.Length)]);
    }

    public void PlayClimbSound()
    {
        AudioManager.instance.PlaySFXSound("Player_Climb", transform.position);

        // AudioManager.instance.PlayOneShotSFX(climbSound[Random.Range(0, climbSound.Length)]);
    }
    public void PlayDeathSound()
    {
        AudioManager.instance.PlaySFXSound("Player_Death", transform.position);

        // AudioManager.instance.PlayOneShotSFX(deathSound[Random.Range(0, deathSound.Length)]);
    }
    public void PlayDamageSound()
    {
        AudioManager.instance.PlaySFXSound("Player_Damage", transform.position);

        // AudioManager.instance.PlayOneShotSFX(damageSound[Random.Range(0, damageSound.Length)]);
    }
}
