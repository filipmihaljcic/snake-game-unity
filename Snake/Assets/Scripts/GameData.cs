using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class GameData : MonoBehaviour
    {
        public static GameData _singleton;

        public Text _scoreText;

        private int _score = 0;

        private void Awake() 
        {
            GameObject[] _gd = GameObject.FindGameObjectsWithTag("Game Data");  

            if (_gd.Length > 1) Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            _singleton = this;
        }

        public void UpdateScore(int _s)
        {
            _score += _s;
            if (_scoreText != null)
                _scoreText.text = "Score: " + _score;
        }
    }
}
