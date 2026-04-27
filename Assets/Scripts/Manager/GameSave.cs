using System;
using UnityEngine;

namespace Avocado
{
    public class GameSave : InteractEventManager
    {
        [SerializeField] GameObject minimapPoint;
        [SerializeField] Transform savePointPosition;
        ShopManager shopManager;
        MinimapCamera minimapCamera;
        bool isHighlightedOnMap = false;
        TpEntity shopID;
        public event Action<TpEntity> onSavePointActivated;
       // public event Action onSavePointDisabled;
        public override void Start()
        {
            base.Start();
            //minimapPoint.SetActive(true);
            minimapCamera = MinimapCamera.Instance;
            shopManager = transform.root.GetComponentInChildren<ShopManager>();
            shopID = shopManager.shopID;    
        }
     /*   private void Update()
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
            minimapPoint.transform.localScale = new Vector3(10, 10, 1);
            isHighlightedOnMap = true;
            onSavePointActivated?.Invoke(shopID);
        }
        void DisablePointOnMap()
        {
            minimapPoint.transform.localScale = new Vector3(5, 5, 1);
            isHighlightedOnMap = false;
            //onSavePointDisabled?.Invoke();
        }*/
        public override void OnInteractEvent()
        {
            base.OnInteractEvent();
            GameManager.instance.AddSavePoint(shopID);
            minimapCamera.enabled = true;
            minimapCamera.transform.position= transform.position;
            GameManager.instance.ToggleMinimap(true);
        }
    }
}
