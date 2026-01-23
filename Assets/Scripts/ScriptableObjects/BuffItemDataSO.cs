
using Unity.Mathematics;
using UnityEngine;

namespace Avocado
{
    [CreateAssetMenu(fileName = "NewBuffData", menuName = "Data/Buff Data/Buff Data", order = 0)]

    public class BuffItemDataSO : ScriptableObject
    {
        public int buffPrice;
        public string buffName;
        public string buffDescription;
        public Sprite buffIcon;
        public STAT buffType;

        public void ApllyBuff(string _shopID)
        {
            if (GameManager.instance.coins < buffPrice)
                return;
            GameManager.instance.SubstractCoin(buffPrice);
            SaveManager.SaveBuffPurchaseInPlayerPrefs(_shopID, 1);
            switch (buffType)
            {
                case STAT.Health:
                    GameManager.instance.HealthBuff();
                    break;
                case STAT.Stamina:
                    GameManager.instance.StaminaBuff();
                    break;
            }
        }

    }
}
