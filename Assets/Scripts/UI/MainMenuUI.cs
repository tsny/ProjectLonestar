using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void LoadDebugLevel()
    {
        SceneManager.LoadScene("SCN_Debug");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
