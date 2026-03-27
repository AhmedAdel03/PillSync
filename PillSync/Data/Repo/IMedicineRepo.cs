using System;
using PillSync.Entites;

namespace PillSync.Data.Repo;

public interface IMedicineRepo
{
    public Task AddMedicine(Medicine medicine,string memberid);
   public Task RemoveMedicine(string medicineId,string memberId);
     public Task <List<Medicine>> GetALLmedicine(string memberId);
     public Task <Medicine> GetMedicineById(string medicineId,string memberId);

}
