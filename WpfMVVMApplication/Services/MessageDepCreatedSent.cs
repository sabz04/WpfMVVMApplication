using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApplication.Models;

namespace WpfMVVMApplication.Services
{
    public class MessageDepCreatedSent : PubSubEvent<Department>
    {
    }
}
