Shader "Unlit/Sin"
{
    Properties
    {
        // Color property for material inspector, default to white
        _Color ("Main Color", Color) = (1,1,1,1)
        _Red ("Red", Float) = 0.5
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // vertex shader
            // this time instead of using "appdata" struct, just spell inputs manually,
            // and instead of returning v2f struct, also just return a single output
            // float4 clip position
            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return mul(UNITY_MATRIX_MVP, vertex);
            }

            fixed4 _Color;
            float _Red;

            // pixel shader, no inputs needed
            fixed4 frag () : SV_Target
            {
                float sintime = _SinTime.z * .5 + .5;
                float costime = _CosTime.z * .5 + .5;
                return float4(sintime, costime, sintime * costime, 1.0f);
            }
            ENDCG
        }
    }
}
