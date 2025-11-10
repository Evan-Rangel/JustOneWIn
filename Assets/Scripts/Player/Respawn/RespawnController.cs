using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class RespawnController : MonoBehaviour
    {
        public MaterialPropertyBlock propertyBlock { get; private set; }
        [SerializeField] List<SpriteRenderer> sprs;

        private void Start()
        {
            propertyBlock = new MaterialPropertyBlock();
        }
        Transform respawn;
        bool respawning = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.name == "Respawn")
            {
                respawn = collision.transform;
            }
            if (collision.transform.name == "Death" && !respawning)
            {
                respawning = true;
                StartCoroutine(RespawnPlayer());
                FakeLight_S.instance.StartRespawnEffect();
            }
        }

        IEnumerator RespawnPlayer()
        {
            PlayRespawnEffect();
            yield return new WaitForSeconds(.5f);
            respawning = false;
            transform.position = respawn.position;
        }
        public void PlayRespawnEffect()
        {
            StartCoroutine(RespawnEffect());
        }
        IEnumerator RespawnEffect()
        {

            Debug.Log("Respawn Effect");
            yield return Helpers.GetWait(0.1f);
            propertyBlock.SetFloat("_Alpha", 0);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

            yield return Helpers.GetWait(0.15f);
            propertyBlock.SetFloat("_Alpha", 1);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

            yield return Helpers.GetWait(0.3f);
            propertyBlock.SetFloat("_Alpha", 0);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

            yield return Helpers.GetWait(0.15f);
            propertyBlock.SetFloat("_Alpha", 1);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

            yield return Helpers.GetWait(0.3f);
            propertyBlock.SetFloat("_Alpha", 0);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }


            yield return Helpers.GetWait(0.15f);
            propertyBlock.SetFloat("_Alpha", 1);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

            yield return Helpers.GetWait(0.3f);
            propertyBlock.SetFloat("_Alpha", 0);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

            yield return Helpers.GetWait(0.15f);
            propertyBlock.SetFloat("_Alpha", 1);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }


            yield return Helpers.GetWait(0.3f);
            propertyBlock.SetFloat("_Alpha", 0);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

            yield return Helpers.GetWait(0.1f);
            propertyBlock.SetFloat("_Alpha", 1);
            foreach (SpriteRenderer item in sprs)
            {
                item.SetPropertyBlock(propertyBlock);
            }

        }
    }
}
