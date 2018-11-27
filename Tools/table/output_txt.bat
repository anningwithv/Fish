SET WORKSPACE=%~dp0
SET SOURCETABLEDIR=%~dp0/../../Tables/Sources/
SET TABLERESDIR=%~dp0/../../UnityProject/HeroAdventure/Assets/StreamingAssets/config/

cd %WORKSPACE%
%~dp0/convertxlsx.exe -i %SOURCETABLEDIR% -o %TABLERESDIR%

pause