param resourceName string
param location string
param instanceNumber string
param tags object
param locationName string
var appInsightsName = '${locationName}-${resourceName}-${instanceNumber}' // eus2-${resourceName}-${instanceNumber}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'ai-${appInsightsName}'
  location: location
  kind: 'web'
  tags: tags
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

output appInsightsInstrumentationKey string = applicationInsights.properties.InstrumentationKey
