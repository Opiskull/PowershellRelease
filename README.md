# PowershellRelease

# How to use

## Commands

### Get-AssemblyVersion

SYNTAX
    Get-AssemblyVersion [-FilePath] <string>
	[<CommonParameters>]
	
### New-FileCommit

SYNTAX
    New-FileCommit [-FilePath] <string> [-RepositoryPath] <string> -UserName <string> -UserEmail <string> -Message <string> [-Tag <string>]
	[<CommonParameters>]
	
### New-Release

SYNTAX
    New-Release [-AssemblyFilePath] <string> [-RepositoryPath] <string> -UserName <string> -UserEmail <string> [-UserPassword <string>]
	[<CommonParameters>]
	
### New-ZipFile

SYNTAX
    New-ZipFile [-ArchiveDestinationPath] <string> [-InputFiles <string[]>] [-InputFolder <string>] [-Force]
	[<CommonParameters>]
	
### Push-Repository

SYNTAX
    Push-Repository [-RepositoryPath] <string> -UserName <string> [-UserPassword <string>] [-Tags <string[]>]
    [<CommonParameters>]
	
### Set-AssemblyVersion

SYNTAX
    Set-AssemblyVersion [-FilePath] <string> [[-NewVersion] <string>] [-IncrementBuild] [-IncrementRevision]
    [<CommonParameters>]