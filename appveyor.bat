setlocal

echo Building ReDock
cd src/ReDock
dotnet restore
dotnet build -c Release || goto error

goto :EOF

:error
echo Failed!
EXIT /b %ERRORLEVEL%