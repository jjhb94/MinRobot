param resourceName string
param location string
param locationName string
param envName string
param instanceNumber string
param tags object
var vnetAddressPrefix = '10.200.0.0/16'
var devSubnetAddressPrefixEast = '10.200.30.0/27'
var devSubnetAddressPrefixCentral = '10.200.32.0/27'
var SubnetEastName = '${envName}subnetEast'
var SubnetCentralName = '${envName}subnetCentral'


var vnetName = 'vnet-${locationName}-${envName}-${resourceName}-${instanceNumber}'

resource vnet 'Microsoft.Network/virtualNetworks@2021-02-01' = {
  name: vnetName
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddressPrefix
      ]
    }
    subnets: [
      {
        name: SubnetEastName
        properties: {
          addressPrefix: devSubnetAddressPrefixEast
          serviceEndpoints: [
            {
              service: 'Microsoft.Web'
              locations: [
                'eastus2'
              ]
            }
            {
              service: 'Microsoft.Sql'
              locations: [
                'eastus2'
              ]
            }
          ]
          delegations: [
            {
              name: 'Microsoft.Web/serverFarms'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
      {
        name: SubnetCentralName
        properties: {
          addressPrefix: devSubnetAddressPrefixCentral
          serviceEndpoints: [
            {
              service: 'Microsoft.Web'
              locations: [
                'centralus'
              ]
            }
          ]
          delegations: [
            {
              name: 'Microsoft.Web/serverFarms'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
    ]
  }
}

output subnetName string = vnetName
output SubnetCentralName string = SubnetCentralName
output SubnetEastName string = SubnetEastName
