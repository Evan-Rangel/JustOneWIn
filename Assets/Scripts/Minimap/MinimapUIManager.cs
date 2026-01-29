using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
namespace Avocado
{
    public class MinimapUIManager : MonoBehaviour
    {
        [SerializeField] GameObject minimapCamera;

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
            currentPointIndex= GameManager.instance.activeSavePoints.IndexOf(tp);
            selectButton.onClick.AddListener(() =>
            {
                GameSaveCapsule capsule = GameManager.instance.currentSpawnPosition.pos.root.GetComponentInChildren<GameSaveCapsule>();
                FakeLight_S.instance.ShadeEffect();
                capsule.closeCapsuleAnimation = true;
                GameManager.instance.ToggleMinimap(false);
                capsule.OnCloseAnimationEnd += () =>
                {
                    GameManager.instance.TeleportPlayerToSavePoint(tp);
                };
            });
            if (Vector2.Distance(minimapCamera.transform.position, tp.pos.position)>10)
            {
                minimapCamera.transform.position = tp.pos.position;
            }
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
            onNextPoint?.Invoke(GameManager.instance.activeSavePoints[currentPointIndex]);
        }
        public void PrevPoint()
        {
            currentPointIndex--;
            if (currentPointIndex < 0)
                currentPointIndex = GameManager.instance.activeSavePoints.Count - 1;
            onPrevPoint?.Invoke(GameManager.instance.activeSavePoints[currentPointIndex]);
        }
        private void OnEnable()
        {
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
