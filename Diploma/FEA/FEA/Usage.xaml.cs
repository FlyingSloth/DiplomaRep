using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FEA
{
	/// <summary>
	/// Interaction logic for Usage.xaml
	/// </summary>
	public partial class Usage : Window
	{
		public Usage()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			textBlock2.Text = "Common characteristics:" + Environment.NewLine + 
								Environment.NewLine + 
								"Layers characteristics:" + Environment.NewLine +
"                               Number of Layers: integer number between 1 and 20;" + Environment.NewLine +
"                               Radiuses: decimal numbers, showing distanve between waveguide's center and layer edge;" + Environment.NewLine +
"                               Permittivities: decimal numbers." + Environment.NewLine +
							    Environment.NewLine +
								"Mode number: integer number between 1 and 25;" + Environment.NewLine +
							    Environment.NewLine +
								"Number of finite elements: integer number between 200 and 1100 - to have best precision and not to overload PC." + Environment.NewLine + 
								Environment.NewLine +
								"Dispersion characteristics (propagation constants of waveguide for every wavenumber):" + Environment.NewLine +
"                               Number of steps of wavenumber;" + Environment.NewLine +
"                               Size of step of wavenumber." + Environment.NewLine +
							    Environment.NewLine +
								"Critical conditions/values(for 2 layers only):" + Environment.NewLine +
"                               data for dispchar and step of radius change.";

			textBlock3.Text = "    All required data is entered on Introduction Page. Layers characteristics are entered using delimeter '|'. At every field you can see hint."
				+ Environment.NewLine +
				"   After entering data press \"Go\"-button and the dialog showing progress appears. Process may last for a long time. During this time program night be interrupted by pressing \"Abort\"-button. This action rolls the program back to Initial Data Page make changes or to terminate program."
				+ Environment.NewLine +
				"   When timer and progressbar show that process is completed, press \"Ok\"-button to see the results"
				+ Environment.NewLine +
				"   Finally, you can see the results on charts and you can save them (button \"Save\"). The results are saved in folder which automatically appears in program folder, this folder contains file with numerical data and png-pictures of charts.";
		}
	}
}
