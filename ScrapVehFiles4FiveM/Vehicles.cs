using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrapVehFiles4FiveM
{
    /**
    * Structure of the data file format
    */
    struct DataFileFormat
    {
        public readonly string Handling = "HANDLING_FILE";
        public readonly string VehicleMeta = "VEHICLE_METADATA_FILE";
        public readonly string Carcols = "CARCOLS_FILE";
        public readonly string Carvariations = "VEHICLE_VARIATION_FILE";
        public readonly string Dlctext = "DLCTEXT_METADATA_FILE";
        public readonly string Layout = "VEHICLE_LAYOUTS_FILE";

        public DataFileFormat()
        {
        }
    }

    /**
    * Class to parse the vehicles files and generate a FiveM package
    */
    public class Vehicles
    {
        private string _package;
        
        // META FILES
        private string _vehicleMeta;
        private string _handlingMeta;
        private string _carcolsMeta;
        private string _carvariationsMeta;
        private string _dlctextMeta;
        private string _vehicleNamesLua;
        private string _vehicleLayoutsMeta;

        private string[] _stream;

        /**
        * Constructor
        * @param inPackage Path to the package
        */
        public Vehicles(string inPackage)
        {
            _package = inPackage;
        }

        /**
        * Search for the files in the package
        * @return bool True if the package exists, false otherwise
        */
        public bool SearchFilesInPackage()
        {
            if (! Directory.Exists(_package) && ! File.Exists(_package))
            {
                Console.WriteLine("Packages does not exist");
                return false;
            }
            if (File.Exists(_package) && Path.GetExtension(_package) != ".zip")
            {
                Console.WriteLine("Packages is not a zip file");
                return false;
            }

            if (File.Exists(_package))
            {
                // Unzip the file
                System.IO.Compression.ZipFile.ExtractToDirectory(_package, Path.GetFileNameWithoutExtension(_package));
                _package = Path.GetFileNameWithoutExtension(_package);
            }
            // Search for the files
            string[] files = Directory.GetFiles(_package, "*.meta", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                switch (file)
                {
                    case "handling.meta":
                        _handlingMeta = file;
                        break;
                    case "vehicles.meta":
                        _vehicleMeta = file;
                        break;
                    case "carcols.meta":
                        _carcolsMeta = file;
                        break;
                    case "carvariations.meta":
                        _carvariationsMeta = file;
                        break;
                    case "dlctext.meta":
                        _dlctextMeta = file;
                        break;
                    case "vehiclelayouts.meta":
                        _vehicleLayoutsMeta = file;
                        break;
                    default:
                        break;
                }
            }

            // Search for the stream files
            string[] ytd = Directory.GetFiles(_package, "*.ytd", SearchOption.AllDirectories);
            string[] yft = Directory.GetFiles(_package, "*.yft", SearchOption.AllDirectories);
            _stream = ytd.Concat(yft).ToArray();

            // Search for the vehicle names
            string[] lua = Directory.GetFiles(_package, "vehicle_names.lua", SearchOption.AllDirectories);
            _vehicleNamesLua = lua[0];
            
            return true;
        }

        /**
        * Generate the out package
        */
        public void GeneratePackage()
        {
            string outPackage = Path.GetFileNameWithoutExtension(_package) + "_out";
            Directory.CreateDirectory(outPackage);

            // Copy the files
            CopyFiles(outPackage);

            // Copy the stream files
            CopyStreamFiles(outPackage);

            // Write the fxmanifest
            WriteFxManifest(outPackage);
        }

        /**
        * Copy the files (Lua and Meta) to the out package
        * @param outPackage Path to the out package
        */
        private void CopyFiles(string outPackage)
        {
            if (! String.IsNullOrEmpty(_handlingMeta))
            {
                File.Copy(_handlingMeta, Path.Combine(outPackage, "handling.meta"));
            }
            if (! String.IsNullOrEmpty(_vehicleMeta))
            {
                File.Copy(_vehicleMeta, Path.Combine(outPackage, "vehicles.meta"));
            }
            if (! String.IsNullOrEmpty(_carcolsMeta))
            {
                File.Copy(_carcolsMeta, Path.Combine(outPackage, "carcols.meta"));
            }
            if (! String.IsNullOrEmpty(_carvariationsMeta))
            {
                File.Copy(_carvariationsMeta, Path.Combine(outPackage, "carvariations.meta"));
            }
            if (! String.IsNullOrEmpty(_dlctextMeta))
            {
                File.Copy(_dlctextMeta, Path.Combine(outPackage, "dlctext.meta"));
            }
            if (! String.IsNullOrEmpty(_vehicleLayoutsMeta))
            {
                File.Copy(_vehicleLayoutsMeta, Path.Combine(outPackage, "vehiclelayouts.meta"));
            }
            if (! String.IsNullOrEmpty(_vehicleNamesLua))
            {
                File.Copy(_vehicleNamesLua, Path.Combine(outPackage, "vehicle_names.lua"));
            }
        }

        /**
        * Copy the stream files (ytd and yft) to the out package
        * @param outPackage Path to the out package
        */
        private void CopyStreamFiles(string outPackage)
        {
            Directory.CreateDirectory(Path.Combine(outPackage, "stream"));
            foreach (string file in _stream)
            {
                File.Copy(file, Path.Combine(outPackage, "stream", Path.GetFileName(file)));
            }
        }

        /**
        * Write the FxManinest to the out package
        * @param outPackage Path to the out package
        */
        private void WriteFxManifest(string outPackage)
        {
            DataFileFormat dataFileFormat = new DataFileFormat();
            string fxManifest = Path.Combine(outPackage, "fxmanifest.lua");
            using (StreamWriter sw = new StreamWriter(fxManifest))
            {
                sw.WriteLine("fx_version 'cerulean'");
                sw.WriteLine("game 'gta5'");
                sw.WriteLine("lua54 'true'");
                sw.WriteLine("");
                sw.WriteLine("files {");
                sw.WriteLine("    '*.meta'");
                sw.WriteLine("}");
                sw.WriteLine("");
                if (! String.IsNullOrEmpty(_handlingMeta))
                {
                    sw.WriteLine("data_file '" + dataFileFormat.Handling + "' '" + Path.GetFileName(_handlingMeta) + "'");
                }
                if (! String.IsNullOrEmpty(_vehicleLayoutsMeta))
                {
                    sw.WriteLine("data_file '" + dataFileFormat.Layout + "' '" + Path.GetFileName(_vehicleLayoutsMeta) + "'");
                }
                if (! String.IsNullOrEmpty(_vehicleMeta))
                {
                    sw.WriteLine("data_file '" + dataFileFormat.VehicleMeta + "' '" + Path.GetFileName(_vehicleMeta) + "'");
                }
                if (! String.IsNullOrEmpty(_carcolsMeta))
                {
                    sw.WriteLine("data_file '" + dataFileFormat.Carcols + "' '" + Path.GetFileName(_carcolsMeta) + "'");
                }
                if (! String.IsNullOrEmpty(_carvariationsMeta))
                {
                    sw.WriteLine("data_file '" + dataFileFormat.Carvariations + "' '" + Path.GetFileName(_carvariationsMeta) + "'");
                }
                if (! String.IsNullOrEmpty(_dlctextMeta))
                {
                    sw.WriteLine("data_file '" + dataFileFormat.Dlctext + "' '" + Path.GetFileName(_dlctextMeta) + "'");
                }
                sw.WriteLine("");
                if (! String.IsNullOrEmpty(_vehicleNamesLua))
                {
                    sw.WriteLine("client_script 'vehicle_names.lua'");
                    sw.WriteLine("");
                }
            }
        }
    }
}