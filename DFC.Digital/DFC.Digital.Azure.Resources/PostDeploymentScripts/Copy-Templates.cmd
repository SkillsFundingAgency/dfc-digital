RENAME %WEBROOT_PATH%\sitemap.xml.template sitemap.xml
IF "%APPSETTING_ENVIRONMENT%" EQU "PRD" (
    ECHO Renaming prod robots.txt 
    RENAME %WEBROOT_PATH%\robots.prod.txt.template robots.txt
    DEL %WEBROOT_PATH%\robots.txt.template
)
IF "%APPSETTING_ENVIRONMENT%" NEQ "PRD" (
    ECHO Renaming non-prod robots.txt 
    RENAME %WEBROOT_PATH%\robots.txt.template robots.txt
    DEL %WEBROOT_PATH%\robots.prod.txt.template
)