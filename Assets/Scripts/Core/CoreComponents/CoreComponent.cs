using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.CoreSystem
{
    public class CoreComponent : MonoBehaviour, ILogicUpdate
    {
        #region References
        protected Core core;
        #endregion

        #region Integers
        #endregion

        #region Floats
        #endregion

        #region Flags
        #endregion

        #region Components
        #endregion

        #region Transforms
        #endregion

        #region Vectors
        #endregion

        #region Unity CallBack Functions
        protected virtual void Awake()
        {
            //Find Core Script
            core = transform.parent.GetComponent<Core>();
            if (core == null) { Debug.LogError("There is no Core on parent"); }//Condition to Warning
            core.AddComponent(this);
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

        #region Interfaces Functions
        public virtual void LogicUpdate()
        {

        }
        #endregion
    }
}

