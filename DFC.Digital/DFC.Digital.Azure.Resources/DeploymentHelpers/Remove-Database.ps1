[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = "Low")]
Param(
    [Parameter(Mandatory = $true)]
    [String]$ServerName,
    [Parameter(Mandatory = $true)]
    [String]$CurrentDatabaseName,
    [Parameter(Mandatory = $true)]
    [String]$CopyDatabaseName
)

try {
    # --- Extract short name from fqdn
    $ServerName = $ServerName.Substring(0, $ServerName.IndexOf("."))

    # --- Retrieve server resource
    Write-Host "Searching for server resource $($ServerName)"
    $ServerResource = Find-AzureRmResource -ResourceNameEquals $ServerName -ResourceType "Microsoft.Sql/servers"
    if (!$ServerResource) {
        throw "Could not find server resource $($ServerName)"
    }

    # --- Remove any databases that aren't master, currentdb and the most recernt copy
    Write-Host "Removing redundant databases"
    $ExcludedDatabaseList = @("master", $CurrentDatabaseName, $CopyDatabaseName)
    $RedundantDatabaseList = Get-AzureRmSqlDatabase -ResourceGroupName $ServerResource.ResourceGroupName -ServerName $ServerName -WhatIf:$false | `
        Where-Object {$_.DatabaseName -NotIn $ExcludedDatabaseList} | `
        Sort-Object -Property DatabaseName -Descending

    Write-Host "Found $($RedundantDatabaseList.Count) databases to remove"
    if ($RedundantDatabaseList) {
        foreach ($Database in $RedundantDatabaseList) {
            if ($PSCmdlet.ShouldProcess($($Database.DatabaseName), "Remove redundant database")) {
                Write-Host "    - Removing $($Database.DatabaseName)"
                $null = Remove-AzureRmSqlDatabase -ResourceGroupName $ServerResource.ResourceGroupName -ServerName $ServerName -DatabaseName $Database.DatabaseName
            }
        }
    }
}
catch {
    throw $_
}