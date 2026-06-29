
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
            if (!GameManager.instance.SubstractCoin(buffPrice))
                return;
            SaveManager.SaveBuffPurchase(_shopID, 2);
            switch (buffType)
            {
                case STAT.Health:
                    GameManager.instance.HealthBuff();
                    break;
                case STAT.Stamina:
                    GameManager.instance.StaminaBuff();
                    break;
                case STAT.HealthRecover:
                    GameManager.instance.HealthRecoverBuff();
                    break;
            }
        }

    }
}
