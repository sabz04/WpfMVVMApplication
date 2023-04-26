using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMVVMApplication.Services;
using WpfTestApplication.Models;

namespace WpfMVVMApplication.DataContext
{
    public class DepartmentRepository : IRepository<Department>
    {
        private static string _filepath;
        MessageBoxService messageBoxService = new MessageBoxService();
        private ObservableCollection<Department> _departments;
        private static DepartmentRepository _departmentRepository;
        public DepartmentRepository(String filepath, bool flag = true) 
        {
            try
            {
                _filepath = filepath;
                if (!File.Exists(filepath) || flag == false)
                {
                    File.Create(filepath).Close();
                    _departments = new ObservableCollection<Department>
            {
                new Department
                {
                    Name = "Отдел маркетинга",
                    Offices = new ObservableCollection<string>
                    {
                        "120", "135", "145"
                    }
                },
                new Department
                {
                    Name = "Отдел бухгалтерского учета",
                    Offices = new ObservableCollection<string>
                    {
                        "124", "125", "155"
                    }
                },
                new Department
                {
                    Name = "IT-отдел",
                    Offices = new ObservableCollection<string>
                    {
                        "128", "129", "139"
                    }
                },
                new Department
                {
                    Name = "Отдел обучения начинающих специалистов",
                    Offices = new ObservableCollection<string>
                    {
                        "111", "115", "114"
                    }
                }
            };
                    Save();
                    _departmentRepository = this;
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

        private void Save()
        {
            string json = JsonConvert.SerializeObject(_departments, JsonDataContext.IGNORE_CYCLES);
            File.WriteAllText(_filepath, json);
        }
        public static DepartmentRepository GetDepartmentInstance()
        {
            if (_departmentRepository == null)
            {
                _departmentRepository = new DepartmentRepository(_filepath);
            }
            return _departmentRepository;
        }
        public Department Create(Department entity)
        {
            GetAll();
            var dep = _departments.FirstOrDefault(x=>x.Name == entity.Name);
            if (dep != null)
                return null;
            _departments.Add(entity);
            Save();
            return entity;
        }

        public bool Delete(Guid id)
        {
            GetAll();
            var employeeToRemove = _departments.FirstOrDefault(dep => dep.Id == id);
            if (employeeToRemove != null)
            {
                _departments.Remove(employeeToRemove);
                Save();
                return true;
            }
            return false;
        }

        public ObservableCollection<Department> GetAll()
        {
            string json = File.ReadAllText(_filepath);
            return _departments = JsonConvert.DeserializeObject<ObservableCollection<Department>>(json, JsonDataContext.IGNORE_CYCLES) ?? new ObservableCollection<Department>();
        }

        public Department GetById(Guid id)
        {
            GetAll();
            var depById = _departments.FirstOrDefault(x=>x.Id == id);
            if(depById == null) { return null; }
            return depById;
        }
        public static ObservableCollection<string> ConvertToObservableCollection(string input)
        {
            ObservableCollection<string> collection = new ObservableCollection<string>();

            // Разделить входную строку на отдельные строки с помощью разделителя "\n"
            string[] lines = input.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            // Добавить каждую строку в коллекцию
            foreach (string line in lines)
            {
                collection.Add(line);
            }

            return collection;
        }
        public Department Update(Department entity)
        {
            GetAll();
            var existingDepartment = _departments.FirstOrDefault(dep => dep.Id == entity.Id);
            if (existingDepartment != null)
            {
                existingDepartment.Name = entity.Name;
                existingDepartment.Offices = entity.Offices;
                existingDepartment.Employees = entity.Employees;
                Save();
                return existingDepartment;
                
            }
            return null;
        }
    }
}
