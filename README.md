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

### Docker (MongoDb)
- make sure you have docker running (desktop or WSL2)
- go to the directory where the docker-compose.yml file is ex: if rootRepo/docker-compose.yml, cd rootRepo
- docker-compose -up -d
- this will pull the latest mongodo image and create a container / volume

#### Initialize MongoDb with data and users
```
// 1. Create User
use admin; // Switch to the admin database for user creation
db.createUser({
  user: "robot_user",
  pwd: "robotlife",
  roles: [{ role: "readWrite", db: "minrobot_db" }]
});

// 2. Create Database and Collections
use minrobot_db; // Switch to your application database
db.createUser({
  user: "robot_user",
  pwd: "robotlife",
  roles: [{ role: "readWrite", db: "minrobot_db" }]
});

db.createCollection("robot_statuses");
db.robot_statuses.insertMany([
  {
    robotId: "TX-010",
    status: "Online",
    batteryLevel: 95,
    uptime: 3600,
    lastUpdated: new Date(),
    positionX: 10.5,
    positionY: 0
  },
  {
    robotId: "TX-027",
    status: "Offline",
    batteryLevel: 10,
    uptime: 0,
    lastUpdated: new Date(),
    positionX: 10.5,
    positionY: 25
  },
  {
    robotId: "TX-042",
    status: "Online",
    batteryLevel: 88,
    uptime: 123456,
    lastUpdated: new Date(),
    positionX: 5,
    positionY: 15
  }
]);

// Create robot_commands collection and insert data
db.createCollection("robot_commands");
db.robot_commands.insertOne({
  robotId: "TX-042",
  commandType: "Rotate",
  commandData: "x:10, y:25",
  createdAt: new Date(),
  status: "pending",
  errorMessage: null,
  degrees: 90.0
});

//Create robot history collection
db.createCollection("robot_history");

//insert into robot history collection
db.robot_history.insertOne({
  robotId: "TX-042",
  commandId: 1,
  commandType: "Rotate",
  commandData: "x:10, y:25",
  Timestamp: new Date()
});
```

### DATA for DB:
- create a db for psql called robotdb ( can be named anything )
https://www.youtube.com/watch?v=KuQUNHCeKCk
- create user and password - 
- inserted this into the psql:

``` 
CREATE TABLE robot_statuses (
    robot_id VARCHAR(10) PRIMARY KEY,
    status VARCHAR(50),
    battery_level INT,
    uptime INT,
    last_updated TIMESTAMP,
    position_x DECIMAL,
    position_y DECIMAL
);
```

```
-- Create the robot_commands table:
CREATE TABLE robot_commands (
    command_id SERIAL PRIMARY KEY,
    robot_id VARCHAR(255) NOT NULL,
    command_type VARCHAR(255) NOT NULL,
    command_data TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL,
    status VARCHAR(50) NOT NULL,
    error_message TEXT,
	command_degrees DOUBLE PRECISION
);

-- Add the check constraint for command_type:
ALTER TABLE robot_commands
ADD CONSTRAINT chk_command_type
CHECK (command_type IN (
    'MoveForward',
    'MoveBackward',
    'TurnLeft',
    'TurnRight',
    'Stop',
    'StartCharging',
    'StopCharging',
    'PickUpItem',
    'DropItem',
    'ScanArea',
    'ReportStatus',
    'Rotate'
));

-- Grant permissions to the robot_user:
GRANT SELECT, INSERT, UPDATE, DELETE ON robot_commands TO robot_user;

-- Grant permissions on the sequence (if command_id is SERIAL):
GRANT USAGE, SELECT ON SEQUENCE robot_commands_command_id_seq TO robot_user;

-- Verify the table structure:
\d robot_commands;

-- Verify the check constraint:
SELECT pg_get_constraintdef(oid)
FROM pg_constraint
WHERE conname = 'chk_command_type';

-- Verify the user permissions:
\dp robot_commands;
```
Need to login using Mongosh
use admin

update your appsettings with the db connection! 
