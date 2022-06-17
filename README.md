**Author: Mboniseni Thethwayo
Date: 17 June 2022**

C# (.Net Core 3.1) Console application that find images/files duplicates in a given directory
by using Graph Theory algorithm: Breadth-First Search.

**Below is the breakdown of the application.**

1. Given a root directory, where all files/sub-folders are located, we note and assign the path 
   for our directory as well as filename for our text file where we write our outputs.
2. We demand necessary access rigths to the directory.
3. We make sure the directory exists, otherwise we console the error to the user.
4. We create a variable, of the type dictionary of string and list of string to hold hash of file as a key and path as value.
5. We Create a stack to hold our directories for future processing
6. We begin our our search at a root directory.
7. We add sub-directories found to stack (Traversing following BFS).
8. When a directory is found, we look for files inside.
9. To make sure that we can differentiate the files at byte level, we implement hash algorith (MD5).
10. Then we check if a file already exists in our dictionary, mentioned in point 4, If it does not exist,
   We add a new item in our dictionary, where key is file hash and value is the new list of string (file paths)
11. We Use file path to update list of lines in our output file: duplicate.txt as well as in console window including the count duplicates
    while grouping the dictionary with file hash.

**Instrusction to run the application**

1. Clone this public repo: https://github.com/MThethSoftwaresSolution/FileDuplicatesFinder.git
2. Change the path in Program.cs to point to your local directory, with source of the images as requested in the email, Make sure you download it: https://juafrbxe8v.sharepoint.com/sites/people/Shared%20Documents/Forms/AllItems.aspx?id=%2Fsites%2Fpeople%2FShared%20Documents%2F%5Ftemplates%2FTechnical%20Assessment%2Fduplicate%2Dimages%2Ezip&parent=%2Fsites%2Fpeople%2FShared%20Documents%2F%5Ftemplates%2FTechnical%20Assessment&p=true&ga=1
3. Then, run the application
4. Look for duplicates.txt file in the root folder that contains your images.

**Thanks**
