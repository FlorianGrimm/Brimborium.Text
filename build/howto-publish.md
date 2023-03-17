# Publish to github

# Once per computer
https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry

```PowerShell
mkdir "${Env:\USERPROFILE}\Documents\secure"
notepad "${Env:\USERPROFILE}\Documents\secure\github.json"
```

```PowerShell
$json = Get-Content "${Env:\USERPROFILE}\Documents\secure\github.json" | ConvertFrom-Json

$NAMESPACE=$json.NAMESPACE
$USERNAME=$json.USERNAME
$GITHUB_TOKEN=$json.GITHUB_TOKEN

dotnet nuget add source --username $USERNAME --password $GITHUB_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/$NAMESPACE/index.json"
```


# Publish

```PowerShell


$json = Get-Content "${Env:\USERPROFILE}\Documents\secure\github.json" | ConvertFrom-Json

$NAMESPACE=$json.NAMESPACE
$USERNAME=$json.USERNAME
$GITHUB_TOKEN=$json.GITHUB_TOKEN

dir Brimborium.Text\nupkg\
dir Brimborium.Text\nupkg\*.nupkg | Remove-Item
dir Brimborium.Text\nupkg\*.snupkg | Remove-Item

dotnet test 

nbgv set-version 1.0.4
dotnet clean
dotnet pack --configuration Release /p:PublicRelease=true

dir Brimborium.Text\nupkg\

$nupkg = dir Brimborium.Text\nupkg\*.nupkg | %{$_.FullName}
dotnet nuget push $nupkg --source "github" --api-key $GITHUB_TOKEN

$nupkg = dir Brimborium.Text\nupkg\*.snupkg | %{$_.FullName}
dotnet nuget push $nupkg --source "github" --api-key $GITHUB_TOKEN

```

nbgv set-version prepare-release 1.0.2

nbgv set-version 1.0.3

dotnet build --configuration Release /p:PublicRelease=true

nuget
https://www.nuget.org/packages/manage/upload
dotnet nuget push Contoso.08.28.22.001.Test.1.0.0.nupkg --api-key qz2jga8pl3dvn2akksyquwcs9ygggg4exypy3bhxy6w6x6 --source https://api.nuget.org/v3/index.json