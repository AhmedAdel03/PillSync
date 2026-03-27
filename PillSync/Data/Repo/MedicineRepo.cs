using System;
using Microsoft.EntityFrameworkCore;
using PillSync.Entites;

namespace PillSync.Data.Repo;

public class MedicineRepo(AppDbContext context) : IMedicineRepo
{
    public async Task AddMedicine(Medicine medicine,string memberid)
    {
        var newmed=new Medicine
        {
            MedicineName=medicine.MedicineName,
            MemberId=memberid,
            Dosage=medicine.Dosage,
            TypeOfDrug=medicine.TypeOfDrug,
            TimeTotake=medicine.TimeTotake,
            Frequency=medicine.Frequency,
            StartDate=medicine.StartDate,
            EndDate=medicine.EndDate,
            Instructions=medicine.Instructions??string.Empty
            
        };
        await context.Medicine.AddAsync(newmed);
        await context.SaveChangesAsync();
         
    }

    public async Task<List<Medicine>> GetALLmedicine(string memberId)
    {
        return await context.Medicine.Where(x=>x.IsDeleted==false&&x.MemberId==memberId).ToListAsync();
    }

    public async Task<Medicine> GetMedicineById(string medicineId,string memberId)
    {
     return await context.Medicine.FirstOrDefaultAsync(x=>x.ID==medicineId&&x.MemberId==memberId);

     } 

    public async Task RemoveMedicine(string medicineId,string memberId)
    {
        var med= await context.Medicine.FirstOrDefaultAsync(x=>x.ID==medicineId&&x.MemberId==memberId);
        if (med == null)
        {
            return;
        }
        med.IsDeleted=true;
        await context.SaveChangesAsync();
         
    }
}
