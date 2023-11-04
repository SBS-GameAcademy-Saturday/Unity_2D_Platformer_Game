using Lesson_12;
using Lesson_4;
using Lesson_6;
using Lesson_9;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_15
{
    public class FlyingEye : MonoBehaviour
    {
        public float flightSpeed = 2f;
        public float waypointReachedDistance = 0.1f;
        public DetectionZone biteDetectionZone;
        public List<Transform> waypoints = new List<Transform>();
        public Collider2D deathColider;


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

        Animator animator;
        Rigidbody2D rb;
        Damageable damageable;


        Transform nextWayPoint;
        private int waypointNum = 0;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            damageable = GetComponent<Damageable>();
        }

        // Start is called before the first frame update
        void Start()
        {
            nextWayPoint = waypoints[waypointNum];
        }

        // Update is called once per frame
        void Update()
        {
            HasTarget = biteDetectionZone.detectedColiders.Count > 0;
        }
        private void FixedUpdate()
        {
            if (damageable.IsAlive)
            {
                if (CanMove)
                {
                    Flight();
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                OnDeath();
            }
        }

        private void Flight()
        {
            //Fly to Next Waypoint
            Vector2 directionToWaypoint = (nextWayPoint.position - transform.position).normalized;

            float distance = Vector2.Distance(nextWayPoint.position, transform.position);
            rb.velocity = directionToWaypoint * flightSpeed;
            UpdateDirection();
            if (distance <= waypointReachedDistance)
            {
                waypointNum++;

                if(waypointNum >= waypoints.Count)
                {
                    waypointNum = 0;
                }

                nextWayPoint = waypoints[waypointNum];
            }
        }

        private void UpdateDirection()
        {
            Vector3 localScale = transform.localScale;
            if (transform.localScale.x > 0)
            {
                if(rb.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
                }
            }
            else
            {
                if (rb.velocity.x > 0)
                {
                    transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
                }
            }
        }

        public void OnDeath()
        {
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
            deathColider.enabled = true;
        }
    }

}
