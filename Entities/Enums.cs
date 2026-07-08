public enum ServiceStatus {
    Pending,
    Diagnosing,
    AwaitingParts,
    InRepair,
    TestDriving,
    Completed,
    Cancelled
}

public enum ServiceCategory {
    Engine,
    Suspension,
    Brake,
    Clutch,
    Transmission,
    Routine,
    Inspection,
    Electrical,
    Cooling
}

public enum TaskStatus {
    Pending,
    InProgress,
    Completed,
    Blocked
}

public enum Priority {
    Critical = 1,
    Urgent = 2,
    Routine = 3
}