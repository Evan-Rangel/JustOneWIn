
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
        public GameObject buffPrefab;

    }
}
