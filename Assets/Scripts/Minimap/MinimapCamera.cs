using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class MinimapCamera : MonoBehaviour
    {
        [SerializeField] Transform _camera;
        [SerializeField]Transform[] _mapObjects;
        private void Update()
        {
            transform.position = new Vector3(_camera.position.x, _camera.position.y, 0);
            foreach (Transform item in _mapObjects)
            {
                if (Vector2.Distance(transform.position, item.position) < 10 && item.localScale == new Vector3(5, 5, 1))
                    item.localScale = new Vector3(10, 10, 1);
               if (Vector2.Distance(transform.position, item.position) >= 10 && item.localScale == new Vector3(10, 10, 1))
                    item.localScale = new Vector3(5, 5, 1);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Colision");
            if (collision.CompareTag("MapObject"))
            {
                Debug.Log("Enter");
            }
        }

    }
}
