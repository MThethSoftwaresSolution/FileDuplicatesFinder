///////////////////////////////////////////////////////
//  Author: Mboniseni Thethwayo                      //
//  Date: 17 June 2022                               //
//  Email: Mboniseh@gmail.com                        //
///////////////////////////////////////////////////////

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace FileDuplicatesFinder
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Entry point of the application...
            //Below is the directory in which the files that we will be iteration resides. Download <<duplicate-images.zip>>,
            //extract it and copy the path, then replace path value below.

            string path = @"C:\Users\Mboniseni\Downloads\duplicate-images";
            //Below is the filename where duplicates files will be logged
            string outputFilePath = path+"\\duplicates.txt";

            //Giving our directory and output file name permissions required for each
            FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, path);
            fileIOPermission.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, outputFilePath);
            try
            {
                fileIOPermission.Demand();

                //Response instance from the duplicates finder
                FileDuplicateDetector duplicateDetector = new FileDuplicateDetector();
                //Class where duplicates finder logic resides
                Outcome outcome = duplicateDetector.GetDuplicates(path, outputFilePath);

                //Number of duplicate found to be written in a console window
                Console.WriteLine("Total duplicate files found - {0}", outcome.DuplicatesCount);

                if (!string.IsNullOrEmpty(outcome.OutputFileName))
                {
                    Console.WriteLine("File containing duplicates file name - {0}", outcome.OutputFileName);
                }
            }
            catch (SecurityException s)
            {
                Console.WriteLine(s.Message);
            }

        }

        
    }


}
