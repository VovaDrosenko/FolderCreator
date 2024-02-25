param(
    [int]$count,
    [string]$names,
    [string]$path
)

$nameArray = $names -split ',' | ForEach-Object { $_.Trim(',') }

$folderNameMain = "FolderWithFolders"

if (-not (Test-Path $path)) {
    New-Item -ItemType Directory -Path $path -Force
}

New-Item -ItemType Directory -Path "$path\$folderNameMain" -Force

foreach ($name in $nameArray) {
    $name = $name.Trim("'")

    $fullPath = Join-Path -Path $path -ChildPath (Join-Path -Path $folderNameMain -ChildPath $name)
    New-Item -ItemType Directory -Path $fullPath -Force
    Start-Sleep -Seconds 0.2
}
