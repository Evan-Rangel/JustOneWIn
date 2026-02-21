using Avocado.Combat.Damage;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject hitParticles;
    private Animator anim;
    [SerializeField] int hits;
    public void Damage(DamageData data)
    {
        hits--;
        Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        anim.SetTrigger("damage");
    }
    void CheckHits()
    {
        if (hits > 0) return;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
