using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Avocado
{
    public class EndGamePanel : MonoBehaviour
    {
        public static EndGamePanel instance;
        [SerializeField] RawImage playerWinner;
        [SerializeField] TMP_Text playerNameWinner;
        [SerializeField] GameObject holder;
        private void Awake()
        {
            instance = this;
        }
        public void SetWinnerPlayer(string name, Texture2D icon)
        {
            Debug.Log("Winner");
            holder.SetActive(true);
            playerWinner.texture = icon;
            playerNameWinner.text = name;
        }
    }
}
