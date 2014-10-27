@echo off
cd %~dp0

::csc.exe文件所在目录
set msvs_path=C:\Windows\Microsoft.NET\Framework\v4.0.30319\

::输出文件
set out_dll=U3DClientData.dll

::应用到的dll文件
set LitJson_dll=../../Plugin/LitJson.dll
set UnityEngine_dll="D:\Program Files (x86)\Unity\Editor\Data\Managed\UnityEngine.dll"

"%msvs_path%csc.exe" /out:%out_dll% /r:%LitJson_dll%;%UnityEngine_dll% /t:library *.cs

copy /y %out_dll% ..\..\Plugin\

del %out_dll%

echo build %out_dll% ok!

pause.