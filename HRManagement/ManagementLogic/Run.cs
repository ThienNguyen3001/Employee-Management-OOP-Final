﻿using System;
using System.Collections.Generic;

namespace ManagementLogic
{
    public class Run
    {
        private Management management = null;
        private static Run instance;
        private uint nextId_E;
        private uint nextId_D;
        private uint nextId_P;
        private Run() { }
        public Management Management { get => management; }

        //singleton design patten
        public static Run Instance 
        {
            get
            {
                if(instance == null)
                    instance = new Run();
                return instance;
            }
        }
        public void Login(string username, string password)
        {
            List<Account> accounts = Data.LoadAccounts();

            Account account = null;
            foreach(Account a in accounts)
            {
                if(a.UserName == username)
                {
                    account = a;
                    break;
                }
            }
            if (account == null || !account.IsValidPassword(password))
            {
                throw new Exception("Tài khoản hoặc mật khẩu sai");
            }
            
            management = Data.LoadData(account.GetFilePath());
            management.SetFilePath(account.GetFilePath());
            management.SetCurrentAccount(account);
            nextId_E = (uint)management.EmployeesList.Count + 1;
            nextId_D = (uint)management.DepartmentList.Count + 1;
            nextId_P = (uint)management.ProjectList.Count + 1;

        }
        public string GenerateId(int who = 1)
        {
            char c;
            if (who == 1)
                c = 'E';
            else if (who == 0)
                c = 'D';
            else
                c = 'P';
            string id = $"{c}{nextId_D.ToString("D4")}";
            nextId_D++;
            return id;
        }
        public void CreateEmployee(string name, string phone, string email, 
                                   string address, bool gender, DateTime birthday, 
                                   DateTime beginWork, Department deparment, uint salary, 
                                   bool isFullTime = true)
        {
            if(isFullTime)
            {
                FulltimeEmployee fulltimeEmployee = new FulltimeEmployee(GenerateId(1),name,phone,email,address,gender,
                                                                 birthday,beginWork,deparment,salary);
                management.Add(fulltimeEmployee);
                return;
            }
            ParttimeEmployee parttimeEmployee = new ParttimeEmployee(GenerateId(1), name, phone, email, address, gender,
                                                                birthday, beginWork, deparment, salary);
            management.Add(parttimeEmployee);
        }
        public void CreateDepart(string name, Employee leader = null)
        {
            Department department = new Department(GenerateId(0), name, leader);
            management.Add(department);
        }
        public void CreateProject(string name, Employee leader = null, string scrip = "")
        {
            Project project = new Project(GenerateId(-1), name, leader, scrip);
            management.Add(project);
        }
        public void RemoveEmployee(Employee employee)
        {
            management.Remove(employee);
        }
        public void RemoveDepart(Department department)
        {
            management.Remove(department);
        }
        public void RemoveProject(Project project)
        {
            management.Remove(project);
        }
        public void EditEmployee(Employee employee, string name = null, string phone = null,
                                 string email = null, string address = null,
                                 bool? gender = null, DateTime? birthday = null,
                                 uint? salary = null)
        {
            management.EditEmployee(employee, name, phone, email, address, gender, birthday, salary);
        }

        public void AddADMIN(string username, string password)
        {
            management.AddADMIN(username, password);
        }
        public void ChangePass(string password, string newPassword)
        {
            management.ChangePasssword(password, newPassword);
        }
        public void ChangeProjectInfo(string projectId, string newName, string newDescription, string newLeader)
        {
            management.ChangeInfoProject(projectId, newName, newDescription, newLeader);
        }
        public void ChangeDepartmentInfo(string departmentId, string newName, string newLeader)
        {
            management.ChangeInfoDepartment(departmentId, newName, newLeader);
        }
        public void IncreaseEmployeeSalary(Employee e)
        {
            management.SalaryIncrease(e);
        }
        public List<Employee> FindEmployee(string keyword = "")
        {
            return management.FindEmployee(keyword);
        }
        public List<Department> FindIDeparment(string keyword = "")
        {
            return management.FindDepartment(keyword);
        }
        public List<Project> FindProject(string keyword = "")
        {
            return management.FindProject(keyword);
        }
    }
}
