using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneButton : MonoBehaviour
{
    public string targetSceneName;

    public void SwitchScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}