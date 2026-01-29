using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class OnEnableFirstButton : MonoBehaviour
    {
        Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }
        private void OnEnable()
        {
            //button.Select();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(gameObject); 
            Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        }
       
    }
}
