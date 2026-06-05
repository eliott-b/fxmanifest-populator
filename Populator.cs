namespace FxManifestPopulator
{
    public static class Populator
    {
        private const string FxManifestFileName = "fxmanifest.lua";
        private const string FxManifestTemplateFileName = "fxmanifest-template.lua";

        private static readonly List<string> _dataFileTypes =
        [
            "HANDLING_FILE",
            "VEHICLE_METADATA_FILE",
            "CARCOLS_FILE",
            "VEHICLE_VARIATION_FILE",
            "DLCTEXT_METADATA_FILE",
            "AMBIENT_PED_MODEL_SET_FILE",
            "PED_METADATA_FILE",
            "VEHICLE_MODEL_SET_FILE"
        ];
        private static readonly Dictionary<string, string> _dataTypeToDataFileName = new()
        {
            { "HANDLING_FILE","handling.meta" },
            { "VEHICLE_METADATA_FILE", "vehicles.meta" },
            { "CARCOLS_FILE", "carcols.meta" },
            { "VEHICLE_VARIATION_FILE", "carvariations.meta" },
            { "DLCTEXT_METADATA_FILE", "dlctext.meta" },
            { "AMBIENT_PED_MODEL_SET_FILE", "ambientpedmodelsets.meta" },
            { "PED_METADATA_FILE", "peds.meta" },
            { "VEHICLE_MODEL_SET_FILE", "vehiclemodelsets.meta" }
        };

        public static void PopulateFxManifest(string resourceDirectory)
        {
            Console.WriteLine("Parsing resoucre directory...");
            string[] metaFiles = Directory.GetFiles(resourceDirectory, "*.meta", SearchOption.AllDirectories);
            string[] mapFiles = Directory.GetFiles(resourceDirectory, "*.ymap", SearchOption.AllDirectories);
            if (metaFiles.Length > 0)
                Console.WriteLine($"Found {metaFiles.Length} .meta files.");
            if (mapFiles.Length > 0)
                Console.WriteLine($"Found {mapFiles.Length} .ymap files.");

            Console.WriteLine($"Populating fxmanifest.lua in '{resourceDirectory}'...");
            string templatePath = Path.Combine(AppContext.BaseDirectory, FxManifestTemplateFileName);
            List<string> fxManifestContent = File.ReadAllLines(templatePath).ToList();

            // check map files
            if (mapFiles.Length > 0)
            {
                fxManifestContent.Add("\nthis_is_a_map 'yes'");
            }

            // check meta files and corresponding data types
            if (metaFiles.Length > 0)
            {
                // Add all meta files to the fxmanifest
                fxManifestContent.Add("\nfiles {");
                foreach (string metaFile in metaFiles)
                {
                    string relativePath = Path.GetRelativePath(resourceDirectory, metaFile).Replace("\\", "/");
                    fxManifestContent.Add($"    '{relativePath}',");
                }
                fxManifestContent.Add("}\n");

                // Add data file types based on the presence of specific meta files
                foreach (string dataFileType in _dataFileTypes)
                {
                    string expectedDataFileName = _dataTypeToDataFileName[dataFileType];
                    string? metaFile = metaFiles.FirstOrDefault(m => Path.GetFileName(m).Equals(expectedDataFileName, StringComparison.OrdinalIgnoreCase));
                    if (metaFile != null)
                    {
                        string relativePath = Path.GetRelativePath(resourceDirectory, metaFile).Replace("\\", "/");
                        fxManifestContent.Add($"{dataFileType} '{relativePath}'");
                    }
                }
            }

            string fxManifestPath = Path.Combine(resourceDirectory, FxManifestFileName);
            File.WriteAllLines(fxManifestPath, fxManifestContent);
        }
    }
}