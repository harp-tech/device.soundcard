!define MUI_VERBOSE 1
!include "FileFunc.nsh"

!define CompanyName "harp-tech"
!define DeviceName "Harp.SoundCard"
!define AppName "${DeviceName}.App"
!define AppNameNoSpaces "${DeviceName}.App"

; --- If a PRERELEASE is passed with -DPRERELEASE=..., include it in display/file names.
!ifdef PRERELEASE
  !define APP_PRERELEASE_TEXT "-${PRERELEASE}"
!else
  !define APP_PRERELEASE_TEXT ""
!endif

!define AppVersion "${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_BUILD}${APP_PRERELEASE_TEXT}"
!define APP_NUMERIC_VERSION "${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_BUILD}"
!define /date YEAR "%Y"
!define OUT_FILE_NAME "${AppNameNoSpaces}.v${VERSION_MAJOR}.${VERSION_MINOR}.${VERSION_BUILD}${APP_PRERELEASE_TEXT}-win-${ARCHITECTURE}"

; NSIS input defaults to artifacts publish layout (CI/build_app.sh pass NSIS_INPUT_DIR explicitly)
!ifndef NSIS_INPUT_DIR
  !define NSIS_INPUT_DIR ".\artifacts\publish\win-${ARCHITECTURE}"
!endif

Unicode true
Name "${AppName} v${AppVersion}"
Icon "${DeviceName}.Design\Assets\logo.ico"
UninstallIcon "${DeviceName}.Design\Assets\logo.ico"

;--------------------------------
;Include Modern UI
  !include "MUI2.nsh"
;--------------------------------

OutFile ".\artifacts\${OUT_FILE_NAME}.exe"
InstallDir "$LOCALAPPDATA\${CompanyName}\${AppName}"
RequestExecutionLevel user

;--------------------------------
;Version information
    VIProductVersion "${APP_NUMERIC_VERSION}.0"
    VIAddVersionKey "ProductName" "${AppName}"
    VIAddVersionKey "CompanyName" "${CompanyName}"
    VIAddVersionKey "FileDescription" "${AppName}"
    VIAddVersionKey "FileVersion" "${APP_NUMERIC_VERSION}"
    VIAddVersionKey "ProductVersion" "${APP_NUMERIC_VERSION}"
    VIAddVersionKey "LegalCopyright" "© ${YEAR} ${CompanyName}"
    VIAddVersionKey "LegalTrademarks" "${CompanyName}"
    VIAddVersionKey "OriginalFilename" "${OUT_FILE_NAME}"

;--------------------------------
;Variables
  Var StartMenuFolder
;--------------------------------
;Interface Settings
  !define MUI_ABORTWARNING
  !define MUI_ICON "${DeviceName}.Design\Assets\logo.ico"
  !define MUI_UNICON "${DeviceName}.Design\Assets\logo.ico"
  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "${DeviceName}.Design\Assets\logo.png"
  !define MUI_HEADERIMAGE_RIGHT
;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  ;!insertmacro MUI_PAGE_LICENSE "${NSISDIR}/Contrib/Modern UI 2/Pages/License.nsh"
  !insertmacro MUI_PAGE_COMPONENTS
  ;!insertmacro MUI_PAGE_DIRECTORY
  
  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\${CompanyName}\${AppName}" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

  !define MUI_STARTMENUPAGE_DEFAULTFOLDER "${CompanyName}\${AppName}"
  
  !insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
  
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages
  !insertmacro MUI_LANGUAGE "English"
;--------------------------------

;Installer Sections
Section "${AppName} v${AppVersion}" FirstSection
  SectionIn RO
  SetOutPath "$INSTDIR"
  
  ; Include everything from the output folder
  File /r `${NSIS_INPUT_DIR}\*.*`

  ; Get estimated size installed
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "EstimatedSize" "$0"

  # Registry information for add/remove programs
	WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "DisplayName" "${CompanyName} - ${AppName}"
	WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "UninstallString" "$\"$INSTDIR\Uninstall.exe$\" /S"
	WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "InstallLocation" "$\"$INSTDIR$\""
	WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "DisplayIcon" "$\"$INSTDIR\logo.ico$\""
	WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "Publisher" "${CompanyName}"
	WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "DisplayVersion" "${AppVersion}"
	WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "VersionMajor" ${VERSION_MAJOR}
	WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "VersionMinor" ${VERSION_MINOR}
	# There is no option for modifying or repairing the install
	WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "NoModify" 1
	WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}" "NoRepair" 1
  
  ;Store installation folder
  WriteRegStr HKCU "Software\${CompanyName}\${AppName}" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\${AppName}.lnk" "$INSTDIR\${AppName}.exe" "" "$INSTDIR\${AppName}.exe" 0
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\Uninstall - ${AppName}.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
  
  !insertmacro MUI_STARTMENU_WRITE_END

SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  Delete "$INSTDIR\Uninstall.exe"

  Delete "$INSTDIR\*.*"

  RMDir /r "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder

  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall - ${AppName}.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\${AppName}.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  
  DeleteRegKey /ifempty HKCU "Software\${CompanyName}\${AppName}"
  DeleteRegKey /ifempty HKCU "Software\${CompanyName}"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${CompanyName} ${AppName}"
  

SectionEnd

; !insertmacro SECTION_BEGIN
; File /r `./bin/Release/net10.0/win-${ARCHITECTURE}/publish/*.*`
; !insertmacro SECTION_END
