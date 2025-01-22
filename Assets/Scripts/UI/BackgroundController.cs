using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class BackgroundController : MonoBehaviour
    {
        private float startPos, length;
        public GameObject cam;
        public float parallaxEffect;

        private void Start()
        {
            // Posición inicial y longitud del fondo
            startPos = transform.position.x;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {
            // Calcular la distancia de parallax en función de la posición de la cámara
            float distance = (cam.transform.position.x * parallaxEffect);

            // Actualizar la posición con un movimiento interpolado
            float targetPos = startPos + distance;
            transform.position = new Vector3(targetPos, transform.position.y, transform.position.z);

            // Reposicionar el fondo para mantener el bucle continuo
            float camOffset = cam.transform.position.x;
            if (camOffset > startPos + length)
            {
                startPos += length;
            }
            else if (camOffset < startPos - length)
            {
                startPos -= length;
            }
        }

    }
}
