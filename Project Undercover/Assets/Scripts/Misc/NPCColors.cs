using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Colors/NpcColors")]
public static class NpcColors {

    private static List<int> remainingIndices;
    private static Color[] colors;

    public static Color GetAvailableColor()
    {
        if (colors == null)
        {
            colors = new Color[12];
            colors[0] = new Color(1, 0, 0);
            colors[1] = new Color(0, 1, 0);
            colors[2] = new Color(1, 0, 1);
            colors[3] = new Color(1, 1, 0);
            colors[4] = new Color(0, 1, 1);
            colors[5] = new Color(1, 0, 1);
            colors[6] = new Color(1, 0.5f, 0);
            colors[7] = new Color(1, 0, 0.5f);
            colors[8] = new Color(0.5f, 1, 0);
            colors[9] = new Color(0, 1, 0.5f);
            colors[10] = new Color(0, 0.5f, 1);
            colors[11] = new Color(0.5f, 0, 1);
            colors[11] = new Color(0.8f, 0.8f, 0.8f);
            colors[11] = new Color(0.25f, 0.25f, 0.25f);
        }
        if (remainingIndices == null || remainingIndices.Count == 0)
        {
            remainingIndices = new List<int>();
            for (int i=0; i < colors.Length; i++)
            {
                remainingIndices.Add(i);
            }
        }
        int randomIndex = Random.Range(0, remainingIndices.Count);
        Color color = colors[remainingIndices[randomIndex]];
        remainingIndices.RemoveAt(randomIndex);
        return color;
    }
}
