using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        SoundManager.PlayBGM(BGMType.MAINMENU);
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        SoundManager.StopBGM();
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
