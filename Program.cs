using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

//Lists to be used for storing objects of the three classes "Asset", "Computer" and "Phone"

List<Asset> assets = new List<Asset>();
List<Phone> phones = new List<Phone>();
List<Computer> computers = new List<Computer>();

LoadAssets();   //Loads the assets already in the json file

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("***********************************************");
Console.WriteLine("                  Asset Tracker                ");
Console.WriteLine("***********************************************\n");
Console.ResetColor();

MainMenu(); //Calls the method for the Main Menu

//Method for the Main Menu
void MainMenu()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\nPlease select option\n");
    Console.WriteLine("1. Add Asset");
    Console.WriteLine("2. List Assets");
    Console.WriteLine("0. Quit and Exit");
    Console.ResetColor();

    Console.Write("\nEnter number: ");
    string menuInput = Console.ReadLine();
    switch (menuInput)
    {
        case "1":
            AddAssets();
            break;
        case "2":
            PrintAssets();
            break;
        case "0":
            Console.WriteLine("\nClosing application");
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid option. Please try again.");
            Console.ResetColor();
            MainMenu();
            break;

    }
}

//Methods used by Menu:

void AddAssets()    //Method for getting user input and sign them to the different components of each corresponding class
{
    while (true)
    {
        Console.Write("Add Category 1 = Computer 2 = Phone (or type 'main' to return to menu): ");
        string categoryInput = Console.ReadLine();
        if (categoryInput.Trim().ToLower() == "main")
        {
            MainMenu();
            break;
        }

        string category = "";   //Declaring the string category with a null value

        switch(categoryInput)
        {
            case "1": category = "Computer";
                break;
            case "2": category = "Phone";
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option. Please try again.");
                Console.ResetColor();
                continue;
        }

        Console.Write("Add Brand (or type 'main' to return to menu): ");
        string brand = Console.ReadLine();
        if (brand.Trim().ToLower() == "main")
        {
            MainMenu();
            break;
        }

        Console.Write("Add Model (or type 'main' to return to menu): ");
        string model = Console.ReadLine();
        if (model.Trim().ToLower() == "main")
        {
            MainMenu();
            break;
        }

        //Using switch to make sure user can only input one of the offices available in the company

        Console.Write("Add Office 1 = Sweden 2 = United Kingdom 3 = France (or type 'main' to return to menu): ");
        string officeInput = Console.ReadLine();
       
        if (officeInput.ToLower().Trim() == "main")
        {
            MainMenu();
            break;
        }
        
        string office = ""; //Declaring the string office with a null value
        string curr = "";   //Declaring the string curr with a null value

        switch (officeInput)
        {
            case "1": office = "SWE";
                      curr = "SEK";
                      break;
            case "2": office = "UK";
                      curr = "GBP";
                      break;
            case "3": office = "FRA";
                      curr = "EUR";
                      break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option. Please try again.");
                Console.ResetColor();
                continue;
        }
        
        DateTime pDate;
        while (true)
        {
            Console.Write("Add Purchase Date (yyyy-MM-dd) (or type 'main' to return to menu): ");
            string pDateInput = Console.ReadLine().Trim().ToLower();
            if (pDateInput == "main")
            {
                MainMenu();
                return;
            }
            if (DateTime.TryParse(pDateInput, out pDate))
            {
                break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid date. Please enter a valid date in yyyy-MM-dd format.");
            Console.ResetColor();
        }


        double price;
        while (true)
        {
            Console.Write("Add Price (or type 'main' to return to menu): ");
            string priceInput = Console.ReadLine().Trim().ToLower();
            if (priceInput == "main")
            {
                MainMenu();
                return; // Exit method instead of breaking the outer loop
            }
            if (double.TryParse(priceInput, out price))     //Error checking to make sure input is a numerical value
            {
                break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid price. Please enter a valid number.");   //Error message if input is invalid
            Console.ResetColor();
        }

        assets.Add(new Asset(category, brand, model, office, pDate, price, curr));  //Creating a new object of "Asset" and adding inputs

        if (category == "phone")    //Adding the inputs to the phone class if it is a phone
        {
            phones.Add(new Phone(category, brand, model, office, pDate, price, curr));
        }
        else if (category == "computer")    //Adding the inputs to the phone class if it is a computer
        {
            computers.Add(new Computer(category, brand, model, office, pDate, price, curr));
        }

        SaveAssets();//Calling Method for saving the added Asset to the Json file

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nProduct added!\n");    //Making it more user friendly
        Console.ResetColor();
    }
}

//Method for printing out the Asset List
void PrintAssets()
{
    AssetSorter sorter = new AssetSorter();
    List<Asset> sortedAssets = sorter.SortAssets(assets);

    Console.WriteLine("\n-------------------------------------------------------------------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("\n      List of Assets  -  List of Assets  -  List of Assets  -  List of Assets  -  List of Assets  ");
    Console.ResetColor();
    Console.WriteLine("\n-------------------------------------------------------------------------------------------------------------");
    Console.WriteLine($"{"Category".PadRight(18)}{"Brand".PadRight(18)}{"Model".PadRight(18)}{"Office".PadRight(18)}{"Purchase Date".PadRight(18)}{"Price".PadRight(10)}{"Currency".PadRight(10)}");
    Console.WriteLine("-------------------------------------------------------------------------------------------------------------");

    foreach (Asset asset in sortedAssets)
    {
        // Calculate remaining lifespan in months
        TimeSpan remainingLifespan = asset.PDate.AddYears(3) - DateTime.Now;
        int remainingMonths = (int)Math.Floor(remainingLifespan.TotalDays / 30.436875); // Approximate number of days in a month

        // Set console color based on remaining lifespan
        if (remainingMonths <= 6 && remainingMonths > 3 )
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        else if (remainingMonths <= 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ResetColor();
        }

        // Print asset information
        Console.WriteLine($"{asset.Category.ToUpper().PadRight(18)}{asset.Brand.PadRight(18)}{asset.Model.PadRight(18)}{asset.Office.ToUpper().PadRight(18)}{asset.PDate.ToString("yyyy-MM-dd").PadRight(18)}{asset.Price.ToString().PadRight(10)}{asset.Curr.ToUpper().PadRight(10)}");

        // Reset console color after printing each asset
        Console.ResetColor();
    }

    MainMenu();
}

//Method for loading the Assets already in the json file
void LoadAssets()
{
    if (!File.Exists("assets.json"))
        return;

    string json = File.ReadAllText("assets.json");
    assets = JsonSerializer.Deserialize<List<Asset>>(json);

    foreach (var asset in assets)
    {
        if (asset.Category.ToLower().Trim() == "phone")
            phones.Add(new Phone(asset.Category, asset.Brand, asset.Model, asset.Office, asset.PDate, asset.Price, asset.Curr));
        else if (asset.Category.ToLower().Trim() == "computer")
            computers.Add(new Computer(asset.Category, asset.Brand, asset.Model, asset.Office, asset.PDate, asset.Price, asset.Curr));
    }
}

//Method for saing Assets added by user to the json file
void SaveAssets()
{
    try
    {
        string json = JsonSerializer.Serialize(assets, new JsonSerializerOptions { WriteIndented = true });
        string filePath = "assets.json";
        File.WriteAllText(filePath, json);
        Console.WriteLine($"Assets saved to {Path.GetFullPath(filePath)}");
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error saving assets: {ex.Message}");
        Console.ResetColor();
    }
}

//The Paren Class - Asset with corresponding Constructor
public class Asset
{
    public Asset() { }//Making class serializable
    public Asset(string category, string brand, string model, string office, DateTime pDate, double price, string curr)
    {
        Category = category;
        Brand = brand;
        Model = model;
        Office = office;
        PDate = pDate;
        Price = price;
        Curr = curr;
    }

    public string Category { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Office { get; set; }
    public DateTime PDate { get; set; }
    public double Price { get; set; }
    public string Curr { get; set; }
}

//Child Class "Phone" that inherits from "Asset"
public class Phone : Asset
{
    public Phone() { }
    public Phone(string category, string brand, string model, string office, DateTime pDate, double price, string curr) : base("Phone", brand, model, office, pDate, price, curr) { }
}

//Child Class "Computer" that inherits from "Asset"
public class Computer : Asset
{
    public Computer() { }
    public Computer(string category, string brand, string model, string office, DateTime pDate, double price, string curr) : base("Computer", brand, model, office, pDate, price, curr) { }
}

//Class for sorting the Assets first by Category and then by Price
class AssetSorter
{
    public List<Asset> SortAssets(List<Asset> assets)
    {
        var sortedAssets = assets
            .GroupBy(asset => asset.Office)
            .SelectMany(group => group.OrderBy(asset => asset.PDate))
            .ToList();

        return sortedAssets;
    }
}