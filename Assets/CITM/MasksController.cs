using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MasksController : MonoBehaviour
{
    private ARFaceManager faceManager;
    private Transform maskContainer;

    private int currentMask = -1;

    void Awake()
    {
        faceManager = FindObjectOfType<ARFaceManager>();
        faceManager.facesChanged += OnFacesChanged;
    }

    void OnDestroy()
    {
        faceManager.facesChanged -= OnFacesChanged;
    }

    void OnFacesChanged(ARFacesChangedEventArgs args)
    {
        if (args.added.Count > 0)
        {
            maskContainer = args.added[0].transform.Find("MaskContainer");

            HideAllMasks();
            ShowMask(0);
        }
    }

    void HideAllMasks()
    {
        foreach (Transform mask in maskContainer)
            mask.gameObject.SetActive(false);
    }

    public void ShowMask(int index)
    {
        if (maskContainer == null) return;

        HideAllMasks();
        maskContainer.GetChild(index).gameObject.SetActive(true);
        currentMask = index;
    }
}