param resourceName string
param environment string
param location string
param storageAccountConnectionString string
param functionWorkerRuntime string 
param appinstrumentsKey string
param appServicePlanId string
param company string
param tags object
param locationName string
var functionAppName = 'func-${company}-${locationName}-${environment}-${resourceName}' // func-robot-eus2-qa-authorization

resource functionAppSettings 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName // this name has to be less than the functionApp name resource ( see functionApp.bicep)
  location: location
  tags: tags
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlanId
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccountConnectionString
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: storageAccountConnectionString
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(functionAppName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'WEBSITE_NODE_DEFAULT_VERSION'
          value: '~14'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appinstrumentsKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionWorkerRuntime
        }
      ]
      netFrameworkVersion: '8.0'
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}

output devFunctionAppName string = functionAppSettings.name
output devTenantId string = functionAppSettings.identity.tenantId
output devTenantPrincipalId string = functionAppSettings.identity.principalId
