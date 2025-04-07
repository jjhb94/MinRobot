# MinRobot

## Purpose: 
- create core functionality

## Requirements: 
- dotnet 8, 
- minimal API, 
- psuedo clean architecture pattern.

Program.cs at root of project contains Extensions for making endpoints easier to read.
Endpoints Folder contains endpoint extensions with routes i.e. `base = /api ; api/status /api/status/{robotId}\`


### PostgreSQL
a postgreSQL db is utilized in this application related to the Domain Interfaces / Infrastructure
factories/ repository pattern. Configure this connection in the appsettings.json

### DATA for DB:
- right now we inserted this into the psql:

``` CREATE TABLE robot_statuses (
    robot_id VARCHAR(10) PRIMARY KEY,
    status VARCHAR(50),
    battery_level INT,
    uptime INT,
    last_updated TIMESTAMP,
    position_x DECIMAL,
    position_y DECIMAL
);
```