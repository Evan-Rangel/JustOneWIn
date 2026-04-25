using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class FabricSearcherDespawner : MonoBehaviour
    {
        Animator anim;
        GameObject currentSearcher;
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        public void OnDoorOpen()
        {
            if (currentSearcher == null)
                return;

            currentSearcher.SetActive(false);
            currentSearcher = null;
        }
        public void SearcherReachedDespawner(GameObject _searcher)
        {
            anim.SetTrigger("Open");
            currentSearcher = _searcher;
        }
    
    }
}
