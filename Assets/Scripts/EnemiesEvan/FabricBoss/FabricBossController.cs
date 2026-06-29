using Avocado.Combat.Damage;
using System;
using System.Collections;
using System.Collections.Generic;
using Avocado.Combat.KnockBack;

using UnityEngine;

namespace Avocado
{
    [Serializable]
    public class FabricBossState
    {
        public D_MeleeAttack attackData;
        public Transform attackPoint;
        public string animationTrigger;
        public float idleTime;
       /* public FabricBossState(D_MeleeAttack attackData, Transform attackPoint, string animationTrigger)
        {
            this.attackData = attackData;
            this.attackPoint = attackPoint;
            this.animationTrigger = animationTrigger;
        }*/
    }
    public class FabricBossController : MonoBehaviour, IDamageable
    {
        public event Action<float> OnHealthChange;
        FabricBossState currentState;

        [SerializeField] FabricBossState fireAttack;
        [SerializeField] FabricBossState horizontalAttack;
        [SerializeField] FabricBossState jumpAttack;
        [SerializeField] FabricBossState meleeAttack;
        [SerializeField] FabricBossState idleState;
        [SerializeField] FabricBossState moveState;

        [SerializeField] float[] speedMultiplierByPhase;
        int currentPhase;
        [SerializeField] float movementSpeed;
        [SerializeField] GameObject allDirAnimator, fireAnimator;
        Rigidbody2D rb;
        int direction;
        FabricEnemyCollision fabricCollision;
        Animator anim;
        float currentHealth;
        [SerializeField] float maxtHealth;
        SpriteRenderer sprite;
        IEnumerator damageEffect;
        float xtargetPosition;
        bool inState;
        Transform playerPosition;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();
            fabricCollision = GetComponentInChildren<FabricEnemyCollision>();
            rb = GetComponent<Rigidbody2D>();
        }
        void PlayerEnter(Collider2D collider)
        {
            if (inState&& currentState.animationTrigger!="idle") return;

            direction = (transform.position.x - collider.transform.position.x > 0) ? -1 : 1;
            transform.localScale = new Vector3(direction, 1, 1);

            ChangeState(moveState);
        }
        void PlayerExit(Collider2D collider)
        { 
        
            xtargetPosition =transform.position.x- collider.transform.position.x;
            direction = (transform.position.x - collider.transform.position.x > 0) ? -1 : 1;
            transform.localScale = new Vector3(direction, 1, 1);
            ChangeState(jumpAttack);

        }
        private void OnEnable()
        {
            currentHealth = maxtHealth;

            currentPhase = 0;
            ChangeState(idleState);
            fabricCollision.OnPlayerEnter += PlayerEnter;
            fabricCollision.OnPlayerExit += PlayerExit;
            playerPosition= GameObject.FindGameObjectWithTag("Player").transform;

            OnHealthChange += CanvasManager.instance.UpdateBossHealthBar;
            OnHealthChange.Invoke(0);
        }
        private void OnDisable()
        {
            anim.SetBool("startDeath", false);
            fabricCollision.OnPlayerEnter -= PlayerEnter;
            fabricCollision.OnPlayerExit -= PlayerExit;
            OnHealthChange -= CanvasManager.instance.UpdateBossHealthBar;
        }
        private void Update()
        {
            if (currentState.animationTrigger != "walk")
                return;
            if (Math.Abs(transform.position.x - playerPosition.position.x) > 3) return;



            NextAttack();



        }
        public void ActiveAllDirAnimator()
        { 
            allDirAnimator.SetActive(true);
            DealDamage();
        }
        public void ActiveFireAnimaotor()
        { 
            fireAnimator.SetActive(true);
            DealDamage();
        }
        void Jump()
        {
            //transform.position= new Vector3(transform.position.x-xtargetPosition, transform.position.y , transform.position.z);
            transform.position= new Vector3(playerPosition.position.x, transform.position.y , transform.position.z);
        }
        void ChangeState(FabricBossState state)
        {
            
            inState = true;
            if (currentState != null)
                anim.SetBool(currentState.animationTrigger,false);
            anim.SetBool(state.animationTrigger, true);
            currentState = state;
            rb.velocity = Vector2.zero;
         
            
            
            if (currentState.animationTrigger == "walk")
                rb.velocity = Vector2.right*direction*movementSpeed* (1/ speedMultiplierByPhase[currentPhase]);
        }
        void DealDamage()
        {
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(currentState.attackPoint.position, currentState.attackData.attackRadius, currentState.attackData.whatIsPlayer);

            foreach (Collider2D collider in detectedObjects)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.Damage(new DamageData(currentState.attackData.attackDamage, gameObject));
                }

                IKnockBackable knockBackable = collider.GetComponent<IKnockBackable>();

                if (knockBackable != null)
                {
                    int direction = (transform.position.x - collider.transform.position.x > 0) ? -1 : 1;
                    //knockBackable.KnockBack(new Combat.KnockBack.KnockBackData(currentState.attackData.knockbackAngle, currentState.attackData.knockbackStrength, (int)transform.localScale.x, gameObject));
                    knockBackable.KnockBack(new KnockBackData(currentState.attackData.knockbackAngle, currentState.attackData.knockbackStrength, direction, gameObject));
                }
            }
        }
        void EndAttack()
        {
            inState = false;
            Invoke("NextAttack", currentState.idleTime * speedMultiplierByPhase[currentPhase]);
            ChangeState(idleState);
        }
        void NextAttack()
        {
            direction = (transform.position.x - playerPosition.position.x > 0) ? -1 : 1;
            transform.localScale = new Vector3(direction, 1, 1);
            if (Math.Abs(transform.position.x - playerPosition.position.x) < 3)
            {
                int attackIndex = UnityEngine.Random.Range(0, 3);

                switch (attackIndex)
                {
                    case 0 :
                        ChangeState(fireAttack);

                        break;
                    case 1:
                        ChangeState(horizontalAttack);

                        break;
                    case 2:
                        ChangeState(meleeAttack);

                        break;
                    default:
                        break;
                }
            }
            else
            { 
                ChangeState(moveState);
            }
        }
        void IDamageable.Damage(DamageData data)
        {
            currentHealth -= data.Amount;
            OnHealthChange?.Invoke(1-( currentHealth / maxtHealth));
            
            float percentageHealth = currentHealth / maxtHealth;

            if (percentageHealth > 0.33f && percentageHealth < 0.66f && currentPhase!=1)
                currentPhase = 1;
            if (percentageHealth < 0.33f && currentPhase != 2)
                currentPhase = 2;

            if (currentHealth <= 0)
            {
                anim.SetBool("startDeath", true);
                rb.velocity = Vector2.zero;
                return;
            }
            if (damageEffect != null)
                StopCoroutine(damageEffect);
            damageEffect = DamageEffect();
            StartCoroutine(damageEffect);
        }
        IEnumerator DamageEffect()
        {
            sprite.enabled = false;

            yield return Helpers.GetWait(0.1f);

            sprite.enabled = true;

            yield return Helpers.GetWait(0.1f);

            sprite.enabled = false;

            yield return Helpers.GetWait(0.1f);

            sprite.enabled = true;
        }
        void DestroyEnemy()
        {
            for (int i = 0; i < 50; i++)
            {
                GameObject coin = GameManager.instance.RequestCoin();
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine. Random.Range(1f, 3f)), ForceMode2D.Impulse);
            }
            CanvasManager.instance.BossDeath();
            anim.SetBool("startDeath", false);
            gameObject.SetActive(false);
        }
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(fireAttack.attackPoint.position, fireAttack.attackData.attackRadius);
            Gizmos.DrawWireSphere(horizontalAttack.attackPoint.position, horizontalAttack.attackData.attackRadius);
            Gizmos.DrawWireSphere(horizontalAttack.attackPoint.position, horizontalAttack.attackData.attackRadius);
            Gizmos.DrawWireSphere(meleeAttack.attackPoint.position, meleeAttack.attackData.attackRadius);
            Gizmos.DrawWireSphere(jumpAttack.attackPoint.position, jumpAttack.attackData.attackRadius);
        }
    }
}
