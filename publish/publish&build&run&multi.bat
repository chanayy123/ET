@echo off
cd ..\Server
dotnet restore
dotnet publish -c Release -r linux-x64 --self-contained false
cd ..\
if exist publish\publish (
	rd publish\publish /s /q
)
if exist publish\Config (
	rd publish\Config /s /q
)
xcopy /s /e Bin\linux-x64\publish\*.* publish\publish\
xcopy /s /e Config\*.* publish\Config\
if not exist publish\Logs (
   md publish\Logs
)
cd publish
docker-compose -f docker-compose-multi.yml -p gameserver down && docker-compose -f docker-compose-multi.yml -p gameserver build && docker-compose -f docker-compose-multi.yml -p gameserver up -d
pause