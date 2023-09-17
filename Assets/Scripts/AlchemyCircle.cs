using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using UnityEditor;

public class AlchemyCircle : MonoBehaviour
{
    //謎のRawImage
    [SerializeField]
    RawImage ri;
    //謎のTexture2D
    Texture2D texture;
    //謎のSize
    int size;
    //Color系
    Color color, backgroundColor, testColor;

    //厚さ？
    int thickness = 4;

    //Inspectorからいじれる thickとsize
    public int THICK, SIZ; // small 4,64 : big 2,128

    [SerializeField]
    Texture2D RuneCircleImage;

    void Start()
    {
        color = Color.white;
        backgroundColor = new Color(0, 0, 0, 0);
        testColor = new Color(.6f, .8f, 1, .8f);

        thickness = THICK;

        //size = SIZ * THICK
        size = SIZ * thickness;

        //RandomInt = UnityEngine.Random.Range(0, 200);

        //使用するTextureのフォーマットを設定している
        texture = new Texture2D(size, size, TextureFormat.ARGB32, false, false);
        //Debug.Log(texture);
        //ri = GetComponent<RawImage>();
        //Debug.Log(ri);

        resetMatrix = new Color[size * size];
        for (int i = 0; i < size * size; i++) resetMatrix[i] = backgroundColor;

        texture.SetPixels(resetMatrix);
        Generate(Random.Range(0, 999));

        //test(0);

        texture.filterMode = FilterMode.Bilinear;
        texture.Apply();
        ri.texture = texture;
        //StartCoroutine(NewCircle());

        //Color[] magicCirclePixels = texture.GetPixels();
        //Debug.Log(magicCirclePixels.Length);

        //TextureCopy(RuneCircleImage);
        //Debug.Log(TextureCopy(RuneCircleImage).Length);

        //PngSave("testMagicCircle", texture);
        JoinTextures(texture, TextureCopy(RuneCircleImage, size));
    }

    int asd = 0;
    Color[] resetMatrix;
    int timerd = 0;

    private void Update()
    {
        //if (timerd++ % 100 == 0)
        //{
        //    texture.SetPixels(resetMatrix);

        //    Generate(asd++);

        //    texture.filterMode = FilterMode.Bilinear;
        //    texture.Apply();
        //    ri.texture = texture;
        //}
    }

    //public void Transmute(int seed)
    //{
    //    texture.SetPixels(resetMatrix);

    //    Generate(seed);

    //    texture.filterMode = FilterMode.Bilinear;
    //    texture.Apply();
    //    ri.texture = texture;
    //}

    //IEnumerator NewCircle()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1);
    //        texture.SetPixels(resetMatrix);

    //        Generate(asd++);

    //        texture.filterMode = FilterMode.Bilinear;
    //        texture.Apply();
    //        ri.texture = texture;
    //    }
    //}

    // ############################################
    // TODO: dimezzare tutti i radius dei cerchi? #
    // ############################################

    void Generate(int id)
    {
        CiaccoRandom randomer = new CiaccoRandom();
        randomer.setSeed(id);

        //int ncol = 60;// min_color 0
        //int xcol = 250;// max_color 255

        // draw the hexagon:
        // hexagon's center coordinates and radius
        //float hex_x = size / 2;
        //float hex_y = size / 2;
        float radius = ((size / 2f) / 2f);//* 3f / 4f); //230

        int randomRot = UnityEngine.Random.Range(0, 4) * 90;

        int latis;
        int l;
        float ang;

        float angdiff, posax, posay;

        //ここから追記

        int option = randomer.getRand(0, 5);//optionは-5 ~ 4 option 0か3しか出ない？
        print("option " + option);
        int polygonNum = randomer.getRand(3, 8);
        print("pnum " + polygonNum);

        //for (int i = 0; i < 40; i++)
        //{
        //    int olygonNum = randomer.getRand(3, 8);
        //    print(olygonNum);
        //}

        //for (int i = 0; i < 40; i++)
        //{
        //    int op = randomer.getRand(0, 5);
        //    print(op);
        //}

        TextureDraw.drawCircle(texture, size / 2, size / 2, (int)radius * 4 / 12 * 5, color, thickness + 1);
        TextureDraw.drawCircle(texture, size / 2, size / 2, (int)radius / 3 * 4, color, thickness + 1);

        if (polygonNum > 4)
        {
            if (option == 0)
            {
                TextureDraw.drawPolygon(texture, polygonNum, (int)radius / 3 * 4, 0 + randomRot, size, color, thickness);
            }
            else if (option == 1)
            {
                TextureDraw.drawPolygon(texture, polygonNum, (int)radius / 3 * 4, 0 + randomRot, size, color, thickness);
                TextureDraw.drawPolygon(texture, polygonNum, (int)radius / 3 * 4, 0 + randomRot + (360 / polygonNum) / 2, size, color, thickness);
            }
            else if (option == 2)
            {
                TextureDraw.drawPolygon(texture, polygonNum, (int)radius / 3 * 4, 0 + randomRot, size, color, thickness);
                TextureDraw.drawPolygon(texture, polygonNum, (int)radius / 3 * 4, 0 + randomRot + (360 / polygonNum) / 3, size, color, thickness);
                TextureDraw.drawPolygon(texture, polygonNum, (int)radius / 3 * 4, 0 + randomRot + (360 / polygonNum) / 3 * 2, size, color, thickness);
            }
        }
        else
        {
            //for (int i = 0; i < polygonNum ; i++)
            //{
            //    angdiff = (Mathf.Deg2Rad * (360 / (latis)));
            //    posax = (((radius / 18) * 11) * Mathf.Cos(i * angdiff));
            //    posay = (((radius / 18) * 11) * Mathf.Sin(i * angdiff));
            //    TextureDraw.drawFilledCircle(texture, (int)(size / 2 + posax), (int)(size / 2 + posay), (int)((radius / 44) * 6), color, backgroundColor, thickness);
            //}
        }


        //一番外側の円を描画
        TextureDraw.drawCircle(texture, size / 2, size / 2, (int)radius, color, thickness);
        //Debug.Log("まず一番外側の円を描画する");

        //外側の円に内接する多角形の頂点の数
        int lati = randomer.getRand(4, 8);
        //print("lati " + lati);

        //外側の円に内接する多角形を生成
        TextureDraw.drawPolygon(texture, lati, (int)radius, 0 + randomRot, size, color, thickness);
        //Debug.Log("次に" + lati + "角形を" + (0 + randomRot) + "度回転させて描画する");

        

        //多角形の頂点から内側に向かって線を引く
        for (l = 0; l < lati; l++)
        {
            ang = Mathf.Deg2Rad * ((360 / (lati))) * l;
            TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)(((size / 2) + radius * Mathf.Cos(ang + (90 + randomRot) * Mathf.Deg2Rad))), (int)(((size / 2) + radius * Mathf.Sin(ang + (90 + randomRot) * Mathf.Deg2Rad))), color, thickness);
        }
        //Debug.Log("次に" + lati + "角形の頂点から内側に向かって線を引く");

        
        if (lati % 2 == 0)
        {
            latis = randomer.getRand(2, 6);
            print("latis " + latis);

            //さらに内側の多角形を決定
            while (latis % 2 != 0) latis = randomer.getRand(3, 6);

            //描画
            TextureDraw.drawFilledPolygon(texture, latis, (int)radius, 180 + randomRot, size, color, backgroundColor, thickness);
            //Debug.Log("次に" + latis + "角形を描画する");

            //線
            for (l = 0; l < latis; l++)
            {
                ang = Mathf.Deg2Rad * ((360 / latis)) * l;
                TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + radius * Mathf.Cos(ang + randomRot * Mathf.Deg2Rad)), (int)((size / 2) + radius * Mathf.Sin(ang + randomRot * Mathf.Deg2Rad)), color, thickness);
            }
            //Debug.Log("次に" + latis + "角形の頂点から内側に向かって線を引く");
        }
        else
        {
            latis = randomer.getRand(2, 6);
            while (latis % 2 == 0) latis = randomer.getRand(3, 6);

            TextureDraw.drawFilledPolygon(texture, latis, (int)radius, 180 + randomRot, size, color, backgroundColor, thickness);
            //Debug.Log("次にFilledの" + latis + "角形を描画する");
            //Debug.Log("latiが奇数ならば内側の図形の頂点からは線をひかない 現在のlati : " + lati + "角形");
        }

        if (randomer.getRand(0, 1) % 2 == 0)
        {
            int ronad = randomer.getRand(0, 4);
            print("ronad " + ronad);

            //idに対応する任意の変数が奇数の場合
            if (ronad % 2 == 1)
            {
                for (l = 0; l < lati + 4; l++)
                {
                    ang = Mathf.Deg2Rad * ((360 / (lati + 4))) * l;
                    TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Cos(ang)), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Sin(ang)), color, thickness);
                }

                TextureDraw.drawFilledPolygon(texture, lati + 4, (int)(radius / 2), 0 + randomRot, size, color, backgroundColor, thickness);
            }
            else if (ronad % 2 == 0)
            {
                for (l = 0; l < lati - 2; l++)
                {
                    ang = Mathf.Deg2Rad * ((360 / (lati - 2))) * l;
                    TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Cos(ang + 180 * Mathf.Deg2Rad)), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Sin(ang)), color, thickness);
                }

                TextureDraw.drawFilledPolygon(texture, lati - 2, (int)(radius / 4), 0 + randomRot, size, color, backgroundColor, thickness);
            }
        }

        if (randomer.getRand(0, 4) % 2 == 0)
        {
            TextureDraw.drawCircle(texture, size / 2, size / 2, (int)((radius / 16f) * 11f), color, thickness);

            if (lati % 2 == 0)
            {
                latis = randomer.getRand(2, 8);

                while (latis % 2 != 0) latis = randomer.getRand(3, 8);

                TextureDraw.drawPolygon(texture, latis, (int)((radius / 3) * 2), 180 + randomRot, size, color, thickness);
            }
            else
            {
                latis = randomer.getRand(2, 8);

                while (latis % 2 == 0) latis = randomer.getRand(3, 8);

                TextureDraw.drawPolygon(texture, latis, (int)((radius / 3) * 2), 180 + randomRot, size, color, thickness);
            }
        }

        int caso = randomer.getRand(0, 3);
        print("caso " + caso);

        
        if (caso == 0)
        {
            for (int i = 0; i < latis; i++)
            {
                angdiff = (Mathf.Deg2Rad * (360 / (latis)));
                posax = (((radius / 18) * 11) * Mathf.Cos(i * angdiff));
                posay = (((radius / 18) * 11) * Mathf.Sin(i * angdiff));
                TextureDraw.drawFilledCircle(texture, (int)(size / 2 + posax), (int)(size / 2 + posay), (int)((radius / 44) * 6), color, backgroundColor, thickness);
            }
        }
        else if (caso == 1)
        {
            for (int i = 0; i < latis; i++)
            {
                angdiff = (Mathf.Deg2Rad * (360 / latis));
                posax = (radius * Mathf.Cos(i * angdiff));
                posay = (radius * Mathf.Sin(i * angdiff));
                TextureDraw.drawFilledCircle(texture, (int)(size / 2 + posax), (int)(size / 2 + posay), (int)((radius / 44) * 6), color, backgroundColor, thickness);
            }
        }
        else if (caso == 2)
        {
            TextureDraw.drawCircle(texture, size / 2, size / 2, (int)((radius / 18) * 6), color, thickness);
            TextureDraw.drawFilledCircle(texture, size / 2, size / 2, (int)((radius / 22) * 6), color, backgroundColor, thickness);
        }
        else if (caso == 3)
        {
            for (int i = 0; i < latis; i++)
            {
                ang = Mathf.Deg2Rad * ((360 / (latis))) * i;
                TextureDraw.drawLine(texture, (int)((size / 2) + ((radius / 3) * 2) * Mathf.Cos(ang)), (int)((size / 2) + ((radius / 3) * 2) * Mathf.Sin(ang)), (int)((size / 2) + radius * Mathf.Cos(ang)), (int)((size / 2) + radius * Mathf.Sin(ang)), color, thickness);
            }
            if (latis == lati)
            {
            }
            else
            {
                TextureDraw.drawFilledCircle(texture, size / 2, size / 2, (int)((radius / 3) * 2), color, backgroundColor, thickness);
                lati = randomer.getRand(3, 6);

                //TextureDraw.drawPolygon(texture, lati, (int)((radius / 3) * 4), 0 + randomRot, size, color, thickness);//外
                TextureDraw.drawPolygon(texture, lati, (int)((radius / 3) * 2), 180 + randomRot, size, color, thickness);//中
            }
        }


        
    }

    //private string GetSavePath(string folderName)
    //{
    //    string directoryPath = UnityEngine.Device.Application.persistentDataPath + "/" + folderName + "/";

    //    if (!Directory.Exists(directoryPath))
    //    {
    //        //まだ存在してなかったら作成
    //        Directory.CreateDirectory(directoryPath);
    //        return directoryPath + "texture.png";
    //    }

    //    return directoryPath + "texture.png";
    //}

    private byte[] ConvertToPng(Texture2D saveTex)
    {
        //Pngに変換
        byte[] bytes = saveTex.EncodeToPNG();
        return bytes;
    }

    private void PngSave(string fileName, Texture2D tex)
    {
        //Assetsにファイルパスを指定
        string filePath = "Assets/" + fileName + ".png";// EditorUtility.SaveFilePanel("Save Texture", "Assets", fileName + ".png", "png");

        if (filePath.Length > 0)
        {
            // pngファイル保存.
            File.WriteAllBytes(filePath, ConvertToPng(tex));
        }
    }

    private static Color[] TextureCopy(Texture2D source, int sourceSize)
    {
        var texture = new Texture2D(sourceSize/* source.width */, sourceSize /* source.height */, TextureFormat.RGBA32, false);
        var renderTexture = new RenderTexture(texture.width, texture.height, 32);

        // もとのテクスチャをRenderTextureにコピー
        Graphics.Blit(source, renderTexture);
        RenderTexture.active = renderTexture;

        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        // RenderTexture.activeの内容をtextureに書き込み
        Color[] copiedTexturePixels = texture.GetPixels();
        RenderTexture.active = null;

        //// 不要になったので削除
        RenderTexture.DestroyImmediate(renderTexture);

        //// pngとして保存
        //File.WriteAllBytes("Assets/CopiedRuneCircle.png", texture.EncodeToPNG());

        AssetDatabase.Refresh();

        // 保存したものをロードしてから返す
        return copiedTexturePixels;
    }

    void JoinTextures(Texture2D magicCircle, Color[] RuneCircle)
    {
        Color[] pixels = magicCircle.GetPixels();
        Color[] blendPixels = RuneCircle;

        Texture2D joinedTexture = new Texture2D(size/* source.width */, size /* source.height */, TextureFormat.RGBA32, false);

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            Color blendPixel = blendPixels[i];

            if (pixel == Color.white || blendPixel == Color.white)
            {
                pixels[i] = new Color(1, 1, 1, 1);
                continue;
            }

            //var baseAlpha = 1.0f - blendPixel.a;
            //var blendAlpha = blendPixel.a;

            //var r = pixel.r * baseAlpha + blendPixel.r * blendAlpha;
            //var g = pixel.g * baseAlpha + blendPixel.g * blendAlpha;
            //var b = pixel.b * baseAlpha + blendPixel.b * blendAlpha;
            //var a = 1.0f;
            //pixels[i] = new Color(r, g, b, a);
            pixels[i] = Color.clear;
        }

        joinedTexture.SetPixels(pixels);
        joinedTexture.Apply();

        //PngSave("CompletedmagicCircle", joinedTexture);

        ri.texture = joinedTexture;
    }

}
