[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [String]$AppServiceName,
    [Parameter(Mandatory = $false)]
    [String]$ReleaseNumber
)

try {

    # --- Get the database version that is currently being used in production
    Write-Host "Searching for app service $AppServiceName"
    $AppServiceResource = Find-AzureRmResource -ResourceNameEquals $AppServiceName -ResourceType "Microsoft.Web/sites" -ErrorAction SilentlyContinue
    if (!$AppServiceResource){
        throw "Could not find app service with name $AppServiceName"
    }

    # --- Extract the build number if it is not provided
    if (!$PSBoundParameters.ContainsKey("ReleaseNumber")){
        $ReleaseNumber = $ENV:RELEASE_RELEASENAME.Split("-")[0]
    }

    # --- Resolve app service and retrieve app settings

    $AppService = Get-AzureRmWebApp -ResourceGroupName $AppServiceResource.ResourceGroupName -Name $AppServiceName
    $SearchIndexVersionSetting = ($AppService.SiteConfig.AppSettings | Where-Object {$_.Name -eq "SearchIndexVersion"}).Value
    if (!$SearchIndexVersionSetting){
        throw "Could not determine current search index version from SearchIndexVersion setting"
    }

    # --- Determine if this is the first run, if not remove the version number
    $FirstRun = [Regex]::Match($SearchIndexVersionSetting, "R[0-9]").Success
    if ($FirstRun -eq "True") {
        $SearchIndex = $SearchIndexVersionSetting.Substring(0, $SearchIndexVersionSetting.LastIndexOf("-"))
    } else {
        $SearchIndex = $SearchIndexVersionSetting
    }

    # --- Generate search index copy name
    $CopySearchIndex = "$($SearchIndex)-R$($ReleaseNumber)"
    Write-Host "Setting search index name to $CopySearchIndex"
    # --- Always return environment variables to vsts
    Write-Output "##vso[task.setvariable variable=CurrentSearchIndexName;]$($SearchIndexVersionSetting)"
    Write-Output "##vso[task.setvariable variable=jobProfileSearchIndex;]$($CopySearchIndex)"
}
catch {
    throw $_
}