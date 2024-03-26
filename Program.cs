namespace Employee_Management_System
{

    enum Department
    {
        IT, HR, Finance, Marketing, Sales, Operations, Management
    }

    // Base class 
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    // Employee class inherits from Person class
    class Employee : Person
    {
        public static int TotalEmployees { get; private set; } = 0;
        public int EmployeeId { get; private set; }
        public double Salary { get; set; }

        public Department Department { get; set; }

        // Constructor to initialize properties
        public Employee(int employeeId, string firstName, string lastName, double salary, Department department)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Salary = salary;
            Department = department;
            TotalEmployees++;
        }

        // Destructor > called when object is destroyed
        ~Employee()
        {
            Console.WriteLine($"Employee object destroyed => {EmployeeId}: {FirstName} {LastName} ");
            TotalEmployees--;
        }

    }

    interface IManager
    {

        void AssignEmployeeToDepartment(Employee employee, Department department);
    }

   class EmployeeManager : IManager
    {
        private List<Employee> employees = new List<Employee>();

        public void AssignEmployeeToDepartment(Employee employee, Department department)
        {
            employee.Department = department;
        }

        // Add employee to list
        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        // Remove employee from list
        public void RemoveEmployee(int employeeId)
        {
            Employee employeeToRemove = employees.Find(emp => emp.EmployeeId == employeeId);
            if (employeeToRemove != null)
            {
                employees.Remove(employeeToRemove);
            }
            else
            {
                Console.WriteLine("Employee not found.");
            }
        }

        // Method to display details of all employees
        public void DisplayAllEmployees()
        {
            string header = string.Format("{0,-7} {1,-10} {2,-20} {3,-15} {4,-20}", " ", "ID", "Name", "Salary", "Department");
            Console.WriteLine("\n-------------------------------------------------------------------\n" + header + "\n-------------------------------------------------------------------");
            int index = 1;
            foreach (var employee in employees)
            {
                string name = $"{employee.FirstName} {employee.LastName}";
                Console.WriteLine($"{" "+ index +".)",-7} {employee.EmployeeId,-10} {name,-20} {employee.Salary,-15} {employee.Department,-20}");
                index++;
            }
        }

        // Method to calculate and display total salary of all employees
        public void DisplayTotalSalary()
        {
            double totalSalary = 0;
            foreach (var employee in employees)
            {
                totalSalary += employee.Salary;
            }
            Console.WriteLine($"Total Salary of all employees: {totalSalary}");
        }

        public void DisplayDepartments()
        {
            // Get all values of the Department enum
            Department[] departments = (Department[])Enum.GetValues(typeof(Department));
            int index = 0;

            foreach (Department department in departments)
            {
                Console.WriteLine($"[{index}] {department}");
                index++;
            }
        }

        public bool EmployeeIdExists(int employeeId) // Check if employee ID already exists
        {
            return employees.Any(emp => emp.EmployeeId == employeeId);
        }

        public Employee? GetEmployeeById(int employeeId) // Get employee by ID
        {
            Employee employee = employees.Find(emp => emp.EmployeeId == employeeId);
            if (employee != null)
            {
                return employee;
            }
            else
            {
                Console.WriteLine("Employee not found.");
                return null;
            }
        }

        public bool NoEmployees() // Check if there are no employees
        {
            return employees.Count == 0;
        }

    }




    internal class Program
    {
    static void Main(string[] args)
        {
            // Creating instances of EmployeeManager
            EmployeeManager employeeManager = new EmployeeManager();
            Department department;

            string action;
            
            do
            {
                Console.WriteLine("\n---------------------------------------\n Welcome to the Employee Management System!\n What would you like to do?\n---------------------------------------");
                Console.WriteLine("[1] Add employee\n[2] Remove Employee\n[3] Display All Employees\n[4] Assign Employee to a Department\n[5] Exit\n");

                action = Console.ReadLine(); // Get user input

               
                switch (action)  // Switch case to perform actions based on user input
                {
                    case "1":
                        Console.WriteLine("Adding employee...\n");
                        string input1;
                        
                        do
                        { // Loop to add multiple employees

                            ActionAddEmployee();
                            
                            Console.Write("\nWould you like to add another employee? (y/n)  ");
                            input1 = Console.ReadLine();
                        } while (input1 != "n"); // Continue adding employees until user enters 'n'

                        break;

                    case "2":
                        //Console.WriteLine("Removing employee...\n");
                        string input2;

                        do
                        {
                            // Check if there are no employees
                            if (employeeManager.NoEmployees())
                            {
                                Console.WriteLine("\nThere are currently no employees registered. Press any key to go back");
                                employeeManager.DisplayAllEmployees();
                                Console.ReadLine();
                                break;
                            }
                            else
                            {
                                ActionRemoveEmployee();
                            }
                            Console.Write("\nWould you like to remove another employee? (y/n)  ");
                            input2 = Console.ReadLine();
                        } while (input2 != "n"); // Continue removing employees until user enters 'n'
                        break;

                    case "3":
                        Console.WriteLine("\nDisplaying all employees...");
                        ActionDisplayAllEmployees();

                        break;

                    case "4":
                        Console.WriteLine("\nAssigning employee to a department...");
                        ActionAssignEmployeeToDepartment();
                        break;

                    case "5":
                        Console.WriteLine("\nExiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
            while (action != "5");

            


             void ActionAddEmployee()
            {
                // Input employee details
                Console.Write("Enter employee ID: ");
                int employeeId;

                while (true)
                { 

                    if (int.TryParse(Console.ReadLine(), out employeeId)) // Check if input is a number
                    {
                        if (employeeManager.EmployeeIdExists(employeeId)) // Check if employee ID already exists
                        {
                            Console.WriteLine("Employee ID already exists. Please enter a different ID.");
                            continue;
                        }
                        break;

                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                }
                
            

                // Input employee Name
                Console.Write("Enter first name: ");
                string firstName = Console.ReadLine();
                Console.Write("Enter last name: ");
                string lastName = Console.ReadLine();


                // Input salary
                Console.Write("Enter salary: ");
                double salary; 
                while (!double.TryParse(Console.ReadLine(), out salary)) // Check if input is a number
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }


                // Input department
                while (true)
                {
                    Console.WriteLine("Enter department [0-6] : ");
                    employeeManager.DisplayDepartments();
                    string input = Console.ReadLine();
                    if (Enum.TryParse(input, out department) && Enum.IsDefined(typeof(Department), department))
                    {
                        department = (Department)Enum.Parse(typeof(Department), input);
                        break; // Valid input, exit the loop
                    }
                    else
                    {
                        Console.WriteLine("Invalid. Please enter within the range given.");
                    }
                }

                // Create employee object and add to list
                Employee emp = new Employee(employeeId, firstName, lastName, salary, department);
                employeeManager.AddEmployee(emp);
                Console.WriteLine("\nEmployee added successfully.\n");

                employeeManager.DisplayAllEmployees();
            }



             void ActionRemoveEmployee()
            {
                Console.Write("\nEnter employee ID to remove:  ");
                employeeManager.DisplayAllEmployees();
                int employeeId;

                while (true) // Loop to check if employee ID exists
                {

                    if (int.TryParse(Console.ReadLine(), out employeeId)) // Check if input is a number
                    {
                        if (employeeManager.EmployeeIdExists(employeeId)) // Check if employee ID does exist
                        {
                            employeeManager.RemoveEmployee(employeeId);
                            Console.WriteLine("\nEmployee removed successfully.");
                            //employeeManager.DisplayAllEmployees();
                            break;
                        }
                        else
                        {
                            Console.Write("Employee not found. Please enter a valid employee ID.  ");
                            continue;
                        }

                    }
                    else
                    {
                        Console.Write("Invalid input. Please enter a valid number.  ");
                    }
                } 
                        
                
            }



            void ActionDisplayAllEmployees()
            {
                employeeManager.DisplayAllEmployees();
            }



            void ActionAssignEmployeeToDepartment()
            {
                string input3;

                if (employeeManager.NoEmployees())
                {
                    Console.WriteLine("\nThere are currently no employees registered. Press any key to continue");
                    employeeManager.DisplayAllEmployees();
                    Console.ReadLine();
                }
                else
                {

                    do
                    {

                        Console.Write("Enter employee ID to assign to a department:");
                        employeeManager.DisplayAllEmployees();

                        int employeeId;
                        while (true)
                        {
                            if (!int.TryParse(Console.ReadLine(), out employeeId))
                            {
                                Console.WriteLine("Invalid employee ID. Please enter a valid integer ID.\n");
                                continue; // Prompt the user again
                            }

                            if (employeeManager.EmployeeIdExists(employeeId))
                            {
                                Employee employee = employeeManager.GetEmployeeById(employeeId);

                                Console.WriteLine("Enter department [0-6] : ");
                                employeeManager.DisplayDepartments();
                                string input = Console.ReadLine();
                                Department department;
                                if (Enum.TryParse(input, out department) && Enum.IsDefined(typeof(Department), department))
                                {
                                    employeeManager.AssignEmployeeToDepartment(employee, department);
                                    Console.WriteLine("Employee assigned to department successfully.\n");
                                    break; // Exit the loop if department is assigned successfully
                                }
                                else
                                {
                                    Console.WriteLine("Invalid department. Please enter a valid department number.\n");
                                    continue; // Prompt the user again
                                }
                            }
                            else
                            {
                                Console.Write("Employee not found.");
                                break;
                            }
                        }


                        Console.WriteLine("Do you want to assign another employee to a department? (y/n)");
                        input3 = Console.ReadLine();
                    }
                    while (input3.ToLower() != "n");
                }
                  
            }


            if (employeeManager.NoEmployees())
            {
                Console.WriteLine("There are no employees.");
            }
            else
            {
                // Perform actions for when there are employees
                employeeManager.DisplayAllEmployees();
            }



            GC.Collect(); // Calling garbage collector to dispose objects
        }
    }
}
