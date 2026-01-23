using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class InteractEventManager : MonoBehaviour
    {
        [SerializeField] GameObject textHolder;
        GameObject player;
        [SerializeField] string textToShow;
        private void Awake()
        {
            textHolder = GameObject.FindGameObjectWithTag("PopOutCanvas");
        }
        public virtual void Start()
        {
            textHolder.SetActive(false);
        }
        public  void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                player = collision.gameObject;
                textHolder.SetActive(true);
                textHolder.transform.position = transform.position + new Vector3(0, 1.5f, 0);
                player.GetComponent<PlayerInputHandler>().OnInteractEventInputChanged += OnInteractEvent;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                textHolder.SetActive(false);
                player.GetComponent<PlayerInputHandler>().OnInteractEventInputChanged -= OnInteractEvent;
                player = null;  
            }
        }
        public virtual void OnInteractEvent()
        {
            textHolder.SetActive(false);
        }
    }
}
