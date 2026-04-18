using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Google.GenAI;
using Google.GenAI.Types;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PillSync.DTOs;
 using PillSync.Entites;

namespace PillSync.Data.Repo;

public class MedicineRepo(
    AppDbContext context,
    IConfiguration configuration) : IMedicineRepo
{
    private static void ResetWeeklyCountersIfNeeded(Medicine medicine)
    {
        if ((DateTime.UtcNow - medicine.WeeklyCounterStartDate).TotalDays < 7)
        {
            return;
        }

        medicine.WeeklyTakenCount = 0;
        medicine.WeeklyMissedCount = 0;
        medicine.WeeklyCounterStartDate = DateTime.UtcNow;
    }

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
            Instructions=medicine.Instructions??string.Empty,
            WeeklyTakenCount = 0,
            WeeklyMissedCount = 0,
            WeeklyCounterStartDate = DateTime.UtcNow
            
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

    public async Task<List<Medicine>> ShowAlternatives(string medicineName, string memberId)
    {
        var sourceMedicine = await context.Medicine
            .FirstOrDefaultAsync(x =>
                x.MemberId == memberId &&
                x.IsDeleted == false &&
                x.MedicineName == medicineName);

        if (sourceMedicine == null)
        {
            return [];
        }

        return await context.Medicine
            .Where(x =>
                x.MemberId == memberId &&
                x.IsDeleted == false &&
                x.TypeOfDrug == sourceMedicine.TypeOfDrug &&
                x.MedicineName != medicineName)
            .ToListAsync();
    }

   public async Task<string> ShowAlternativesByAi(string medicineName)
{
     
    var apiKey = configuration["AI:ApiKey"] 
        ?? throw new InvalidOperationException("Gemini API Key is missing from configuration.");

     
    var client = new Client(apiKey: apiKey);
 
var  Instruction = new Content
{
    Parts = [
        new Part {
            Text="make the response dose not exceed more than 10 word"
        }
    ]
};
     var prompt = $"Suggest safe alternative medicines for: {medicineName}. " +
                 "Answer briefly in english with bullet points and include a warning to consult a doctor.";

    
        
        var response = await client.Models.GenerateContentAsync(
            model: "gemini-2.5-flash", 
            contents: prompt ,
            config:new GenerateContentConfig
            {
                SystemInstruction=Instruction
            }
        );

         
        return response.Text;
    }

    public async Task<bool> RegisterDoseStatus(string medicineId, string memberId, bool isTaken)
    {
        var medicine = await context.Medicine.FirstOrDefaultAsync(x =>
            x.ID == medicineId &&
            x.MemberId == memberId &&
            x.IsDeleted == false);

        if (medicine == null)
        {
            return false;
        }

        ResetWeeklyCountersIfNeeded(medicine);

        if (isTaken)
        {
            medicine.WeeklyTakenCount++;
        }
        else
        {
            medicine.WeeklyMissedCount++;
        }

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<WeeklyAdherenceDTO>> GetWeeklyAdherence(string memberId)
    {
        var medicines = await context.Medicine
            .Where(x => x.MemberId == memberId && x.IsDeleted == false)
            .ToListAsync();

        foreach (var medicine in medicines)
        {
            ResetWeeklyCountersIfNeeded(medicine);
        }

        await context.SaveChangesAsync();

        return medicines.Select(medicine =>
        {
            var total = medicine.WeeklyTakenCount + medicine.WeeklyMissedCount;
            var adherencePercentage = total == 0 ? 0 : Math.Round((double)medicine.WeeklyTakenCount / total * 100, 2);

            return new WeeklyAdherenceDTO
            {
                MedicineId = medicine.ID,
                MedicineName = medicine.MedicineName,
                WeeklyTakenCount = medicine.WeeklyTakenCount,
                WeeklyMissedCount = medicine.WeeklyMissedCount,
                AdherencePercentage = adherencePercentage
            };
        }).ToList();
    }
     
}
 
   

     
 
 
