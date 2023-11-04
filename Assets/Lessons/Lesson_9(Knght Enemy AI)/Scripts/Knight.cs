using Lesson_4;
using Lesson_6;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_9
{
    public enum WalkableDirection
    {
        Right,
        Left,
    }
    [RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
    public class Knight : MonoBehaviour
    {
        public float walkSpeed = 3.0f;
        public float walkStopRate = 0.6f;
        public DetectionZone attackZone;
        public DetectionZone cliffDetectionZone;

        Rigidbody2D rb;
        TouchingDirections touchingDirections;
        Animator animator;

        private WalkableDirection _walkDirection = WalkableDirection.Right;
        private Vector2 walkDirectionVector = Vector2.right;

        [SerializeField]
        private bool _hasTarget = false;

        public bool HasTarget
        {
            get { return _hasTarget; }
            private set
            {
                _hasTarget = value;
                animator.SetBool(AnimationStrings.HasTarget, _hasTarget);
            }
        }

        public bool CanMove
        {
            get { return animator.GetBool(AnimationStrings.CanMove); }
        }

        public WalkableDirection WalkDirection
        {
            get { return _walkDirection; }
            set
            {
                if (_walkDirection != value)
                {
                    gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                    if (value == WalkableDirection.Right)
                    {
                        walkDirectionVector = Vector2.right;
                    }
                    else if (value == WalkableDirection.Left)
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
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            HasTarget = attackZone.detectedColiders.Count > 0;
        }

        private void FixedUpdate()
        {
            if (touchingDirections.IsGrounded && touchingDirections.IsOnWall
                    && cliffDetectionZone.detectedColiders.Count == 0)
            {
                FlipDirection();
            }
            if (CanMove)
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x , 0, walkStopRate), rb.velocity.y);
        }

        private void FlipDirection()
        {
            if (WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else if (WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else
            {
                Debug.LogError("FlipDirection : ");
            }
        }

        public void OnCliffDetected()
        {
            if (touchingDirections.IsGrounded)
            {
                FlipDirection();
            }
        }


    }

}
