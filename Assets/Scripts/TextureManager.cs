using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class TextureManager : MonoBehaviour
{
    [SerializeField]
    public Texture2D RuneImage;

    void TextureMix(Texture2D tex1, Texture2D tex2, int size, RawImage rawImage)
    {
        UnityEngine.Color[] Tex1Pixels = tex1.GetPixels();
        UnityEngine.Color[] Tex2Pixels = tex2.GetPixels();

        UnityEngine.Color[] BlendPixels = new UnityEngine.Color[Tex1Pixels.Length];

        for (int i = 0; i < Tex1Pixels.Length; i++)
        {
            UnityEngine.Color tex1pixel = Tex1Pixels[i];
            UnityEngine.Color tex2pixel = Tex2Pixels[i];

            //var baseAlpha = 1.0f - blendPixel.a;
            //var blendAlpha = blendPixel.a;
            if (tex1pixel == UnityEngine.Color.white && tex2pixel == UnityEngine.Color.white
                || tex1pixel.a == 0 && tex2pixel.a == 0)
            {
                Tex1Pixels[i] = new UnityEngine.Color(0, 0, 0, 0);
            }
            else
            {
                BlendPixels[i] = new UnityEngine.Color(1, 1, 1, 1);
            }

            
            //var r = tex1pixel.r * baseAlpha + tex2pixel.r * blendAlpha;
            //var g = tex1pixel.g * baseAlpha + tex2pixel.g * blendAlpha;
            //var b = tex1pixel.b * baseAlpha + tex2pixel.b * blendAlpha;

            //var a = 1.0f;

            //Tex1Pixels[i] = new UnityEngine.Color(r, g, b, a);
        }

        Texture2D composedTexture = new Texture2D(size, size, TextureFormat.ARGB32, false, false);

        composedTexture.SetPixels(BlendPixels);
        composedTexture.Apply();

        rawImage.texture = composedTexture;
    }
}
