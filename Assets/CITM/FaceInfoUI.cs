using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceInfoUI : MonoBehaviour
{
    public GameObject infoPanel;
    public ARFaceManager faceManager;

    void Update()
    {
        if (faceManager.trackables.count > 0)
        {
            infoPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }
}