@echo off
setlocal enabledelayedexpansion

echo.

for /f "tokens=2*" %%a in ('git lfs ls-files') do (
    set "filepath=%%b"
    set "filepath=!filepath:/=\!"
    
    if exist "!filepath!" (
        echo Isleniyor: !filepath!
        git lfs smudge < "!filepath!" > "!filepath!.tmp"
        move /Y "!filepath!.tmp" "!filepath!" >nul
    )
)

echo.
echo Tamamlandi!