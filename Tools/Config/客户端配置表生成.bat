set WORKSPACE=..\..

set LUBAN_DLL=..\Tools\Luban\Luban.dll
set CONF_ROOT=..\Config

del /f /s /q ..\client\Assets\HotFIxUpdate\ConfigGen\*.*
del /f /s /q ..\client\Assets\GameRes\ConfigDatas\*.*

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-bin ^
    -d bin ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=..\client\Assets\HotFIxUpdate\ConfigGen\ ^
    -x outputDataDir=..\client\Assets\GameRes\ConfigDatas\

pause