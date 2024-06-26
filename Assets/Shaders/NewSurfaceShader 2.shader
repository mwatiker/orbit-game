Shader "Custom/NewSurfaceShader 2"
{
Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Opacity ("Opacity", Range(0,1)) = 1.0
        _AuroraIterations ("Aurora Iterations", Range(1, 50)) = 50
        _StarIterations ("Star Iterations", Range(1, 10)) = 4
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            float2x2 mm2(in float a) { float c = cos(a), s = sin(a); return float2x2(c, -s, s, c); }
            float2x2 m2 = float2x2(0.95534, -0.29552, 0.29552, 0.95534);

            sampler2D _MainTex;
            float _Opacity;
            int _AuroraIterations;
            int _StarIterations;

            float tri(in float x) {
                return clamp(abs(frac(x) - .5), 0.01, 0.49);
            }
            
            fixed2 tri2(in fixed2 p) { return fixed2(tri(p.x) + tri(p.y), tri(p.y + tri(p.x))); }

            float triNoise2d(in fixed2 p, float spd)
            {
                float z = 1.8;
                float z2 = 2.5;
                float rz = 0.;
                fixed2 bp = p;
                for (int i = 0; i < 5; i++)
                {
                    fixed2 dg = tri2(bp*1.85)*.75;
                    p -= dg / z2;

                    bp *= 1.3;
                    z2 *= .45;
                    z *= .42;
                    p *= 1.21 + (rz - 1.0)*.02;

                    rz += tri(p.x + tri(p.y))*z;
                    p = mul(p, -m2);
                }
                return clamp(1. / pow(rz*29., 1.3), 0., .55);
            }

            float hash21(in fixed2 n) { return frac(sin(dot(n, fixed2(12.9898, 4.1414))) * 43758.5453); }

			fixed3 hash33(fixed3 p)
			{
				p = frac(p * fixed3(443.8975, 397.2973, 491.1871));
				p += dot(p.zxy, p.yxz + 19.27);
				return frac(fixed3(p.x * p.y, p.z*p.x, p.y*p.z));
			}

            fixed4 aurora(fixed3 ro, fixed3 rd)
            {
                fixed4 col = fixed4(0, 0, 0, 0);
                fixed4 avgCol = fixed4(0, 0, 0, 0);

                for (int i = 0; i < _AuroraIterations; i++)
                {
                    float of = 0.006 * hash21(_ScreenParams.xy) * smoothstep(0., 15., i);
                    float pt = ((.8 + pow(i, 1.4)*.002) - ro.y) / (rd.y*2. + 0.4);
                    pt -= of;
                    fixed3 bpos = ro + pt * rd;
                    fixed2 p = bpos.zx;
                    float rzt = triNoise2d(p, 0.06);
                    fixed4 col2 = fixed4(0, 0, 0, rzt);
                    col2.rgb = (sin(1. - fixed3(2.15, -.5, 1.2) + i * 0.043) * 0.5 + 0.5) * rzt;
                    avgCol = lerp(avgCol, col2, .5);

                    col += avgCol*exp2(-i*0.065 - 2.5)*smoothstep(0., 5., i);
                }

                col *= (clamp(rd.y*15. + .4, 0., 1.));
                return col*1.8;
            }

            fixed3 stars(in fixed3 p)
            {
                fixed3 c = fixed3(0., 0., 0.);
                float res = _ScreenParams.x*1.;

                for (int i = 0; i < _StarIterations; i++)
                {
                    fixed3 q = frac(p*(.15*res)) - 0.5;
                    fixed3 id = floor(p*(.15*res));
                    fixed2 rn = hash33(id).xy;
                    float c2 = 1. - smoothstep(0., .6, length(q));
                    c2 *= step(rn.x, .0005 + i*i*0.001);
                    c += c2*(lerp(fixed3(1.0, 0.49, 0.1), fixed3(0.75, 0.9, 1.), rn.y)*0.1 + 0.9);
                    p *= 1.3;
                }
                return c*c*.8;
            }

			

            fixed3 bg(in fixed3 rd)
            {
                float sd = dot(normalize(fixed3(-0.5, -0.6, 0.9)), rd)*0.5 + 0.5;
                sd = pow(sd, 5.);
                fixed3 col = lerp(fixed3(0.05, 0.1, 0.2), fixed3(0.1, 0.05, 0.2), sd);
                return col*.63;
            }

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed2 q = (i.uv*_ScreenParams.xy) / _ScreenParams.xy;
                fixed2 p = q - 0.5;
                p.x *= _ScreenParams.x / _ScreenParams.y;

                fixed3 ro = fixed3(0, 0, -6.7);
                fixed3 rd = normalize(fixed3(p, 1.3));

                fixed3 col = bg(rd);

				fixed4 aur = aurora(ro, rd);
				col += stars(rd);
				col = col*(1. - aur.a) + aur.rgb;
				rd.y = abs(rd.y);
                    col = bg(rd)*0.6;
                    fixed4 aura = aurora(ro, rd);
                    col += stars(rd)*0.1;
                    col = col*(1. - aura.a) + aura.rgb;

                // if (rd.y > 0.) {
                //     fixed4 aur = aurora(ro, rd);
                //     col += stars(rd);
                //     col = col*(1. - aur.a) + aur.rgb;
                // }
                // else // Reflections
                // {
                //     rd.y = abs(rd.y);
                //     col = bg(rd)*0.6;
                //     fixed4 aur = aurora(ro, rd);
                //     col += stars(rd)*0.1;
                //     col = col*(1. - aur.a) + aur.rgb;
                // }

                fixed4 color = fixed4(col, _Opacity); // Apply opacity

                return color;
            }
            ENDCG
        }
    }
}
