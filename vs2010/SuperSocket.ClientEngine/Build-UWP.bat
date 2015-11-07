@echo off

set fdir="%ProgramFiles%\MSBuild\14.0\Bin"

if not exist %fdir% (
	set fdir="%ProgramFiles(x86)%\MSBuild\14.0\Bin"
)

set msbuild=%fdir%\msbuild.exe


%msbuild% SuperSocket.ClientEngine.UWP.sln /p:Configuration=Debug /t:Clean;Rebuild /p:OutputPath=bin\UWP\Debug
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

%msbuild% SuperSocket.ClientEngine.UWP.sln /p:Configuration=Release /t:Clean;Rebuild /p:OutputPath=bin\UWP\Release
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

pause