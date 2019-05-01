using EmployeeHierachy.Engine.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHierachy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee employee = new Employee("cc.csv");
           var tt=  employee.SalaryBudget("Employee2");
            var uu = 0;
        }
    }
}
