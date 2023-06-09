using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderCoatingManagement.Contracts
{
    internal interface ISavable
    {
        string ConvertStringToSave();
    }
}
