using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Replay()
    {
        SceneManager.LoadScene("Quizz");
    }
    private AsyncOperation asyncLoad;

    public void PrepareToLoadScene(float delay)
    {
        StartCoroutine(LoadSceneAsyncRoutine(delay));
    }

    private IEnumerator LoadSceneAsyncRoutine(float delay)
    {
        asyncLoad = SceneManager.LoadSceneAsync("Quizz");
        asyncLoad.allowSceneActivation = false;

        // Wait until scene is almost loaded
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Wait delay, then activate
        yield return new WaitForSeconds(delay);
    }

    public void ContinueLoading()
    {
        asyncLoad.allowSceneActivation = true;
    }
}
