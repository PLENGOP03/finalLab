using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalLab.Database;
using finalLab.Models;

namespace finalLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly DataDbContext _dbContext;

        public PositionsController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Get Method
        //- getPositions : แสดงข้อมูล Positions ทั้งหมด
        [HttpGet]
        public async Task<ActionResult<List<Positions>>> getPositions()
        {
            var Positions = await _dbContext.Positions.ToListAsync();

            if (Positions.Count == 0)
            {
                return NotFound();

            }

            return Ok(Positions);
        }


        //- getPositionById : แสดงข้อมูล Positions ที่ตรงกับ Id ที่ระบุมา
        [HttpGet("{id}")]
        public async Task<ActionResult<Positions>> getPositionById(string id)
        {
            var Positions = await _dbContext.Positions.FindAsync(id);

            if (Positions == null)
            {
                return NotFound();
            }

            return Ok(Positions);
        }

        //- getEmpByPositionId : แสดงรายชื่อพนักงานทุกคนที่อยู่ในตำแหน่งที่ระบุ
        [HttpGet("Positions ID")]
        public async Task<ActionResult<List<Employees>>> getEmpPositionsID(string positionId)
        {
            var Employees = _dbContext.Employees.Where(e => e.positionId == positionId).ToList();

            if (Employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(Employees);
        }



        // Post Method
        //-createPositions : เพิ่มข้อมูลตำแหน่งใหม่
        [HttpPost]
        public async Task<ActionResult<Positions>> createPositions(Positions Positions)
        {
            try
            {
                _dbContext.Positions.Add(Positions); //Add Positions to Database
                await _dbContext.SaveChangesAsync(); //Save
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok(Positions);
        }

        // Put Method
        //- updatePositions : อัพเดทข้อมูลตำแหน่ง
        [HttpPut("id")]
        public async Task<ActionResult<Positions>> updatePositions(string id, Positions newPositions)
        {
            var Positions = await _dbContext.Positions.FindAsync(id);
            if (Positions == null)
            {
                return NotFound();
            }

            Positions.positionId = newPositions.positionId;
            Positions.positionName = newPositions.positionName;
            Positions.baseSalary = newPositions.baseSalary;
            Positions.salaryIncreaseRate = newPositions.salaryIncreaseRate;

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        // Delete Method
        //- deletePositions : ลบข้อมูลตำแหน่งโดยห้ามลบถ้าหากยังมีพนักงานที่อยู่ในตำแหน่งนั้น
        [HttpDelete]
        public async Task<ActionResult<Positions>> deletePositions(string id)
        {
            var Employees = _dbContext.Employees.Where(e => e.positionId == id).ToList();
            if (Employees != null && Employees.Count > 0)
            {
                return BadRequest("Delete location data, do not delete it if there is still an employee at that location.");
            }
            var position = _dbContext.Positions.SingleOrDefault(p => p.positionId == id);
            if (position == null)
            {
                return NotFound();
            }
            _dbContext.Positions.Remove(position);
            _dbContext.SaveChanges();
            return Ok();

        }
    }
}
