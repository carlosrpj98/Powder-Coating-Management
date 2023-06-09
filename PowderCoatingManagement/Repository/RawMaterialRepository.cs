using PowderCoatingManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderCoatingManagement
{
    internal class RawMaterialRepository : IRepositoryTxt
    {
        private readonly string directoryPath = @"C:\Code\C#\PowderCoatingManagement\";
        private readonly string rawMaterialFileName = "RawMaterials.txt";

        public string DirectoryPath { get => directoryPath;}
        public string FileName { get => rawMaterialFileName;}

        public void CheckForExistingFile()
        {
            string path = $"{DirectoryPath}{FileName}";

            bool fileExists = File.Exists(path);
            if (!fileExists)
            {
                //Create the directoryPath
                if (Directory.Exists(path)) Directory.CreateDirectory(path);

                //Create an empty file
                using FileStream fs = File.Create(path);

            }
        }

        public List<RawMaterial> LoadDataFromFile()
        {
            List<RawMaterial> rawMaterials = new();
            string path = $"{directoryPath}{rawMaterialFileName}";

            try
            {
                CheckForExistingFile();

                string[] rawMaterialsAsString = File.ReadAllLines(path);
                for (int i = 0; i < rawMaterialsAsString.Length; i++)
                {
                    string[] splittedRawMaterials = rawMaterialsAsString[i].Split(';');

                    //Getting Id
                    string id = splittedRawMaterials[0];

                    //Parsing string to RawMaterialType
                    bool success = Enum.TryParse(splittedRawMaterials[1], out RawMaterialType type);
                    if (!success) type = RawMaterialType.None;

                    //getting name
                    string name = splittedRawMaterials[2];

                    //Getting brand name 
                    string brandName = splittedRawMaterials[3];

                    //Getting price per kg
                    success = float.TryParse(splittedRawMaterials[4], out float price);

                    //Instantiating the raw material object and adding to the list
                    RawMaterial rawMaterial = new(id, type, name, brandName, price);
                    rawMaterials.Add(rawMaterial);
                }

            }
            catch (IndexOutOfRangeException orx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error parsing the file, please check the data!");
                Console.WriteLine(orx.Message);
            }
            catch (FileNotFoundException fnfex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File not found!");
                Console.WriteLine(fnfex.Message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong loading the file!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }
            return rawMaterials;
        }

        public void SaveToFile(List<ISavable> savables)
        {
            StringBuilder sb = new StringBuilder();
            string path = $"{directoryPath}{rawMaterialFileName}";

            foreach (ISavable savable in savables)
            {
                sb.Append(savable.ConvertStringToSave());
                sb.Append('\n');
            }
            File.WriteAllText(path, sb.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success! Raw materials saved!");
            Console.ResetColor();
        }
    }
}
