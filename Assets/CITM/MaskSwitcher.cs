using UnityEngine;

public class MaskSwitcher : MonoBehaviour
{
    public static MaskSwitcher instance;

    public GameObject[] masks;
    private int currentMask = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ShowMask(0);
    }

    public void NextMask()
    {
        currentMask = (currentMask + 1) % masks.Length;
        ShowMask(currentMask);
    }

    public void PreviousMask()
    {
        currentMask--;
        if (currentMask < 0) currentMask = masks.Length - 1;
        ShowMask(currentMask);
    }

    void ShowMask(int index)
    {
        for (int i = 0; i < masks.Length; i++)
        {
            masks[i].SetActive(i == index);
        }
    }
}