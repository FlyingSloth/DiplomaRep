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
using System.ComponentModel;
using FEA;

namespace FEA
{
	/// <summary>
	/// Interaction logic for Progress.xaml
	/// </summary>
	public partial class Progress : Window
	{
		#region "Global params"
		private MainWindow _f1;
		public BackgroundWorker _bg = new BackgroundWorker();
		public bool isExit = false;
		//public bool isPaused = false;
		public string characteristics = "";
		#endregion
		#region "Constructors"
		public Progress()
		{
			InitializeComponent();
		}
		public Progress(MainWindow f1)
		{
			InitializeComponent();
			_f1 = f1;
			_bg.ProgressChanged += new ProgressChangedEventHandler(_bg_ProgressChanged);
			_bg.WorkerSupportsCancellation = true;
			_bg.WorkerReportsProgress = true;
		}
		#endregion
		void _bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progrCalculation.Value = (int)(progrCalculation.Maximum * e.ProgressPercentage/100);
			int time = Convert.ToInt32(e.UserState);
			if (time <= 0) lblTime.Content = "Process is finilazing";
			if (time < 60 && time > 0) lblTime.Content = time + " sec";
			if (time > 60)
			{
				time = (int)(Convert.ToDouble(time) / 60 + 1.0/3);
				lblTime.Content = "Over " +  time + " min";
			}
		}
		void res_Closed(object sender, EventArgs e)
		{
			isExit = true;
			this.Close();
		}
		private void btnAbort_Click(object sender, RoutedEventArgs e)
		{
			if (_bg.IsBusy)
				_bg.CancelAsync();
			_f1.Show();
			this.Close();
		}
		private void btnShowRes_Click(object sender, RoutedEventArgs e)
		{
			Results res;
			if (!_f1.isDispersion)
				res = new Results(this, _f1.crit, !_f1.isCritVal);
			else
				res = new Results(this, _f1.disp);
			this.Hide();
			res.Show();
			res.Closed += new EventHandler(res_Closed);
		}
		public void SetTime(string val)
		{
			lblTime.Content = val;
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_bg.RunWorkerAsync();
		}
		private void Window_Closed(object sender, EventArgs e)
		{
			if (_bg.IsBusy)
				_bg.CancelAsync();
			_f1.Show();
			this.Close();
		}
		/*
		private void btnPause_Click(object sender, RoutedEventArgs e)
		{
			if (!isPaused)
			{
				isPaused = true;
				btnPause.Content = "Resume";
			}
			else
			{
				isPaused = false;
				btnPause.Content = "Pause";
			}
		}
		 */
	}
}
