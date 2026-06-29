using Avocado.CoreSystem;
using UnityEngine;

namespace Avocado
{
    public class GrabWorldItem : MonoBehaviour
    {
        Bobber bobber;

        private void Start()
        {
            if (SaveManager.GetGrabUnlocked())
                Destroy(gameObject);
            GetComponentInChildren<Bobber>().StartBobbing();

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.transform.GetComponentInChildren<Core>().grabUnlocked = true;
                SaveManager.SaveGrabUnlocked(true);
                CanvasManager.instance.ActiveAbilityHolder("climb");

                Destroy(gameObject);
            }
        }
    }
}
