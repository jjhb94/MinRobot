// 1. Create User
use admin; // Switch to the admin database for user creation
// db.createUser({
//   user: "robot_user",
//   pwd: "robotlife",
//   roles: [{ role: "readWrite", db: "admin" }]
// });

// 2. Create Database and Collections
use minrobot_db; // Switch to your application database
db.createUser({
  user: "robot_user",
  pwd: "robotlife",
  roles: [{ role: "readWrite", db: "minrobot_db" }],
  mechanisms: ["SCRAM-SHA-256"] // Specify SCRAM-SHA-256
});

db.createCollection("robot_statuses");
db.robot_statuses.insertMany([
  {
    id: new ObjectId("67f76119c4247fa785f18150"),
    robotId: "TX-010",
    status: "Online",
    batteryLevel: 95,
    uptime: 3600,
    lastUpdated: new Date(),
    positionX: 10.5,
    positionY: 0
  },
  {
    id: new ObjectId("67f7658f1e4fe01d94b23eb4"),
    robotId: "TX-027",
    status: "Offline",
    batteryLevel: 10,
    uptime: 0,
    lastUpdated: new Date(),
    positionX: 10.5,
    positionY: 25
  },
  {
    id: new ObjectId("67f76d5ff43788ff049793ad"),
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
  id: "67f77a452b4025e4e74befdd",
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
  id: "67f77a452b4025e4e74befdd",
  robotId: "TX-042",
  commandId: 1,
  commandType: "Rotate",
  commandData: "x:10, y:25",
  Timestamp: new Date()
});