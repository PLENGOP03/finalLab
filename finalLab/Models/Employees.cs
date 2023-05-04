using System.ComponentModel.DataAnnotations; // key
using System.ComponentModel.DataAnnotations.Schema;

namespace finalLab.Models
{

    public class Employees 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Auto increment
        [StringLength(10)]
        public string empid { get; set; }

        public string empName { get; set; }

        public string Email { get; set; }

        public string phoneNumber { get; set; }

        public DateTime hireDate { get; set; }

        public string positionId { get; set; }


    }
}
