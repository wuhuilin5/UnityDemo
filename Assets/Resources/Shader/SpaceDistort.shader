Shader "Custom/SpaceDistort" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Distortion ("Distortion", Range(0.01,0.1)) = 0.02
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Overlay-8" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 GrabPass {
  "_myGrabTexture"
 }
 Pass {
  Tags { "QUEUE"="Overlay-8" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  ZWrite Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
"!!ARBvp1.0
# 13 ALU
PARAM c[6] = { { 0.5 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
TEMP R1;
DP4 R0.w, vertex.position, c[2];
DP4 R0.z, vertex.position, c[4];
DP4 R0.x, vertex.position, c[1];
MOV R1.w, R0.z;
DP4 R1.z, vertex.position, c[3];
MOV R1.x, R0;
MOV R0.y, R0.w;
MOV R1.y, R0.w;
ADD R0.xy, R0.z, R0;
MOV result.position, R1;
MOV result.texcoord[0].zw, R1;
MUL result.texcoord[0].xy, R0, c[0].x;
MAD result.texcoord[2].xy, vertex.texcoord[0], c[5], c[5].zwzw;
END
# 13 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
"vs_2_0
; 13 ALU
def c5, 0.50000000, 0, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
dp4 r0.w, v0, c1
dp4 r0.z, v0, c3
dp4 r0.x, v0, c0
mov r1.w, r0.z
dp4 r1.z, v0, c2
mov r1.x, r0
mov r0.y, -r0.w
mov r1.y, r0.w
add r0.xy, r0.z, r0
mov oPos, r1
mov oT0.zw, r1
mul oT0.xy, r0, c5.x
mad oT2.xy, v1, c4, c4.zwzw
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 48
Vector 32 [_MainTex_ST]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedclmigchgniheafgkmjokldngfanbecklabaaaaaalaacaaaaadaaaaaa
cmaaaaaaiaaaaaaapaaaaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa
fmaaaaaaacaaaaaaaaaaaaaaadaaaaaaacaaaaaaadamaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklfdeieefcliabaaaaeaaaabaagoaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagfaaaaaddccabaaaacaaaaaa
giaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
abaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
dgaaaaafpccabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaamdcaabaaaaaaaaaaa
egaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaaialpaaaaaaaaaaaaaaaapgapbaaa
aaaaaaaadgaaaaafmccabaaaabaaaaaakgaobaaaaaaaaaaadiaaaaakdccabaaa
abaaaaaaegaabaaaaaaaaaaaaceaaaaaaaaaaadpaaaaaadpaaaaaaaaaaaaaaaa
dcaaaaaldccabaaaacaaaaaaegbabaaaabaaaaaaegiacaaaaaaaaaaaacaaaaaa
ogikcaaaaaaaaaaaacaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 48
Vector 32 [_MainTex_ST]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedkeapjgoaafhpafhlhofdngeollmgchjcabaaaaaaoeadaaaaaeaaaaaa
daaaaaaagaabaaaacaadaaaaheadaaaaebgpgodjciabaaaaciabaaaaaaacpopp
oiaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaacaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaiadpaaaaialpaaaaaadpaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjaafaaaaadaaaaapiaaaaaffjaadaaoekaaeaaaaae
aaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoekaaaaakkja
aaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaaeabaaadia
aaaaoeiaagaaoekaaaaappiaafaaaaadaaaaadoaabaaoeiaagaakkkaaeaaaaae
abaaadoaabaaoejaabaaoekaabaaookaaeaaaaaeaaaaadmaaaaappiaaaaaoeka
aaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaacaaaaamoaaaaaoeiappppaaaa
fdeieefcliabaaaaeaaaabaagoaaaaaafjaaaaaeegiocaaaaaaaaaaaadaaaaaa
fjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaad
dcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaa
abaaaaaagfaaaaaddccabaaaacaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafpccabaaaaaaaaaaaegaobaaa
aaaaaaaadcaaaaamdcaabaaaaaaaaaaaegaabaaaaaaaaaaaaceaaaaaaaaaiadp
aaaaialpaaaaaaaaaaaaaaaapgapbaaaaaaaaaaadgaaaaafmccabaaaabaaaaaa
kgaobaaaaaaaaaaadiaaaaakdccabaaaabaaaaaaegaabaaaaaaaaaaaaceaaaaa
aaaaaadpaaaaaadpaaaaaaaaaaaaaaaadcaaaaaldccabaaaacaaaaaaegbabaaa
abaaaaaaegiacaaaaaaaaaaaacaaaaaaogikcaaaaaaaaaaaacaaaaaadoaaaaab
ejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
aaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaa
faepfdejfeejepeoaafeeffiedepepfceeaaklklepfdeheogiaaaaaaadaaaaaa
aiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaafmaaaaaaacaaaaaaaaaaaaaa
adaaaaaaacaaaaaaadamaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfcee
aaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
Float 0 [_Distortion]
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_myGrabTexture] 2D 1
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 5 ALU, 2 TEX
PARAM c[1] = { program.local[0] };
TEMP R0;
TEX R0.xy, fragment.texcoord[2], texture[0], 2D;
MUL R0.xy, R0, c[0].x;
MOV R0.z, fragment.texcoord[0].w;
MAD R0.xy, R0, fragment.texcoord[0].w, fragment.texcoord[0];
TXP result.color, R0.xyzz, texture[1], 2D;
END
# 5 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Float 0 [_Distortion]
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_myGrabTexture] 2D 1
"ps_2_0
; 4 ALU, 2 TEX
dcl_2d s0
dcl_2d s1
dcl t0
dcl t2.xy
texld r0, t2, s0
mul r0.xy, r0, c0.x
mov r0.zw, t0
mad r0.xy, r0, t0.w, t0
texldp r0, r0, s1
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 1
SetTexture 1 [_myGrabTexture] 2D 0
ConstBuffer "$Globals" 48
Float 16 [_Distortion]
BindCB  "$Globals" 0
"ps_4_0
eefiecedcjpoignmedaahooeihnpdoibflklkjdmabaaaaaaaaacaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapalaaaafmaaaaaaacaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adadaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcciabaaaaeaaaaaaaekaaaaaa
fjaaaaaeegiocaaaaaaaaaaaacaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaa
abaaaaaaffffaaaagcbaaaadlcbabaaaabaaaaaagcbaaaaddcbabaaaacaaaaaa
gfaaaaadpccabaaaaaaaaaaagiaaaaacabaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaacaaaaaaeghobaaaaaaaaaaaaagabaaaabaaaaaadiaaaaaidcaabaaa
aaaaaaaaegaabaaaaaaaaaaaagiacaaaaaaaaaaaabaaaaaadcaaaaajdcaabaaa
aaaaaaaaegaabaaaaaaaaaaapgbpbaaaabaaaaaaegbabaaaabaaaaaaaoaaaaah
dcaabaaaaaaaaaaaegaabaaaaaaaaaaapgbpbaaaabaaaaaaefaaaaajpccabaaa
aaaaaaaaegaabaaaaaaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaadoaaaaab
"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 1
SetTexture 1 [_myGrabTexture] 2D 0
ConstBuffer "$Globals" 48
Float 16 [_Distortion]
BindCB  "$Globals" 0
"ps_4_0_level_9_1
eefiecedmmoeibjcpejdbcppfacmalnedhonogllabaaaaaaoiacaaaaaeaaaaaa
daaaaaaabeabaaaaeeacaaaaleacaaaaebgpgodjnmaaaaaanmaaaaaaaaacpppp
keaaaaaadiaaaaaaabaacmaaaaaadiaaaaaadiaaacaaceaaaaaadiaaabaaaaaa
aaababaaaaaaabaaabaaaaaaaaaaaaaaaaacppppbpaaaaacaaaaaaiaaaaaapla
bpaaaaacaaaaaaiaabaaadlabpaaaaacaaaaaajaaaaiapkabpaaaaacaaaaaaja
abaiapkaecaaaaadaaaacpiaabaaoelaabaioekaafaaaaadaaaaadiaaaaaoeia
aaaaaakaaeaaaaaeaaaaadiaaaaaoeiaaaaapplaaaaaoelaagaaaaacaaaaaeia
aaaapplaafaaaaadaaaaadiaaaaakkiaaaaaoeiaecaaaaadaaaacpiaaaaaoeia
aaaioekaabaaaaacaaaicpiaaaaaoeiappppaaaafdeieefcciabaaaaeaaaaaaa
ekaaaaaafjaaaaaeegiocaaaaaaaaaaaacaaaaaafkaaaaadaagabaaaaaaaaaaa
fkaaaaadaagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaagcbaaaadlcbabaaaabaaaaaagcbaaaaddcbabaaa
acaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacabaaaaaaefaaaaajpcaabaaa
aaaaaaaaegbabaaaacaaaaaaeghobaaaaaaaaaaaaagabaaaabaaaaaadiaaaaai
dcaabaaaaaaaaaaaegaabaaaaaaaaaaaagiacaaaaaaaaaaaabaaaaaadcaaaaaj
dcaabaaaaaaaaaaaegaabaaaaaaaaaaapgbpbaaaabaaaaaaegbabaaaabaaaaaa
aoaaaaahdcaabaaaaaaaaaaaegaabaaaaaaaaaaapgbpbaaaabaaaaaaefaaaaaj
pccabaaaaaaaaaaaegaabaaaaaaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaa
doaaaaabejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaa
adaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaa
apalaaaafmaaaaaaacaaaaaaaaaaaaaaadaaaaaaacaaaaaaadadaaaafdfgfpfa
gphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaa
aiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfe
gbhcghgfheaaklkl"
}
}
 }
}
SubShader { 
 Pass {
  ZWrite Off
  Blend Zero One
  SetTexture [_MainTex] { combine texture }
 }
}
}