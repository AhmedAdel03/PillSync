namespace PillSync.DTOs;

public class RegisterDoseStatusDTO
{
    public required bool IsTaken { get; set; }
}

public class WeeklyAdherenceDTO
{
    public required string MedicineId { get; set; }
    public required string MedicineName { get; set; }
    public int WeeklyTakenCount { get; set; }
    public int WeeklyMissedCount { get; set; }
    public double AdherencePercentage { get; set; }
}
