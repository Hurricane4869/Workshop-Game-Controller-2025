using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("SkipTutorial"))
        {
            SceneManager.LoadSceneAsync(2);
        }
    }
}
