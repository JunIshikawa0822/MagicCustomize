Shader "Unlit/test"
{
    // Unity上でやり取りをするプロパティ情報
    // マテリアルのInspectorウィンドウ上に表示され、スクリプト上からも設定できる
    Properties
    {
        _Number ("Number", Range(3, 8)) = 0 // Color プロパティー (デフォルトは白)   a____
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
        Pass
        {
            CGPROGRAM   // プログラムを書き始めるという宣言
 
            // 関数宣言
            #pragma vertex vert   // "vert" 関数を頂点シェーダー使用する宣言
            #pragma fragment frag  // "frag" 関数をフラグメントシェーダーと使用する宣言

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
            int _Number; // マテリアルからのカラー   a____

            float Polygon(float2 p, float size, float r, float adjust, float anglead)
            {
                float n = 2 * PI / r;
                float theta = atan2(p.x, p.y);
                float ad = adjust;
                float e = cos(floor(0.5 + (theta + anglead)/ n) * n - (theta + anglead + ad)) * length(p) - size;
                float b = 0.01 / abs(e);

                return b;
            }

            float Line(float2 p, float size, float r, float adjust)
            {
                float n = 2 * PI / r;
                float theta = atan2(p.x, p.y);
            }

            float Circle(float2 p, float size)
            {
                float d = length(p) - size;
                float b = 0.01 / abs(d); // Brightness
                return b;
            }

            float4 Draw(float2 uvPos)
            { 
                float2 p = uvPos * 2 - 1;

                float3 Polygons = 0;
                Polygons += Circle(p, 0.9) * float3(0.1, 0.01, 1.0);
                Polygons += Circle(p, 0.85) * float3(0.1, 0.01, 1.0);
                

                Polygons += Circle(p, 0.4) * float3(0.1, 0.01, 1.0);
                Polygons += Circle(p, 0.35) * float3(0.1, 0.01, 1.0);
                Polygons += Polygon(p, 0.3, 8, 0, _Time.y * 0.1) * float3(0.1, 0.01, 1.0);
                Polygons += Polygon(p, 0.7, 7, 0, _Time.y * 1.5) * float3(0.1, 0.01, 1.0);
                Polygons += Polygon(p, 0.5, 7, 0, _Time.y * 0.5) * float3(0.1, 0.01, 1.0);

                //Polygons += Polygon(p, 0.3, 8, 45, 1) * float3(0.01, 0.01, 0.05);
                

                return float4(Polygons, 1);
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


