using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson_4
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float walkSpeed = 5.0f;
        public float runSpeed = 8.0f;

        public float CurrentMoveSpeed
        {
            get
            {
                if (IsMoving)
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

        Rigidbody2D rb;
        Animator animator;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }

        private void SetFacingDirection(Vector2 moveInput)
        {
            if(moveInput.x > 0 && !isFacingRight)
            {
                isFacingRight = true;
            }
            else if(moveInput.x < 0 && isFacingRight)
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

    }
}