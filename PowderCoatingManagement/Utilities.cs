using PowderCoatingManagement.Contracts;
using PowderCoatingManagement.Repository;
using System.Globalization;
using System.Text;

namespace PowderCoatingManagement
{
    internal class Utilities

    {
        private static List<RawMaterial> rawMaterialStorage = new();
        private static List<Formula> formulaStorage = new();

        internal static void DefaultCulture()
        {
            //Setting the defalt culture as en-US
            string defaultCulture = "en-US";
            CultureInfo culture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo(defaultCulture);
        }

        internal static void ShowMenu()
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("********************");
            Console.WriteLine("* Select an action *");
            Console.WriteLine("********************");

            Console.WriteLine("1. Add new raw material");
            Console.WriteLine("2. Add new formula");
            Console.WriteLine("3. Consult/Edit raw materials");
            Console.WriteLine("4. Consult/Edit/Remove formulas");
            Console.WriteLine("5. Save All Data");
            Console.WriteLine("0. Close application");

            Console.Write("Your selection: ");

            string userSelection = Console.ReadLine() ?? string.Empty;

            switch (userSelection)
            {
                case "1":
                    AddNewRawMaterial();
                    break;
                case "2":
                    CreateFormula();
                    break;
                case "3":
                    ShowRawMaterials(rawMaterialStorage);
                    ShowRawMaterialsSubMenu();
                    break;
                case "4":
                    ShowFormulas();
                    break;
                case "5":
                    SaveAllData();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again");
                    break;
            }
        }

        internal static void InitializeStock()
        {
            RawMaterialRepository rawMaterialRepository = new();
            FormulaRepository formulaRepository = new();

            rawMaterialStorage = rawMaterialRepository.LoadDataFromFile();
            formulaStorage = formulaRepository.LoadDataFromFile();
        }

        internal static void LoadedMessage ()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loaded {rawMaterialStorage.Count} raw materials and {formulaStorage.Count} formulas!");
            Console.WriteLine("Press any key to continue...");

            Console.ResetColor();
            Console.ReadLine();
        }

        internal static void SortingStorage()
        {
            rawMaterialStorage = rawMaterialStorage.OrderBy(rm => rm.Id).ToList();
            formulaStorage = formulaStorage.OrderBy(fm => fm.Id).ToList();
        }

        private static void SaveAllData()
        {
            SortingStorage();

            RawMaterialRepository rawMaterialrepository = new();
            FormulaRepository formulaRepository = new();

            List<ISavable> data = new List<ISavable>();

            //Iterating through raw materials stock list
            foreach (ISavable item in rawMaterialStorage)
            {
                data.Add(item);
            }
            rawMaterialrepository.SaveToFile(data);
            data.Clear();

            //Iterating through formulas list
            foreach (ISavable item in formulaStorage)
            {
                data.Add(item);
            }
            formulaRepository.SaveToFile(data);

            //Updating the data when saving
            InitializeStock();

            Console.ReadLine();
            ShowMenu();
        }

        internal static RawMaterialType RawMaterialTypeMenu()
        {
            RawMaterialType type = RawMaterialType.None;

            Console.Clear();
            Console.WriteLine("What type of raw material you want to register?");
            Console.WriteLine("1. Resin\n2. Curing Agent\n3. Additive\n4. Pigment\n5. Post Addable Additive\n6. Filler \n0. Cancel");

            Console.Write("Your selection: ");
            string userSelection = Console.ReadLine() ?? string.Empty;

            switch (userSelection)
            {
                case "1":
                    type = RawMaterialType.Resin;
                    break;
                case "2":
                    type = RawMaterialType.CuringAgent;
                    break;
                case "3":
                    type = RawMaterialType.Additive;
                    break;
                case "4":
                    type = RawMaterialType.Pigment;
                    break;
                case "5":
                    type = RawMaterialType.PostAddableAdditive;
                    break;
                case "6":
                    type = RawMaterialType.Filler;
                    break;
                case "0":
                    ShowMenu();
                    break;
                default:
                    Console.WriteLine("Invalid selection, please try again.");
                    RawMaterialTypeMenu();
                    break;
            }
            return type;
        }



        internal static void AddNewRawMaterial()
        {
            RawMaterialType type = RawMaterialTypeMenu();

            Console.WriteLine("Type the name of the raw material");
            string name = Console.ReadLine() ?? string.Empty;


            Console.WriteLine("What's the name of the manufacturer of the product?");
            string brandName = Console.ReadLine() ?? string.Empty;

            float price;
            bool isFloat;
            // Formatting input to dot separator in decimal numbers and check parsing
            do
            {
                Console.WriteLine("What's the price(US Dollar) per kilogram?");
                string priceInput = Console.ReadLine() ?? string.Empty;

                isFloat = float.TryParse(priceInput.Replace(",", "."), out price);
                if (!isFloat)
                {
                    Console.WriteLine("Incorrect input");
                }
            }
            while (!isFloat);

            RawMaterial newRawMaterial = new(type, name, brandName, price);
            rawMaterialStorage.Add(newRawMaterial);

            string userSelection;
            do
            {
                Console.WriteLine("Add more one raw material?\n1. Yes\n2. No");
                userSelection = Console.ReadLine() ?? string.Empty;
            } while (userSelection != "1" && userSelection != "2");

            switch (userSelection)
            {
                case "1":
                    AddNewRawMaterial();
                    break;
                case "2":
                    ShowMenu();
                    break;
            }
        }

        internal static bool IdExist(string id)
        {
            //Loop used when is a formula
            if (id.Contains("FO"))
            {
                foreach (Formula formula in formulaStorage)
                {
                    if (formula.Id == id)
                    {
                        return true;
                    }
                }
            }

            //Loop used when is a raw material
            foreach (RawMaterial rawMaterial in rawMaterialStorage)
            {
                if (rawMaterial.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        //When creating a new raw material
        public static string GenerateId(RawMaterialType type)
        {
            Random rdm = new Random();
            int num = rdm.Next(10000);

            string prefix = string.Empty;

            switch (type)
            {
                case RawMaterialType.Resin:
                    prefix = "RS";
                    break;
                case RawMaterialType.CuringAgent:
                    prefix = "CA";
                    break;
                case RawMaterialType.Pigment:
                    prefix = "PG";
                    break;
                case RawMaterialType.Additive:
                    prefix = "AD";
                    break;
                case RawMaterialType.PostAddableAdditive:
                    prefix = "PA";
                    break;
                case RawMaterialType.Filler:
                    prefix = "FL";
                    break;
                default:
                    break;
            }
            string id = string.Format("{0}{1:0000}", prefix, num);

            if (IdExist(id))
            { return GenerateId(); }
            else
            { return id; }
        }

        //generating id to formula
        public static string GenerateId()
        {
            Random rdm = new();
            string id = string.Format("FO{0:0000}", rdm.Next(10000));
            if (IdExist(id))
            { return GenerateId(); }
            else
            { return id; }
        }

        internal static RawMaterial GetRawMaterialToFormula()
        {
            RawMaterialType type = RawMaterialTypeMenu();
            List<RawMaterial> rawMaterialList = new();
            List<string> idlist = new();

            Console.WriteLine($"Raw materials of type \"{type}\" in stock:");
            foreach (var rawMaterial in rawMaterialStorage)
            {
                if (rawMaterial.Type == type)
                {
                    rawMaterialList.Add(rawMaterial);
                    idlist.Add(rawMaterial.Id);
                }
            }

            ShowRawMaterials(rawMaterialList);

            string userAnswer;
            do
            {
                Console.WriteLine("What is the Id of the product you want to add?");
                userAnswer = Console.ReadLine() ?? string.Empty;
                userAnswer = userAnswer.Trim().ToUpper();
            }
            while (!idlist.Contains(userAnswer));

            return rawMaterialList[idlist.IndexOf(userAnswer)];
        }

        internal static FormulaItem GetFormulaItens()
        {
            RawMaterial rawMaterial = GetRawMaterialToFormula();

            float amountPerKg;
            bool success;
            do
            {
                Console.WriteLine($"Type the amount of {rawMaterial.Name}(kg) in 1kg of the formula");
                success = float.TryParse(Console.ReadLine(), out amountPerKg);
            } while (!success);

            FormulaItem formulaItem = new(rawMaterial, float.Round(amountPerKg, 5));

            return formulaItem;
        }

        internal static void CreateFormula()
        {
            Console.Clear();
            //prompting the name
            Console.WriteLine("Type the name of the formula you want to register");
            string name = Console.ReadLine() ?? string.Empty;

            //Getting the date
            DateOnly date = new DateOnly();
            bool success;
            do
            {
                Console.WriteLine("Type the formula date (MM/DD/YYYY)");
                Console.Write("Your selection: ");
                success = DateOnly.TryParse(Console.ReadLine(), out date);
            } while (!success);

            List<FormulaItem> formulaItens = GetFormulaItensList();

            Formula formula = new(name, formulaItens, date);
            formulaStorage.Add(formula);

            SuccessMessage();

            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadLine();
            ShowMenu();

        }

        internal static List<FormulaItem> GetFormulaItensList()
        {
            float totalAmount = 0;
            List<FormulaItem> formulaList = new();
            string answer;
            do
            {
                if (formulaList.Count > 0)
                {
                    foreach (FormulaItem item in formulaList)
                    {
                        Console.WriteLine($"\nRaw Material: {item.RawMaterialItem.Name} Amount: {item.Amount}\n");
                    }
                    Console.WriteLine($"Total Amount(kg): {totalAmount}");
                }

                Console.WriteLine("\nAdd an item to the formula?\n1. Yes\n2. Cancel");
                Console.Write("Your selection: ");
                answer = Console.ReadLine() ?? string.Empty;
                if (answer == "2")
                {
                    ShowMenu();
                }
                else if (answer == "1")
                {
                    FormulaItem formulaItem = GetFormulaItens();
                    formulaList.Add(formulaItem);
                    totalAmount += formulaItem.Amount;
                }
                else
                {
                    Console.WriteLine("Invalid selection!\n");
                }
            } while (answer != "2" && totalAmount < 1);

            if (totalAmount != 1.0)
            {
                Console.WriteLine($"The total amount of the formula must be 1kg, your current total is {totalAmount}, please, check your formula and try again");
                GetFormulaItensList();
            }
            Console.WriteLine("The total amount is 1kg, adding the itens to the formula...");
            return formulaList;
        }

        internal static void ShowRawMaterials(List<RawMaterial> list)
        {
            if (!rawMaterialStorage.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The list of raw materials is empty!");
                Console.ResetColor();
                Console.ReadLine();
                ShowMenu();
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            string template = "|{0,-6}|{1,-20}|{2,-50}|{3,-50}|{4,-10:c}|";
            string title = string.Format(template, "Id", "Type", "Name", "Manufacturer", "Price(kg)");
            string line = string.Concat(Enumerable.Repeat("-", title.Length));

            //Header
            Console.WriteLine(line);
            Console.WriteLine(title);
            Console.WriteLine(line);

            Console.ResetColor();

            foreach (var rawMaterial in list)
            {
                string formatted = string.Format(template, rawMaterial.Id, rawMaterial.Type, rawMaterial.Name, rawMaterial.BrandName, rawMaterial.PricePerKg);
                Console.WriteLine(formatted);
            }

            Console.WriteLine(line);
        }

        internal static void ShowRawMaterialsSubMenu()
        {
            Console.WriteLine("1. Edit raw material.\n2. return to main menu.");
            Console.Write("Your selection: ");
            string answer = Console.ReadLine() ?? string.Empty;
            switch (answer)
            {
                case "1":
                    EditRawMaterial();
                    break;
                case "2":
                    ShowMenu();
                    break;
                default:
                    ShowRawMaterials(rawMaterialStorage);
                    ShowRawMaterialsSubMenu();
                    break;
            }
        }

        internal static void EditRawMaterial()
        {
            Console.WriteLine("\nWhat is the id of the raw material you want to edit?");
            Console.Write("Your selection: ");
            string answer = Console.ReadLine() ?? string.Empty;
            answer = answer.Trim().ToUpper();

            RawMaterial rawMaterial = GetRawMaterialThroughId(answer);

            if (rawMaterial == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred while getting the raw material through id, please, try again");
                Console.ResetColor();
                EditRawMaterial();
            }

            Console.WriteLine("\n1. Edit name\n2. Edit manufacturer\n3. Edit price per kg\n4. Cancel");
            Console.Write("Your selection: ");
            answer = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("\n");

            switch (answer)
            {
                case "1":
                    Console.WriteLine($"Type the new name of id:{rawMaterial.Id}");
                    Console.Write("New name: ");
                    string newName = Console.ReadLine() ?? string.Empty;

                    Console.WriteLine("\n");
                    Console.WriteLine($"Change the name of id:{rawMaterial.Id} from \"{rawMaterial.Name}\" to \"{newName}\"?\n1.Yes\n2.Cancel");
                    Console.Write("Your selection: ");
                    answer = Console.ReadLine() ?? string.Empty;

                    if (answer == "1")
                    {
                        rawMaterial.Name = newName;
                        SuccessMessage();
                    }
                    break;

                case "2":
                    Console.WriteLine($"Type the new manufacturer of id:{rawMaterial.Id}");
                    Console.Write("New manufacturer: ");
                    string newBrand = Console.ReadLine() ?? string.Empty;

                    Console.WriteLine("\n");
                    Console.WriteLine($"Change the manufacturer of id:{rawMaterial.Id} from \"{rawMaterial.BrandName}\" to \"{newBrand}\"?\n1.Yes\n2.Cancel");
                    Console.Write("Your selection: ");
                    answer = Console.ReadLine() ?? string.Empty;

                    if (answer == "1")
                    {
                        rawMaterial.BrandName = newBrand;
                        SuccessMessage();
                    }
                    break;

                case "3":
                    float newPrice;
                    bool success;
                    do
                    {
                        Console.WriteLine($"Type the new price per kg of id:{rawMaterial.Id}");
                        Console.Write("New price: ");


                        success = float.TryParse(Console.ReadLine(), out newPrice);

                    } while (!success);

                    Console.WriteLine($"\nChange the price per kg of id:{rawMaterial.Id} from \"{rawMaterial.PricePerKg}\" to \"{newPrice}?\"\n1.Yes\n2.Cancel");
                    Console.Write("Your selection: ");
                    answer = Console.ReadLine() ?? string.Empty;

                    if (answer == "1")
                    {
                        rawMaterial.PricePerKg = newPrice;
                        SuccessMessage();
                    }
                    break;
                case "4":
                    ShowMenu();
                    break;

                default:
                    ShowRawMaterials(rawMaterialStorage);
                    ShowRawMaterialsSubMenu();
                    break;
            }
            Console.WriteLine("Press any key to return.");
            Console.ReadLine();
            ShowMenu();
        }

        internal static void SuccessMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success! Don't forget to save your data!");
            Console.ResetColor();
        }

        internal static RawMaterial GetRawMaterialThroughId(string id)
        {
            RawMaterial rawMaterial = null;
            foreach (RawMaterial rm in rawMaterialStorage)
            {
                if (rm.Id == id) rawMaterial = rm;
            }

            return rawMaterial;
        }

        internal static void ShowFormulas()
        {
            if (!formulaStorage.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The list of formulas is empty!");
                Console.ResetColor();
                Console.ReadLine();
                ShowMenu();
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            string templateFormula = "|{0,-10}|{1,-50}|{2,-40}|{3,-20:c}|";

            foreach (var formula in formulaStorage)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string formatted = string.Format(templateFormula, $"Id: {formula.Id}", $"Name: {formula.Name}", $"Date: {formula.Date}", $"Price(kg): {formula.PricePerKg.ToString("C2")}");
                string line = string.Concat(Enumerable.Repeat("-", formatted.Length));

                Console.WriteLine(line);
                Console.WriteLine(formatted);
                Console.WriteLine(line);
                Console.ResetColor();
            }

            ShowFormulasSubMenu();
        }

        internal static void ShowFormulasSubMenu()
        {
            Console.WriteLine("1. Show more info\n2. Delete a formula\n3. Edit formula\n4. Return to main menu");
            Console.Write("Your selection: ");
            string answer = Console.ReadLine() ?? string.Empty;

            switch (answer)
            {
                case "1":
                    Console.WriteLine("\nWhat's the id of the formula you want to show?");
                    Console.Write("Your selection: ");
                    answer = Console.ReadLine() ?? string.Empty;
                    answer = answer.Trim().ToUpper();
                    Formula formula = GetFormulaThroughId(answer);

                    if (formula == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("An error occurred while getting the formula through id, please, try again");
                        Console.ResetColor();
                        ShowFormulas();
                    }
                    else
                    {
                        Console.Clear();
                        ShowFormulaMoreInfo(formula);
                        Console.WriteLine("Type any key to return.");
                        Console.ReadLine();
                        ShowFormulas();

                    }
                    break;

                case "2":
                    DeleteFomula();
                    break;

                case "3":
                    EditFormula();
                    break;

                case "4":
                    ShowMenu();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid selection!");
                    Console.ResetColor();
                    ShowFormulas();
                    break;
            }
        }

        internal static void EditFormula()
        {
            Console.WriteLine("\nType the id of the formula you want to edit.");
            Console.Write("Your selection: ");

            string id = Console.ReadLine() ?? string.Empty;
            id = id.Trim().ToUpper();
            Formula formula = GetFormulaThroughId(id);
            if (formula == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid id, please, try again!");
                Console.ResetColor();
                EditFormula();
            }
            Console.WriteLine("\n");
            Console.WriteLine("1. Edit name\n2. Edit raw material\n3. Edit date");
            Console.Write("Your selection: ");
            string answer = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("\n");

            switch (answer)
            {
                case "1":
                    Console.WriteLine($"Type the new name of id:{formula.Id}");
                    Console.Write("New name: ");
                    string newName = Console.ReadLine() ?? string.Empty;

                    Console.WriteLine($"Change the name of id:{formula.Id} from \"{formula.Name}\" to \"{newName}\"?\n1. Yes\n2. No");
                    Console.Write("Your selection: ");
                    answer = Console.ReadLine() ?? string.Empty;

                    if (answer == "1")
                    {
                        formula.Name = newName;
                        SuccessMessage();
                    }
                    break;

                case "2":
                    ShowFormulaMoreInfo(formula);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Attention, if you want to change the amount of itens in the formula, you will need to add all the itens again (rebuild)!");
                    Console.ResetColor();
                    Console.WriteLine("1. Rebuild formula itens\n2. Change only an item\n3. Change date\n4. Cancel");
                    Console.Write("Your selection: ");
                    answer = Console.ReadLine() ?? string.Empty;
                    if (answer == "1")
                    {
                        List<FormulaItem> formulaItens = GetFormulaItensList();
                        formula.FormulaItemList = formulaItens;
                        SuccessMessage();
                    }
                    else if (answer == "2")
                    {

                        List<FormulaItem> formulaItemList = new();
                        List<string> idList = new List<string>();
                        foreach (FormulaItem item in formula.FormulaItemList)
                        {
                            idList.Add(item.RawMaterialItem.Id);
                            formulaItemList.Add(item);
                        }

                        string toReplaceId;
                        do
                        {
                            Console.WriteLine("\nType the id of the item you want to change.");
                            Console.Write("Your selection: ");
                            toReplaceId = Console.ReadLine() ?? string.Empty;
                            toReplaceId = toReplaceId.Trim().ToUpper();

                        } while (!idList.Contains(toReplaceId));

                        RawMaterial toReplaceRawMaterial = GetRawMaterialThroughId(toReplaceId);

                        ShowRawMaterials(rawMaterialStorage);


                        string newRawMaterialId;
                        RawMaterial newRawMaterial;
                        do
                        {
                            Console.WriteLine("\nType the id of the raw material you want to add.");
                            Console.Write("Id: ");
                            newRawMaterialId = Console.ReadLine() ?? string.Empty;
                            newRawMaterialId = newRawMaterialId.Trim().ToUpper();
                            newRawMaterial = GetRawMaterialThroughId(newRawMaterialId);

                            if (newRawMaterial == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("An error occurred while searching for the id provided, please, try again.");
                                Console.ResetColor();
                            }

                        } while (newRawMaterial == null);

                        //Replacing the raw material in the list with the new raw material given 
                        foreach (FormulaItem item in formulaItemList)
                        {
                            if (item.RawMaterialItem == toReplaceRawMaterial)
                            {
                                item.RawMaterialItem = newRawMaterial;
                            }
                        }

                        Console.WriteLine($"Replace the raw material id:{toReplaceRawMaterial.Id} with the id:{newRawMaterial.Id}?\n1. Yes\n2. Cancel");
                        Console.Write("Your selection: ");
                        answer = Console.ReadLine() ?? string.Empty;

                        if (answer == "1")
                        {
                            formula.FormulaItemList = formulaItemList;
                            SuccessMessage();
                        }
                    }
                    break;

                case "3":
                    bool success;
                    DateOnly newDate;
                    do
                    {
                        Console.WriteLine($"Type the new date of id:{formula.Id} (MM/DD/YYYY)");
                        Console.Write("New date: ");

                        success = DateOnly.TryParse(Console.ReadLine(), out newDate);

                    } while (!success);

                    Console.WriteLine($"Change the date of id:{formula.Id} from \"{formula.Date}\" to \"{newDate}\"?\n1. Yes\n2. No");
                    Console.Write("Your selection: ");
                    answer = Console.ReadLine() ?? string.Empty;

                    if (answer == "1")
                    {
                        formula.Date = newDate;
                        SuccessMessage();
                    }
                    break;
                default:
                    ShowFormulas();
                    break;
            }
            Console.WriteLine("Press any key to return to main menu.");
            Console.ReadLine();
            ShowMenu();
        }

        internal static void ShowFormulaMoreInfo(Formula formula)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            string templateFormula = "|{0,-10}|{1,-50}|{2,-20}|{3,-20:c}|";

            StringBuilder sb = new StringBuilder();

            Console.ForegroundColor = ConsoleColor.Yellow;
            string formatted = string.Format(templateFormula, $"Id: {formula.Id}", $"Name: {formula.Name}", $"Date: {formula.Date}", $"Price(kg): {formula.PricePerKg.ToString("C2")}");
            string line = string.Concat(Enumerable.Repeat("-", formatted.Length));

            Console.WriteLine(line);
            Console.WriteLine(formatted);
            Console.WriteLine(line);
            Console.ResetColor();

            foreach (FormulaItem item in formula.FormulaItemList)
            {
                formatted = string.Format(templateFormula, item.RawMaterialItem.Id, item.RawMaterialItem.Name, item.Amount, (item.RawMaterialItem.PricePerKg * item.Amount));
                sb.AppendLine(formatted);
            }
            sb.AppendLine(line);
            sb.Append(string.Format(templateFormula, "Total:", "", "1kg", formula.PricePerKg));
            Console.WriteLine(sb);
            Console.WriteLine(line);

        }

        internal static void DeleteFomula()
        {
            Console.WriteLine("Type the id of the formula you want to remove.");
            Console.Write("Your selection: ");
            string id = Console.ReadLine() ?? string.Empty;
            id = id.Trim().ToUpper();

            Formula formula = GetFormulaThroughId(id);

            if (formula == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid id, please, try again!");
                Console.ResetColor();
                DeleteFomula();
            }

            Console.WriteLine($"1. Remove formula Id:{formula.Id}\n2. Cancel");
            Console.Write("Your selection: ");
            string answer = Console.ReadLine() ?? string.Empty;

            switch (answer)
            {
                case "1":
                    formulaStorage.Remove(formula);
                    SuccessMessage();
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "2":
                    ShowMenu();
                    break;
                default:
                    ShowFormulas();
                    break;
            }
        }

        internal static Formula GetFormulaThroughId(string id)
        {
            Formula formula = null;
            foreach (var fml in formulaStorage)
            {
                if (fml.Id == id) formula = fml;
            }

            return formula;
        }
    }
}
