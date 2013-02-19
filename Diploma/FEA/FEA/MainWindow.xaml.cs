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
		#region Global params
		public BackgroundWorker bw;
		WorkObject obj;
		Progress pr;
		public Results _res;

		//layers
		double[] Rad;
		double[] pRad;
		double[] Perm;
		#endregion
		#region Initial data
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
        #region Entering data
		private void txtbLayersNumber_LostFocus(object sender, RoutedEventArgs e)
        {
			this.txtbxRadius.Text = "";
			this.txtbxPerm.Text = "";
			if (txtbFEN.Text.Length != 0)
            {
				int test;
				ParseInt(txtbLayersNumber.Text, label1.Content.ToString(), out test);
                if ( test != null)
                {
					if (Posit(test) && test <= 20)
					{
						this.layersN = test;

						Rad = new double[test];
						Perm = new double[test];

						for (int i = 0; i < test - 1; i++)
						{
							this.txtbxRadius.Text += "|";
							this.txtbxPerm.Text += "|";
						}
					}
					if (test == 2)
					{
						rdbtnCrit.IsEnabled = true;
						label8.IsEnabled = true;
						chbCritVal.IsEnabled = true;
						txtRStep.IsEnabled = true;

					}
					else
					{
						rdbtnCrit.IsEnabled = false;
						label8.IsEnabled = false;
						chbCritVal.IsEnabled = false;
						txtRStep.IsEnabled = false;
					}
                }
            }
        }
		private void txtbFEN_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbFEN.Text.Length != 0)
            {
                int test = 0;
				ParseInt(txtbFEN.Text, label3.Content.ToString(), out test);
                if (test != null)
                {
                    if (test >= 200 && test <= 1100)
                        this.FEN = test;
                    else
                    {
                        this.FEN = 200;
                        MessageBox.Show("Число конечных элементов не может быть меньше 200. Количество элементов автоматически выставлено 200", "Предупреждение", MessageBoxButton.OK);

                    }
                }
            }
        }
		private void txtModeN_LostFocus(object sender, RoutedEventArgs e)
        {
			ParseInt(txtModeN.Text, label2.Content.ToString(), out mode);
			if (!Posit(mode) || mode > 25)
				MessageBox.Show("Номер моды находится вне вычисляемых границ");
        }
		private void txtWNstepN_LostFocus(object sender, RoutedEventArgs e)
        {
			ParseInt(txtWNstepN.Text, label6.Content.ToString(), out this.stepWNN);
        }
		private void txtWNsteps_LostFocus(object sender, RoutedEventArgs e)
        {
			ParseDouble(txtWNsteps.Text, label7.Content.ToString(), out this.stepWNSize);
        }
		private void txtRStep_LostFocus(object sender, RoutedEventArgs e)
        {
			ParseDouble(txtRStep.Text, label8.Content.ToString(), out this.stepRSize);
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
        #region Validation
        private bool isValid()
        {
            string msg = "";
            bool allValid = true;
			if (this.layersN == 0)
			{
				if (txtbLayersNumber.Text.Length != 0)
				{
					ParseInt(txtbLayersNumber.Text, label1.Content.ToString(), out this.layersN);
					if (Posit(this.layersN) && this.layersN <= 20)
						allValid = true;
				}
				else
				{
					msg += "Number of Layers is invalid\n";
					allValid = false;
				}
			}
			if (this.mode == 0)
            {
				if (txtModeN.Text.Length != 0)
				{
					ParseInt(txtModeN.Text, label2.Content.ToString(), out this.mode);
					if (Posit(this.mode) && this.mode <= 25)
						allValid = true;
				}
				else
				{
					msg += "Mode Number is invalid\n";
					allValid = false;
				}
            }
			if (FEN == 0)
			{
				if (txtbFEN.Text.Length != 0)
				{
					int test;
					if (int.TryParse(txtbFEN.Text, out test))
					{
						if (test >= 200 && test <= 1100)
							this.FEN = test;
						else
						{
							this.FEN = 200;
							MessageBox.Show("Number of finite elements might not be less than 200. Now it's automatically set as 200.", "Warning", MessageBoxButton.OK);
						}
					}
				}
				else
				{
					msg += "Number of finite elements is invalid\n";
					allValid = false;
				}
			}
			if (this.stepWNN == 0)
            {
				if (txtWNstepN.Text.Length != 0)
				{
					ParseInt(txtWNstepN.Text, label6.Content.ToString(), out this.stepWNN);
					if (Posit(this.stepWNN))
						allValid = true;
				}
				else
				{
					msg += "Number of Steps of WaveNumber is invalid\n";
					allValid = false;
				}
            }
			if (this.stepWNSize == 0.0)
            {
				if (txtWNstepN.Text.Length != 0)
				{
					ParseDouble(txtWNstepN.Text, label7.Content.ToString(), out this.stepWNSize);
					if (Posit(this.stepWNSize) && this.stepWNSize >= 0.000001)
						allValid = true;
				}
				else
				{
					msg += "Size of Steps of WaveNumber is invalid\n";
					allValid = false;
				}
            }
			if (!isDispersion)
			{
				if (this.stepRSize == 0)
				{
					if (txtRStep.Text.Length != 0)
					{
						ParseDouble(txtRStep.Text, label8.Content.ToString(), out this.stepRSize);
						if (Posit(this.stepRSize) && this.stepRSize >= 0.000001)
							allValid = true;
					}
					else
					{
						msg += "Size of Steps of Radius is invalid\n";
						allValid = false;
					}
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
		#region Parse
		private bool isR(string R)
		{
			string[] str = new string[layersN];
			char[] sep = {'|'};
			str = R.Split(sep);
			pRad = new double[layersN];
			double sumR = 0.0;
			for (int i = 0; i < layersN; i++)
			{
				double r = 0.0;
				ParseDouble(str[i], "Радиус слоя #" + Convert.ToString(i+1), out r);
				if (r > 0)
				{
					pRad[i] = r;
				}
				else return false;
			}
			for (int i = 0; i < layersN; i++)
			{
				sumR = pRad[layersN-1];
				Rad[i] = pRad[i] / sumR;
			}
			return true;
		}
		private bool isE(string E)
		{
			string[] str = new string[layersN];
			char[] sep = { '|' };
			str = E.Split(sep);
			for (int i = 0; i < layersN; i++)
			{
				double e = 0.0;
				ParseDouble(str[i], "Проницаемость слоя #" + Convert.ToString(i+1), out e);
				if (e != 0.0)
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
		private void ParseInt(string str, string objectName, out int output)
		{
			output = 0;
			int test;
			try
			{
				test = int.Parse(str, new System.Globalization.CultureInfo(System.Globalization.CultureInfo.InstalledUICulture.Name));
				if (Posit(test))
					output = test;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Невозможно чтение поля с названием " + "\"" + objectName + "\". Проверьте правильность ввода.");
			}
		}
		private void ParseDouble(string str, string objectName, out double output)
		{
			output = 0.0;
			double test;
			try
			{
				test = double.Parse(str, new System.Globalization.CultureInfo(System.Globalization.CultureInfo.InstalledUICulture.Name));
				if (Posit(test))
					output = test;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Невозможно чтение поля с названием " + "\"" + objectName + "\". Проверьте правильность ввода.");
			}
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
					if (this._res != null) this._res.Close();
					if (stepRSize < pRad[layersN - 1])
					{
						pr = new Progress(this);
						pr._bg.DoWork += new DoWorkEventHandler(bw_DoWork);
						this.Hide();
						pr.Show();
						pr.Closed += new EventHandler(pr_Closed);
					}
					else MessageBox.Show("Вычисление критических значений невозможно: заданный шаг радиуса выходит за границы волновода", "Ошибка!");
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("Введите характеристики слоёв волновода", "Ошибка!", System.Windows.Forms.MessageBoxButtons.OK);
				}
            }
			
        }
		void pr_Closed(object sender, EventArgs e)
		{
			if (pr.isExit) this.Close();
		}
		#region BackGroundWorker functions
		void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			obj = new WorkObject();
			var bgw = sender as BackgroundWorker;
			if (this.isDispersion)
			{
				disp = new WorkObject.DISP[stepWNN];
				int iniprog = 0;
				pr.dt.calculatingtype = "Dispersion";
				pr.dt.Layers = Layers;
				pr.dt.mode = mode;
				pr.dt.stepWNN = stepWNN;
				pr.dt.stepWNS = stepWNSize;
				disp = obj.dispersion(FEN, stepWNN, stepWNSize, mode, Layers, ref bgw, ref iniprog, 1, false);
			}
			else
			{
				int N = Convert.ToInt32(1.0 / stepRSize);
				crit = new WorkObject.CRIT[2*N];
				if (isCritVal)
					pr.dt.calculatingtype = "Critical values";
				else
					pr.dt.calculatingtype = "Critical conditions";
				pr.dt.Layers = Layers;
				pr.dt.mode = mode;
				pr.dt.stepWNN = stepWNN;
				pr.dt.stepWNS = stepWNSize;
				pr.dt.stepRS = stepRSize;
				crit = obj.Crit(FEN, stepRSize / pRad[layersN - 1], stepWNN, stepWNSize, mode, Layers, ref bgw, !isCritVal);
			}
			pr._bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
		}
		void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			pr.SetTime("Процесс завершён");
			pr.Enable();
		}
		#endregion
		#region MenuItems
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
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
		#endregion
	}
}
