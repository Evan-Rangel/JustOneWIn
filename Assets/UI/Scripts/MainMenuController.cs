using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Avocado
{
    public class MainMenuController : MonoBehaviour
    {
        Animator animator;
        [SerializeField]GameObject menuPanel;
        private void Awake()
        {
            
            animator = GetComponent<Animator>();
        }
        public void EndIntro()
        { 
        menuPanel.SetActive(true);
            
        }
        
    }
}
