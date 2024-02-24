Shader "Unlit/test"
{
    // Unity上でやり取りをするプロパティ情報
    // マテリアルのInspectorウィンドウ上に表示され、スクリプト上からも設定できる
    Properties
    {
        _Dim ("Seed", float) = 3 // Color プロパティー (デフォルトは白)   a____
        _MainTex("Base (RGB)",2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
    }

    //CGINCLUDE

    //float4 paint(float2 uv)
    //{
        //return _Color;
    //}

    //ENDCG

    // サブシェーダー
    // シェーダーの主な処理はこの中に記述する
    // サブシェーダーは複数書くことも可能が、基本は一つ
    SubShader
    {
        // パス
        // 1つのオブジェクトの1度の描画で行う処理をここに書く
        // これも基本一つだが、複雑な描画をするときは複数書くことも可能
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100

        Pass
        {
            CGPROGRAM   // プログラムを書き始めるという宣言
 
            // 関数宣言
            #pragma vertex vert   // "vert" 関数を頂点シェーダー使用する宣言
            #pragma fragment frag  // "frag" 関数をフラグメントシェーダーと使用する宣言
            //#pragma surface surf Lambert vertex:vert alpha

            #define PI 3.14159265

            //参照するファイルの中身をそのままこの場所に展開する。
            //そして、呼び出す関数はそれより前に書かなければいけないので、vert関数の後にこのincludeを書くとコンパイルエラー
            #include "UnityCG.cginc"

            // 構造体の定義
            struct appdata // vert関数の入力
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct fin // vert関数の出力からfrag関数の入力へ
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };
 
            // 変数宣言
            int _Dim; // マテリアルからのカラー
            sampler2D _MainTex;
            fixed4 _Color;

            float4 Polygon(float2 pos, float radius, float polygonNum, float adjust, float anglead, float thickness, float4 color)
            {
                float n = 2 * PI / polygonNum;
                float theta = atan2(pos.x, pos.y);

                //float dal = cos(cos(floor(0.5 + (theta + anglead)/ n) * n - (theta + anglead + adjust)));
                float rad = (radius / 2 * cos(PI/polygonNum));

                float cal = cos(floor(0.5 + (theta + anglead)/ n) * n - (theta + anglead + adjust)) * length(pos) - rad;

                float val = abs(cal);
                //float b = 0.01 / abs(e);
                //float b = abs(e) / 0.01;
                float4 f;

                if(val > rad)
                {
                    f = 0 * color;
                }
                else if(val < rad - thickness)
                {
                    f = 0 * color;
                }
                else
                {
                    f = 1 * color;
                }

                return f;
            }

            float4 FilledCircle(float2 pos, float radius, float thickness, float4 color) : Color
            {
                float dist = length(pos);
                float val = abs(dist);
                float4 f;

                if (val < radius)
                {
                    // 内部を透明にする
                    if (val > radius - thickness)
                    {
                        f = color;
                    }
                    else
                    {
                        f = float4(0, 0, 0, 0);
                    }
                }
                else
                {
                    // 外側を透明にする
                    f = float4(0, 0, 0, 0);
                }

                return f;
            }

            float4 Circle(float2 pos, float radius, float thickness, float4 color)
            {
                float dist = length(pos);
                float val = abs(dist);
                float4 f;

                //float b = abs(d) / 0.01;

                //float d = length(center) - size;
                //float b = 0.0001 / abs(d) ;

                if(val > radius)
                {
                    f = 0 * color;
                }
                else if(val < radius - thickness)
                {
                    f = 0 * color;
                }
                else
                {
                    f = float4(0, 0, 0, 1) * color;
                }

                return f;
            }

            float4 Draw(float2 uvPos)
            { 
                float2 p = uvPos * 2 - 1;

                fixed4 Polygons = 0;
                
                Polygons += Circle(p, 0.8, 0.01, (0, 0, 0, 1));
                //Polygons += Circle(p, 0.2, 0.01, (0, 0, 0, 1));
                
                //Polygons += FilledCircle(0, 0.9, 0.01, (1, 1, 1, 1));
                //Polygons += Circle(p, 0.4);
                //Polygons += Circle(p, 0.35);
                //Polygons += Polygon(p, 0.9, 5, 0, _Time.y * 0.1, 0.005, (1, 1, 1, 1));
                //Polygons += Polygon(p, 0.8, 5, 0, _Time.y * 0.1, 0.01, (1, 1, 1, 1));
                //Polygons += InnerPolygon(p, 0.45, 5, _Time.y * 0.1, 0.01, (1, 1, 1, 1));
                //Polygons += Polygon(p, 0.7, 7, 0, _Time.y * 1.5, 0.01, (1, 1, 1, 1));
                //Polygons += Polygon(p, 0.4, 7, 0, _Time.y * 0.5, 0.01, (1, 1, 1, 1));

                //Polygons += Polygon(p, 0.3, 8, 0, 1, 0.01, (1, 1, 1, 1));
                

                return fixed4(Polygons);
            }        

            // 頂点シェーダー　SV_POSITIONとかいうセマンティクス
            fin vert(appdata v) // 構造体を使用した入出力
            {
                fin o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            float4 frag(fin IN) : SV_TARGET // 構造体finを使用した入力
            {
                return Draw(IN.texcoord.xy); 
            }
 
            ENDCG   // プログラムを書き終わるという宣言
        }
    }
}


