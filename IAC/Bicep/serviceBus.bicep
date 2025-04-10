@description('The name of your application')
param resourceName string

@description('The number of this specific instance')
@maxLength(3)
param instanceNumber string

@description('The Azure region where all resources in this example should be created')
param location string
param company string
param tags object
param locationName string
param envName string
var serviceBusName = '${company}-${locationName}-${envName}-${resourceName}-${instanceNumber}'

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-01-01-preview' = {
  name: 'sb-${serviceBusName}'
  location: location
  tags: tags
  sku: {
    name: 'Standard'
  }
  properties: {}
}

resource serviceBusQueue 'Microsoft.ServiceBus/namespaces/queues@2022-01-01-preview' = {
  parent: serviceBusNamespace
  name: 'sb-${serviceBusName}'
  properties: {
    lockDuration: 'PT5M'
    maxSizeInMegabytes: 1024
    requiresDuplicateDetection: false
    requiresSession: false
    defaultMessageTimeToLive: 'P10675199DT2H48M5.4775807S'
    deadLetteringOnMessageExpiration: false
    duplicateDetectionHistoryTimeWindow: 'PT10M'
    maxDeliveryCount: 10
    autoDeleteOnIdle: 'P10675199DT2H48M5.4775807S'
    enablePartitioning: false
    enableExpress: false
  }
}
