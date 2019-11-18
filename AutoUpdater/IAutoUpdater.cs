using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;

namespace AutoUpdater
{
    public interface IAutoUpdater
    {
        string ApplicationName { get; }
        string ApplicationId { get;}
        Assembly ApplicationAssembly { get; }
        Icon ApplicationIcon { get; }
        Uri UpdateConfigLocation { get; }
        Form Context { get; }
    }
}
