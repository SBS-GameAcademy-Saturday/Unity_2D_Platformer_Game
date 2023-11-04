using Lesson_4;
using Lesson_6;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson_7
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(TouchingDirections))]
    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 5.0f;
        public float runSpeed = 8.0f;
        public float airWalkSpeed = 3.0f;
        public float airRunSpeed = 5.0f;
        public float jumpImpulse =10f;
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

        public bool CanMove
        {
            get { return animator.GetBool(AnimationStrings.CanMove); }
        }


        Rigidbody2D rb;
        Animator animator;
        TouchingDirections touchingDirections;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = GetComponent<TouchingDirections>();
        }

        //private void InputSetting()
        //{
        //    PlayerInput playerInput = GetComponent<PlayerInput>();
        //    playerInput.actionEvents.pl
        //}

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
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

        public void OnJump(InputAction.CallbackContext context)
        {

            //TODO Check if alice as well
            if (context.started && touchingDirections.IsGrounded && CanMove)
            {
                animator.SetTrigger(AnimationStrings.Jump);
                rb.velocity = new Vector2(rb.velocity.x,jumpImpulse);
            }
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            //TODO Check if alice as well
            if (context.started && touchingDirections.IsGrounded)
            {
                animator.SetTrigger(AnimationStrings.Attack);
            }
        }
    }
}