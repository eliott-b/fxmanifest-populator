using FxManifestPopulator;

if (args.Length == 0)
{
    Console.WriteLine("Usage: fxmanifest-populator.exe <resource-directory>");
    return;
}

string resourceDirectory = args[0];
if (!Directory.Exists(resourceDirectory))
{
    Console.Error.WriteLine($"Error: Directory '{resourceDirectory}' does not exist.");
    return;
}

try
{
    Populator.PopulateFxManifest(resourceDirectory);
    Console.WriteLine("fxmanifest.lua has been successfully populated.");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
}
