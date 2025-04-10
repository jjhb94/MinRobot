param location string
param resourceName string
param instanceNumber string
@description('An array of NameValues that needs to be added as environment variables')
param environmentVariables array
param appServicePlanId string
param locationName string 
param tags object
var serviceName = '${locationName}-${resourceName}-${instanceNumber}'

// Reference: https://docs.microsoft.com/azure/templates/microsoft.web/sites?tabs=bicep
resource appServiceApp 'Microsoft.Web/sites@2021-02-01' = {
  name: 'app-${serviceName}'
  location: location
  // kind: 'app'
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    clientAffinityEnabled: false
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'

      ftpsState: 'FtpsOnly'
      http20Enabled: true
      minTlsVersion: '1.2'
      appSettings: union(environmentVariables, [
        {
          name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
          value: false
        }
        ])
      }
    }
}

output planId string = appServiceApp.id
output application_name string = appServiceApp.name
output application_url string = appServiceApp.properties.hostNames[0]
