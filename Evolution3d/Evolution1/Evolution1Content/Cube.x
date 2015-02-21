xof 0302txt 0064
template Header {
 <3D82AB43-62DA-11cf-AB39-0020AF71E433>
 WORD major;
 WORD minor;
 DWORD flags;
}

template Vector {
 <3D82AB5E-62DA-11cf-AB39-0020AF71E433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template Coords2d {
 <F6F23F44-7686-11cf-8F52-0040333594A3>
 FLOAT u;
 FLOAT v;
}

template Matrix4x4 {
 <F6F23F45-7686-11cf-8F52-0040333594A3>
 array FLOAT matrix[16];
}

template ColorRGBA {
 <35FF44E0-6C7C-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <D3E16E81-7835-11cf-8F52-0040333594A3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template IndexedColor {
 <1630B820-7842-11cf-8F52-0040333594A3>
 DWORD index;
 ColorRGBA indexColor;
}

template Boolean {
 <4885AE61-78E8-11cf-8F52-0040333594A3>
 WORD truefalse;
}

template Boolean2d {
 <4885AE63-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template MaterialWrap {
 <4885AE60-78E8-11cf-8F52-0040333594A3>
 Boolean u;
 Boolean v;
}

template TextureFilename {
 <A42790E1-7810-11cf-8F52-0040333594A3>
 STRING filename;
}

template Material {
 <3D82AB4D-62DA-11cf-AB39-0020AF71E433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshFace {
 <3D82AB5F-62DA-11cf-AB39-0020AF71E433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template MeshFaceWraps {
 <4885AE62-78E8-11cf-8F52-0040333594A3>
 DWORD nFaceWrapValues;
 Boolean2d faceWrapValues;
}

template MeshTextureCoords {
 <F6F23F40-7686-11cf-8F52-0040333594A3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}

template MeshMaterialList {
 <F6F23F42-7686-11cf-8F52-0040333594A3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material]
}

template MeshNormals {
 <F6F23F43-7686-11cf-8F52-0040333594A3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template MeshVertexColors {
 <1630B821-7842-11cf-8F52-0040333594A3>
 DWORD nVertexColors;
 array IndexedColor vertexColors[nVertexColors];
}

template Mesh {
 <3D82AB44-62DA-11cf-AB39-0020AF71E433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

Header{
1;
0;
1;
}

Mesh {
 212;
 -0.50000;0.50000;0.50000;,
 -0.30000;0.50000;0.50000;,
 -0.30000;0.30000;0.50000;,
 -0.50000;0.30000;0.50000;,
 -0.10000;0.50000;0.50000;,
 -0.10000;0.30000;0.50000;,
 0.10000;0.50000;0.50000;,
 0.10000;0.30000;0.50000;,
 0.30000;0.50000;0.50000;,
 0.30000;0.30000;0.50000;,
 0.50000;0.50000;0.50000;,
 0.50000;0.30000;0.50000;,
 -0.30000;0.10000;0.50000;,
 -0.50000;0.10000;0.50000;,
 -0.10000;0.10000;0.50000;,
 0.10000;0.10000;0.50000;,
 0.30000;0.10000;0.50000;,
 0.50000;0.10000;0.50000;,
 -0.30000;-0.10000;0.50000;,
 -0.50000;-0.10000;0.50000;,
 -0.10000;-0.10000;0.50000;,
 0.10000;-0.10000;0.50000;,
 0.30000;-0.10000;0.50000;,
 0.50000;-0.10000;0.50000;,
 -0.30000;-0.30000;0.50000;,
 -0.50000;-0.30000;0.50000;,
 -0.10000;-0.30000;0.50000;,
 0.10000;-0.30000;0.50000;,
 0.30000;-0.30000;0.50000;,
 0.50000;-0.30000;0.50000;,
 -0.30000;-0.50000;0.50000;,
 -0.50000;-0.50000;0.50000;,
 -0.10000;-0.50000;0.50000;,
 0.10000;-0.50000;0.50000;,
 0.30000;-0.50000;0.50000;,
 0.50000;-0.50000;0.50000;,
 0.50000;0.50000;0.50000;,
 0.50000;0.50000;0.30000;,
 0.50000;0.30000;0.30000;,
 0.50000;0.30000;0.50000;,
 0.50000;0.50000;0.10000;,
 0.50000;0.30000;0.10000;,
 0.50000;0.50000;-0.10000;,
 0.50000;0.30000;-0.10000;,
 0.50000;0.50000;-0.30000;,
 0.50000;0.30000;-0.30000;,
 0.50000;0.50000;-0.50000;,
 0.50000;0.30000;-0.50000;,
 0.50000;0.10000;0.30000;,
 0.50000;0.10000;0.50000;,
 0.50000;0.10000;0.10000;,
 0.50000;0.10000;-0.10000;,
 0.50000;0.10000;-0.30000;,
 0.50000;0.10000;-0.50000;,
 0.50000;-0.10000;0.30000;,
 0.50000;-0.10000;0.50000;,
 0.50000;-0.10000;0.10000;,
 0.50000;-0.10000;-0.10000;,
 0.50000;-0.10000;-0.30000;,
 0.50000;-0.10000;-0.50000;,
 0.50000;-0.30000;0.30000;,
 0.50000;-0.30000;0.50000;,
 0.50000;-0.30000;0.10000;,
 0.50000;-0.30000;-0.10000;,
 0.50000;-0.30000;-0.30000;,
 0.50000;-0.30000;-0.50000;,
 0.50000;-0.50000;0.30000;,
 0.50000;-0.50000;0.50000;,
 0.50000;-0.50000;0.10000;,
 0.50000;-0.50000;-0.10000;,
 0.50000;-0.50000;-0.30000;,
 0.50000;-0.50000;-0.50000;,
 0.50000;0.50000;-0.50000;,
 0.30000;0.50000;-0.50000;,
 0.30000;0.30000;-0.50000;,
 0.50000;0.30000;-0.50000;,
 0.10000;0.50000;-0.50000;,
 0.10000;0.30000;-0.50000;,
 -0.10000;0.50000;-0.50000;,
 -0.10000;0.30000;-0.50000;,
 -0.30000;0.50000;-0.50000;,
 -0.30000;0.30000;-0.50000;,
 -0.50000;0.50000;-0.50000;,
 -0.50000;0.30000;-0.50000;,
 0.30000;0.10000;-0.50000;,
 0.50000;0.10000;-0.50000;,
 0.10000;0.10000;-0.50000;,
 -0.10000;0.10000;-0.50000;,
 -0.30000;0.10000;-0.50000;,
 -0.50000;0.10000;-0.50000;,
 0.30000;-0.10000;-0.50000;,
 0.50000;-0.10000;-0.50000;,
 0.10000;-0.10000;-0.50000;,
 -0.10000;-0.10000;-0.50000;,
 -0.30000;-0.10000;-0.50000;,
 -0.50000;-0.10000;-0.50000;,
 0.30000;-0.30000;-0.50000;,
 0.50000;-0.30000;-0.50000;,
 0.10000;-0.30000;-0.50000;,
 -0.10000;-0.30000;-0.50000;,
 -0.30000;-0.30000;-0.50000;,
 -0.50000;-0.30000;-0.50000;,
 0.30000;-0.50000;-0.50000;,
 0.50000;-0.50000;-0.50000;,
 0.10000;-0.50000;-0.50000;,
 -0.10000;-0.50000;-0.50000;,
 -0.30000;-0.50000;-0.50000;,
 -0.50000;-0.50000;-0.50000;,
 -0.50000;0.50000;-0.50000;,
 -0.50000;0.50000;-0.30000;,
 -0.50000;0.30000;-0.30000;,
 -0.50000;0.30000;-0.50000;,
 -0.50000;0.50000;-0.10000;,
 -0.50000;0.30000;-0.10000;,
 -0.50000;0.50000;0.10000;,
 -0.50000;0.30000;0.10000;,
 -0.50000;0.50000;0.30000;,
 -0.50000;0.30000;0.30000;,
 -0.50000;0.50000;0.50000;,
 -0.50000;0.30000;0.50000;,
 -0.50000;0.10000;-0.30000;,
 -0.50000;0.10000;-0.50000;,
 -0.50000;0.10000;-0.10000;,
 -0.50000;0.10000;0.10000;,
 -0.50000;0.10000;0.30000;,
 -0.50000;0.10000;0.50000;,
 -0.50000;-0.10000;-0.30000;,
 -0.50000;-0.10000;-0.50000;,
 -0.50000;-0.10000;-0.10000;,
 -0.50000;-0.10000;0.10000;,
 -0.50000;-0.10000;0.30000;,
 -0.50000;-0.10000;0.50000;,
 -0.50000;-0.30000;-0.30000;,
 -0.50000;-0.30000;-0.50000;,
 -0.50000;-0.30000;-0.10000;,
 -0.50000;-0.30000;0.10000;,
 -0.50000;-0.30000;0.30000;,
 -0.50000;-0.30000;0.50000;,
 -0.50000;-0.50000;-0.30000;,
 -0.50000;-0.50000;-0.50000;,
 -0.50000;-0.50000;-0.10000;,
 -0.50000;-0.50000;0.10000;,
 -0.50000;-0.50000;0.30000;,
 -0.50000;-0.50000;0.50000;,
 -0.30000;0.50000;-0.50000;,
 -0.30000;0.50000;-0.30000;,
 -0.50000;0.50000;-0.30000;,
 -0.10000;0.50000;-0.50000;,
 -0.10000;0.50000;-0.30000;,
 0.10000;0.50000;-0.50000;,
 0.10000;0.50000;-0.30000;,
 0.30000;0.50000;-0.50000;,
 0.30000;0.50000;-0.30000;,
 0.50000;0.50000;-0.30000;,
 -0.30000;0.50000;-0.10000;,
 -0.50000;0.50000;-0.10000;,
 -0.10000;0.50000;-0.10000;,
 0.10000;0.50000;-0.10000;,
 0.30000;0.50000;-0.10000;,
 0.50000;0.50000;-0.10000;,
 -0.30000;0.50000;0.10000;,
 -0.50000;0.50000;0.10000;,
 -0.10000;0.50000;0.10000;,
 0.10000;0.50000;0.10000;,
 0.30000;0.50000;0.10000;,
 0.50000;0.50000;0.10000;,
 -0.30000;0.50000;0.30000;,
 -0.50000;0.50000;0.30000;,
 -0.10000;0.50000;0.30000;,
 0.10000;0.50000;0.30000;,
 0.30000;0.50000;0.30000;,
 0.50000;0.50000;0.30000;,
 -0.30000;0.50000;0.50000;,
 -0.50000;0.50000;0.50000;,
 -0.10000;0.50000;0.50000;,
 0.10000;0.50000;0.50000;,
 0.30000;0.50000;0.50000;,
 0.50000;0.50000;0.50000;,
 -0.50000;-0.50000;-0.30000;,
 -0.30000;-0.50000;-0.30000;,
 -0.30000;-0.50000;-0.50000;,
 -0.10000;-0.50000;-0.30000;,
 -0.10000;-0.50000;-0.50000;,
 0.10000;-0.50000;-0.30000;,
 0.10000;-0.50000;-0.50000;,
 0.30000;-0.50000;-0.30000;,
 0.30000;-0.50000;-0.50000;,
 0.50000;-0.50000;-0.30000;,
 -0.50000;-0.50000;-0.10000;,
 -0.30000;-0.50000;-0.10000;,
 -0.10000;-0.50000;-0.10000;,
 0.10000;-0.50000;-0.10000;,
 0.30000;-0.50000;-0.10000;,
 0.50000;-0.50000;-0.10000;,
 -0.50000;-0.50000;0.10000;,
 -0.30000;-0.50000;0.10000;,
 -0.10000;-0.50000;0.10000;,
 0.10000;-0.50000;0.10000;,
 0.30000;-0.50000;0.10000;,
 0.50000;-0.50000;0.10000;,
 -0.50000;-0.50000;0.30000;,
 -0.30000;-0.50000;0.30000;,
 -0.10000;-0.50000;0.30000;,
 0.10000;-0.50000;0.30000;,
 0.30000;-0.50000;0.30000;,
 0.50000;-0.50000;0.30000;,
 -0.50000;-0.50000;0.50000;,
 -0.30000;-0.50000;0.50000;,
 -0.10000;-0.50000;0.50000;,
 0.10000;-0.50000;0.50000;,
 0.30000;-0.50000;0.50000;,
 0.50000;-0.50000;0.50000;;
 
 150;
 4;3,2,1,0;,
 4;2,5,4,1;,
 4;5,7,6,4;,
 4;7,9,8,6;,
 4;9,11,10,8;,
 4;13,12,2,3;,
 4;12,14,5,2;,
 4;14,15,7,5;,
 4;15,16,9,7;,
 4;16,17,11,9;,
 4;19,18,12,13;,
 4;18,20,14,12;,
 4;20,21,15,14;,
 4;21,22,16,15;,
 4;22,23,17,16;,
 4;25,24,18,19;,
 4;24,26,20,18;,
 4;26,27,21,20;,
 4;27,28,22,21;,
 4;28,29,23,22;,
 4;31,30,24,25;,
 4;30,32,26,24;,
 4;32,33,27,26;,
 4;33,34,28,27;,
 4;34,35,29,28;,
 4;39,38,37,36;,
 4;38,41,40,37;,
 4;41,43,42,40;,
 4;43,45,44,42;,
 4;45,47,46,44;,
 4;49,48,38,39;,
 4;48,50,41,38;,
 4;50,51,43,41;,
 4;51,52,45,43;,
 4;52,53,47,45;,
 4;55,54,48,49;,
 4;54,56,50,48;,
 4;56,57,51,50;,
 4;57,58,52,51;,
 4;58,59,53,52;,
 4;61,60,54,55;,
 4;60,62,56,54;,
 4;62,63,57,56;,
 4;63,64,58,57;,
 4;64,65,59,58;,
 4;67,66,60,61;,
 4;66,68,62,60;,
 4;68,69,63,62;,
 4;69,70,64,63;,
 4;70,71,65,64;,
 4;75,74,73,72;,
 4;74,77,76,73;,
 4;77,79,78,76;,
 4;79,81,80,78;,
 4;81,83,82,80;,
 4;85,84,74,75;,
 4;84,86,77,74;,
 4;86,87,79,77;,
 4;87,88,81,79;,
 4;88,89,83,81;,
 4;91,90,84,85;,
 4;90,92,86,84;,
 4;92,93,87,86;,
 4;93,94,88,87;,
 4;94,95,89,88;,
 4;97,96,90,91;,
 4;96,98,92,90;,
 4;98,99,93,92;,
 4;99,100,94,93;,
 4;100,101,95,94;,
 4;103,102,96,97;,
 4;102,104,98,96;,
 4;104,105,99,98;,
 4;105,106,100,99;,
 4;106,107,101,100;,
 4;111,110,109,108;,
 4;110,113,112,109;,
 4;113,115,114,112;,
 4;115,117,116,114;,
 4;117,119,118,116;,
 4;121,120,110,111;,
 4;120,122,113,110;,
 4;122,123,115,113;,
 4;123,124,117,115;,
 4;124,125,119,117;,
 4;127,126,120,121;,
 4;126,128,122,120;,
 4;128,129,123,122;,
 4;129,130,124,123;,
 4;130,131,125,124;,
 4;133,132,126,127;,
 4;132,134,128,126;,
 4;134,135,129,128;,
 4;135,136,130,129;,
 4;136,137,131,130;,
 4;139,138,132,133;,
 4;138,140,134,132;,
 4;140,141,135,134;,
 4;141,142,136,135;,
 4;142,143,137,136;,
 4;146,145,144,108;,
 4;145,148,147,144;,
 4;148,150,149,147;,
 4;150,152,151,149;,
 4;152,153,46,151;,
 4;155,154,145,146;,
 4;154,156,148,145;,
 4;156,157,150,148;,
 4;157,158,152,150;,
 4;158,159,153,152;,
 4;161,160,154,155;,
 4;160,162,156,154;,
 4;162,163,157,156;,
 4;163,164,158,157;,
 4;164,165,159,158;,
 4;167,166,160,161;,
 4;166,168,162,160;,
 4;168,169,163,162;,
 4;169,170,164,163;,
 4;170,171,165,164;,
 4;173,172,166,167;,
 4;172,174,168,166;,
 4;174,175,169,168;,
 4;175,176,170,169;,
 4;176,177,171,170;,
 4;139,180,179,178;,
 4;180,182,181,179;,
 4;182,184,183,181;,
 4;184,186,185,183;,
 4;186,71,187,185;,
 4;178,179,189,188;,
 4;179,181,190,189;,
 4;181,183,191,190;,
 4;183,185,192,191;,
 4;185,187,193,192;,
 4;188,189,195,194;,
 4;189,190,196,195;,
 4;190,191,197,196;,
 4;191,192,198,197;,
 4;192,193,199,198;,
 4;194,195,201,200;,
 4;195,196,202,201;,
 4;196,197,203,202;,
 4;197,198,204,203;,
 4;198,199,205,204;,
 4;200,201,207,206;,
 4;201,202,208,207;,
 4;202,203,209,208;,
 4;203,204,210,209;,
 4;204,205,211,210;;
 
 MeshMaterialList {
  1;
  150;
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0,
  0;;
  Material {
   0.800000;0.800000;0.800000;1.000000;;
   5.000000;
   0.000000;0.000000;0.000000;;
   0.000000;0.000000;0.000000;;
   TextureFilename {
    "5.png";
   }
  }
 }
 MeshTextureCoords {
  212;
  0.000000;0.000000;,
  0.200000;0.000000;,
  0.200000;0.200000;,
  0.000000;0.200000;,
  0.400000;0.000000;,
  0.400000;0.200000;,
  0.600000;0.000000;,
  0.600000;0.200000;,
  0.800000;0.000000;,
  0.800000;0.200000;,
  1.000000;0.000000;,
  1.000000;0.200000;,
  0.200000;0.400000;,
  0.000000;0.400000;,
  0.400000;0.400000;,
  0.600000;0.400000;,
  0.800000;0.400000;,
  1.000000;0.400000;,
  0.200000;0.600000;,
  0.000000;0.600000;,
  0.400000;0.600000;,
  0.600000;0.600000;,
  0.800000;0.600000;,
  1.000000;0.600000;,
  0.200000;0.800000;,
  0.000000;0.800000;,
  0.400000;0.800000;,
  0.600000;0.800000;,
  0.800000;0.800000;,
  1.000000;0.800000;,
  0.200000;1.000000;,
  0.000000;1.000000;,
  0.400000;1.000000;,
  0.600000;1.000000;,
  0.800000;1.000000;,
  1.000000;1.000000;,
  0.000000;0.000000;,
  0.200000;0.000000;,
  0.200000;0.200000;,
  0.000000;0.200000;,
  0.400000;0.000000;,
  0.400000;0.200000;,
  0.600000;0.000000;,
  0.600000;0.200000;,
  0.800000;0.000000;,
  0.800000;0.200000;,
  1.000000;0.000000;,
  1.000000;0.200000;,
  0.200000;0.400000;,
  0.000000;0.400000;,
  0.400000;0.400000;,
  0.600000;0.400000;,
  0.800000;0.400000;,
  1.000000;0.400000;,
  0.200000;0.600000;,
  0.000000;0.600000;,
  0.400000;0.600000;,
  0.600000;0.600000;,
  0.800000;0.600000;,
  1.000000;0.600000;,
  0.200000;0.800000;,
  0.000000;0.800000;,
  0.400000;0.800000;,
  0.600000;0.800000;,
  0.800000;0.800000;,
  1.000000;0.800000;,
  0.200000;1.000000;,
  0.000000;1.000000;,
  0.400000;1.000000;,
  0.600000;1.000000;,
  0.800000;1.000000;,
  1.000000;1.000000;,
  0.000000;0.000000;,
  0.200000;0.000000;,
  0.200000;0.200000;,
  0.000000;0.200000;,
  0.400000;0.000000;,
  0.400000;0.200000;,
  0.600000;0.000000;,
  0.600000;0.200000;,
  0.800000;0.000000;,
  0.800000;0.200000;,
  1.000000;0.000000;,
  1.000000;0.200000;,
  0.200000;0.400000;,
  0.000000;0.400000;,
  0.400000;0.400000;,
  0.600000;0.400000;,
  0.800000;0.400000;,
  1.000000;0.400000;,
  0.200000;0.600000;,
  0.000000;0.600000;,
  0.400000;0.600000;,
  0.600000;0.600000;,
  0.800000;0.600000;,
  1.000000;0.600000;,
  0.200000;0.800000;,
  0.000000;0.800000;,
  0.400000;0.800000;,
  0.600000;0.800000;,
  0.800000;0.800000;,
  1.000000;0.800000;,
  0.200000;1.000000;,
  0.000000;1.000000;,
  0.400000;1.000000;,
  0.600000;1.000000;,
  0.800000;1.000000;,
  1.000000;1.000000;,
  0.000000;0.000000;,
  0.200000;0.000000;,
  0.200000;0.200000;,
  0.000000;0.200000;,
  0.400000;0.000000;,
  0.400000;0.200000;,
  0.600000;0.000000;,
  0.600000;0.200000;,
  0.800000;0.000000;,
  0.800000;0.200000;,
  1.000000;0.000000;,
  1.000000;0.200000;,
  0.200000;0.400000;,
  0.000000;0.400000;,
  0.400000;0.400000;,
  0.600000;0.400000;,
  0.800000;0.400000;,
  1.000000;0.400000;,
  0.200000;0.600000;,
  0.000000;0.600000;,
  0.400000;0.600000;,
  0.600000;0.600000;,
  0.800000;0.600000;,
  1.000000;0.600000;,
  0.200000;0.800000;,
  0.000000;0.800000;,
  0.400000;0.800000;,
  0.600000;0.800000;,
  0.800000;0.800000;,
  1.000000;0.800000;,
  0.200000;1.000000;,
  0.000000;1.000000;,
  0.400000;1.000000;,
  0.600000;1.000000;,
  0.800000;1.000000;,
  1.000000;1.000000;,
  0.200000;0.000000;,
  0.200000;0.200000;,
  0.000000;0.200000;,
  0.400000;0.000000;,
  0.400000;0.200000;,
  0.600000;0.000000;,
  0.600000;0.200000;,
  0.800000;0.000000;,
  0.800000;0.200000;,
  1.000000;0.200000;,
  0.200000;0.400000;,
  0.000000;0.400000;,
  0.400000;0.400000;,
  0.600000;0.400000;,
  0.800000;0.400000;,
  1.000000;0.400000;,
  0.200000;0.600000;,
  0.000000;0.600000;,
  0.400000;0.600000;,
  0.600000;0.600000;,
  0.800000;0.600000;,
  1.000000;0.600000;,
  0.200000;0.800000;,
  0.000000;0.800000;,
  0.400000;0.800000;,
  0.600000;0.800000;,
  0.800000;0.800000;,
  1.000000;0.800000;,
  0.200000;1.000000;,
  0.000000;1.000000;,
  0.400000;1.000000;,
  0.600000;1.000000;,
  0.800000;1.000000;,
  1.000000;1.000000;,
  0.000000;0.800000;,
  0.200000;0.800000;,
  0.200000;1.000000;,
  0.400000;0.800000;,
  0.400000;1.000000;,
  0.600000;0.800000;,
  0.600000;1.000000;,
  0.800000;0.800000;,
  0.800000;1.000000;,
  1.000000;0.800000;,
  0.000000;0.600000;,
  0.200000;0.600000;,
  0.400000;0.600000;,
  0.600000;0.600000;,
  0.800000;0.600000;,
  1.000000;0.600000;,
  0.000000;0.400000;,
  0.200000;0.400000;,
  0.400000;0.400000;,
  0.600000;0.400000;,
  0.800000;0.400000;,
  1.000000;0.400000;,
  0.000000;0.200000;,
  0.200000;0.200000;,
  0.400000;0.200000;,
  0.600000;0.200000;,
  0.800000;0.200000;,
  1.000000;0.200000;,
  0.000000;0.000000;,
  0.200000;0.000000;,
  0.400000;0.000000;,
  0.600000;0.000000;,
  0.800000;0.000000;,
  1.000000;0.000000;;
 }
 MeshVertexColors {
  212;
  0;1.000000;1.000000;1.000000;1.000000;,
  1;1.000000;1.000000;1.000000;1.000000;,
  2;1.000000;1.000000;1.000000;1.000000;,
  3;1.000000;1.000000;1.000000;1.000000;,
  4;1.000000;1.000000;1.000000;1.000000;,
  5;1.000000;1.000000;1.000000;1.000000;,
  6;1.000000;1.000000;1.000000;1.000000;,
  7;1.000000;1.000000;1.000000;1.000000;,
  8;1.000000;1.000000;1.000000;1.000000;,
  9;1.000000;1.000000;1.000000;1.000000;,
  10;1.000000;1.000000;1.000000;1.000000;,
  11;1.000000;1.000000;1.000000;1.000000;,
  12;1.000000;1.000000;1.000000;1.000000;,
  13;1.000000;1.000000;1.000000;1.000000;,
  14;1.000000;1.000000;1.000000;1.000000;,
  15;1.000000;1.000000;1.000000;1.000000;,
  16;1.000000;1.000000;1.000000;1.000000;,
  17;1.000000;1.000000;1.000000;1.000000;,
  18;1.000000;1.000000;1.000000;1.000000;,
  19;1.000000;1.000000;1.000000;1.000000;,
  20;1.000000;1.000000;1.000000;1.000000;,
  21;1.000000;1.000000;1.000000;1.000000;,
  22;1.000000;1.000000;1.000000;1.000000;,
  23;1.000000;1.000000;1.000000;1.000000;,
  24;1.000000;1.000000;1.000000;1.000000;,
  25;1.000000;1.000000;1.000000;1.000000;,
  26;1.000000;1.000000;1.000000;1.000000;,
  27;1.000000;1.000000;1.000000;1.000000;,
  28;1.000000;1.000000;1.000000;1.000000;,
  29;1.000000;1.000000;1.000000;1.000000;,
  30;1.000000;1.000000;1.000000;1.000000;,
  31;1.000000;1.000000;1.000000;1.000000;,
  32;1.000000;1.000000;1.000000;1.000000;,
  33;1.000000;1.000000;1.000000;1.000000;,
  34;1.000000;1.000000;1.000000;1.000000;,
  35;1.000000;1.000000;1.000000;1.000000;,
  36;1.000000;1.000000;1.000000;1.000000;,
  37;1.000000;1.000000;1.000000;1.000000;,
  38;1.000000;1.000000;1.000000;1.000000;,
  39;1.000000;1.000000;1.000000;1.000000;,
  40;1.000000;1.000000;1.000000;1.000000;,
  41;1.000000;1.000000;1.000000;1.000000;,
  42;1.000000;1.000000;1.000000;1.000000;,
  43;1.000000;1.000000;1.000000;1.000000;,
  44;1.000000;1.000000;1.000000;1.000000;,
  45;1.000000;1.000000;1.000000;1.000000;,
  46;1.000000;1.000000;1.000000;1.000000;,
  47;1.000000;1.000000;1.000000;1.000000;,
  48;1.000000;1.000000;1.000000;1.000000;,
  49;1.000000;1.000000;1.000000;1.000000;,
  50;1.000000;1.000000;1.000000;1.000000;,
  51;1.000000;1.000000;1.000000;1.000000;,
  52;1.000000;1.000000;1.000000;1.000000;,
  53;1.000000;1.000000;1.000000;1.000000;,
  54;1.000000;1.000000;1.000000;1.000000;,
  55;1.000000;1.000000;1.000000;1.000000;,
  56;1.000000;1.000000;1.000000;1.000000;,
  57;1.000000;1.000000;1.000000;1.000000;,
  58;1.000000;1.000000;1.000000;1.000000;,
  59;1.000000;1.000000;1.000000;1.000000;,
  60;1.000000;1.000000;1.000000;1.000000;,
  61;1.000000;1.000000;1.000000;1.000000;,
  62;1.000000;1.000000;1.000000;1.000000;,
  63;1.000000;1.000000;1.000000;1.000000;,
  64;1.000000;1.000000;1.000000;1.000000;,
  65;1.000000;1.000000;1.000000;1.000000;,
  66;1.000000;1.000000;1.000000;1.000000;,
  67;1.000000;1.000000;1.000000;1.000000;,
  68;1.000000;1.000000;1.000000;1.000000;,
  69;1.000000;1.000000;1.000000;1.000000;,
  70;1.000000;1.000000;1.000000;1.000000;,
  71;1.000000;1.000000;1.000000;1.000000;,
  72;1.000000;1.000000;1.000000;1.000000;,
  73;1.000000;1.000000;1.000000;1.000000;,
  74;1.000000;1.000000;1.000000;1.000000;,
  75;1.000000;1.000000;1.000000;1.000000;,
  76;1.000000;1.000000;1.000000;1.000000;,
  77;1.000000;1.000000;1.000000;1.000000;,
  78;1.000000;1.000000;1.000000;1.000000;,
  79;1.000000;1.000000;1.000000;1.000000;,
  80;1.000000;1.000000;1.000000;1.000000;,
  81;1.000000;1.000000;1.000000;1.000000;,
  82;1.000000;1.000000;1.000000;1.000000;,
  83;1.000000;1.000000;1.000000;1.000000;,
  84;1.000000;1.000000;1.000000;1.000000;,
  85;1.000000;1.000000;1.000000;1.000000;,
  86;1.000000;1.000000;1.000000;1.000000;,
  87;1.000000;1.000000;1.000000;1.000000;,
  88;1.000000;1.000000;1.000000;1.000000;,
  89;1.000000;1.000000;1.000000;1.000000;,
  90;1.000000;1.000000;1.000000;1.000000;,
  91;1.000000;1.000000;1.000000;1.000000;,
  92;1.000000;1.000000;1.000000;1.000000;,
  93;1.000000;1.000000;1.000000;1.000000;,
  94;1.000000;1.000000;1.000000;1.000000;,
  95;1.000000;1.000000;1.000000;1.000000;,
  96;1.000000;1.000000;1.000000;1.000000;,
  97;1.000000;1.000000;1.000000;1.000000;,
  98;1.000000;1.000000;1.000000;1.000000;,
  99;1.000000;1.000000;1.000000;1.000000;,
  100;1.000000;1.000000;1.000000;1.000000;,
  101;1.000000;1.000000;1.000000;1.000000;,
  102;1.000000;1.000000;1.000000;1.000000;,
  103;1.000000;1.000000;1.000000;1.000000;,
  104;1.000000;1.000000;1.000000;1.000000;,
  105;1.000000;1.000000;1.000000;1.000000;,
  106;1.000000;1.000000;1.000000;1.000000;,
  107;1.000000;1.000000;1.000000;1.000000;,
  108;1.000000;1.000000;1.000000;1.000000;,
  109;1.000000;1.000000;1.000000;1.000000;,
  110;1.000000;1.000000;1.000000;1.000000;,
  111;1.000000;1.000000;1.000000;1.000000;,
  112;1.000000;1.000000;1.000000;1.000000;,
  113;1.000000;1.000000;1.000000;1.000000;,
  114;1.000000;1.000000;1.000000;1.000000;,
  115;1.000000;1.000000;1.000000;1.000000;,
  116;1.000000;1.000000;1.000000;1.000000;,
  117;1.000000;1.000000;1.000000;1.000000;,
  118;1.000000;1.000000;1.000000;1.000000;,
  119;1.000000;1.000000;1.000000;1.000000;,
  120;1.000000;1.000000;1.000000;1.000000;,
  121;1.000000;1.000000;1.000000;1.000000;,
  122;1.000000;1.000000;1.000000;1.000000;,
  123;1.000000;1.000000;1.000000;1.000000;,
  124;1.000000;1.000000;1.000000;1.000000;,
  125;1.000000;1.000000;1.000000;1.000000;,
  126;1.000000;1.000000;1.000000;1.000000;,
  127;1.000000;1.000000;1.000000;1.000000;,
  128;1.000000;1.000000;1.000000;1.000000;,
  129;1.000000;1.000000;1.000000;1.000000;,
  130;1.000000;1.000000;1.000000;1.000000;,
  131;1.000000;1.000000;1.000000;1.000000;,
  132;1.000000;1.000000;1.000000;1.000000;,
  133;1.000000;1.000000;1.000000;1.000000;,
  134;1.000000;1.000000;1.000000;1.000000;,
  135;1.000000;1.000000;1.000000;1.000000;,
  136;1.000000;1.000000;1.000000;1.000000;,
  137;1.000000;1.000000;1.000000;1.000000;,
  138;1.000000;1.000000;1.000000;1.000000;,
  139;1.000000;1.000000;1.000000;1.000000;,
  140;1.000000;1.000000;1.000000;1.000000;,
  141;1.000000;1.000000;1.000000;1.000000;,
  142;1.000000;1.000000;1.000000;1.000000;,
  143;1.000000;1.000000;1.000000;1.000000;,
  144;1.000000;1.000000;1.000000;1.000000;,
  145;1.000000;1.000000;1.000000;1.000000;,
  146;1.000000;1.000000;1.000000;1.000000;,
  147;1.000000;1.000000;1.000000;1.000000;,
  148;1.000000;1.000000;1.000000;1.000000;,
  149;1.000000;1.000000;1.000000;1.000000;,
  150;1.000000;1.000000;1.000000;1.000000;,
  151;1.000000;1.000000;1.000000;1.000000;,
  152;1.000000;1.000000;1.000000;1.000000;,
  153;1.000000;1.000000;1.000000;1.000000;,
  154;1.000000;1.000000;1.000000;1.000000;,
  155;1.000000;1.000000;1.000000;1.000000;,
  156;1.000000;1.000000;1.000000;1.000000;,
  157;1.000000;1.000000;1.000000;1.000000;,
  158;1.000000;1.000000;1.000000;1.000000;,
  159;1.000000;1.000000;1.000000;1.000000;,
  160;1.000000;1.000000;1.000000;1.000000;,
  161;1.000000;1.000000;1.000000;1.000000;,
  162;1.000000;1.000000;1.000000;1.000000;,
  163;1.000000;1.000000;1.000000;1.000000;,
  164;1.000000;1.000000;1.000000;1.000000;,
  165;1.000000;1.000000;1.000000;1.000000;,
  166;1.000000;1.000000;1.000000;1.000000;,
  167;1.000000;1.000000;1.000000;1.000000;,
  168;1.000000;1.000000;1.000000;1.000000;,
  169;1.000000;1.000000;1.000000;1.000000;,
  170;1.000000;1.000000;1.000000;1.000000;,
  171;1.000000;1.000000;1.000000;1.000000;,
  172;1.000000;1.000000;1.000000;1.000000;,
  173;1.000000;1.000000;1.000000;1.000000;,
  174;1.000000;1.000000;1.000000;1.000000;,
  175;1.000000;1.000000;1.000000;1.000000;,
  176;1.000000;1.000000;1.000000;1.000000;,
  177;1.000000;1.000000;1.000000;1.000000;,
  178;1.000000;1.000000;1.000000;1.000000;,
  179;1.000000;1.000000;1.000000;1.000000;,
  180;1.000000;1.000000;1.000000;1.000000;,
  181;1.000000;1.000000;1.000000;1.000000;,
  182;1.000000;1.000000;1.000000;1.000000;,
  183;1.000000;1.000000;1.000000;1.000000;,
  184;1.000000;1.000000;1.000000;1.000000;,
  185;1.000000;1.000000;1.000000;1.000000;,
  186;1.000000;1.000000;1.000000;1.000000;,
  187;1.000000;1.000000;1.000000;1.000000;,
  188;1.000000;1.000000;1.000000;1.000000;,
  189;1.000000;1.000000;1.000000;1.000000;,
  190;1.000000;1.000000;1.000000;1.000000;,
  191;1.000000;1.000000;1.000000;1.000000;,
  192;1.000000;1.000000;1.000000;1.000000;,
  193;1.000000;1.000000;1.000000;1.000000;,
  194;1.000000;1.000000;1.000000;1.000000;,
  195;1.000000;1.000000;1.000000;1.000000;,
  196;1.000000;1.000000;1.000000;1.000000;,
  197;1.000000;1.000000;1.000000;1.000000;,
  198;1.000000;1.000000;1.000000;1.000000;,
  199;1.000000;1.000000;1.000000;1.000000;,
  200;1.000000;1.000000;1.000000;1.000000;,
  201;1.000000;1.000000;1.000000;1.000000;,
  202;1.000000;1.000000;1.000000;1.000000;,
  203;1.000000;1.000000;1.000000;1.000000;,
  204;1.000000;1.000000;1.000000;1.000000;,
  205;1.000000;1.000000;1.000000;1.000000;,
  206;1.000000;1.000000;1.000000;1.000000;,
  207;1.000000;1.000000;1.000000;1.000000;,
  208;1.000000;1.000000;1.000000;1.000000;,
  209;1.000000;1.000000;1.000000;1.000000;,
  210;1.000000;1.000000;1.000000;1.000000;,
  211;1.000000;1.000000;1.000000;1.000000;;
 }
}
