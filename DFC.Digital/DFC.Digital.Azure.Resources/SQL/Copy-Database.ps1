[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [String]$AppServiceName,
    [Parameter(Mandatory = $true)]
    [String]$ServerName,
    [Parameter(Mandatory = $false)]
    [String]$ReleaseNumber
)

try {

    # --- Extract short name from fqdn
    $ServerName = $ServerName.Substring(0, $s.IndexOf("."))

    # --- Retrieve server resource
    Write-Host "Searching for server resource $($ServerName)"
    $ServerResource = Find-AzureRmResource -ResourceNameEquals $ServerName -ResourceType "Microsoft.Sql/servers"
    if (!$ServerResource) {
        throw "Could not find server resource $($ServerName)"
    }

    # --- Extract the build number if it is not provided
    if (!$PSBoundParameters.ContainsKey("ReleaseNumber")){
        $ReleaseNumber = $ENV:RELEASE_RELEASENAME.Split("-")[0]
    }

    # --- Get the database version that is currently being used in production
    Write-Host "Searching for app service $AppServiceName"
    $AppServiceResource = Find-AzureRmResource -ResourceNameEquals $AppServiceName -ResourceType "Microsoft.Web/sites" -ErrorAction SilentlyContinue
    if (!$AppServiceResource){
        throw "Could not find app service with name $AppServiceName"
    }

    # --- Resolve app service and retrieve app settings
    $AppService = Get-AzureRmWebApp -ResourceGroupName $AppServiceResource.ResourceGroupName -Name $AppServiceName
    $DatabaseVersionAppSetting = ($AppService.SiteConfig.AppSettings | Where-Object {$_.Name -eq "DatabaseVersion"}).Value
    if (!$DatabaseVersionAppSetting){
        throw "Could not determine current database version from DatabaseVersion setting"
    }

    # --- Determine if this is the first run, if not remove the version number
    $FirstRun = [Regex]::Match($DatabaseVersionAppSetting, "\d{3}$").Success
    if ($FirstRun -eq "False") {
        $DatabaseName = $DatabaseVersionAppSetting.Substring(0, $DatabaseVersionAppSetting.LastIndexOf("-"))
    }

    # --- Generate database copy name
    $CopyDatabaseName = "$($DatabaseName)-$($ReleaseNumber)"

    # --- Check for existing db matching $CopyDatabaseName
    $ExistingDatabaseCopy = Get-AzureRmSqlDatabase -ResourceGroupName $ServerResource.ResourceGroupName -ServerName $ServerName -DatabaseName $CopyDatabaseName -ErrorAction SilentlyContinue
    Write-Host "Copying $($DatabaseVersionAppSetting) to $($CopyDatabaseName)"

    if (!$ExistingDatabaseCopy) {
        # --- Execute copy
        Write-Host "Copying $($DatabaseVersionAppSetting) to $($CopyDatabaseName)"
        $DatabaseCopyParameters = @{
            ResourceGroupName = $ServerResource.ResourceGroupName
            ServerName        = $ServerName
            DatabaseName      = $DatabaseVersionAppSetting
            CopyDatabaseName  = $CopyDatabaseName
        }
        $StopWatch = [System.Diagnostics.StopWatch]::StartNew()
        $null = New-AzureRmSqlDatabaseCopy @DatabaseCopyParameters
        $ElapsedTime = $StopWatch.Elapsed.ToString('hh\:mm\:ss')
        Write-Host "Database copy completed in $ElapsedTime"

        # --- Remove any databases that aren't master, currentdb and the most recernt copy
        Write-Host "Removing redundant databases"
        $ExcludedDatabaseList = @("master", $DatabaseVersionAppSetting, $CopyDatabaseName)
        $RedundantDatabaseList = Get-AzureRmSqlDatabase -ResourceGroupName $ServerResource.ResourceGroupName -ServerName $ServerName | `
            Where-Object {$_.DatabaseName -NotIn $ExcludedDatabaseList} | `
            Sort-Object -Property DatabaseName -Descending
        $RedundantDatabaseList
        <#
        if ($RedundantDatabaseList) {
            Write-Host "Found $($RedundantDatabaseList.Count) databases to remove"
            foreach ($Database in $RedundantDatabaseList) {
                Write-Host "    - Removing $($Database.DatabaseName)"
                $null = Remove-AzureRmSqlDatabase -ResourceGroupName $ServerResource.ResourceGroupName -ServerName $ServerName -DatabaseName $($Database.DatabaseName)
            }
        } else {
            Write-Host "No database copies to remove. Skipping"
        }
        #>
        # --- Return environment variables to vsts
        Write-Output "##vso[task.setvariable variable=CurrentDatabaseName;]$($DatabaseVersionAppSetting)"
        Write-Output "##vso[task.setvariable variable=CopyDatabaseName;]$($CopyDatabaseName)"        
    } else {
        Write-Host "A database copy with name $CopyDatabaseName exists. Skipping"
    }
}
catch {
    throw $_
}