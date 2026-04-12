using UnityEngine;

[System.Serializable]
public class Question
{
    public string questionText;
    public bool correctAnswer;
    [TextArea(3, 6)]
    public string explanation;
    public int maskIndex;
}
