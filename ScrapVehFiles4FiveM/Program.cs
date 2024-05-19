using ScrapVehFiles4FiveM;

// TODO: Get path from package and copy files 
// TODO: Create package at the same location

Vehicles? vehicles = null;
if (args.Length == 0)
{
    Console.WriteLine("Package: ");
    string? package = Console.ReadLine();
    if (string.IsNullOrEmpty(package))
    {
        Console.WriteLine("No package provided");
        return 1;
    }
    vehicles = new Vehicles(package);
}
else
{
    vehicles = new Vehicles(args[0]);
}
bool ok = vehicles.SearchFilesInPackage();
if (ok)
{
    vehicles.GeneratePackage();
    return 0;
}
else
{
    return 1;
}
