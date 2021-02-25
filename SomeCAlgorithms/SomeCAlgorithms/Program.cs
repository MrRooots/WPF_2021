using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SomeCAlgorithms {
  internal static class Program {
    private static void GetSortReturn(string filename = "file.in") {
      // Trying to get the path to the file
      try {
        var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
        if (directoryInfo == null) return;
        string line;
        var path = Path.Combine(directoryInfo.FullName, filename);
          
        // Read line of numbers from file
        using (var fileStream = new StreamReader(path)) { line = fileStream.ReadLine(); }
          
        // Iterating through splitted by ' ' line
        if (line != null) {  // If line exist
          // Collect !numbers! from the line into List
          var listToSort = line.Split(' ').Select(Convert.ToDouble).ToList();
          listToSort.Sort();  // Sorting the array
            
          File.WriteAllText(path, string.Empty);  // Clear all file content
            
          // Write all sorted array content into file.in
          using (var fileStream = new StreamWriter(path)) {
            foreach (var number in listToSort) {
              fileStream.Write(number.ToString(CultureInfo.InvariantCulture) + " ");
            }  
          }
        }
      }
      catch (Exception e) {
        Console.WriteLine("Error occured: " + e.Message);
      }

    }

    public static void Main(string[] args) {
      // Read array array from file, sort it and put it back
      Console.Write("Enter a file name (default file.in): ");
      var filename = Console.ReadLine();
      
      if (filename != "") {
        GetSortReturn(filename);
      }
      else {
        GetSortReturn();
      }
    }
  }
}