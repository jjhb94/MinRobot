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
db.createUser({
  user: "robot_user",
  pwd: "robotlife",
  roles: [{ role: "readWrite", db: "minrobot_db" }],
});

db.createCollection("robot_statuses");
db.robot_statuses.insertMany([
  {
    robotId: "TX-010",
    status: "Online",
    batteryLevel: 95.5,
    uptime: 3600,
    lastUpdated: new Date(),
    position: { x: 10.5, y: 0 }
  },
  {
    robotId: "TX-027",
    status: "Offline",
    batteryLevel: 10.0,
    uptime: 0,
    lastUpdated: new Date(),
    position: { x: 10.5, y: 25.0 }
  },
  {
    robotId: "TX-042",
    status: "Online",
    batteryLevel: 88.0,
    uptime: 123456,
    lastUpdated: new Date(),
    position: { x: 5.0, y: 15.0 }
  },
]);
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


update your appsettings with the db connection! 
