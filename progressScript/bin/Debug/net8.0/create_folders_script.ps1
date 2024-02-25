param(
    $count,
    $name,
    $path
)

$current = 0
$folderNameMain = "FolderWithFolders"

if (-not (Test-Path $path)) {
    New-Item -ItemType Directory -Path $path -Force
}

New-Item -ItemType Directory -Path "$path\$folderNameMain" -Force

for ($i = 1; $i -le $count; $i++) {
    $folderName = "${name}$i"
    New-Item -ItemType Directory -Path "$path\$(Join-Path -Path $folderNameMain -ChildPath $folderName)" -Force
    $current++
    $progress = $current * 100 / $count
    Write-Output $progress
    Start-Sleep -Seconds 0.2
}
