targetScope = 'subscription'
// https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/patterns-configuration-set
// If an environment is set up (dev, test, prod...), it is used in the application name
@allowed([
  'dev'
  'test'
  'stg'
  'prod'
])
param environment string
param appName string
@allowed([
  'eastus2'
])
param location string
param instanceNumber string
param locationName string

var defaultTags = {
  environment: environment
  application: resourceName
  instance: instanceNumber
}

var resourceName = '${environment}-${appName}'

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${locationName}-${resourceName}-${instanceNumber}' // minrobot-dev-009-eastus2-rg  rg-eus2-dev-minrobot-009
  // if you don't want to run this module, run the pwrshell line below to create resource group
  // $devResourceGroup = New-AzResourceGroup -Name minrobot-dev-009-eastus2-rg -Location eastus2
  location: location
  tags: defaultTags
}
