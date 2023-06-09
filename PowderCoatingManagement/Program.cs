using PowderCoatingManagement;

Utilities.DefaultCulture();

PrintWelcome();

Utilities.InitializeStock();
Utilities.LoadedMessage();

Utilities.ShowMenu();

#region Layout

static void PrintWelcome()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(@"
  ____                  _              ____            _   _               __  __                                                   _   
 |  _ \ _____      ____| | ___ _ __   / ___|___   __ _| |_(_)_ __   __ _  |  \/  | __ _ _ __   __ _  __ _  ___ _ __ ___   ___ _ __ | |_ 
 | |_) / _ \ \ /\ / / _` |/ _ \ '__| | |   / _ \ / _` | __| | '_ \ / _` | | |\/| |/ _` | '_ \ / _` |/ _` |/ _ \ '_ ` _ \ / _ \ '_ \| __|
 |  __/ (_) \ V  V / (_| |  __/ |    | |__| (_) | (_| | |_| | | | | (_| | | |  | | (_| | | | | (_| | (_| |  __/ | | | | |  __/ | | | |_ 
 |_|   \___/ \_/\_/ \__,_|\___|_|     \____\___/ \__,_|\__|_|_| |_|\__, | |_|  |_|\__,_|_| |_|\__,_|\__, |\___|_| |_| |_|\___|_| |_|\__|
                                                                   |___/                            |___/                               
");
    do
    {
        Console.WriteLine("Press <Enter> to login");

    }
    while (Console.ReadKey(true).Key != ConsoleKey.Enter);

    Console.ResetColor();
    Console.Clear();
    #endregion
}

