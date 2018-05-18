<#

.SYNOPSIS
Cancels (resets) an in progress swap

.DESCRIPTION
Checks if a swap is in progress and cancels it if it is

.PARAMETER AppName
Required: The name of the Application Insights instance

.PARAMETER ResourceGroupName
The name of the destination Resource Group for the resource

.PARAMETER sourceSlot
The name of the source slot to swap from; defaults to staging

.PARAMETER targetSlot
The name of the destination slot to swap to; defaults to production

.EXAMPLE
Complete-SwapWithPreview -AppName $appName -ResourceGroupName $resourceGroup

.LINK
http://ruslany.net/2016/10/using-powershell-to-manage-azure-web-app-deployment-slots/

#>

Param (
    [Parameter(Mandatory = $true)]
    [string]$AppName,
    [Parameter(Mandatory = $false)]
    [ValidateNotNullOrEmpty()]
    [String]$ResourceGroupName = $ENV:ResourceGroup,
    [Parameter(Mandatory = $false)]
    [string]$sourceSlot = 'staging',
    [Parameter(Mandatory = $false)]
    [string] $targetSlot = 'production'
)

$webapp = Get-AzureRmWebApp -ResourceGroupName $ResourceGroupName -name $AppName
if ($webapp.TargetSwapSlot -eq $sourceSlot) {
    Write-Output "Completing swap of $AppName"
    Switch-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $AppName -SourceSlotName $sourceSlot -DestinationSlotName $targetSlot -SwapWithPreviewAction CompleteSlotSwap

    $slotDetails = Get-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $AppName -Slot $targetSlot
    Write-Output "Final config of $targetSlot"
    Write-Output $slotDetails.SiteConfig.AppSettings
} else {
    Write-Output "No swap in progress"
}
