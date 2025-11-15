using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{

    public class FabricTorretController : MonoBehaviour
    {
        Animator anim;
        [SerializeField] Transform pivotPoint;
        [SerializeField] Transform bulletSpawn;
        [SerializeField] Quaternion temp;
        FabricTorretCanon canon;
        [SerializeField] AnimationCurve AnimationCurve;
        Transform target;
        bool aiming;
        float timer=0f;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            canon = GetComponentInChildren<FabricTorretCanon>();
            aiming = false;
        }
      /*  private void Update()
        {
            if (aiming )
            {

                if (timer < 4f)
                {
                    AimingAtPlayer();
                }
                else
                {
                    Shoot();
                }
               // StartCoroutine(TimeToShoot());
                timer += Time.deltaTime;

            }

        }*/
        public void ResetTimer()
        {
            Debug.Log("Reset");
                        timer = 0;

        }
        void AimingAtPlayer()
        {
            Vector2 direction = (target.position - pivotPoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
            pivotPoint.localRotation = Quaternion.RotateTowards(pivotPoint.localRotation, targetRotation, 200 * Time.deltaTime);
        }
        public void Aiming( Transform _target)
        {
            anim.SetBool("Aiming", true);
            anim.SetBool("Shoot", false);
           // anim.enabled = false;
            //aiming = true;
            //target = _target;
            canon.Aiming(_target);
        }

        public void Shoot()
        {
            Debug.Log("Shoot");
           // aiming =false;
            anim.enabled = true;
            anim.SetBool("Shoot", true);
            anim.SetBool("Aiming", false);
        }
        IEnumerator TimeToShoot()
        { 
            yield return new WaitForSeconds(0.5f);
            Shoot();
            yield return new WaitForSeconds(1f);
            Aiming(target);
        }

     
    }

    
}
