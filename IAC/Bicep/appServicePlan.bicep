param envName string
param location string
param resourceName string
param instanceNumber string
param tierSKU string
@description('An array of NameValues that needs to be added as environment variables')
param tags object
param locationName string
param serviceName string = '${locationName}-${envName}-${resourceName}-${instanceNumber}'

resource appServicePlan 'Microsoft.Web/serverFarms@2020-12-01' = {
  name: 'asp-${serviceName}'
  location: location
  tags: tags
  kind: 'app'
  properties: {
    reserved: true
  }
  sku: {
    name: tierSKU
  }
}

output serverFarmId string = appServicePlan.name
