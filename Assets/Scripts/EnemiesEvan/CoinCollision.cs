using UnityEngine;

namespace Avocado
{
    public class CoinCollision : MonoBehaviour
    {
        [SerializeField] AudioClip[] coinSound;
        [SerializeField] AudioClip[] coinFloor;
        int value=1;        
        
        private void OnEnable()
        {
            value = 1;
            transform.root.localScale = Vector3.one;

            if (Random.Range(0, 10) > 8)
            {
                value = 5;
                transform.root.localScale = Vector3.one*2;
            }

            Invoke("DisableByTime", 45);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                AudioManager.instance.PlaySFXSound("CoinHitFloor", transform.position);
                return;
            }
            GameManager.instance.AddCoin(value);
                AudioManager.instance.PlaySFXSound("CoinSound", transform.position);

            transform.root.gameObject.SetActive(false);
        }
        void DisableByTime()
        { 
            transform.root.gameObject.SetActive(false);
        }
    }
}
