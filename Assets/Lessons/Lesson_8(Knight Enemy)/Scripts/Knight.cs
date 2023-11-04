using Lesson_6;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_8
{
    public enum WalkableDirection
    {
        Right,
        Left,
    }
    [RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections))]
    public class Knight : MonoBehaviour
    {
        public float walkSpeed = 3.0f;

        Rigidbody2D rb;
        TouchingDirections touchingDirections;
        private WalkableDirection _walkDirection;
        private Vector2 walkDirectionVector = Vector2.right;

        public WalkableDirection WalkDirection
        {
            get { return _walkDirection; }
            set 
            {
                if (_walkDirection != value) 
                {
                    gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                    if(value == WalkableDirection.Right)
                    {
                        walkDirectionVector = Vector2.right;
                    }
                    else if(value == WalkableDirection.Left)
                    {
                        walkDirectionVector = Vector2.left;
                    }
                }
                _walkDirection = value; 
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            touchingDirections = GetComponent<TouchingDirections>();
        }

        private void FixedUpdate()
        {
            if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
            {
                FlipDirection();
            }

            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        }

        private void FlipDirection()
        {
            if(WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else if(WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else
            {
                Debug.LogError("FlipDirection : ");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
