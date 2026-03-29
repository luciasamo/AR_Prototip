using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceInfoUI : MonoBehaviour
{
    public GameObject infoPanel;
    private ARFaceManager faceManager;

    void Start()
    {
        faceManager = FindObjectOfType<ARFaceManager>();
        infoPanel.SetActive(false);
    }

    void OnEnable()
    {
        if (faceManager != null)
            faceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        if (faceManager != null)
            faceManager.facesChanged -= OnFacesChanged;
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            infoPanel.SetActive(true);
        }

        if (args.removed.Count > 0)
        {
            infoPanel.SetActive(false);
        }
    }
}