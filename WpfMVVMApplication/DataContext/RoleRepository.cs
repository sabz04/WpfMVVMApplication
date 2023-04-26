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
    public class RoleRepository : IRepository<Role>
    {
        private ObservableCollection<Role> _roles = new ObservableCollection<Role>();
        MessageBoxService messageBoxService = new MessageBoxService();
        private static string  _jsonFilePath;
        private static RoleRepository _roleRepository;
        public RoleRepository(string jsonFilePath, bool flag = true)
        {
            try
            {
                _jsonFilePath = jsonFilePath;
                if (!File.Exists(_jsonFilePath) || flag == false)
                {
                    File.Create(_jsonFilePath).Close();
                    _roles = new ObservableCollection<Role>();
                    _roles.AddRange(new List<Role>
                {
                    new Role
                    {
                        Name = "Начинающий специалист",

                    },
                    new Role
                    {
                        Name = "Стажер"
                    },
                    new Role
                    {
                        Name = "Hr-менеджер"
                    }
                });
                    SaveRolesToJson();
                    _roleRepository = this;
                }
                else
                {
                    LoadRolesFromJson();
                }
            }
            catch(Exception ex) 
            {
               throw new Exception(ex.Message);
            }
            
        }
        public static RoleRepository GetRoleInstance()
        {
            if (_roleRepository == null)
            {
                _roleRepository = new RoleRepository(_jsonFilePath);
            }
            return _roleRepository;
        }
        public Role GetById(Guid id)
        {
            return _roles.FirstOrDefault(r => r.Id == id);
        }

        public Role Create(Role entity)
        {
            _roles.Add(entity);
            SaveRolesToJson();
            return entity;
        }

        public Role Update(Role entity)
        {
            var existingRole = GetById(entity.Id);
            if (existingRole != null)
            {
                // Update properties of the existing role
                existingRole.Name = entity.Name;
                SaveRolesToJson();
            }
            return existingRole;
        }

        public bool Delete(Guid id)
        {
            var roleToRemove = GetById(id);
            if (roleToRemove != null)
            {
                _roles.Remove(roleToRemove);
                SaveRolesToJson();
                return true;
            }
            return false;
        }

        public ObservableCollection<Role> GetAll()
        {
            return _roles;
        }

        private void LoadRolesFromJson()
        {
            if (File.Exists(_jsonFilePath))
            {
                string json = File.ReadAllText(_jsonFilePath);
                _roles = JsonConvert.DeserializeObject<ObservableCollection<Role>>(json);
            }
        }

        private void SaveRolesToJson()
        {
            string json = JsonConvert.SerializeObject(_roles);
            File.WriteAllText(_jsonFilePath, json);
        }
    }
}
