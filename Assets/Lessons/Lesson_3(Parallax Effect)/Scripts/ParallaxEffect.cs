using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_3
{
    public class ParallaxEffec : MonoBehaviour
    {
        public Camera cam;
        public Transform followTarget;

        //Starting position for the parallax game object;
        Vector2 startingPosition;

        // start z Value of the Parallax game object;
        float startingZ;

        Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

        float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
        float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget) > 0 ? cam.farClipPlane : cam.nearClipPlane);
        float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

        // Start is called before the first frame update
        void Start()
        {
            startingPosition = transform.position;
            startingZ = transform.localPosition.z;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 newPosition = startingPosition + camMoveSinceStart / parallaxFactor;

            transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
        }
    }

}
