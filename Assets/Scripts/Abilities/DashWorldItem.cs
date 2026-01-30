using Avocado.CoreSystem;
using UnityEngine;

namespace Avocado
{
    public class DashWorldItem : MonoBehaviour
    {
        Bobber bobber;

        
        private void Start()
        {
            if (SaveManager.GetDashUnlocked())
                Destroy(gameObject);
            GetComponentInChildren<Bobber>().StartBobbing();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.transform.GetComponentInChildren<Core>().dashUnlocked = true;
                SaveManager.SaveDashUnlocked(true);
                GameManager.instance.ActiveAbilityHolder("dash");

                Destroy(gameObject);
            }
        }
    }
}
