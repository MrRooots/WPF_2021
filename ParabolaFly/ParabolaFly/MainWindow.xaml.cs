using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ParabolaFly {
  // Subclass that store data about current state of point in space
  // Also it provides main functions to calculate speed, coords etc. of the point
  public class Point {
    public double X, Y, MomentSpeed;                  // MomentSpeed = v(t) == speed at moment t
    public readonly double TimeStep;
    private readonly double _angle, _speed;
    private const double G = 9.81;

    // Constructor (by default all params equals 0)
    public Point(double timeStep, double x = 0, double y = 0, double speed = 0, double angle = 0) {
      X        = x;
      Y        = y;
      _speed    = speed;
      _angle    = ConvertToRadians(angle);  // To use Math.Sin(), Math.Cos()
      TimeStep = timeStep;
    }
    
    // Converts given angle in degrees into radians
    private static double ConvertToRadians(double a) { return a * Math.PI / 180; }
    
    // Simple function to calculate the square of given number
    private static double GetSquare(double value) { return value * value; }
    
    // Special function to calculate the total fly time
    public double GetFlyTime() { return 2 * _speed * Math.Sin(_angle) / G; }
    
    // Calculate X coord of the point at the moment t
    private double CalculateX(double time) { return _speed * Math.Cos(_angle) * time; }
    
    // Calculate Y coord of the point at the moment t
    private double CalculateY(double time) { return _speed * Math.Sin(_angle) * time - (G * GetSquare(time) / 2); }
    
    // Calculate the speed of the point at the moment t
    private double CalculateSpeed(double time) {
      double speedX = _speed * Math.Cos(_angle), 
             speedY = _speed * Math.Sin(_angle);
      
      return Math.Sqrt((speedY - G * time) * (speedY - G * time) + GetSquare(speedX));
    }
    
    // Calculate the fly length
    private double GetFlyLength() { return GetSquare(_speed) * Math.Sin(2 * _angle); }
    
    // Calculate the max height of the fly
    private double GetMaxFlyHeight() { return GetSquare(_speed * GetSquare(Math.Sin(_angle))) / 2 * G; }
    
    // Collect all params of the fly into one string
    public string CollectAllParams() {
      return string.Format(
        "Total fly time: {1}{0}Fly total length: {2}{0}Fly maximum height: {3}", 
        Environment.NewLine, GetFlyTime(), GetFlyLength(), GetMaxFlyHeight());
    }
    
    // Function to execute all calculation functions
    public void CalculateCoords(double time) {
      Y        = CalculateY(time);
      X        = CalculateX(time);
      MomentSpeed = CalculateSpeed(time);
    }
  }

  public partial class MainWindow {
    private static void StartVisualization(Point myPoint, FrameworkElement mainGrid) {
      // Variables
      const string filename = "output.out"; // Filename
      double currentTime = 0;
      string line;
      
      // Get the Text Block element and clear it (if exist)
      var output = (TextBlock) mainGrid.FindName("Output");
      if (output != null) output.Text = "";

      // File manipulations
      var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent; // Get the path to the file
      if (directoryInfo == null) return; // No such file in directory
      var path = Path.Combine(directoryInfo.FullName, filename); // Create path variable

      // Calculating position until the point reaches the ground (x, 0)
      using (var fileStream = new StreamWriter(path)) {
        if (output != null) {
          while (currentTime < myPoint.GetFlyTime()) {
            currentTime += myPoint.TimeStep;
            
            myPoint.CalculateCoords(Math.Round(currentTime, 4));
            
            line = string.Format("Time (t): {0}\tX coord: {1}\tY coord: {2}\tSpeed at moment t: {3}",
              Math.Round(currentTime, 3),
              myPoint.X, myPoint.Y, myPoint.MomentSpeed);

            if (!(myPoint.Y >= 0)) continue;
            fileStream.WriteLine(line);
            output.Text += line + Environment.NewLine;
          }
        }

        line = string.Format("Touch Down! (At {0} seconds)", myPoint.GetFlyTime());
        fileStream.Write(line);
        if (output != null) output.Text += line;
      }

      var paramsOutput = (TextBlock) mainGrid.FindName("ParamsOutput");
      if (paramsOutput != null) paramsOutput.Text = myPoint.CollectAllParams();
    }

    public MainWindow() {
      InitializeComponent();

      // Collect the input data into subclass Point
      CalculateButton.Click += (sender, args) => {
        // Check if all fields are not empty
        if (InputAngle.Text != string.Empty && InputSpeed.Text != string.Empty && InputTimeStep.Text != string.Empty) {
          const int x = 0, y = 0;
          var speed    = Convert.ToDouble(InputSpeed.Text.Replace('.', ','));
          var angle    = Convert.ToDouble(InputAngle.Text.Replace('.', ','));
          var timeStep = Convert.ToDouble(InputTimeStep.Text.Replace('.', ','));
          var myPoint = new Point(timeStep, x, y, speed, angle);  // Create a new Point object

          StartVisualization(myPoint, MainGrid);                  // This method will start drawing the myPoint fly
        }
        else {
          // If one or more fields are empty -> Error message
          MessageBox.Show("Some fields are empty! Please fill them all!");
        }
      };
    }
  }
}