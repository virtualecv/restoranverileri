[Setup]
AppName=Restoran Tarayıcı
AppVersion=1.0.0
DefaultDirName={pf}\RestoranTarayıcı
DefaultGroupName=Restoran Tarayıcı
OutputDir=.
OutputBaseFilename=RestoranTarayiciSetup
Compression=lzma
SolidCompression=yes

[Files]
Source: "publish\RestoranTarayici.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Restoran Tarayıcı"; Filename: "{app}\RestoranTarayici.exe"
Name: "{commondesktop}\Restoran Tarayıcı"; Filename: "{app}\RestoranTarayici.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Masaüstü kısayolu oluştur"; GroupDescription: "Ek görevler:"