<#

.SYNOPSIS
Checks the database(s) currently in use and removes earlier versions

.DESCRIPTION
Checks the database referenced in the production slot (and optional additon slot)
and removes any databases that are of the same form but are not in use

.PARAMETER ServerName
Required: The name of the SQL Server where the database(s) are stored

.PARAMETER AppName
Required: The name of the App Service instance

.PARAMETER ResourceGroupName
The name of the Resource Group for the App Service resource

.PARAMETER SQLResourceGroupName
The name of the SQL Server Resource Group if different from above

.PARAMETER IncludeSlot
The name of the source slot to swap from; defaults to staging

.PARAMETER DryRun
Make a test pass with the supplied parameters. No changes will be made if this switch is passed.

.EXAMPLE
Cancel-SwapWithPreview -AppName $appName -ResourceGroupName $resourceGroup

.LINK
http://ruslany.net/2016/10/using-powershell-to-manage-azure-web-app-deployment-slots/

#>

[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [String]$ServerName,
    [Parameter(Mandatory = $true)]
    [string]$AppName,
    [Parameter(Mandatory = $false)]
    [ValidateNotNullOrEmpty()]
    [String]$ResourceGroupName = $ENV:ResourceGroup,
    [Parameter(Mandatory = $false)]
    [String]$SQLResourceGroupName,
    [Parameter(Mandatory = $false)]
    [String]$IncludeSlot,
    [Parameter(Mandatory = $false)]
    [Switch]$DryRun
)

$dbnames = @("master")

try {
    # --- Extract short name from fqdn
    $ServerName = $ServerName.Substring(0, $ServerName.IndexOf("."))

    # --- Retrieve server resource
    Write-Host "Searching for server resource $($ServerName)"
    if ($SQLResourceGroupName) {
        $ServerResource = Get-AzureRmResource -ResourceGroupName $SQLResourceGroupName -ResourceName $ServerName -ResourceType "Microsoft.Sql/servers"
    } else {
        $ServerResource = Get-AzureRmResource -ResourceGroupName $ResourceGroupName -ResourceName $ServerName -ResourceType "Microsoft.Sql/servers"
    }
    if (!$ServerResource) {
        throw "Could not find server resource $($ServerName)"
    }

    # Get the databaseversion app setting from production slot
    # This is used to create the masterDbName (name all dbs begin with)
    $prodslot = Get-AzureRmWebApp -ResourceGroup $ResourceGroupName -Name $AppName
    foreach ($as in $prodslot.SiteConfig.AppSettings) {
        if ($as.Name -eq 'DatabaseVersion') {
            $dbnames += $as.Value
            $dbversioned = [Regex]::Match($as.Value, "(?i)R([0-9]+)")
            if ($dbversioned.Success) {
                # $dbversioned.Value = rxxx
                # $dbversioned.Captures.Groups[1].Value = xxx
                # where xxx is the version number
                $masterDbName = $as.Value.Substring(0, $as.Value.Length - $dbversioned.Captures.Groups[1].Value.Length)
            }
            else {
                # database is not versioned
                $masterDbName = $as.Value
            }
        }
    }

    # If another slot was specified, get the databaseversion app setting from that slot
    if ($IncludeSlot) {
        $stgslot = Get-AzureRmWebAppSlot -ResourceGroup $ResourceGroupName -Name $AppName -Slot $IncludeSlot
        foreach ($as in $stgslot.SiteConfig.AppSettings) {
            if ($as.Name -eq 'DatabaseVersion') {
                $dbnames += $as.Value
            }
        }
    }

    # Loop through all dbs on the server
    # remove any that start with masterDbName but are not used by the slot(s)
    $AllDatabasesList = Get-AzureRmSqlDatabase -ResourceGroupName $ServerResource.ResourceGroupName -ServerName $ServerName
    foreach ($db in $AllDatabasesList) {
        if ($db.DatabaseName.StartsWith($masterDbName)) {
            if ($db.DatabaseName -notin $dbnames) {
                if ($DryRun.IsPresent) {
                    Write-Output "    - Would remove $($db.DatabaseName)"
                } else {
                    Write-Output "    - Removing $($db.DatabaseName)"
                    $null = Remove-AzureRmSqlDatabase -ResourceGroupName $ServerResource.ResourceGroupName -ServerName $ServerName -DatabaseName $db.DatabaseName
                }
            }
        }
    }
}
catch {
    throw $_
}