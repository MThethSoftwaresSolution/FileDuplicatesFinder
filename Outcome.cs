using System;
using System.Collections.Generic;
using System.Text;

namespace FileDuplicatesFinder
{
    public class Outcome
    {
        /// <summary>
        /// DuplicatesCount that will be display in a console window
        /// </summary>
        public int DuplicatesCount { get; set; } = 0;
        /// <summary>
        /// File name where the duplicate's file name is listed
        /// </summary>
        public string OutputFileName { get; set; } = "";
    }
}
