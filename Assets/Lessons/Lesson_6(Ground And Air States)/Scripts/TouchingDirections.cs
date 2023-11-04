using Lesson_4;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_6
{
    public class TouchingDirections : MonoBehaviour
    {
        public ContactFilter2D castFilter;
        public float groundDistance = 0.05f;
        public float wallDistance = 0.2f;
        public float ceilingDistance = 0.05f;

        private bool _isGrouded = true;
        public bool IsGrounded
        {
            get { return _isGrouded; }
            private set
            {
                _isGrouded = value;
                animator.SetBool(AnimationStrings.IsGrounded, _isGrouded);
            }
        }

        private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0? Vector2.right : Vector2.left;

        private bool _isOnWall= false;
        public bool IsOnWall
        {
            get { return _isOnWall; }
            private set
            {
                _isOnWall = value;
                animator.SetBool(AnimationStrings.IsOnWall, _isOnWall);
            }
        }

        private bool _isOnCeiling = false;
        public bool IsOnCeiling
        {
            get { return _isOnCeiling; }
            private set
            {
                _isOnCeiling = value;
                animator.SetBool(AnimationStrings.IsOnCeiling, _isOnCeiling);
            }
        }

        CapsuleCollider2D touchingCol;
        Animator animator;

        RaycastHit2D[] goundHits = new RaycastHit2D[5];
        RaycastHit2D[] wallHits = new RaycastHit2D[5];
        RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

        private void Awake()
        {
            touchingCol = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            IsGrounded = touchingCol.Cast(Vector2.down, castFilter, goundHits, groundDistance) > 0;
            IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits , wallDistance) > 0;
            IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
        }
    }

}
