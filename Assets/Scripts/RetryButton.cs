using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public GameObject retryGameObject;
    public GameStateSO gameState;

    private void Start()
    {
        // Ensure retryGameObject is active initially
        retryGameObject.SetActive(true);
    }

    public void ReloadCurrentScene()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void Update()
    {
            // Check if the game state is either Win or Lose
        if (gameState.currentState == GameStateSO.State.Win || gameState.currentState == GameStateSO.State.Lose)
        {
            // Disable the retryGameObject
            retryGameObject.SetActive(false);
        }
    }

}
