using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV1.Mvvm.Service
{
    public static class ServiceFactory
    {
        public static IMessageService MessageService { get; set; } = new ConsoleMessageService();
    }
}
