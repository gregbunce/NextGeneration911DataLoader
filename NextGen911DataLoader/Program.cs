﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core;
using ArcGIS.Core.Data;

namespace NextGen911DataLoader
{
    class Program
    {
        [STAThread]

        static void Main(string[] args)
        {
            bool eltRoads = false;
            bool etlAddresspoints = false;
            bool etlPsaps = false;
            bool etlMuni = false;

            // Check that minimum command line args are present.
            if (args.Length < 4)
            {
                Console.WriteLine("You must provide the following command line arguments: [location of output fgdb database], [sde instance], [sde database name], [sde user/pass], [list of valid layer names to elt (in any order): roads, addresspoints, psaps, muni]");
                return;
            }

            // Check what layers the user wants to etl.
            foreach (string s in args)
            {
                System.Console.WriteLine(s);

                switch (s)
                {
                    case "roads":
                        eltRoads = true;
                        break;
                    case "addresspoints":
                        etlAddresspoints = true;
                        break;
                    case "psaps":
                        etlPsaps = true;
                        break;
                    case "muni":
                        etlMuni = true;
                        break;
                    default:
                        break;
                }
            }

            // Host.Initialize before constructing any objects from ArcGIS.Core
            try
            {
                ArcGIS.Core.Hosting.Host.Initialize();
            }
            catch (Exception e)
            {
                // Error (missing installation, no license, 64 bit mismatch, etc.)
                Console.WriteLine(string.Format("Initialization failed: {0}", e.Message));
                return;
            }

            string fgdbPath = args[0];

            //// Connect to File Geodatabase
            //Geodatabase NG911Utah = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(args[0])));

            // Connect to SDID Database //
            DatabaseConnectionProperties sgidConnectionProperties = commands.ConnectToSGID.Execute(args[1], args[2], args[3]);

            // ETL Psap Data to NG911
            if (etlPsaps)
            {
                commands.LoadPsapData.Execute(sgidConnectionProperties, fgdbPath);
            }

            // ETL Roads Data to NG911
            if (eltRoads)
            {
                commands.LoadRoads.Execute(sgidConnectionProperties, fgdbPath);
            }

            // ETL address point to NG911
            if (etlAddresspoints)
            {
                commands.LoadAddressPnts.Execute(sgidConnectionProperties, fgdbPath);
            }


            // Get SGID feature classes //
            //FeatureClass psap = sgid.OpenDataset<FeatureClass>("SGID10.SOCIETY.PSAPBoundaries")
            //FeatureClass roads = sgid.OpenDataset<FeatureClass>("SGID10.TRANSPORTATION.Roads");
            //FeatureClass addressPnts = sgid.OpenDataset<FeatureClass>("SGID10.LOCATION.AddressPoints");
            //FeatureClass muni = sgid.OpenDataset<FeatureClass>("SGID10.BOUNDARIES.Municipalities");

            // Keep the console window open
            Console.WriteLine("Done!  Press any key to continue...");
            Console.Read();
        }
    }
}
