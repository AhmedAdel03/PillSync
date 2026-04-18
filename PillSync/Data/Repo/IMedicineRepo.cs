using System;
using PillSync.DTOs;
using PillSync.Entites;

namespace PillSync.Data.Repo;

public interface IMedicineRepo
{
    public Task AddMedicine(Medicine medicine,string memberid);
   public Task RemoveMedicine(string medicineId,string memberId);
     public Task <List<Medicine>> GetALLmedicine(string memberId);
     public Task <Medicine> GetMedicineById(string medicineId,string memberId);
     public Task<List<Medicine>> ShowAlternatives(string medicineName, string memberId);
     public Task<string> ShowAlternativesByAi(string medicineName);
     public Task<bool> RegisterDoseStatus(string medicineId, string memberId, bool isTaken);
     public Task<List<WeeklyAdherenceDTO>> GetWeeklyAdherence(string memberId);

}
