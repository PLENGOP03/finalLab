using System.ComponentModel.DataAnnotations; // key
using System.ComponentModel.DataAnnotations.Schema;


namespace finalLab.Models
{
   
    public class Positions 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Auto increment
        [StringLength(10)]
        public string positionId{ get; set; }

        public string positionName { get; set; }

        public float baseSalary { get; set; }

        public float salaryIncreaseRate { get; set; }




    }
}
