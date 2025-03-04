@echo off
chcp 65001 > NUL

del /f /s /q ..\client\Assets\Scripts\HotUpdate\PBGen\*.*
protoc --proto_path=./protos/ --csharp_out=../client/Assets/Scripts/HotUpdate/PBGen/ ./protos/*.proto

pause