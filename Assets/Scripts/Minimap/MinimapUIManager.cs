using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
namespace Avocado
{
    public class MinimapUIManager : MonoBehaviour
    {
        [SerializeField] GameObject minimapCamera;
        [SerializeField] public TeleportController[] teleportControllers;
        [SerializeField] TMP_Text zoneNameText;
        [SerializeField] Button selectButton;
        public event Action<TpEntity> onNextPoint;
        public event Action<TpEntity> onPrevPoint;

        public static MinimapUIManager instance;
        int currentPointIndex = 0;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        
        private void Start()
        {
            minimapCamera= MinimapCamera.Instance.gameObject;
        }
        public void ShowInfo(TpEntity tp)
        {
            zoneNameText.text = tp.zoneName;
            currentPointIndex = GameManager.instance.GetTeleportIndexByName(tp.zoneName);
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(() =>
            {
                TeleportPlayerAnimController.instance.PlayTeleportEndAnim();
                CanvasManager.instance.ToggleMinimap(false);
                FakeLight_S.instance.ShadeEffect(null);
                GameSaveCapsule capsule = GameManager.instance.playerSavePosition.root.GetComponentInChildren<GameSaveCapsule>();
                capsule.StartCloseCapsuleAnimation();
                capsule.OnCloseAnimationEnd += () =>
                {
                    GameManager.instance.TeleportPlayerTo(tp.sceneName);
                };
            });
        }
       
        public void HideInfo()
        { 
            zoneNameText.text = "";
        }
        public void NextPoint()
        {
            currentPointIndex++;
            if (currentPointIndex >= GameManager.instance.activeSavePoints.Count)
                currentPointIndex=0;
            Debug.Log("Current Index: "+ currentPointIndex+ "; Max: "+ GameManager.instance.activeSavePoints.Count);
            TpEntity tp = GameManager.instance.activeSavePoints[currentPointIndex];
            minimapCamera.transform.position = GameManager.instance.GetTeleportPositionByName(tp.zoneName);
            onNextPoint?.Invoke(tp);
        }
        public void PrevPoint()
        {
            currentPointIndex--;
            if (currentPointIndex < 0)
                currentPointIndex = GameManager.instance.activeSavePoints.Count - 1;
            Debug.Log("Current Index: " + currentPointIndex + "; Max: " + GameManager.instance.activeSavePoints.Count);
            TpEntity tp = GameManager.instance.activeSavePoints[currentPointIndex];
            minimapCamera.transform.position = GameManager.instance.GetTeleportPositionByName(tp.zoneName);
            onPrevPoint?.Invoke(tp);
        }
        private void OnEnable()
        {
            for (int i = 0; i < teleportControllers.Length; i++)
            {
                teleportControllers[i].CheckForVisibility();
            }
            minimapCamera.SetActive(true);
            onNextPoint += ShowInfo;
            onPrevPoint += ShowInfo;
        }

        private void OnDisable()
        {
            onNextPoint -= ShowInfo;
            onPrevPoint -= ShowInfo;
            if (minimapCamera)
                minimapCamera.SetActive(false);
        }
    }
}
