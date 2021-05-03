Shader "Unlit/Test Shaders/TestShader" // namespace
{
    Properties // public variables
    {
        MainTex ("Albedo Texture", 2D) = "white" {} // this is a property, "albedo texture" is a name, _MainTex is the variable name, and 2D is the type of variable
        TintColor("Tint Color", Color) = (1, 1, 1, 1) // default to white
        Transparency("Transparency", Range(0.0, 1)) = 0.25 // transparency, range shows a slider in the inspector (note, DO NOT USE F UP HERE) 
        CutoutThreshold("Cutout Threshold", Range(0.0, 1.0)) = .2
        // v properties for the mf wiggle
        Distance("Distance", float) = 1
        Amplitude("Amplitude", float) = 1
        Speed("Speed", float) = 1
        Amount("Amount", Range(0.0, 1.0)) = 1
    }
    SubShader // where the magic happens, shaders can have multiple subshaders
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" } // tells unity how to render the shader
        LOD 100 // level of detail

        Zwrite Off // do not shade pixels in the depth buffer
        Blend /* blend mode */ SrcAlpha /* the alpha channel */ OneMinusSrcAlpha // soft blending??

        Pass // essentially like the Main function
        {
            CGPROGRAM // tells the compiler that this is where the CG part starts
            // pragmas are essentially special compile instructions called "preprocessor directives"
            #pragma vertex vert // says: we are going to have a vertex function named vert (name not return type)
            #pragma fragment frag // says: we are going to have a fragment function named frag (name not return type)

            #include "UnityCG.cginc" // self explanitory, this is like monobehavior inhertance for shaders (not really but same idea)

            ///<summary>
            /// Ok so basically
            /// an unlit shader consists of everything below
            /// flow: mesh -> vertex function(properties), fragment function(properties) -> image
            /// properties are set in inspector
            /// vert function: modifies the shape of the object
            /// fragment function: applies colors to the whape of whatever comes out of the vertex function
            /// </summary>

            // vars
            sampler2D MainTex; // create a texture sampler for the main texture
            float4 MainTex_ST; // not sure why this is here, removing it causes errors

            float4 TintColor; // make sure names match
            float Transparency;
            float CutoutThreshold;
            float Distance;
            float Amplitude;
            float Speed;
            float Amount;

            struct appdata // structs are data objects, these get passed into the functions below
            {
                float4 vertex : POSITION; // what is the colon for? its called a "semantic binding" and it essentially tells the shader how its used in rendering
                float2 uv : TEXCOORD0; 
                // ^ TEXCOORD0 is essentially the first texture coordinate channel, not sure what its useful for atm
            };
            // essentially this passes in the verteces in a float4 and the float2 uv coordinates into the appData object

            struct v2f
            {
                float2 uv : TEXCOORD0; // more coordinates, this time for the
                float4 vertex : SV_POSITION; // SV_POSITION means sccreen space position, not sure why its a float4 doe
            };

            v2f vert (appdata v) // vertex function
            {
                v2f output; // create a new verts

                // while still being relative to the object:
                v.vertex.x += sin(_Time.y /* this is a value provided by UnityCG, it is a float4, and y is the time in seconds*/ * Speed + v.vertex.y * Amplitude) * Distance * Amount;

                output.vertex = UnityObjectToClipPos(v.vertex); // essentially turns the current object's local space into clip space, inherited from UnityCG.cginc 
                // see https://cdn.discordapp.com/attachments/740730093835649094/831993988244439040/unknown.png for help
                output.uv = TRANSFORM_TEX(v.uv, MainTex); // transforms the texture
                // this is where tiling and offset is applied



                return output;
            } // this gets passed into the frag function later on.

            fixed4 frag (v2f i) : SV_Target // fragment function, its bound to SV_Target which is a render target for the frame buffer
            {
                // sample the texture
                fixed4 color = tex2D(MainTex, i.uv); // create a new color, and pass in the albedo and the uv for the albedo
                // tex2D basically takes the color of a pixel based off the uv coordinate and draws it wherever
                 
                // multiply tint to output color
                color += TintColor;

                color.a = Transparency; // set alpha channel to transparency

                clip(color.r - CutoutThreshold); // used to discard certain pixel data (can be written as below as well)
                //if (col.r < CutoutThreshold)
                //    discard;

                return color;
            }
            ENDCG
        }
    }
}
