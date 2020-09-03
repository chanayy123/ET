
cd ./ProtoTool
dotnet Proto2CS.dll
cd ../../Tools/MakePacketIdProto/
dotnet build 
cd ../../Proto/ProtoTool
dotnet MakePacketIdProto.dll

pause