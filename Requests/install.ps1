$addinPath = $env:LOCALAPPDATA + "\Excel\Addins\Requests\";
$url = "https://api.github.com/repos/pathio/excel-requests/releases/latest";
$docsUrl = "https://github.com/pathio/excel-requests";

try
{
    #add the enum for the binary types
    #Using more user friendly names since they won't likely be used outside this context
    Add-Type "
        public enum BinaryType 
        {
            BIT32 = 0, // A 32-bit Windows-based application,           SCS_32BIT_BINARY
            DOS   = 1, // An MS-DOS – based application,            SCS_DOS_BINARY
            WOW   = 2, // A 16-bit Windows-based application,           SCS_WOW_BINARY
            PIF   = 3, // A PIF file that executes an MS-DOS based application, SCS_PIF_BINARY
            POSIX = 4, // A POSIX based application,                SCS_POSIX_BINARY
            OS216 = 5, // A 16-bit OS/2-based application,              SCS_OS216_BINARY
            BIT64 = 6  // A 64-bit Windows-based application,           SCS_64BIT_BINARY
        }"
}
catch {} #type already been loaded, do nothing



function Get-BinaryType 
{
    [CmdletBinding(  
                            SupportsShouldProcess   = $false,
                            ConfirmImpact       = "none",
                            DefaultParameterSetName = ""
                    )]

    param
    (
        [Parameter(
                            HelpMessage             = "Enter binary file(s) to examine",
                            Position            = 0,
                            Mandatory               = $true,
                            ValueFromPipeline           = $true,
                            ValueFromPipelineByPropertyName = $true
                    )]
        [ValidateNotNullOrEmpty()]
        [ValidateScript({Test-Path $_.FullName})]
        [IO.FileInfo[]]
        $Path,

        [Alias("PassThru")]
        [switch]
        $PassThrough
    )

    begin 
    {
        try
        {
            # create the win32 signature
            $Signature = 
                '
                    [DllImport("kernel32.dll")]
                    public static extern bool GetBinaryType(
                                                                            string lpApplicationName,
                                                                            ref int lpBinaryType
                                                                        );
                '

            # Create a new type that lets us access the Windows API function
            Add-Type -MemberDefinition $Signature `
                        -Name                 BinaryType `
                        -Namespace             Win32Utils
        }
        catch {} #type already been loaded, do nothing
    }

    process 
    {
        foreach ($Item in $Path)
        {
            $ReturnedType = -1
            Write-Verbose "Attempting to get type for file: $($Item.FullName)"
            $Result = [Win32Utils.BinaryType]::GetBinaryType($Item.FullName, [ref] $ReturnedType)

            #if the function returned $false, indicating an error, or the binary type wasn't returned
            if (!$Result -or ($ReturnedType -eq -1))
            {
                Write-Error "Failed to get binary type for file $($Item.FullName)"
            }
            else
            {
                $ToReturn = [BinaryType]$ReturnedType
                if ($PassThrough) 
                {
                    #get the file object, attach a property indicating the type, and passthru to pipeline
                    Get-Item $Item.FullName -Force |
                        Add-Member -MemberType noteproperty -Name BinaryType -Value $ToReturn -Force -PassThru 
                }
                else
                { 
                    #Put enum object directly into pipeline
                    $ToReturn 
                }
            }
        }
    }
}


Write-Host "Running excel-requests Addin Installer...";
Write-Host "For help, visit $docsUrl";
Write-Host "Searching for installed Excel version...";

$excelPath = (Get-ItemProperty -Path "Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\excel.exe").'(default)'
$excelVersion = (Split-Path (Split-Path $excelPath -Parent) -Leaf).replace("Office", "") + ".0"

$bitness = "32";
if ((Get-BinaryType $excelPath) -eq [BinaryType]::BIT64)
{
	$bitness = "64";
}

Write-Host "Path: $excelPath";
Write-Host "Version: $excelVersion";
Write-Host "Bitness: $bitness-bit";



Write-Host Checking latest available excel requests release...;
$release = Invoke-RestMethod -Method Get -Uri $url
$version = $release.name;

Write-Host Latest addin version is $version;

$path = "";
$versionPath = $addinPath + $version;
$registryPath = "Registry::HKEY_CURRENT_USER\SOFTWARE\Microsoft\Office\$excelVersion\Excel\Options";

New-Item -ItemType Directory -Force -Path $versionPath


foreach ($asset in $release.assets) {
	$target = $versionPath + "\"+ $asset.name;
	$downloadUrl = $asset.browser_download_url;
	if(![System.IO.File]::Exists($target)){
		Write-Host Downloading asset from $downloadUrl => $target...;
		Invoke-WebRequest -Uri $downloadUrl -OutFile $target;
	}
}



Write-Host "Installing Addin...";

$properties = Get-Item -Path $registryPath | Select-Object -ExpandProperty property | where {$_ -like 'OPEN*'} | Sort-Object $_


if($properties.Count -eq 0)
{
	Write-Host "Count=0"
	New-ItemProperty -Path $registryPath -Name "OPEN" -Value "/R $assetPath"

}
else 
{
	Write-Host "Count=$properties.Count"

}
