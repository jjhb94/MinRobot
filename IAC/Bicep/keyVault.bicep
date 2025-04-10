param resourceName string
param location string
param envName string
param company string
param tags object
param funcAppId string
@secure()
param minrobotDb1 string
@secure()
param minrobotDb2 string
param locationName string
var vaultName = 'kv-${company}-${locationName}-${envName}-${resourceName}9' //kv-robot-eus2-qa-psp kv-robot-eus2-dev-access

resource keyVault_Name 'Microsoft.KeyVault/vaults@2021-10-01' = {
  name: vaultName
  location: location
  tags: tags
  properties: {
    createMode: 'default'
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    enableSoftDelete: true
    enableRbacAuthorization: true
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
    sku: {
      family: 'A'
      name: 'standard'
    }
    softDeleteRetentionInDays: 7
    tenantId: subscription().tenantId
    publicNetworkAccess: 'Enabled'
    vaultUri: 'https://${resourceName}'
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: '7d70ca66-7d17-4f0e-afe2-xxxxxxxxxxxx' // az ad user show --id jjhb94@live.com --query id
        permissions: {
          keys: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'Decrypt'
            'Encrypt'
            'UnwrapKey'
            'WrapKey'
            'Verify'
            'Sign'
            'Release'
            'Rotate'
            'GetRotationPolicy'
            'SetRotationPolicy'
          ]
          secrets: [
            'Get'
            'List'
            'Set'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
          ]
          certificates: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'ManageContacts'
            'ManageIssuers'
            'GetIssuers'
            'ListIssuers'
            'SetIssuers'
            'DeleteIssuers'
          ]
          storage: [
            'all'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: 'b6662aba-0801-438d-ad2f-xxxxxxxxxxxx' // az ad user show --id jjhb94@live.com --query id
        permissions: {
          keys: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'Decrypt'
            'Encrypt'
            'UnwrapKey'
            'WrapKey'
            'Verify'
            'Sign'
            'Release'
            'Rotate'
            'GetRotationPolicy'
            'SetRotationPolicy'
          ]
          secrets: [
            'Get'
            'List'
            'Set'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
          ]
          certificates: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'ManageContacts'
            'ManageIssuers'
            'GetIssuers'
            'ListIssuers'
            'SetIssuers'
            'DeleteIssuers'
          ]
          storage: [
            'all'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: funcAppId // this object id is the webApp output principalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
    provisioningState: 'Succeeded'
  }
}


resource keyVaultName_MinrobotDb1ConnectionString 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: keyVault_Name
  name: 'minrobot_db1'
  tags: {
    'file-encoding': 'utf-8'
  }
  properties: {
    value: minrobotdb1 // update to use mongodb instead of SQL
    attributes: {
      enabled: true
    }
  }
}

resource keyVaultName_MinrobotDb2ConnectionString 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: keyVault_Name
  name: 'minrobot_db2'
  tags: {
    'file-encoding': 'utf-8'
  }
  properties: {
    value: minrobotdb1 // update to use mongodb instead of SQL
    attributes: {
      enabled: true
    }
  }
}

output dbConenctionStringUri array = [
  keyVaultName_MinrobotDb1ConnectionString.properties.secretUri, keyVaultName_MinrobotDb2ConnectionString.properties.secretUri
]
