using Lesson_12;
using Lesson_4;
using Lesson_6;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson_19
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(TouchingDirections))]
    [RequireComponent(typeof(Damageable))]
    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 5.0f;
        public float runSpeed = 8.0f;
        public float airWalkSpeed = 3.0f;
        public float airRunSpeed = 5.0f;
        public float jumpImpulse = 10f;
        public float walljump_Y_Impulse = 10f;
        public float walljump_X_Impulse = 4f;
        public int maxJumpCount = 2;
        public float CurrentMoveSpeed
        {
            get
            {
                if (CanMove)
                {
                    if (IsMoving && !touchingDirections.IsOnWall)
                    {
                        if (touchingDirections.IsGrounded)
                        {
                            currentJumpCount = 0;
                            if (IsRunning)
                            {
                                return runSpeed;
                            }
                            else
                            {
                                return walkSpeed;
                            }
                        }
                        else
                        {
                            if (IsRunning)
                            {
                                return airRunSpeed;
                            }
                            else
                            {
                                return airWalkSpeed;
                            }
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        Vector2 moveInput;

        [SerializeField]
        private bool _isMoving = false;
        public bool IsMoving
        {
            get { return _isMoving; }
            set
            {
                _isMoving = value;
                animator.SetBool(AnimationStrings.IsMoving, _isMoving);
            }
        }

        [SerializeField]
        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                animator.SetBool(AnimationStrings.IsRunning, _isRunning);
            }
        }

        [SerializeField]
        private bool _isFacingRight = true;
        public bool isFacingRight
        {
            get { return _isFacingRight; }
            private set
            {
                if (_isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
                _isFacingRight = value;
            }
        }

        public bool CanJump
        {
            get 
            {
                if(maxJumpCount <= currentJumpCount)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsWallJump
        {
            get { return animator.GetBool(AnimationStrings.IsWallJump); }
            //set { animator.SetBool(AnimationStrings.IsWallJump, value); }
        }

        public bool CanMove
        {
            get { return animator.GetBool(AnimationStrings.CanMove); }
        }

        public bool IsAlive
        {
            get { return animator.GetBool(AnimationStrings.IsAlive); }
        }

        Rigidbody2D rb;
        Animator animator;
        TouchingDirections touchingDirections;
        Damageable damageable;

        int currentJumpCount = 0;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = GetComponent<TouchingDirections>();
            damageable = GetComponent<Damageable>();
        }

        private void FixedUpdate()
        {

            if (!damageable.LockVelocity && !IsWallJump)
            {
                Move();
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }


            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }

        public void Move()
        {
            float x_value = Input.GetAxis("Horizontal");
            moveInput = new Vector2(x_value, 0);/*context.ReadValue<Vector2>();*/
            if (IsAlive)
            {
                IsMoving = moveInput != Vector2.zero;
                SetFacingDirection(moveInput);
            }
            else
            {
                IsMoving = false;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            //moveInput = context.ReadValue<Vector2>();
            //if (IsAlive)
            //{
            //    IsMoving = moveInput != Vector2.zero;
            //    SetFacingDirection(moveInput);
            //}
            //else
            //{
            //    IsMoving = false;
            //}
        }

        private void SetFacingDirection(Vector2 moveInput)
        {
            if (moveInput.x > 0 && !isFacingRight)
            {
                isFacingRight = true;
            }
            else if (moveInput.x < 0 && isFacingRight)
            {
                isFacingRight = false;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsRunning = true;
            }
            else if (context.canceled)
            {
                IsRunning = false;
            }
        }

        //버튼이나 키를 통해서 호출
        public void OnJump(InputAction.CallbackContext context)
        {
            //TODO Check if alice as well
            if (context.started && touchingDirections.IsGrounded && CanMove)
            {
                animator.SetTrigger(AnimationStrings.Jump);
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                //++currentJumpCount;
                return;
            }
            //벽 점프
            else if(context.started && !touchingDirections.IsGrounded 
                && touchingDirections.IsOnWall && CanMove && CanJump)
            {
                animator.SetTrigger(AnimationStrings.Jump);

                //IsWallJump = true;
                if (isFacingRight)
                {
                    SetFacingDirection(new Vector2(-1, 1));
                    rb.velocity = new Vector2(rb.velocity.x - (walljump_X_Impulse), walljump_Y_Impulse);
                }
                else
                {
                    SetFacingDirection(new Vector2(1, 1));
                    rb.velocity = new Vector2(rb.velocity.x + (walljump_X_Impulse), walljump_Y_Impulse);
                }
                ++currentJumpCount;
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            //TODO Check if alice as well
            if (context.started)
            {
                animator.SetTrigger(AnimationStrings.Attack);
            }
        }

        public void OnRangedAttack(InputAction.CallbackContext context)
        {
            //TODO Check if alice as well
            if (context.started && touchingDirections.IsGrounded)
            {
                animator.SetTrigger(AnimationStrings.RangedAttack);
            }
        }

        public void OnHit(int damage, Vector2 knockback)
        {
            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        }
    }
}