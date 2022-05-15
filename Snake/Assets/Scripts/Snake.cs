using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Project.Scripts
{
    public class Snake : MonoBehaviour
    {
        [Tooltip("Segments of snake that we will instantiate.")] public Transform _segmentPrefab;

        public GameObject _gameOverPanel, _scoreMultiplier;

        public Texture _aliveIcon, _deadIcon;

        public RawImage[] _icons;

        private Vector2 _direction;

        private List<Transform> _snakeSegments;

        public BoxCollider2D _multiplierGridArea;

        private int _foodScore = 10;

        private int _numberOfLives = 3;

        // tracks when to spawn our multiplier
        private int _multiplierCounter = 0;
        
        private int _multiplierScore;

        private void Start() 
        {
            _snakeSegments = new List<Transform>(); 
            // add first segment of snake(head) 
            _snakeSegments.Add(transform);

            _gameOverPanel.SetActive(false);
            _scoreMultiplier.SetActive(false);
        }

        private void FixedUpdate() 
        {   
            SnakePositioning();
        }

        private void Update() 
        {
            Movement();    
        }

        private void SnakePositioning()
        {
            // this assures that each segment follows the one in front of it
            for (int i = _snakeSegments.Count - 1; i > 0; i--)
               _snakeSegments[i].position = _snakeSegments[i - 1].position;

            // Round is used to simulate grid snapping 
            // (because grid snapping only happens using int values)
            transform.position = new Vector3
            (Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            0.0f);    
        }

        private void Movement()
        {
            if (Input.GetKeyDown(KeyCode.W))
              _direction = Vector2.up;

            else if (Input.GetKeyDown(KeyCode.S))
              _direction = Vector2.down;

            else if (Input.GetKeyDown(KeyCode.A))
              _direction = Vector2.left;
        
            else if (Input.GetKeyDown(KeyCode.D))
              _direction = Vector2.right;
        }

        private void GrowSnake()
        {
            Transform _segment = Instantiate(_segmentPrefab);
            // set segment position behind the head
            _segment.position = _snakeSegments[_snakeSegments.Count - 1].position;
            // add segements into the list 
            _snakeSegments.Add(_segment);
        }

        private void ResetGame()
        {
            for (int i = 1; i < _snakeSegments.Count; i++)
                Destroy(_snakeSegments[i].gameObject);

            // clear our list
            _snakeSegments.Clear();
            // add head back again
            _snakeSegments.Add(transform);

            //reset our snake position to zero
            transform.position = Vector2.zero;

            _multiplierCounter = 0;

            _numberOfLives--;

            if (_numberOfLives == 0)
            {
                _gameOverPanel.SetActive(true);
                this.enabled = false;
            }

            for (int i = 0; i < _icons.Length; i++)
                if (i >= _numberOfLives)
                    _icons[i].texture = _deadIcon;
        }

        private void SpawnMultiplier()
        {
            Bounds _multiplierBounds = _multiplierGridArea.bounds;

            float _multiplierX = Random.Range(_multiplierBounds.min.x, _multiplierBounds.max.x);
            float _multiplierY = Random.Range(_multiplierBounds.min.y, _multiplierBounds.max.y);

            _scoreMultiplier.transform.position = new Vector3(Mathf.Round(_multiplierX), Mathf.Round(_multiplierY), 0.0f);
            // activate our score multiplier game object 
            _scoreMultiplier.SetActive(true);
            Invoke(nameof(DeactivateMultiplier), 3f);
        }

        private void DeactivateMultiplier()
        {
            _scoreMultiplier.SetActive(false);
        }

        private void OnTriggerEnter2D([NotNull]Collider2D _other) 
        {
            if (_other.tag == "Food") 
            {
                GrowSnake();
                GameData._singleton.UpdateScore(_foodScore);
                _multiplierCounter++;

                if (_multiplierCounter == 5)
                {
                    SpawnMultiplier();
                    _multiplierCounter = 0;
                }
            }

            else if (_other.tag == "Multiplier")
            {
                // set random multiplier value
                _multiplierScore = Random.Range(2, 11);
                // multiply our score by multiplier 
                GameData._singleton.UpdateScore(_foodScore * int(_multiplierScore));
                // deactivate it 
                _scoreMultiplier.SetActive(false);
            }

            else if (_other.tag == "Obstacle") 
                ResetGame();
        }
     }
 }
