@description('The name of your application')
param resourceName string

@description('The number of this specific instance')
@maxLength(3)
param instanceNumber string

@description('The Azure region where all resources in this example should be created')
param location string

@description('A list of tags to apply to the resources')
param tags object

param company string
param locationName string
param envName string

@description('name of the sql prefix.')
param serverPrefix string = '${company}-${locationName}-${envName}-${resourceName}-${instanceNumber}'

@description('The administrator username of the SQL logical server.')
param administratorLogin string = 'sql${replace(resourceName, '-', '')}root'

@description('The administrator password of the SQL logical server.')
@secure()
param administratorPassword string = newGuid()

@description('Enable Auditing of Microsoft support operations (DevOps)')
param isMSDevOpsAuditEnabled bool = false

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: 'sql-${serverPrefix}'
  location: location
  tags: tags
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorPassword
  }
}

resource sqlServerFirewallRule 'microsoft.Sql/servers/firewallRules@2022-05-01-preview' = {
  parent: sqlServer
  name: 'sql-fwrule-${serverPrefix}'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}

resource sqlDatabase 'microsoft.Sql/servers/databases@2021-11-01' = {
  parent: sqlServer
  name: 'sql-db'
  location: location
  tags: tags
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    autoPauseDelay: 60
    minCapacity: 1
  }
}

resource auditingSettings 'microsoft.Sql/servers/auditingSettings@2021-11-01' = {
  parent: sqlServer
  name: 'default'
  properties: {
    state: 'Enabled'
    isAzureMonitorTargetEnabled: true
  }
}

resource devOpsAuditingSettings 'microsoft.Sql/servers/devOpsAuditingSettings@2021-11-01' = if (isMSDevOpsAuditEnabled) {
  parent: sqlServer
  name: 'default'
  properties: {
    state: 'Enabled'
    isAzureMonitorTargetEnabled: true
  }
}

output db_url string = sqlServer.properties.fullyQualifiedDomainName
output db_username string = administratorLogin
// output db_password string = administratorPassword
