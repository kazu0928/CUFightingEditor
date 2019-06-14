//ジオメトリシェーダ勉強用のワイヤーフレーム
//https://styly.cc/ja/tips/shadedcity_discont_shader/#i-6
Shader "Custom/Geometry/Wireframe"
{
    //プロパティ（インスペクタに表示）
    Properties
    {
        [PowerSlider(3.0)]
        _WireframeVal ("Wireframe width", Range(0., 0.5)) = 0.05
        _FrontColor ("Front color", color) = (1., 1., 1., 1.)
        _BackColor ("Back color", color) = (1., 1., 1., 1.)
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle] _RemoveDiag("Remove diagonals?", Float) = 0.//トグルのアトリビュートはキーワードのオンオフを可能にする


    }
    //ここに記述
    SubShader
    {
        //タグ付け
        //Queue==レンダリング順
        //RenderType＝＝グループ分類、そこまで気にしなくていいけど一応違いはある
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
 
        //いくつもPassをかけば何個でも？
        Pass
        {
            //カリング制御（他にもデプスバッファとかアルファブレンドとかいろいろ設定できる）
            //Back 視点と反対側のポリゴンをレンダリングしないようにします。 (デフォルト) .
            //Front 視点と同じ側のポリゴンをレンダリングしない。オブジェクトを反転するのに使用します。
            //Off カリングを無効にして、すべての面を描画します。特殊なエフェクトで使用します。※リファレンスより
            Cull Front

            //こことENDCGの間はCG言語
            CGPROGRAM
            //シェーダ関数宣言みたいなもの(vert->geom->fragの順番)
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
 
            //上記の[Toggle]がついたキーワードでオンオフが切り替わる（if文みたいに使える）(キーワードのオンオフはほかにも方法あり)
            #pragma shader_feature __ _REMOVEDIAG_ON
 
            //いつもの
            #include "UnityCG.cginc"
 
            //構造体定義
            struct v2g {
                //セマンティクスつき変数
                float4 worldPos : SV_POSITION;//必須
            };
 
            struct g2f {
                float4 pos : SV_POSITION;//出力座標
                float3 bary : TEXCOORD0;//出力テクスチャ座標
            };
 
            //vertexシェーダ
            v2g vert(appdata_base v) {
                v2g o;
                // 入力セマンティクスではPOSITIONはモデル空間での座標で入ってくる
                // 物体の頂点ごとのワールド座標を取得し、worldPosに代入している unity_ObjectToWorld == UNITY_MATRIX_M
                // 下記の部分は必要ない（o.worldPos = mul(unity_ObjectToWorld, v.vertex)でいい）が、w値はアフィン変換に必要な値で1であることを示すために記述。参考ＵＲＬ　http://marupeke296.com/DXG_No55_WhatIsW.html
                float4 a = float4(v.vertex.xyz,1);
                o.worldPos = mul(unity_ObjectToWorld, a);
                o.worldPos = float4(o.worldPos.xyz,1);
                return o;
            }
            
            [maxvertexcount(3)]//出力する頂点数の最大値
            void geom(triangle v2g IN[3]/*頂点シェーダーでreturnした値がここに入る(point,triangle,line,Primitiveがあり配列数が変わる)*/,
                      inout TriangleStream<g2f> triStream/*メッシュの面を出力(geomは戻り値void),これもpointStream,LineStreamもある*/)
            {
                float3 param = float3(0., 0., 0.);
                //上記のトグルがオンだったら
                #if _REMOVEDIAG_ON
                float EdgeA = length(IN[0].worldPos - IN[1].worldPos);
                float EdgeB = length(IN[1].worldPos - IN[2].worldPos);
                float EdgeC = length(IN[2].worldPos - IN[0].worldPos);
                //ここで斜めのエッジ（一番長いエッジを算出している、多分）
                if(EdgeA > EdgeB && EdgeA > EdgeC)
                    param.y = 1.;
                else if (EdgeB > EdgeC && EdgeB > EdgeA)
                    param.x = 1.;
                else
                    param.z = 1.;
                #endif
 
                g2f o;
                //プロジェクション座標変換
                o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
                o.bary = float3(1., 0., 0.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
                o.bary = float3(0., 0., 1.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
                o.bary = float3(0., 1., 0.) + param;
                triStream.Append(o);
            }
 
            float _WireframeVal;
            fixed4 _BackColor;
 
            fixed4 frag(g2f i) : SV_Target {
            //セットした太さより大きければ処理を破棄
            if(!any(bool3(i.bary.x < _WireframeVal, i.bary.y < _WireframeVal, i.bary.z < _WireframeVal)))
                 discard;//現在計算中のピクセル情報を破棄
 
                return _BackColor;
            }
 
            ENDCG
        }
        //二つ目の処理
        Pass
        {
            //裏側のポリゴンを表示してワイヤーフレームのようにしている
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
 
            #pragma shader_feature __ _REMOVEDIAG_ON
 
            #include "UnityCG.cginc"
 
            struct v2g {
                float4 worldPos : SV_POSITION;
            };
 
            struct g2f {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
            };
 
            v2g vert(appdata_base v) {
                v2g o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
 
            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
                float3 param = float3(0., 0., 0.);
 
                #if _REMOVEDIAG_ON
                float EdgeA = length(IN[0].worldPos - IN[1].worldPos);
                float EdgeB = length(IN[1].worldPos - IN[2].worldPos);
                float EdgeC = length(IN[2].worldPos - IN[0].worldPos);
 
                if(EdgeA > EdgeB && EdgeA > EdgeC)
                    param.y = 1.;
                else if (EdgeB > EdgeC && EdgeB > EdgeA)
                    param.x = 1.;
                else
                    param.z = 1.;
                #endif
 
                g2f o;
                o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
                o.bary = float3(1., 0., 0.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
                o.bary = float3(0., 0., 1.) + param;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
                o.bary = float3(0., 1., 0.) + param;
                triStream.Append(o);
            }
 
            float _WireframeVal;
            fixed4 _FrontColor;
 
            fixed4 frag(g2f i) : SV_Target {
            if(!any(bool3(i.bary.x <= _WireframeVal, i.bary.y <= _WireframeVal, i.bary.z <= _WireframeVal)))
                 discard;
                return _FrontColor;
            }
 
            ENDCG
        }
        Pass
        {
                Blend SrcAlpha One
                Cull Back
            	CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				struct appdata {
					float4 vertex : POSITION;
					fixed4 color  : COLOR;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 vColor : COLOR;
                    fixed4 worldPos:TEXCOORD0;
				};
				
				v2f vert (appdata v) {
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.vColor = v.color;

					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target {
                    float4 pos = mul(unity_ObjectToWorld,i.worldPos);
                    float dist = length(_WorldSpaceCameraPos-pos);
                    fixed4 col = fixed4(i.vColor.xyz,dist*0.02);
                     if(dist*0.02 < 0)
                    {
                        discard;
                    }
					return col;
				}
			ENDCG
        }
    }
}