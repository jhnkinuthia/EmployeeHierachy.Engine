using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeHierachy.Engine.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHierachy.Engine.Repository.Tests
{
    [TestClass()]
    public class EmployeeTests
    {

        [TestMethod()]
        public void SalaryBudgetTest()
        {
            Employee employee = new Employee("cc.csv");
            employee.SalaryBudget("Employee2");
        }
    }
}