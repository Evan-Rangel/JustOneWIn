using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class RespawnController : MonoBehaviour
    {
        public MaterialPropertyBlock propertyBlock { get; private set; }
        [SerializeField] List<SpriteRenderer> sprs;
        Rigidbody2D rb;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
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
                if (collision.transform.root.TryGetComponent<PlatformMovement>(out PlatformMovement _platform))
                {
                    _platform.DisableCollider();
                }
                respawning = true;
                StartCoroutine(RespawnPlayer());
                FakeLight_S.instance.StartRespawnEffect();
            }
        }

        IEnumerator RespawnPlayer()
        {
            PlayRespawnEffect();
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            yield return new WaitForSeconds(.5f);
            rb.constraints = RigidbodyConstraints2D.None| RigidbodyConstraints2D.FreezeRotation;

            respawning = false;
            transform.position = respawn.position;
        }
        public void PlayRespawnEffect()
        {
            StartCoroutine(RespawnEffect());
        }
        IEnumerator RespawnEffect()
        {
            Color alphaOn= Color.white;
            Color alphaOff= new Color(1,1,1,0);


            yield return Helpers.GetWait(0.1f);


            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOff);
            }

            yield return Helpers.GetWait(0.15f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOn);
            }

            yield return Helpers.GetWait(0.3f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOff);
            }

            yield return Helpers.GetWait(0.15f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOn);
            }

            yield return Helpers.GetWait(0.3f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOff);
            }


            yield return Helpers.GetWait(0.15f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOn);
            }

            yield return Helpers.GetWait(0.3f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOff);
            }

            yield return Helpers.GetWait(0.15f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOn);
            }


            yield return Helpers.GetWait(0.3f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOff);
            }

            yield return Helpers.GetWait(0.1f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOn);
            }

        }
        public void FastDamageEffect()
        {
            StartCoroutine(DamageEffect());
        }
        IEnumerator DamageEffect()
        {
            Color alphaOn = Color.white;
            Color alphaOff = new Color(1, 1, 1, 0);
            yield return Helpers.GetWait(0.1f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOff);
            }
            yield return Helpers.GetWait(0.15f);
            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOn);
            }
            yield return Helpers.GetWait(0.3f);

            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOff);
            }
            yield return Helpers.GetWait(0.15f);
            foreach (SpriteRenderer item in sprs)
            {
                item.material.SetColor("_Color", alphaOn);
            }
               
        }
    }
}
