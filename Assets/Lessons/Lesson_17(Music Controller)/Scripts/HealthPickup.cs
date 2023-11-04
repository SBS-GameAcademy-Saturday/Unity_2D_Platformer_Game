using Lesson_12;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lesson_17
{
    public class HealthPickup : MonoBehaviour
    {
        public int healthRestore = 20;
        public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

        AudioSource pickupSource;

        private void Awake()
        {
            pickupSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable & damageable.Health < damageable.MaxHealth)
            {
                bool wasHealed = damageable.Heal(healthRestore);

                if (wasHealed)
                {
                    if (pickupSource)
                    {
                        //Destroy(this.gameObject);·Î ÀÎÇØ ÆÄ±«
                        AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                    }
                    Destroy(this.gameObject);
                }

            }
        }

        private void Update()
        {
            transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
        }


    }
}