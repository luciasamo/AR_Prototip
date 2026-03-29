using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class FaceInfoUI : MonoBehaviour
{
    public GameObject infoPanel;
    public ARFaceManager faceManager;

    void OnEnable()
    {
        faceManager.facesChanged += OnFacesChanged;
    }

    void OnDisable()
    {
        faceManager.facesChanged -= OnFacesChanged;
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            infoPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }
}