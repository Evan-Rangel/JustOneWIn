using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class TeleportController : MonoBehaviour
    {
        MinimapCamera minimapCamera;
        bool isHighlightedOnMap = false;
        [field: SerializeField] public TpEntity shopID { get; private set; }
        public event Action<TpEntity> onSavePointActivated;
        SpriteRenderer spr;
        private void Awake()
        {
            spr=GetComponent<SpriteRenderer>(); 
        }
        // Start is called before the first frame update
        void Start()
        {
            minimapCamera = MinimapCamera.Instance;

        }
        public void CheckForVisibility()
        {
            if (SaveManager.IsZoneUnlocked(shopID.zoneName))
                spr.enabled = true;

        }
        // Update is called once per frame
        void Update()
        {
            if (!minimapCamera.isActiveAndEnabled)
                return;
            if (Vector2.Distance(transform.position, minimapCamera.transform.position) < 10 && !isHighlightedOnMap)
                ActivePointOnMap();
            if (Vector2.Distance(transform.position, minimapCamera.transform.position) >= 10 && isHighlightedOnMap)
                DisablePointOnMap();
        }
        private void OnEnable()
        {
            Invoke("DelayOnEnable", 0.1f);
        }
        private void DelayOnEnable()
        {
            //onSavePointDisabled += MinimapUIManager.instance.HideInfo;
            onSavePointActivated += GameManager.instance.HideInfoMinimap;
            //onSavePointActivated += MinimapUIManager.instance.ShowInfo;
        }
        private void OnDisable()
        {
            //onSavePointDisabled -= MinimapUIManager.instance.HideInfo;
            onSavePointActivated -= GameManager.instance.HideInfoMinimap;
            //onSavePointActivated -= MinimapUIManager.instance.ShowInfo;

        }
        void ActivePointOnMap()
        {
            transform.localScale = new Vector3(2, 2, 1);
            isHighlightedOnMap = true;
            onSavePointActivated?.Invoke(shopID);
        }
        void DisablePointOnMap()
        {
            transform.localScale = new Vector3(1, 1, 1);
            isHighlightedOnMap = false;
            //onSavePointDisabled?.Invoke();
        }
    }
}
