using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParabolaFly {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  
  // Subclass that store data about current state of point in space
  public class Point {
    private double x, y, speed, angle;

    public Point(double x, double y, double speed, double angle) {
      this.x = x;
      this.y = y;
      this.speed = speed;
      this.angle = angle;
    }

    public double get_x() {
      return this.x;
    }

    public double get_y() {
      return this.y;
    }

    public double get_speed() {
      return this.speed;
    }

    public double get_angle() {
      return this.angle;
    }
  }

  public partial class MainWindow {
    private static void StartVisualization(Point myPoint) {
      while (myPoint.get_speed() >= 0) {
        
      }
    }

    public MainWindow() {
      InitializeComponent();
      double x = double.NaN, y = double.NaN, speed = double.NaN, angle = double.NaN;
      
      // Collect the input data into subclass Point
      CollectData.Click += (sender, args) => {
        x = Convert.ToDouble(InputXCoord.Text.Replace('.', ','));
        y = Convert.ToDouble(InputYCoord.Text.Replace('.', ','));
        speed = Convert.ToDouble(InputSpeed.Text.Replace('.', ','));
        angle = Convert.ToDouble(InputAngle.Text.Replace('.', ','));
        // Check if all fields are filled with data
        if (!double.IsNaN(x) && !double.IsNaN(y) && !double.IsNaN(speed) && !double.IsNaN(angle)) {
          var myPoint = new Point(
            x, y, speed, angle
          );

          StartVisualization(myPoint);  // This method will start drawing the myPoint 
        }
        else {
          MessageBox.Show("Some fields are empty! Please fill them all!");
        }

        // MessageBox.Show(
        //   myPoint.x.ToString() + ' ' + myPoint.y.ToString() + ' ' + myPoint.speed.ToString() + ' ' + myPoint.angle.ToString()
        // );
      };
    }
  }
}