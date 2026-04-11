using UnityEngine;

public class NavigationHandler : MonoBehaviour
{
    public void NextMask()
    {
        if (MaskSwitcher.instance != null)
            MaskSwitcher.instance.NextMask();
        else
            Debug.LogWarning("MaskSwitcher.instance no encontrado");
    }

    public void PreviousMask()
    {
        if (MaskSwitcher.instance != null)
            MaskSwitcher.instance.PreviousMask();
        else
            Debug.LogWarning("MaskSwitcher.instance no encontrado");
    }
}