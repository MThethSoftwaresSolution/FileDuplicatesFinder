using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FileDuplicatesFinder
{
    public class FileDuplicateDetector
    {
        public Outcome GetDuplicates(string rootDirectory, string outputFilePath)
        {
            //Starting location to begin our search
            var startLocation = Directory.GetDirectories(rootDirectory);
            Outcome outcome = new Outcome();

            if (IsDirectoryEmpty(rootDirectory))
            {
                //If there is nothing in the directory, just alert the user. No need to go furthure with execution. Just return a default response instance
                Console.WriteLine("No folders/files in the root directory provided");
                return outcome;
            }

            //Dictionary to hold hash of a file as key and path as value
            var cd = new ConcurrentDictionary<string, List<string>>();
            var outputDuplicates = new ConcurrentDictionary<string, List<string>>();

            //Add this location to our stack that will hold all directories for future processing
            var stack = new ConcurrentStack<string>();
            stack.PushRange(startLocation);

            //Look for folders inside our starting location, because were are going to use this step for all sub-folders we are going to loop 
            while (stack.Count > 0)
            {
                Parallel.For(0, stack.Count, x =>
                {
                    //Get first item from stack, else return this instance
                    if (!stack.TryPop(out var dir)) return;

                    try
                    {
                        //Get all subdirectories in this directory
                        foreach (var subDir in Directory.GetDirectories(dir))
                        {
                            //Add these sub directories to this directory following Graph Theory pattern: Breadth First Search (BFS)
                            //BFS is the manner of traversing hierarchical or graph-base data structure
                            stack.Push(subDir);
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //When the first directory is found, we can start to look for the files inside
                    try
                    {
                        //To differentiate between files on a byte level, we need to implement hash algorith. We picked MD5 for this task
                        using (var md5 = MD5.Create())
                        {
                            //Let us get all the files in this directory
                            foreach (var file in Directory.GetFiles(dir))
                            {
                                //Compute hash for the file
                                var key = md5.ComputeHash(File.ReadAllBytes(file))
                                .Aggregate("", (current, next) => current + next.ToString("X2"));

                                //Check ih hash exists in our directionary
                                if (!cd.ContainsKey(key))
                                {
                                    //Add hash and create new list to store all the file duplicates
                                    cd.TryAdd(key, new List<string>());
                                }

                                //Add file path to the list
                                cd[key].Add(file);

                                //Temporarly add all duplicates where path associated with our hash is greater than one.
                                if (cd[key].Count > 1)
                                {
                                    outputDuplicates.TryAdd(key, cd[key]);
                                }

                            }
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }

            //Group dictionary by hash as keys and list of file names as value then select list of paths.
            //Nested iteration is not a good idea in big datasets, However in this case it shouldn't compromise the perfomance
            //that much as we are dealing with the already dupplited files
            var similarList = outputDuplicates.GroupBy(f => f.Key)
            .Select(g => new { Files = g.Select(z => z.Value).ToList() }).ToList();

            //Iterate paths and update our response object
            foreach (var item in similarList)
            {
                foreach (var files in item.Files)
                {
                    outcome.DuplicatesCount = outcome.DuplicatesCount + files.Count();
                    //Write file names that are duplicates in our output file
                    using (var tw = new StreamWriter(outputFilePath, true))
                    {
                        //tw.WriteLine("New run at: "+ DateTime.Now.ToString());

                        foreach (var fileName in files)
                        {
                            try
                            {
                                Console.WriteLine(fileName);
                                outcome.OutputFileName = outputFilePath;
                                tw.WriteLine(fileName);

                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }



                }

            }

            return outcome;
        }

        public bool IsDirectoryEmpty(string rootDirectory)
        {
            return !Directory.EnumerateFileSystemEntries(rootDirectory).Any();
        }
    }
}
