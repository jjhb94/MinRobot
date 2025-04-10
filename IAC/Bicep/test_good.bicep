// targetScope = 'subscription'
// https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/patterns-configuration-set
// If an environment is set up (dev, test, prod...), it is used in the application name
@allowed([
  'development'
  'test'
  'stage'
  'production'
  'dr'
])
param environment string
@allowed([
  'Standard_LRS'
  'Standard_GRS'
])
param storageSKU string
param tierSKU string
param company string
param appName string
param location string
param locationName string
param instanceNumber string
param functionWorkerRuntime string
param envName string
param bizUnit string
param accessScope string
param dateTime string = utcNow('mm/dd/yyyy')
@secure()
param minrobotDb1 string
@secure()
param minrobotDb2 string

var resourceName = appName

var defaultTags = {
  environment: environment
  'cost-center': 'robotsCost'
  'product-owner': bizUnit
  'workload-owner': 'jjhb94@live.com'
  'workload-infra-owner': 'jjhb94@live.com'
  'data-classification': accessScope
  'app-name': resourceName
  'product-name': resourceName
  region: 'East US 2'
  'deployment-date': dateTime
  'end-of-life': 'permanent'
}

// var environmentConfigurationMap = { // configure this more later
//   Development: {
//     appServicePlan: {
//       sku: {
//         name: 'S1'
//         capacity: 1
//       }
//     }
//     storageAccount: {
//       sku: {
//         name: 'Standard_LRS'
//       }
//     }
//   }
//   Test: {
//     appServicePlan: {
//       sku: {
//         name: 'F1'
//       }
//     }
//     storageAccount: {
//       sku: {
//         name: 'Standard_GRS'
//       }
//     }
//   }
// } 

// resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
//   name: '${resourceName}-${instanceNumber}-${location}-rg'
//   location: location
//   tags: defaultTags
// }

module storageAccount 'storage.bicep' = {
  name: '${resourceName}${instanceNumber}strg' // this is just the deployment name, which doesn't affect the resource itself
  params: {
    location: location
    locationName: locationName
    resourceName: resourceName
    envName: envName
    storageSKU: storageSKU
    tags: defaultTags
  }
}

module appServicePlan './appServicePlan.bicep' = {
  name: '${resourceName}-${location}-asp'
  params: {
    envName: envName
    instanceNumber: instanceNumber
    location: location
    locationName: locationName
    resourceName: resourceName
    tierSKU: tierSKU
    tags: defaultTags
  }
}

module vnet './vnet.bicep' = {
  name: '${resourceName}-${location}-${instanceNumber}-vnet'
  params: {
    instanceNumber: instanceNumber
    envName: envName
    location: location
    locationName: locationName
    resourceName: resourceName
    tags: defaultTags
  }
}

module appInsights './appInsights.bicep' = {
  name: '${resourceName}-${location}-${instanceNumber}-ai'
  // scope: resourceGroup(rg.name)
  params: {
    location: location
    locationName: locationName
    instanceNumber: instanceNumber
    resourceName: resourceName
    tags: defaultTags
  }
}

module webApp './appService.bicep' = { // this creates both the app service app and the app service plan 
  name: '${resourceName}-${location}-webApp'
  // scope: resourceGroup(rg.name)
  params: {
    location: location
    locationName: locationName
    resourceName: resourceName
    instanceNumber: instanceNumber
    environmentVariables: applicationEnvironmentVariables
    appServicePlanId: appServicePlan.outputs.serverFarmId
    tags: defaultTags
  }
  dependsOn: [
    appServicePlan
  ]
}

module functionAppModule 'functionApp.bicep' = {
  name: '${resourceName}-fnapp-settings'
  // scope: resourceGroup(rg.name)
  params: {
    resourceName: resourceName
    environment: envName
    location: location
    locationName: locationName
    company: company
    storageAccountConnectionString: storageAccount.outputs.storageAccountConnectionString
    functionWorkerRuntime: functionWorkerRuntime
    appinstrumentsKey: appInsights.outputs.appInsightsInstrumentationKey
    appServicePlanId: appServicePlan.outputs.serverFarmId
    tags: defaultTags
  }
  dependsOn: [
    appServicePlan
  ]
}

module keyVault 'keyVault.bicep' = { // TODO: need to add a module for access control
  name: '${resourceName}-${location}-kv'
  // scope: resourceGroup(rg.name)
  params: {
    envName: envName
    resourceName: resourceName
    location: location
    locationName: locationName
    company: company
    minrobotDb1: minrobotDb1
    minrobotDb2: minrobotDb2
    tags: defaultTags
    funcAppId: functionAppModule.outputs.devTenantId
  }
}

module serviceBus 'serviceBus.bicep' = {
  name: '${resourceName}-${location}-serviceBus'
  // scope: resourceGroup(rg.name)
  params: {
    company: company
    instanceNumber: instanceNumber
    location: location
    locationName: locationName
    resourceName: resourceName
    envName: envName
    tags: defaultTags
  }
}

module database './database.bicep' = {
  name: '${resourceName}-${location}-${instanceNumber}-sql'
  // scope: resourceGroup(rg.name)
  params: {
    company: company
    location: location
    locationName: locationName
    resourceName: resourceName
    envName: envName
    tags: defaultTags
    instanceNumber: instanceNumber
  }
}

var applicationEnvironmentVariables = [
// You can add your custom environment variables here
      {
        name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
        value: appInsights.outputs.appInsightsInstrumentationKey
      }
      {
        name: 'DATABASE_URL'
        value: database.outputs.db_url
      }
      {
        name: 'DATABASE_USERNAME'
        value: database.outputs.db_username
      }
      // {
      //   name: 'DATABASE_PASSWORD'
      //   value: database.outputs.db_password
      // }
]

// output application_name string = webApp.outputs.application_name
output appServiceAppName string = webApp.outputs.application_name
// output application_url string = webApp.outputs.application_url
output appServiceAppHostName string = webApp.outputs.application_url
// output resource_group string = rg.name
