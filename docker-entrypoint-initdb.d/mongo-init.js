// Switch to the admin database for user creation
use admin;

// Create the robot_user with all admin privileges
db.createUser({
  user: "robot_user",
  pwd: "robotlife",
  roles: [{ role: "root", db: "admin" }]
});

// Switch to your application database
use minrobot_db;

// Create robot_statuses collection and insert data
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
})