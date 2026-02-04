using UnityEngine;

namespace Avocado
{
    public class FabricMinibossController : MonoBehaviour
    {
        Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
    }
}
