using EmployeeHierachy.Engine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHierachy.Engine.Repository
{
    public class Employee
    {
        private string _csvPath;
        private List<EmployeeDetails> _employeesList;

        /// <summary>
        /// Employee class constructor takes a csv file path of the employee details list 
        /// </summary>
        /// <param name="csvPath"></param>
        public Employee(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
            {
                throw new Exception("The path is not valid, please enter a valid path");
            }
            _csvPath = csvPath;
            _employeesList = GetEmployeeList();
        }
        
        private List<EmployeeDetails> GetEmployeeList()
        {
            try
            {
                List<EmployeeDetails> employees = File.ReadAllLines(_csvPath).Select(x => CastEmployeeDetails(x)).ToList();
                ValidateEmployeeEmployeeList(employees);
                return employees;
            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while parsing the csv error is ");
            }

        }
        /// <summary>
        /// Validates all the Condions given
        /// </summary>
        /// <param name="employees"></param>
        private void ValidateEmployeeEmployeeList(List<EmployeeDetails> employees)
        {

            if (EmployeeReportsToOneManager(employees.Select(t => t.EmployeeId).ToList()))
            {
                throw new Exception("Employee should report to one manager");
            }
            if (!HasOneCEO(employees))
            {
                throw new Exception("There should be only one CEO");
            }
            if (!AllManagersAreEmployees(employees))
            {
                throw new Exception("All Managers Should be employees");
            }
        }

        private string GetManagerId(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            else
            {
                return value;
            }

        }

        private bool HasOneCEO(List<EmployeeDetails> employees)
        {
            try
            {
                var hasOneCEO = employees.Where(t => t.ManagerId.Equals(string.IsNullOrWhiteSpace(t.ManagerId))).Count() == 1 ? true : false;
                return hasOneCEO;
            }
            catch (Exception)
            {
                return true;
            }

        }

        private bool EmployeeReportsToOneManager(List<string> list)
        {
            var duplicates = list.GroupBy(p => p).Where(g => g.Count() > 1).Select(g => g.Key);
            return (duplicates.Count() > 1);
        }

        private EmployeeDetails CastEmployeeDetails(string line)
        {
            string[] data = line.Split(',');
            var empployeeDetails = new EmployeeDetails();
            try
            {
                empployeeDetails.EmployeeId = data[0];
                empployeeDetails.ManagerId = GetManagerId(data[1]);
                empployeeDetails.Salary = ValidateEmployeeSalary(data[2]);
                return empployeeDetails;
            }
            catch (Exception)
            {

                throw new Exception("The file has invalid columns ");
            }


        }

        private int ValidateEmployeeSalary(string amount)
        {
            int _amount;
            if (int.TryParse(amount, out _amount))
            {
                return _amount;
            }
            else
            {
                throw new Exception("Salary Amount is not valid");
            }
        }
        private bool AllManagersAreEmployees(List<EmployeeDetails> employees)
        {

            foreach (var item in employees)
            {
                if (!Check(item.ManagerId, employees))
                {
                    return false;
                }
            }
            return true;
        }
        private bool Check(string Id, List<EmployeeDetails> employees)
        {
            if (Id == null)
                return true;
            foreach (var item in employees)
            {
                if (employees.Where(t => t.EmployeeId == Id).Any())
                {
                    return true;
                }
            }
            return false;
        }
        public long SalaryBudget(string managerId)
        {
            return MapToTree(_employeesList).GetSalaryBudget(managerId);

        }
        private EmployeeHierarchy MapToTree(List<EmployeeDetails> employees)
        {
            
            
            EmployeeDetails employeeDetails = employees.Where(i => i.ManagerId == null).FirstOrDefault();
            EmployeeHierarchy _employeeHierarchy =  new EmployeeHierarchy(employeeDetails.Salary, employeeDetails.EmployeeId, " ");
            _employeeHierarchy.Boss= new EmployeeNode(employeeDetails.Salary, employeeDetails.EmployeeId, " ");
            _employeeHierarchy.Boss.juniors = CastToEmployeeNode(employees.Where(t => t.ManagerId == _employeeHierarchy.Boss.EmployeeId));

            foreach (var item in _employeeHierarchy.Boss.juniors)
            {
                if(employees.Where(t => t.ManagerId == item.EmployeeId).Any())
                {
                    var details = employees.Where(t => t.ManagerId == item.EmployeeId).ToList();
                    _employeeHierarchy.Boss.juniors.Where(u=>u.EmployeeId== details.FirstOrDefault().ManagerId).FirstOrDefault().juniors= CastToEmployeeNode(details);
                }
            }

            return _employeeHierarchy;
        }

        private List<EmployeeNode> CastToEmployeeNode(IEnumerable<EmployeeDetails> enumerable)
        {
            List<EmployeeNode> employeeNodes = new List<EmployeeNode>();
            foreach (var item in enumerable)
            {
                employeeNodes.Add(new EmployeeNode(item.Salary, item.EmployeeId, item.ManagerId));
            }
            return employeeNodes;
        }

        private List<EmployeeHierarchy> CastToEmployeeHierarchy(List<EmployeeDetails> collection)
        {
            List<EmployeeHierarchy> data = new List<EmployeeHierarchy>();
            foreach (var item in collection.GroupBy(j=>j.ManagerId).Skip(1))
            {
                
                var details = item.FirstOrDefault();
                data.Add(new EmployeeHierarchy(details.Salary, details.EmployeeId, details.ManagerId, CastToEmployeeHierarchyAll(item)));
            }
           
            return data;
        }
        private List<EmployeeHierarchy> CastToEmployeeHierarchyAll(IGrouping<string, EmployeeDetails> collection)
        {
            List<EmployeeHierarchy> data = new List<EmployeeHierarchy>();
            foreach (var item in collection)
            {
                data.Add(new EmployeeHierarchy(item.Salary, item.EmployeeId, item.ManagerId));
            }
            return data;
        }
    }
}
