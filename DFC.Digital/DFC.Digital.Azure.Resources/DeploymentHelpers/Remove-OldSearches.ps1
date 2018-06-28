<#

.SYNOPSIS
Checks the search(es) currently in use and removes earlier versions

.DESCRIPTION
Checks the search indexes referenced in the production slot (and optional additon slot)
and removes any indexes that are of the same form but are not in use

.PARAMETER SearchName
Required: The name of the Azure Search service where the index(es) are stored

.PARAMETER AppName
Required: The name of the App Service instance

.PARAMETER ResourceGroupName
The name of the Resource Group for the App Service resource and search resource

.PARAMETER IncludeSlot
The name of the source slot to swap from

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
    [String]$SearchName,
    [Parameter(Mandatory = $true)]
    [string]$AppName,
    [Parameter(Mandatory = $false)]
    [ValidateNotNullOrEmpty()]
    [String]$ResourceGroupName = $ENV:ResourceGroup,
    [Parameter(Mandatory = $false)]
    [String]$IncludeSlot,
    [Parameter(Mandatory = $false)]
    [Switch]$DryRun
)

$idxnames = @()

try {
    # --- Retrieve server resource
    Write-Host "Searching for server resource $($SearchName)"
    $ServerResource = Get-AzureRmResource -ResourceGroupName $ResourceGroupName -ResourceName $SearchName -ResourceType "Microsoft.Search/searchServices"
    if (!$ServerResource) {
        throw "Could not find server resource $($SearchName)"
    }

    # search access variables
    $primaryKey = (Invoke-AzureRmResourceAction -Action listAdminKeys -ResourceId $ServerResource.ResourceId -Force).PrimaryKey
    $searchHeader = @{
        'api-key' = $primaryKey
    }

    # Get the databaseversion app setting from production slot
    # This is used to create the masterDbName (name all dbs begin with)
    $prodslot = Get-AzureRmWebApp -ResourceGroup $ResourceGroupName -Name $AppName
    foreach ($as in $prodslot.SiteConfig.AppSettings) {
        if ($as.Name -eq 'SearchIndexVersion') {
            $idxnames += $as.Value
            $idxversioned = [Regex]::Match($as.Value, "(?i)R([0-9]+)")
            if ($idxversioned.Success) {
                # $idxversioned.Value = rxxx
                # $idxversioned.Captures.Groups[1].Value = xxx
                # where xxx is the version number
                $masterIdxName = $as.Value.Substring(0, $as.Value.Length - $idxversioned.Captures.Groups[1].Value.Length)
            }
            else {
                # database is not versioned
                $masterIdxName = $as.Value
            }
        }
    }

    # If another slot was specified, get the databaseversion app setting from that slot
    if ($IncludeSlot) {
        $stgslot = Get-AzureRmWebAppSlot -ResourceGroup $ResourceGroupName -Name $AppName -Slot $IncludeSlot
        foreach ($as in $stgslot.SiteConfig.AppSettings) {
            if ($as.Name -eq 'SearchIndexVersion') {
                $idxnames += $as.Value
            }
        }
    }

    # Loop through all indexes in search
    # remove any that start with masterIdxName but are not used by the slot(s)
    $getIndexUri = "https://{0}.search.windows.net/indexes?api-version={1}" -f $ServerResource.Name, '2017-11-11'
    $IndexesResponse = Invoke-WebRequest -UseBasicParsing -Method Get -Uri $getIndexUri -Headers $searchHeader
    if ($IndexesResponse.StatusCode -eq 200) {
        $reponseJson = $IndexesResponse.Content | ConvertFrom-Json
        foreach ($idx in $reponseJson.value) {
            if ($idx.Name.StartsWith($masterIdxName)) {
                if ($idx.Name -notin $idxnames) {
                    if ($DryRun.IsPresent) {
                        Write-Output "    - Would remove $($idx.Name)"
                    }
                    else {
                        Write-Output "    - Removing $($idx.Name)"
                        $deleteIndexUri = "https://{0}.search.windows.net/indexes/{1}?api-version={2}" -f $ServerResource.Name, $idx.Name, '2017-11-11'
                        $deleteResponse = Invoke-WebRequest -UseBasicParsing -Method Delete -Uri $deleteIndexUri -Headers $searchHeader
                        Write-Output "      Result: $($deleteResponse.statusCode)"
                    }
                }
            }
        } 
    }
}
catch {
    throw $_
}