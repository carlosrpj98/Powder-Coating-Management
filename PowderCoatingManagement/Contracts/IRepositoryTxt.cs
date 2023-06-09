using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderCoatingManagement.Contracts
{
    internal interface IRepositoryTxt
    {
        string DirectoryPath { get; }
        string FileName { get; }

        void CheckForExistingFile();

        void SaveToFile(List<ISavable> savables);
    }
}
