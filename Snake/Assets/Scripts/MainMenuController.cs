using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class MainMenuController : MonoBehaviour
    {
        public GameObject _optionsPanel;

        private void Start() 
        {
            _optionsPanel.SetActive(false);
        }

        public void OpenOptionsPanel()
        {
            _optionsPanel.SetActive(true);
        }
        
        public void CloseOptionsPanel()
        {
            _optionsPanel.SetActive(false);
        }
        
        public void LoadMainScene()
        {
            SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
        }
    }
}
