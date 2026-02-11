using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricBossActivator : MonoBehaviour
    {
        [SerializeField] GameObject boss;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                boss.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
