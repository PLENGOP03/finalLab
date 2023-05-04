using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalLab.Database;
using finalLab.Models;

namespace finalLab.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class EmployeeController : ControllerBase
        {
            private readonly DataDbContext _dbContext;

            public EmployeeController(DataDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            //Get Method
            //-getEmployees : แสดงข้อมูล Employee ทุกคน
            [HttpGet]
            public async Task<ActionResult<List<Employees>>> getEmployees()
            {
                var Employees = await _dbContext.Employees.ToListAsync();

                if (Employees.Count == 0)
                {
                    return NotFound();

                }

                return Ok(Employees);
            }
            //-getEmployeeById : แสดงข้อมูล Employee ที่ตรงกับ Id ที่ระบุมา      
            [HttpGet("{id}")]
            public async Task<ActionResult<Employees>> getEmployeeById(string id)
            {
                var Employees = await _dbContext.Employees.FindAsync(id);

                if (Employees == null)
                {
                    return NotFound();
                }

                return Ok(Employees);
            }


        //-getEmployeesalary : แสดงเงินเดือนปัจจุบันของ Employee ที่ระบุผ่าน Id
        [HttpGet("Current Salary")]
        public async Task<ActionResult<Employees>> getEmployeesalary(string id, int year)
        {

            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            var position = _dbContext.Positions.Find(employee.positionId);
            if (position == null)
            {
                return NotFound();
            }
            var salary = (position.baseSalary + (position.baseSalary * position.salaryIncreaseRate)) * (year - 1);

            return Ok(salary);
        }

        //-calEmpSalaryInYear : แสดงผลการคำนวนเงินเดือนของ Employee ที่ระบุในอนาคตอีก n ปีข้างหน้า
        [HttpGet("Future Salary")]
        public async Task<ActionResult<Employees>> calEmpSalaryInYear(string id, int year)
        {
            var employee = _dbContext.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            var position = _dbContext.Positions.Find(employee.positionId);
            if (position == null)
            {
                return NotFound();
            }

            var currentYear = DateTime.Now.Year;
            var totalYear = year - 1;
            var salary = position.baseSalary;

            if (totalYear <= currentYear)
            {
                return BadRequest();
            }

            for (int i = currentYear + 1; i <= totalYear; i++)
            {
                salary = (float)(salary * 1.15);
            }

            return Ok(salary);
        }



        //Post Method
        //-createEmployees : เพิ่มข้อมูลพนักงานใหม่ โดยมีเงื่อนไขคือ positionId ของพนักงานจะต้องมีอยู่ใน Table Positions
        [HttpPost]
        public async Task<ActionResult<Employees>> createEmployees(Employees Employees)
        {
            try
            {
                var position = _dbContext.Positions.FirstOrDefault(p => p.positionId == Employees.positionId);
                if (position == null)
                {
                    return BadRequest("Invalid position ID");
                }

                _dbContext.Employees.Add(Employees);

                _dbContext.SaveChanges();
            }

            catch (DbUpdateException)

            {
                return BadRequest();
            }
            return Ok(Employees);
        }


        //Put Method
        //- updateEmployees : อัพเดทข้อมูลพนักงานโดยมีเงื่อนไขคือถ้ามีการอัพเดท positionId จะต้องเป็น position ที่มีอยู่ใน Table Positions
        [HttpPut]
        public async Task<ActionResult<Employees>> putEmployees(string id, Employees newEmployees)
        {
            try
            {
                if (_dbContext.Positions.FirstOrDefault(p => p.positionId == newEmployees.positionId) == null)
                {
                    return BadRequest("Invalid position ID");
                }
                var Employees = await _dbContext.Employees.FindAsync(id);
                if (Employees == null)
                {
                    return NotFound();
                }
                Employees.empName = newEmployees.empName;
                Employees.Email = newEmployees.Email;
                Employees.phoneNumber = newEmployees.phoneNumber;
                Employees.hireDate = newEmployees.hireDate;
                Employees.positionId = newEmployees.positionId;

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            return Ok(newEmployees);
        }


        //Delete Method
        //- deleteEmployees : ลบข้อมูลพนักงาน
        [HttpDelete]
            public async Task<ActionResult<Employees>> deleteEmployees(string id)
            {
                var Employees = await _dbContext.Employees.FindAsync(id);

                if (Employees == null)
                {
                    return NotFound();
                }

                //Remove Employees
                _dbContext.Employees.Remove(Employees);

                //save
                await _dbContext.SaveChangesAsync();

                return Ok(Employees);
            }

        }
    }

