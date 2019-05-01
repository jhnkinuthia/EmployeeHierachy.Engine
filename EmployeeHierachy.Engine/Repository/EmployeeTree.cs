using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHierachy.Engine.Repository
{
    /// <summary>
    /// This creates an employee Node 
    /// </summary>
    public class EmployeeNode
    {
        
        public List<EmployeeNode> juniors { get; set; }

        // Shows whether the current node has a parent or not

        private bool hasManager;
        public int Salary { get; set; }
        public string EmployeeId { get; set; }
        public string ManagerId { get; set; }
        public EmployeeNode(int salary, string Id, string Manager)
        {
            
            Salary = salary;
            EmployeeId = Id;
            ManagerId = Manager;
            juniors = new List<EmployeeNode>();

        }
        public int EmployeeCount

        {

            get

            {

                return this.juniors.Count;

            }

        }
       
        public void AddEmployee(EmployeeNode employee)
        {

            if (employee == null)

            {

                throw new ArgumentNullException(

                    "Cannot insert null value!");

            }



            if (employee.hasManager)

            {

                throw new ArgumentException(

                    "The Employee already has a Manager!");

            }



            employee.hasManager = true;

            this.juniors.Add(employee);

        }
        public EmployeeNode GetChild(int index)
        {

            return this.juniors[index];

        }

    }

    /// <summary>
    /// The tree structure for the Employee
    /// </summary>
    public class EmployeeHierarchy
    {
        private EmployeeNode boss;
        long amount = 0;
        public EmployeeHierarchy(int salary, string Id, string Manager)
        {
            boss = new EmployeeNode(salary, Id, Manager);
        }
        public EmployeeHierarchy(int salary, string Id, string Manager, List<EmployeeHierarchy> children)
        : this(salary, Id, Manager)
        {
            foreach (var child in children)

            {

                this.boss.AddEmployee(child.boss);

            }

        }
       
        public EmployeeHierarchy(List<EmployeeNode> juniors)
        {

            foreach (var junior in juniors)
            {

                this.boss.AddEmployee(junior);

            }

        }


        public EmployeeNode Boss
        {
            get
            {
                return this.boss;
            }
            set
            {

            }

        }
       
        private long JuniorsSalarys(EmployeeNode root,string managerId)
        {
            
            EmployeeNode child = root;
            bool isRightNode = child.EmployeeId.Equals(managerId);


                if (isRightNode)
                {
                    amount = root.Salary;
                    amount+= Salarys(root);
                amount += root.juniors.Sum(t => t.Salary);
                isRightNode = false;
                return amount;
                }
                else
                {
                    foreach (var item in root.juniors)
                    {
                    if (isRightNode)
                        break;
                        JuniorsSalarys(item, managerId);
                    }
                   
                }
               

            return amount;

        }

        private long Salarys(EmployeeNode root)
        {
            

            foreach (var item in root.juniors)
            {
                amount += item.Salary;
            }
            return amount;

        }

        /// <summary>
        /// Transvers the whole tree summing up the salaries
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public long GetSalaryBudget(string managerId)
        {

            return JuniorsSalarys(this.boss,managerId);

        }
    }


}
