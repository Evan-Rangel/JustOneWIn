using UnityEngine;

namespace Avocado
{
    public class HookWorldItem : MonoBehaviour
    {
        Bobber bobber;
        private void Start()
        {
            if (SaveManager.GetGrappleUnlocked())
                Destroy(gameObject);
            GetComponentInChildren<Bobber>().StartBobbing();

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.transform.GetComponent<GrappleHandler>().enabled = true;
                SaveManager.SaveGrappleUnlocked(true);
                CanvasManager.instance.ActiveAbilityHolder("hook");
                Destroy(gameObject);
            }
        }
    }
}
