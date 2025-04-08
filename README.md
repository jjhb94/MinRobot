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
    error_message TEXT
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
    'ReportStatus'
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
