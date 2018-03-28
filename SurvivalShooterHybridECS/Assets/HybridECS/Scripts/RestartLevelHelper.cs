using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelHelper : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
