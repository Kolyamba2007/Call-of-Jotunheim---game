using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefullFunc : MonoBehaviour
{
    public static Texture2D ColorTexture(Texture2D source, Color color)
    {
        if (source != null)
        {
            Texture2D result = source;
            for (int i = 0; i < result.width; i++)
            {
                for (int j = 0; j < result.height; j++)
                {
                    result.SetPixel(i, j, color);
                }
            }
            return result;
        }
        else return null;
    }
    public static Texture2D ColorTexture(Color color)
    {
        Texture2D result = Texture2D.whiteTexture;
        for (int i = 0; i < result.width; i++)
        {
            for (int j = 0; j < result.height; j++)
            {
                result.SetPixel(i, j, color);
            }
        }
        result.Apply();
        return result;
    }
}
