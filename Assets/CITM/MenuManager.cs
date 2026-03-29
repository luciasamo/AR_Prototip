using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject arContent;

    void Start()
    {
        menuPanel.SetActive(true);
        arContent.SetActive(false);
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        arContent.SetActive(true);
    }
}