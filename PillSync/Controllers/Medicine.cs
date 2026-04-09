using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PillSync.Data.Repo;
using PillSync.Entites;

namespace PillSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Medicines(IMedicineRepo medicineRepo) : ControllerBase
    {
        [Authorize]
        [HttpPost("addMedicine")]
        public async Task<ActionResult> AddMedicine(Medicine medicine)
        {
         var memberId=User.FindFirstValue(ClaimTypes.NameIdentifier);
         if(memberId==null) return Unauthorized();
         try
         {
            await medicineRepo.AddMedicine(medicine,memberId);
            return NoContent();
         }
         catch (System.Exception)
         {
            
            return BadRequest("medicine is incorrenct");
         }
        }
       [Authorize]
        [HttpPut("removeMedicine/{MedicineId}")]
        public async Task<ActionResult>removeMedicine(string MedicineId)
        {
         var memberId=User.FindFirstValue(ClaimTypes.NameIdentifier);
          if(memberId==null) return Unauthorized();
          try
          {
             await medicineRepo.RemoveMedicine(MedicineId,memberId);
             return NoContent();
          }
          catch (System.Exception ex)
          {
            return BadRequest(ex.Message);
          }
        
        
        }
         [Authorize]
        [HttpGet("{MedicineId}")]
        public async Task<ActionResult<Medicine>>getmedicine(string MedicineId)
        {
         var memberId=User.FindFirstValue(ClaimTypes.NameIdentifier);
          if(memberId==null) return Unauthorized();
          try
          {
           var medicine=await medicineRepo.GetMedicineById(MedicineId,memberId);
             return medicine;
          }
          catch (System.Exception)
          {
            return BadRequest("medicine Not found");
          }
        
        
        }
         [Authorize]
        [HttpGet("getAllMedicines")]
        public async Task<ActionResult<List<Medicine>>>getAllMedicines()
        {
         var memberId=User.FindFirstValue(ClaimTypes.NameIdentifier);
          if(memberId==null) return Unauthorized();
          try
          {
           var medicines=await medicineRepo.GetALLmedicine(memberId);
             return medicines;
          }
          catch (System.Exception)
          {
            return BadRequest("medicine Not found");
          }
        
        
        }
        [Authorize]
        [HttpGet("showAlternative/{medicineName}")]
        public async Task<ActionResult<object>> ShowAlternative(string medicineName)
        {
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (memberId == null) return Unauthorized();
            try
            {
                var aiResponse = await medicineRepo.ShowAlternativesByAi(medicineName);
                return Ok(new
                {
                    medicineName,
                    response = aiResponse
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
