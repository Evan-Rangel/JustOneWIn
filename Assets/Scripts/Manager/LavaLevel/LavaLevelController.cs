using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class LavaLevelController : MonoBehaviour
    {
        [SerializeField] float speed;
        [field:SerializeField] public bool IsRising { get; private set; }
        private void OnEnable()
        {
            Invoke(nameof(StartRising), 4f);
        }
        public void ResetPosition(Vector2 position)
        {
            IsRising = false;
            transform.position = position+Vector2.down*10;
            Invoke(nameof(StartRising), 4f);
        }
        public void StartRising()
        {
            IsRising = true;
        }

        private void Update()
        {
            if (IsRising)
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }
}
