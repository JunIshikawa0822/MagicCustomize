using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyCircle : MonoBehaviour
{
    [SerializeField]
    TextureManager textureManager;
    
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

    int RandomInt;

    void Start()
    {
        color = Color.white;
        backgroundColor = new Color(0, 0, 0, 0);
        testColor = new Color(.6f, .8f, 1, .8f);

        thickness = THICK;

        //size = SIZ * THICK
        size = SIZ * thickness;

        Debug.Log(size);

        //RandomInt = UnityEngine.Random.Range(0, 200);

        //使用するTextureのフォーマットを設定している
        texture = new Texture2D(size, size, TextureFormat.ARGB32, false, false);

        //Debug.Log(texture);
        //ri = GetComponent<RawImage>();
        //Debug.Log(ri);

        resetMatrix = new Color[size * size];
        for (int i = 0; i < size * size; i++) resetMatrix[i] = backgroundColor;

        texture.SetPixels(resetMatrix);
        int n = UnityEngine.Random.Range(0, 100);
        Debug.Log("n" + n);

        Generate(n);

        //test(0);

        texture.filterMode = FilterMode.Bilinear;

        texture.Apply();

        ri.texture = texture;

        //textureManager.TextureMix(texture, textureManager.RuneImage, 1000000, )
        //StartCoroutine(NewCircle());
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

        //一番外側の円を描画
        TextureDraw.drawCircle(texture, size / 2, size / 2, (int)radius, color, thickness);

        //外側の円に内接する多角形の頂点の数
        int lati = randomer.getRand(4, 8);
        print("lati " + lati);

        //外側の円に内接する多角形を生成
        TextureDraw.drawPolygon(texture, lati, (int)radius, 0, size, color, thickness);

        int l;
        float ang;

        //多角形の頂点から内側に向かって線を引く
        //for (l = 0; l < lati; l++)
        //{
        //    ang = Mathf.Deg2Rad * ((360 / (lati))) * l;
        //    TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + radius * Mathf.Cos(ang)), (int)((size / 2) + radius * Mathf.Sin(ang)), color, thickness);
        //}

        int latis;
        if (lati % 2 == 0)
        {
            latis = randomer.getRand(2, 6);
            print("latis " + latis);

            //さらに内側の多角形を決定
            while (latis % 2 != 0) latis = randomer.getRand(3, 6);

            //描画
            TextureDraw.drawFilledPolygon(texture, latis, (int)radius, 180, size, color, backgroundColor, thickness);

            //線
            for (l = 0; l < latis; l++)
            {
                ang = Mathf.Deg2Rad * ((360 / latis)) * l;
                TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + radius * Mathf.Cos(ang)), (int)((size / 2) + radius * Mathf.Sin(ang)), color, thickness);
            }
        }
        else
        {
            latis = randomer.getRand(2, 6);
            while (latis % 2 == 0) latis = randomer.getRand(3, 6);

            TextureDraw.drawFilledPolygon(texture, latis, (int)radius, 180, size, color, backgroundColor, thickness);
            //latiが奇数角形ならば内側の図形の頂点からは線をひかない
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

                TextureDraw.drawFilledPolygon(texture, lati + 4, (int)(radius / 2), 0, size, color, backgroundColor, thickness);
            }
            else if (ronad % 2 == 0)
            {
                for (l = 0; l < lati - 2; l++)
                {
                    ang = Mathf.Deg2Rad * ((360 / (lati - 2))) * l;
                    TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Cos(ang)), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Sin(ang)), color, thickness);
                }

                TextureDraw.drawFilledPolygon(texture, lati - 2, (int)(radius / 4), 0, size, color, backgroundColor, thickness);
            }
        }

        if (randomer.getRand(0, 4) % 2 == 0)
        {
            TextureDraw.drawCircle(texture, size / 2, size / 2, (int)((radius / 16f) * 11f), color, thickness);

            if (lati % 2 == 0)
            {
                latis = randomer.getRand(2, 8);

                while (latis % 2 != 0) latis = randomer.getRand(3, 8);

                TextureDraw.drawPolygon(texture, latis, (int)((radius / 3) * 2), 180, size, color, thickness);
            }
            else
            {
                latis = randomer.getRand(2, 8);

                while (latis % 2 == 0) latis = randomer.getRand(3, 8);

                TextureDraw.drawPolygon(texture, latis, (int)((radius / 3) * 2), 180, size, color, thickness);
            }
        }

        int caso = randomer.getRand(0, 3);
        print("caso " + caso);

        float angdiff, posax, posay;
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

                TextureDraw.drawPolygon(texture, lati, (int)((radius / 3) * 4), 0, size, color, thickness);//外
                TextureDraw.drawPolygon(texture, lati, (int)((radius / 3) * 2), 180, size, color, thickness);//中
            }
        }

    }
}
