using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialPanel;

    void Start()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f; 
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        tutorialPanel.SetActive(false);
        
        Time.timeScale = 1f;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}