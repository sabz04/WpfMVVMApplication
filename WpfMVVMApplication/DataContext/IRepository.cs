using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApplication.Models
{
    public interface IRepository<T>
    {
        T GetById(Guid id);
        T Create(T entity);
        T Update(T entity);
        bool Delete(Guid id);
        ObservableCollection<T> GetAll();
    }


}
