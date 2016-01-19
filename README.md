# PowershellRelease

## How to use
copy folder with dlls and import the module with Import-Module .\PowershellHelper.dll

## Commands

### Get-AssemblyVersion

```
Get-AssemblyVersion [-AssemblyFilePath] <string>
[<CommonParameters>]
```
	
### Get-LatestCommit

```
Get-LatestCommit [-RepositoryPath] <string>
[<CommonParameters>]
```
	
### New-FileCommit

```
New-FileCommit [-FilePath] <string> [-RepositoryPath] <string> -UserName <string> -UserEmail <string> -Message <string> [-Tag <string>]
[<CommonParameters>]
```
	
### New-Release

```
New-Release [-AssemblyFilePath] <string> [-RepositoryPath] <string> -UserName <string> -UserEmail <string> [-UserPassword <string>]
[<CommonParameters>]
```
	
### New-ZipFile

```
New-ZipFile [-ArchiveDestinationPath] <string> [-InputFiles <string[]>] [-InputFolder <string>] [-Force]
[<CommonParameters>]
```
	
### Push-Repository

```
Push-Repository [-RepositoryPath] <string> -UserName <string> [-UserPassword <string>] [-Tags <string[]>]
[<CommonParameters>]
```
	
### Set-AssemblyVersion

```
Set-AssemblyVersion [-AssemblyFilePath] <string> [[-NewVersion] <string>] [-IncrementBuild] [-IncrementRevision]
[<CommonParameters>]
```

### Set-Branch

```
Set-Branch [-RepositoryPath] <string> [-Branch] <string>
[<CommonParameters>]
```
