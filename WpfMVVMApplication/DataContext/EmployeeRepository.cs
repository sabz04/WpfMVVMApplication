using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using WpfMVVMApplication.DataContext;
using WpfMVVMApplication.Services;
using WpfTestApplication.Models;

public class EmployeeRepository : IRepository<Employee>
{
    private static string _filePath;
    MessageBoxService messageBoxService = new MessageBoxService();
    private ObservableCollection<Employee> _employees;
    private static EmployeeRepository _employeeRepository;

    public EmployeeRepository(string filepath, bool flag = true)
    {

        try
        {
            _filePath = filepath;
            if (filepath == "")
                return;
            if (!File.Exists(filepath) || flag == false)
            {
                File.Create(filepath).Close();
                _employees = new ObservableCollection<Employee>();
                SaveEmployeesToFile();
                _employeeRepository = this;
            }
            else
            {
                GetAll();
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static EmployeeRepository GetEmployeeInstance()
    {
        
        if (_employeeRepository == null )
        {
            _employeeRepository = new EmployeeRepository(_filePath);
        }
        return _employeeRepository;
    }
    public Employee Create(Employee entity)
    {
        GetAll();
        if (entity.FirstName == null || entity.LastName == null)
        {
            return null;
        }
        if(entity.Position == null)
        {
            entity.Position = RoleRepository.GetRoleInstance().GetAll().ToList().FirstOrDefault();
        }
        _employees.Add(entity);
        SaveEmployeesToFile();
        return entity;
    }

    public bool Delete(Guid id)
    {
        GetAll();
        Employee employeeToRemove = _employees.ToList().Find(emp => emp.Id == id);
        if (employeeToRemove != null)
        {
            _employees.Remove(employeeToRemove);
            SaveEmployeesToFile();
            return true;
        }
        return false;
    }

    public ObservableCollection<Employee> GetAll()
    {
        string json = File.ReadAllText(_filePath);
        return _employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json, JsonDataContext.IGNORE_CYCLES) ?? new ObservableCollection<Employee>(); ;
    }

    public Employee GetById(Guid id)
    {
        GetAll();
        return _employees.ToList().Find(emp => emp.Id == id);
    }

    public Employee Update(Employee entity)
    {
        GetAll();
        Employee employeeToUpdate = _employees.ToList().Find(emp => emp.Id == entity.Id);
        if (employeeToUpdate != null)
        {
            employeeToUpdate.FirstName = entity.FirstName;
            employeeToUpdate.LastName = entity.LastName;
            employeeToUpdate.Position = entity.Position;
            employeeToUpdate.Salary = entity.Salary;
            employeeToUpdate.Department = entity.Department;
            SaveEmployeesToFile();
            return employeeToUpdate;
        }
        return null;
    }
    public ObservableCollection<Role> GetRolesFromJson()
    {
        // Чтение JSON из файл

        // Извлечение ролей
        ObservableCollection<Role> roles = new ObservableCollection<Role>();
        
        foreach (var employee in _employees)
        {
            if (employee.Position != null && !roles.Contains(employee.Position))
            {
                roles.Add(employee.Position);
            }
        }
        return roles;
    }
    
    private void SaveEmployeesToFile()
    {
        string json = JsonConvert.SerializeObject(_employees, JsonDataContext.IGNORE_CYCLES);
        File.WriteAllText(_filePath, json);
    }
}
