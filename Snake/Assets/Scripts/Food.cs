using UnityEngine;
using JetBrains.Annotations;

namespace Project.Scripts
{
    public class Food : MonoBehaviour
    {
        [Tooltip("Area in which food can generate.\nPrevents spawning in unwanted positions.")]
        public BoxCollider2D _gridArea;

        private void Start() 
        {
            RandomizeFoodPosition();
        }

        private void RandomizeFoodPosition()
        {
            // set bounds of our 2D box collider
            Bounds _bounds = _gridArea.bounds;
            
            // generate random values between bounds on x axis
            float _x = Random.Range(_bounds.min.x, _bounds.max.x);
            float _y = Random.Range(_bounds.min.y, _bounds.max.y);

            // set food position according to generated values above
            transform.position = new Vector3(Mathf.Round(_x), Mathf.Round(_y), 0.0f);
        }

        private void OnTriggerEnter2D([NotNull]Collider2D _other) 
        {
            if(_other.tag == "Snake") RandomizeFoodPosition();
        }
    }
}