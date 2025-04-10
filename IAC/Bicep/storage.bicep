param location string
param resourceName string
param envName string
param storageSKU string
param tags object
param locationName string
var storageAccountName = 'st${locationName}${envName}${resourceName}'

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  tags: tags
  sku: {
    name: storageSKU
  }
  kind: 'Storage'
  properties: {
    supportsHttpsTrafficOnly: true
  }
  // resource tableService 'tableServices' = {
  //   name: 'default'
  //   properties: {
  //   }
  // }
}

output storageAccountConnectionString string = 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listKeys(resourceId(resourceGroup().name, 'Microsoft.Storage/storageAccounts', storageAccountName), '2022-09-01').keys[0].value};EndpointSuffix=core.windows.net'
output storageAccountName string = storageAccountName
