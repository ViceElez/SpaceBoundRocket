using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool collisionEnabled = true;
    private void Update()
    {
        RespondToDebug();
        
    }

    private void RespondToDebug()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
           collisionEnabled = !collisionEnabled;
        }

    }
    
    public void ResetGame()
    {
        StartCoroutine(LoadFirstLevel());
    }

    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }
    private IEnumerator LoadFirstLevel()
    {
       yield return new WaitForSeconds(1);  
        SceneManager.LoadScene(0);
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1);
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex==SceneManager.sceneCountInBuildSettings)
        {
           nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);

    }
}
