using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class TeleportPlayerAnimController : MonoBehaviour
    {
        Animator anim;
        [SerializeField] SpriteRenderer playerSprite;
        public static TeleportPlayerAnimController instance;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
            anim = GetComponent<Animator>();
        }

        void DisablePlayerSprite()
        { 
            GameManager.instance.ChangeState(GameManager.GameState.UI);
            playerSprite.enabled = false;
        }
        void EnablePlayerSprite()
        {
            GameManager.instance.ChangeState(GameManager.GameState.Gameplay);
            playerSprite.enabled = true;
        }   
        void EndAnimationComplete()
        {
            //GameManager.instance.ResetLevel();
        }
        public void PlayTeleportAnim()
        {
            anim.SetTrigger("Teleport");
        }
        public void PlayTeleportEndAnim()
        {
            anim.SetTrigger("TeleportEnd");
        }

    }
}
