@echo off
chcp 65001 > NUL

del /f /s /q D:\三国影游\SGYY\server\game\leenzee-game-data\src\main\java\com\leenzee\game\data\pb\*.*
protoc --proto_path=./protos/  --java_out=../server/game/leenzee-game-data/src/main/java/  ./protos/*.proto


pause