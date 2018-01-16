using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject PauseMenuPanel;

    void Start() {
        PauseMenuPanel.SetActive(false);
    }

    void Update () {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
	}

    void Pause() {
        PauseMenuPanel.SetActive(true);
        Time.timeScale= 0f;
        GameIsPaused = true;
    }

    public void Resume() {
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Quit() {
        Application.Quit();
    }
}
