@echo off
cd ..\Server
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
docker-compose -p=gameServer  down && docker-compose -p=gameServer  build && docker-compose -p=gameServer  up -d
pause