using Lesson_4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lesson_12
{
    public class Damageable : MonoBehaviour
    {
        public UnityEvent<int, Vector2> damageableHit;
        //Lesson 15
        public UnityEvent damageableDeath;
        //Lesson 18
        public UnityEvent<int, int> healthChanged;
        public bool LockVelocity
        {
            get { return animator.GetBool(AnimationStrings.LockVelocity); }
            set { animator.SetBool(AnimationStrings.LockVelocity, value); }
        }

        Animator animator;

        [SerializeField]
        private int _maxHealth = 100;

        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        [SerializeField]
        private int _health = 100;

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                //Lesson 18
                healthChanged.Invoke(_health, MaxHealth);

                if (_health <= 0)
                {
                    IsAlive = false;
                }
            }
        }

        [SerializeField]
        private bool _isAlive = true;

        [SerializeField]
        private bool isInvincible = false;

        private float timeSinceHit = 0;

        [SerializeField]
        public float invincibilityTime = 0.25f;

        public bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                _isAlive = value;
                animator.SetBool(AnimationStrings.IsAlive, _isAlive);
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isInvincible)
            {
                if (timeSinceHit > invincibilityTime)
                {
                    isInvincible = false;
                    timeSinceHit = 0;
                }

                timeSinceHit += Time.deltaTime;
                return;
            }
        }

        public bool GetHit(int damage)
        {
            if (IsAlive && !isInvincible)
            {
                Health -= damage;
                isInvincible = true;
                return true;
            }
            return false;
        }
        public bool GetHit(int damage, Vector2 knockback)
        {
            if (IsAlive && !isInvincible)
            {
                Health -= damage;
                isInvincible = true;

                animator.SetTrigger(AnimationStrings.Hit);
                LockVelocity = true;

                damageableHit?.Invoke(damage, knockback);

                CharacterEvents.characterDamaged?.Invoke(gameObject, damage);

                return true;
            }
            return false;
        }

        public bool Heal(int healthRestore)
        {
            if (IsAlive && Health < MaxHealth)
            {
                int maxheal = Mathf.Max(MaxHealth - Health, 0);
                int actualHeal = Mathf.Min(maxheal, healthRestore);
                Health += actualHeal;

                CharacterEvents.characterHealed?.Invoke(gameObject, actualHeal);
                return true;
            }
            return false;
        }
    }

}
