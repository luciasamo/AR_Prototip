using UnityEngine;

public class MaskUIController : MonoBehaviour
{
    public void Next()
    {
        MaskSwitcher.instance.NextMask();
    }

    public void Previous()
    {
        MaskSwitcher.instance.PreviousMask();
    }
}