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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using FEA;

namespace FEA
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			txtbxRadius.Text = "";
			txtbxPerm.Text = "";
		}
		#region "Global params"
		public BackgroundWorker bw;
		WorkObject obj;
		Progress pr;

		//layers
		double[] Rad;
		double[] Perm;
		#endregion
		#region "Initial data"
		int layersN = 0;
        int FEN = 0;
        int mode = 0;
        int stepWNN = 0;

        double stepWNSize = 0.0;
        double stepRSize = 0.0;
        
        public bool isCritVal = false;
		public bool isDispersion = true;

		public WorkObject.CRIT[] crit;
		public WorkObject.DISP[] disp;
		WorkObject.LAY[] Layers;
        #endregion
        #region "Entering data"
        private void txtbLayersNumber_LostFocus(object sender, RoutedEventArgs e)
        {
			this.txtbxRadius.Text = "";
			this.txtbxPerm.Text = "";
			if (txtbFEN.Text.Length != 0)
            {
				int test;
                if (int.TryParse(txtbLayersNumber.Text, out test))
                {
					if (Posit(test))
					{
						this.layersN = test;

						Rad = new double[test];
						Perm = new double[test];

						for (int i = 0; i < test - 1; i++)
						{
							this.txtbxRadius.Text += ";";
							this.txtbxPerm.Text += ";";
						}
						this.txtbxRadius.Text += "1";
						this.txtbxPerm.Text += "1";
					}
					if (txtbLayersNumber.Text == "2")
					{
						rdbtnCrit.Visibility = Visibility.Visible;
						label8.Visibility = Visibility.Visible;
						chbCritVal.Visibility = Visibility.Visible;
						txtRStep.Visibility = Visibility.Visible;

					}
					else
					{
						rdbtnCrit.Visibility = Visibility.Hidden;
						label8.Visibility = Visibility.Hidden;
						chbCritVal.Visibility = Visibility.Hidden;
						txtRStep.Visibility = Visibility.Hidden;
					}
                }
            }
        }
        private void txtbFEN_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbFEN.Text.Length != 0)
            {
                int test;
                if (int.TryParse(txtbFEN.Text, out test))
                {
                    if (test >= 200)
                        this.FEN = test;
                    else
                    {
                        this.FEN = 200;
                        MessageBox.Show("Number of finite elements might not be less than 200. Now it's automatically set as 200.", "Warning", MessageBoxButton.OK);

                    }
                }
            }
        }
        private void txtModeN_LostFocus(object sender, RoutedEventArgs e)
        {
        int test;
        if (int.TryParse(txtModeN.Text, out test))
            {
                if (Posit(test))
                    this.mode = test;
            }
        }
        private void txtWNstepN_LostFocus(object sender, RoutedEventArgs e)
        {
            int test;
            if (int.TryParse(txtWNstepN.Text, out test))
                {
                    if (Posit(test))    
                        this.stepWNN = test;
                }
        }
        private void txtWNsteps_LostFocus(object sender, RoutedEventArgs e)
            {
                double test;
                if (double.TryParse(txtWNsteps.Text, out test))
                    {
                        if (Posit(test))
                            this.stepWNSize = test;
                    }
            }
        private void txtRStep_LostFocus(object sender, RoutedEventArgs e)
        {
            double test;
            if (double.TryParse(txtRStep.Text, out test))
                {
                    if (Posit(test))    
                        this.stepRSize = test;
                }
        }
        private void chbCritVal_Checked(object sender, RoutedEventArgs e)
        {
            this.isCritVal = true;
        }
        private void rdbtnCrit_Checked(object sender, RoutedEventArgs e)
        {
            this.isDispersion = false;
        }
		private void rdbtnDisp_Checked(object sender, RoutedEventArgs e)
		{
			this.isDispersion = true;
		}
        #endregion
        #region "Validation"
        private bool isValid()
        {
            string msg = "";
            bool allValid = true;
            if (layersN == 0)
            {
                msg += "Fill Number of Layers\n";
                allValid = false;
            }
            if(mode == 0)
            {
                msg += "Fill Mode Number\n";
                allValid = false;
            }
            if (stepWNN == 0)
            {
                msg += "Fill Number of Steps of WaveNumber";
                allValid = false;
            }
            if (stepWNSize == 0)
            {
                msg += "Fill Size of Steps of WaveNumber";
                allValid = false;
            }
			if (!isDispersion)
			{
				if (isCritVal && stepRSize == 0)
				{
					msg += "Fill Size of Steps of Radius";
					allValid = false;
				}
			}
            if (!allValid)
            {
                MessageBox.Show(msg, "Not all data provided!", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private bool Posit(int i)
        {
            if (i > 0) return true;
            return false;
        }

        private bool Posit(double i)
        {
            if (i > 0) return true;
            return false;
        }
        #endregion
		#region "ParseLayers"
		private bool isR(string R)
		{
			string[] str = new string[layersN];
			char[] sep = {';'};
			str = R.Split(sep);
			for (int i = 0; i < layersN; i++)
			{
				double r;
				if (double.TryParse(str[i], out r) && r > 0)
				{
					if (i < layersN - 1 && r >= 1) return false;
					Rad[i] = r;
				}
				else return false;
			}
			return true;
		}

		private bool isE(string E)
		{
			string[] str = new string[layersN];
			char[] sep = { ';' };
			str = E.Split(sep);
			for (int i = 0; i < layersN; i++)
			{
				double e;
				if (double.TryParse(str[i], out e))
				{
					Perm[i] = e;
				}
				else return false;
			}
			return true;
		}

		private WorkObject.LAY[] layers(bool isR, bool isE)
		{
			if (isR && isE)
			{
				for (int i = 0; i < layersN; i++)
				{
					Layers[i].R = Rad[i];
					Layers[i].perm = Perm[i];
				}
				return Layers;
			}
			return null;
		}
		#endregion
		private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
				Layers = new WorkObject.LAY[layersN];
				Layers = layers(isR(this.txtbxRadius.Text), isE(this.txtbxPerm.Text));
				if (Layers != null)
				{
					pr = new Progress(this);
					pr._bg.DoWork+=new DoWorkEventHandler(bw_DoWork);
					this.Hide();
					pr.Show();
					pr.Closed += new EventHandler(pr_Closed);
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("Enter characteristics of Layers!", "Error", System.Windows.Forms.MessageBoxButtons.OK);
				}
            }
			
        }
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		void pr_Closed(object sender, EventArgs e)
		{
			if (pr.isExit) this.Close();
		}
		#region "BackGroundWorker functions"
		void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			obj = new WorkObject();
			var bgw = sender as BackgroundWorker;
			if (this.isDispersion)
			{
				disp = new WorkObject.DISP[stepWNN];
				int coef = 0;
				pr.characteristics = "Dispersion LayersN " + layersN.ToString() + " Step of wavenumber " +  stepWNSize.ToString();
				disp = obj.dispersion(FEN, stepWNN, stepWNSize, mode, Layers, ref bgw, ref coef, 1, false);
			}
			else
			{
				int N = Convert.ToInt32(1.0 / stepRSize);
				crit = new WorkObject.CRIT[2*N];
				if (isCritVal)
					pr.characteristics = "Critical values. ";
				pr.characteristics = "Critical conditions. ";
				pr.characteristics += "LayersN " + layersN.ToString() + " Step of wavenumber " + stepWNSize.ToString();
				crit = obj.Crit(FEN, stepRSize, stepWNN, stepWNSize, mode, Layers, ref bgw, !isCritVal);
			}
			pr._bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
		}
		void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			pr.SetTime("Process completed");
		}
		#endregion

		private void MenuItem_Click_1(object sender, RoutedEventArgs e)
		{
			About ab = new About();
			ab.Show();
		}

		private void MenuItem_Click_2(object sender, RoutedEventArgs e)
		{
			Usage us = new Usage();
			us.Show();
		}

		private void MenuItem_Click_3(object sender, RoutedEventArgs e)
		{
			Authors au = new Authors();
			au.Show();
		}
	}
}
