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
      positionX: 10.5,
      positionY: 20.2,
    },
    {
      robotId: "TX-027",
      status: "Offline",
      batteryLevel: 10.0,
      uptime: 0,
      lastUpdated: new Date(),
      positionX: 5.0,
      positionY: 15.0,
    },
    {
      robotId: "TX-042",
      status: "Online",
      batteryLevel: 80.0,
      uptime: 7200,
      lastUpdated: new Date(),
      positionX: 0.0,
      positionY: 0.0,
    },
  ]);

  db.robot_commands.insertOne({
    robot_id: "TX-042",
    command_type: "Rotate",
    command_data: { x: 10, y: 25 },
    degrees: 90.0,
    status: "pending",
    error_message: null,
    created_at: new Date()
  });
  