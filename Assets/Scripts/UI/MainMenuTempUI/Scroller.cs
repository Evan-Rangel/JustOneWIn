using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class Scroller : MonoBehaviour
    {
        [SerializeField] private Renderer _img;
        [SerializeField] private float speed;

        private void Update()
        {
            _img.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
        }
    }
}
