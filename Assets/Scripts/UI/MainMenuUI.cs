using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string sceneToLoad = "SCN_Debug";

    public void LoadDebugLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadNebulaLevel()
    {
        SceneManager.LoadScene("SCN_Nebula");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
