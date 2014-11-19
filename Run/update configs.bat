@echo off
cd ./Debug/AuthServer/
Stump.GUI.AuthConsole.exe -config
cd ./../../
cd ./Debug/WorldServer/
Stump.GUI.WorldConsole.exe -config
