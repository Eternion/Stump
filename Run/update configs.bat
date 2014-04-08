@echo off
cd ./Debug/AuthServer/
Stump.GUI.AuthConsole.exe -config
cd ../WorldServer/
Stump.GUI.WorldConsole.exe -config
