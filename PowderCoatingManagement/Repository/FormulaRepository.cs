using PowderCoatingManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderCoatingManagement.Repository
{
    internal class FormulaRepository : IRepositoryTxt
    {

        private readonly string directoryPath = @"C:\Code\C#\PowderCoatingManagement\";
        private readonly string formulaFileName = "Formulas.txt";

        public string DirectoryPath { get => directoryPath; }
        public string FileName { get => formulaFileName;}

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

        public List<Formula> LoadDataFromFile()
        {
            List<Formula> formulaList = new();
            string path = $"{DirectoryPath}{FileName}";

            try
            {
                CheckForExistingFile();

                string[] formulasAsString = File.ReadAllLines(path);
                for (int i = 0; i < formulasAsString.Length; i++)
                {
                    string[] splittedFormula = formulasAsString[i].Split(';');

                    //Getting id
                    string id = splittedFormula[0];

                    //Getting name
                    string name = splittedFormula[1];

                    //Getting te price per kg
                    // bool success = float.TryParse(splittedFormula[2], out float pricePerKg);

                    //Getting the date
                    bool success = DateOnly.TryParse(splittedFormula[3], out DateOnly date);

                    //Getting the formula itens containing id and amount per
                    List<FormulaItem> formulaItensList = new();
                    string[] splittedFormulaItens = splittedFormula[4].Split("\\");
                    for (int j = 0; j < splittedFormulaItens.Length - 1; j++)
                    {
                        string[] singleItens = splittedFormulaItens[j].Split("|");

                        //Getting raw material based on id
                        RawMaterial rawMaterial = Utilities.GetRawMaterialThroughId(singleItens[0]);

                        //Getting amount per kg 
                        success = float.TryParse(singleItens[1], out float amount);

                        //Intantiating and adding to list
                        FormulaItem formulaItem = new(rawMaterial, amount);
                        formulaItensList.Add(formulaItem);
                    }
                    Formula formula = new(id, name, formulaItensList, date);
                    formulaList.Add(formula);
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
            return formulaList;
        }
    

        public void SaveToFile(List<ISavable> savables)
        {
            StringBuilder sb = new StringBuilder();
            string path = $"{directoryPath}{formulaFileName}";

            foreach (ISavable savable in savables)
            {
                sb.Append(savable.ConvertStringToSave());
                sb.Append('\n');
            }
            File.WriteAllText(path, sb.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success! Formulas saved!");
            Console.ResetColor();
        }
    }
}
