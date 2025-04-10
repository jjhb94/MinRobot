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

#### Initialize MongoDb (Docker) with data and users
- Go to the root of this repo and run `docker-compose up -d
- After the above docker-compose completes running use `docker ps` to see the `id` of the container
- then run this `docker exec -it <container_name_or_ID> mongosh` or "`docker exec -it fa7f40339e08 mongosh "mongodb://root:yourStrongPassword@fa7f40339e08:27017/admin"` (making sure you have mongosh the shell client installed)
- this will take you to the mongosh shell / termninal where you run the following:
`use minrobot_db` or the db you wish to connect to
`show users`  depending on this output maybe the user was not created in the script ( first time really setting this up in mongodb!!)
`db.getUser(robot_user)` or other username as parameter
- depeding on the output, the user will most likely not exist.
#### Create user: 
```
db.createUser({
  user: "robot_user",
  pwd: "robotlife",
  roles: [{ role: "readWrite", db: "minrobot_db" }],
  mechanisms: ["SCRAM-SHA-256"] // Specify SCRAM-SHA-256
});
```
again, run`show users` and you should see output.
If you run 
`db.robot_commands.find()` or `db.robot_history.find()` or `db.robot_history.find()`
and you don't see anything, there is not data!!

#### Create Data

```
// 1. Create User
use admin; // Switch to the admin database for user creation
db.createUser({
  user: "robot_user",
  pwd: "robotlife",
  roles: [{ role: "readWrite", db: "admin" }]
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
```
run `db.robot_statuses.find()` or `db.robot_statuses.find().pretty()`
make sure there is data in the table! and specifally an _id!

```
// Create robot_commands collection and insert data
db.createCollection("robot_commands");
```
Insert into robot_commands and capture the _id
```
let commandResult = db.robot_commands.insertOne({
  robotId: "TX-042",
  commandType: "Rotate",
  commandData: "x:10, y:25",
  createdAt: new Date(),
  status: "pending",
  errorMessage: null,
  degrees: 90.0
});
```
Extract the _id from the result
```
let commandId = commandResult.insertedId;
```
Create robot_history collection (if it doesn't exist)
```
db.createCollection("robot_history");
```
Insert into robot_history, using the captured commandId
```
db.robot_history.insertOne({
  commandId: commandId, // Use the captured _id
  robotId: "TX-042",
  commandType: "Rotate",
  commandData: "x:10, y:25",
  timestamp: new Date()
});
```
this will return an objectId (_id) reltated to this command! and then use that to assign a commandId to the object
ex: `67f7eda01f977caf9fd861e8`
again, run `db.robot_commands.find()` or `db.robot_commands.find().pretty()`

- to drop a table and remake datta use `db.robot_commands.drop();` etc...
- or to drop the database `db.dropDatabase();`

#### Ther are caveats
- I would suggest building all the tables and then seeding the robot_statuses
- then use the POST command to create new robots with data for better history retention
and correlation to objectId/commandId 
##### note:
baked in mongodb _Id; this is something I learned recently and 
as I am not a DBA I did not think about this design; I would restructure the objects more definitively had I known this - hence the reason the history robot model `RobotHistory.cs` has a field called `HistoryId`.

```
//Create robot history collection
db.createCollection("robot_history");

//insert into robot history collection
<!-- db.robot_history.insertOne({
  id: "67f77a452b4025e4e74befdd"
  robotId: "TX-042",
  commandId: 1,
  commandType: "Rotate",
  commandData: "x:10, y:25",
  Timestamp: new Date()
}); -->

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
