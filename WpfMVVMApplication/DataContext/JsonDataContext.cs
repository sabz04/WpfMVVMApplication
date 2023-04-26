using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApplication.Models;

namespace WpfMVVMApplication.DataContext
{
    public class JsonDataContext
    {
        public  String ROOT_FILE_PATH = "D:\\config.json";
        public  String EMPLOYEES_LINK;
        public  String ROLES_LINK;
        public  String DEPARTMENTS_LINK;

        private static JsonDataContext _Instance;
        public static JsonSerializerSettings IGNORE_CYCLES =  new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore, 
        };
        
        public JsonDataContext(String emps = null, String roles = null, string Deps = null) {
            if (String.IsNullOrEmpty(emps))
            {
                var config = DeserializeConfig();
                if(config != null)
                {
                    this.EMPLOYEES_LINK = config[0];
                    this.DEPARTMENTS_LINK = config[2];
                    this.ROLES_LINK = config[1];
                }
            }
           
            
            _Instance = this;
        }
        public static JsonDataContext GetInstance(String emps = null, String roles = null, string Deps = null)
        {
            if(_Instance != null) return _Instance;
            else { _Instance = new JsonDataContext(emps, roles, Deps); return _Instance; }
           
        }
        public void InitJsonDatabase()
        {
            try
            {
                var con = DeserializeConfig();
                new EmployeeRepository(con[0]);
                new RoleRepository(con[1]);
                new DepartmentRepository(con[2]);
            
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        static bool HasReadAccess(string filePath)
        {
            try
            {
                
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    return true; 
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        public void SerializeConfig()
        {
            if(String.IsNullOrEmpty(EMPLOYEES_LINK) && String.IsNullOrEmpty(ROLES_LINK) && String.IsNullOrEmpty(DEPARTMENTS_LINK))
            {

            }
            else
            {
                string json = JsonConvert.SerializeObject(new List<String> { EMPLOYEES_LINK, ROLES_LINK, DEPARTMENTS_LINK }, JsonDataContext.IGNORE_CYCLES);
                File.WriteAllText(ROOT_FILE_PATH, json);
            }
        }
        public  List<String> DeserializeConfig()
        {
            if(File.Exists(ROOT_FILE_PATH))
            {
                string json = File.ReadAllText(ROOT_FILE_PATH);
                 List<String> configList = JsonConvert.DeserializeObject<List<String>>(json, JsonDataContext.IGNORE_CYCLES) ?? new List<String>();
                return configList;
            }
            return null;
        }
    }
}
