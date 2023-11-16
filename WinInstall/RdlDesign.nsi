Unicode true
;--------------------------------
!define PRODUCT_VERSION "1.2"
!define NET_VERSION "4.6.1"
!define EXE_NAME "RdlDesigner"
!define PRODUCT_NAME "QS: RdlDesigner"
!define SHORTCUT_NAME "QS RdlDesigner"
!define MENU_DIR_NAME "RdlDesigner"
!define APP_DIR_NAME "RdlDesigner"
!define UNINSTAL_KEY "RdlDesigner"
!define SETUP_POSTFIX ""


; The name of the installer
Name "${PRODUCT_NAME}"

; The file to write
OutFile "${EXE_NAME}-${PRODUCT_VERSION}${SETUP_POSTFIX}.exe"

!include "MUI.nsh"
!include "x64.nsh"

!addplugindir "NsisDotNetChecker\bin"
!include "NsisDotNetChecker\nsis\DotNetChecker_ru.nsh"

; The default installation directory
InstallDir "$PROGRAMFILES\${APP_DIR_NAME}"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------
; Pages

!insertmacro MUI_PAGE_LICENSE "news.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages
 
!insertmacro MUI_LANGUAGE "Russian"

;--------------------------------
; The stuff to install
Section "${PRODUCT_NAME}" SecProgram

  SectionIn RO

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File /r "Files\*.*"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}" "DisplayIcon" '"$INSTDIR\${EXE_NAME}.exe"'
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}" "Publisher" "Quality Solution"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}" "NoRepair" 1
  WriteUninstaller "uninstall.exe"

  ; Start Menu Shortcuts
  SetShellVarContext all
  CreateDirectory "$SMPROGRAMS\${MENU_DIR_NAME}"
  CreateShortCut "$SMPROGRAMS\${MENU_DIR_NAME}\Удаление.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\${MENU_DIR_NAME}\${SHORTCUT_NAME}.lnk" "$INSTDIR\${EXE_NAME}.exe" "" "$INSTDIR\${EXE_NAME}.exe" 0

SectionEnd

Section "MS .NET Framework ${NET_VERSION}" SecFramework
  SectionIn RO

  !insertmacro CheckNetFramework 461
 
SectionEnd

Section "Ярлык на рабочий стол" SecDesktop

  SetShellVarContext all

  SetOutPath $INSTDIR
  CreateShortCut "$DESKTOP\${SHORTCUT_NAME}.lnk" "$INSTDIR\${EXE_NAME}.exe" "" "$INSTDIR\${EXE_NAME}.exe" 0
 
SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecProgram ${LANG_Russian} "Majorsilence Reporting"
  LangString DESC_SecFramework ${LANG_Russian} "Для работы программы необходима платформа .NET Framework. При необходимости будет выполнена установка через интернет."
  LangString DESC_SecDesktop ${LANG_Russian} "Установит ярлык программы на рабочий стол"

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecProgram} $(DESC_SecProgram)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecFramework} $(DESC_SecFramework)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecGTK} $(DESC_SecGTK)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} $(DESC_SecDesktop)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
; Uninstaller

Section "Uninstall"
  
  SetShellVarContext all
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${UNINSTAL_KEY}"

  ; Remove files and uninstaller
  Delete $INSTDIR\*
  Delete $INSTDIR\uninstall.exe

  Delete $INSTDIR\ru-RU\*
  RMDir $INSTDIR\ru-RU

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\${MENU_DIR_NAME}\*.*"
  Delete "$DESKTOP\${SHORTCUT_NAME}.lnk"

  ; Remove directories used
  RMDir "$SMPROGRAMS\${MENU_DIR_NAME}"
  RMDir "$INSTDIR"
SectionEnd
