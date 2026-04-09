using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Google.GenAI;
using Google.GenAI.Types;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
 using PillSync.Entites;

namespace PillSync.Data.Repo;

public class MedicineRepo(
    AppDbContext context,
    IConfiguration configuration) : IMedicineRepo
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
     
}
 
   

     
 
 
