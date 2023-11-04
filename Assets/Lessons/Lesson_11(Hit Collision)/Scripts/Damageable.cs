using Lesson_4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lesson_11
{
    public class Damageable : MonoBehaviour
    {
        public UnityEvent<int, Vector2> damageableHit;

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

                //Lesson_12에서 설명
                CharacterEvents.characterDamaged?.Invoke(gameObject, damage);

                return true;
            }
            return false;
        }
    }

}
