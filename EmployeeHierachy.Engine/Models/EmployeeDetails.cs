using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHierachy.Engine.Models
{
   public class EmployeeDetails
    {
        public string EmployeeId { get; set; }
        public string ManagerId { get; set; }
        public int Salary { get; set; }
        public bool HasJuniors { get; set; }
        public bool IsCEO { get; set; }
        public List<EmployeeDetails> Juniors { get; set; }
        public EmployeeDetails(string employeeId, string managerId, int salary, bool hasJuniors, bool isCEO)
        {
            EmployeeId = employeeId;
            ManagerId = managerId;
            Salary = salary;
            HasJuniors = hasJuniors;
            IsCEO = isCEO;
        }
        public EmployeeDetails(string employeeId, string managerId, int salary, bool hasJuniors, bool isCEO, List<EmployeeDetails> juniors)
        {
            EmployeeId = employeeId;
            ManagerId = managerId;
            Salary = salary;
            HasJuniors = hasJuniors;
            IsCEO = isCEO;
            Juniors = juniors;
        }
        public EmployeeDetails()
        {
            
        }

    }
}
