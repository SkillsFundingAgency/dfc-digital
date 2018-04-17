[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [String]$AppServiceName
)

try {
   # --- Get the database version that is currently being used in production
   Write-Host "Searching for app service $AppServiceName"
   $AppServiceResource = Find-AzureRmResource -ResourceNameEquals $AppServiceName -ResourceType "Microsoft.Web/sites" -ErrorAction SilentlyContinue
   if (!$AppServiceResource){
       throw "Could not find app service with name $AppServiceName"
   }

   # --- Resolve app service and retrieve app settings
   $AppService = Get-AzureRmWebApp -ResourceGroupName $AppServiceResource.ResourceGroupName -Name $AppServiceName

   $AppSettings = @{}
   foreach ($AppSetting in $AppService.SiteConfig.AppSettings){
       $AppSettings.Add($AppSetting.Name, $AppSetting.Value)
   }

   Write-Host "Updating app settings"
   $null = Set-AzureRmWebApp -ResourceGroupName $AppServiceResource.ResourceGroupName -Name $AppServiceName -AppSettings $AppSettings
} catch {
    throw $_
}