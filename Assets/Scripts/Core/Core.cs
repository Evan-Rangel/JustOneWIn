using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Avocado.CoreSystem
{
    public class Core : MonoBehaviour
    {
        #region States


        private readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();
        #endregion

        #region Unity CallBack Functions
        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {

        }
        #endregion

        #region Get Own Functions
        public void LogicUpdate()
        {
            foreach (CoreComponent component in CoreComponents)
            {
                component.LogicUpdate();
            }
        }

        public void AddComponent(CoreComponent component)
        {
            if (!CoreComponents.Contains(component))
            {
                CoreComponents.Add(component);
            }
        }

        public T GetCoreComponent<T>() where T : CoreComponent
        {
            var comp = CoreComponents.OfType<T>().FirstOrDefault();

            if (comp)
            {
                return comp;
            }

            comp = GetComponentInChildren<T>();

            if (comp)
            {
                return comp;
            }

            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");

            return null;
        }

        public T GetCoreComponent<T>(ref T value) where T : CoreComponent
        {
            value = GetCoreComponent<T>();
            return value;
        }
        #endregion
    }
}

