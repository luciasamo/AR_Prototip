using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{


    public void StartGame()
    {
        SceneManager.LoadScene("ARDemoScene");
    }

    public void Salir()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
}