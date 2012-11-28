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
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace FEA
{
	/// <summary>
	/// Interaction logic for Results.xaml
	/// </summary>
	public partial class Results : Window
	{
		Progress _pr;
		WorkObject.CRIT[] _crit;
		WorkObject.DISP[] _disp;
		bool _isCond = false;

		System.Windows.Forms.ToolTip tooltip = new System.Windows.Forms.ToolTip();
		System.Drawing.Point clickPosition = new System.Drawing.Point();
		System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();

		#region "Constructors"
		public Results()
		{
			InitializeComponent();
		}
		public Results(Progress pr, WorkObject.CRIT[] crit, bool isCond)
		{
			InitializeComponent();
			_pr = pr;
			_pr._f1.Show();
			_pr._f1._res = this;
			_pr._f1.Closing += new System.ComponentModel.CancelEventHandler(_f1_Closing);
			_crit = crit;
			_isCond = isCond;

			if (isCond)
			{
				this.Title = "Critical conditions";
				chartIm.ChartAreas.Add(new ChartArea("Im"));

				chartIm.ChartAreas["Im"].AxisY.IsReversed = true;
				chartIm.ChartAreas["Im"].AxisX.IsReversed = true;

				chartIm.Series.Add(new Series("ser1"));
				chartIm.Series["ser1"].ChartArea = "Im";
				chartIm.Series["ser1"].ChartType = SeriesChartType.Line;
				chartIm.Series.Add(new Series("ser2"));
				chartIm.Series["ser2"].ChartArea = "Im";
				chartIm.Series["ser2"].ChartType = SeriesChartType.Line;
				string[] axisX = new string[crit.Length];
				double[] axisYIm1 = new double[crit.Length];
				double[] axisYIm2 = new double[crit.Length];
				for (int i = 0; i < crit.Length; i++)
				{
					axisX[i] = crit[i].D[0].k.ToString();
					axisYIm1[i] = crit[i].D[0].y.Im();
					axisYIm2[i] = crit[i].D[1].y.Im();
				}
				chartIm.Series["ser1"].Points.DataBindXY(axisX, axisYIm1);
				chartIm.Series["ser2"].Points.DataBindXY(axisX, axisYIm2);
				for (int i = 0; i < crit.Length; i++)
				{
					chartIm.Series["ser1"].Points[i].Label = crit[i].R.ToString();
					chartIm.Series["ser2"].Points[i].Label = crit[i].R.ToString();
				}
				chartIm.ChartAreas["Im"].AxisY.Title = "Im(y) - propagation constant";
				chartIm.ChartAreas["Im"].AxisX.Title = "k - wavenumber";
			}
			else
			{
				this.Title = "Critical values";
				string[][] axisX = new string[crit.Length][];
				double[][] axisYIm = new double[crit.Length][];
				double[][] axisYRe = new double[crit.Length][];
				for (int i = 0; i < crit.Length; i++)
				{
					chartRe.ChartAreas.Add(new ChartArea(i.ToString()));
					chartIm.ChartAreas.Add(new ChartArea(i.ToString()));

					chartIm.ChartAreas[i.ToString()].AxisY.IsReversed = true;
					
					axisX[i] = new string[crit[i].D.Length];
					axisYIm[i] = new double[crit[i].D.Length];
					axisYRe[i] = new double[crit[i].D.Length];

					chartIm.Series.Add(new Series(i.ToString()));
					chartIm.Series[i].ChartArea = i.ToString();
					chartIm.Series[i].ChartType = SeriesChartType.Line;
					chartRe.Series.Add(new Series(i.ToString()));
					chartRe.Series[i].ChartArea = i.ToString();
					chartRe.Series[i].ChartType = SeriesChartType.Line;

					chartIm.Series[i].AxisLabel = crit[i].R.ToString();
					chartRe.Series[i].AxisLabel = crit[i].R.ToString();
					
					for (int j = 0; j < crit[i].D.Length; j++)
					{
						axisX[i][j]= crit[i].D[j].k.ToString();
						axisYIm[i][j] = crit[i].D[j].y.Im();
						axisYRe[i][j] = crit[i].D[j].y.Re();
					}
					chartIm.Series[i].Points.DataBindXY(axisX[i], axisYIm[i]);
					chartRe.Series[i].Points.DataBindXY(axisX[i], axisYRe[i]);
					chartIm.ChartAreas[i.ToString()].AxisY.Title = "Im(y)";
					chartRe.ChartAreas[i.ToString()].AxisY.Title = "Re(y)";
					chartIm.ChartAreas[i.ToString()].AxisX.Title = "k - wavenumber, R = " + crit[i].R.ToString();
					chartRe.ChartAreas[i.ToString()].AxisX.Title = "k - wavenumber, R = " + crit[i].R.ToString();
				}

			}
		}
		public Results(Progress pr, WorkObject.DISP[] disp)
		{
			InitializeComponent();
			_pr = pr;
			_pr._f1.Show();
			_pr._f1._res = this;
			_pr._f1.Closing += new System.ComponentModel.CancelEventHandler(_f1_Closing);
			_disp = disp;

			this.Title = "Dispersion characteristics";
			chartRe.ChartAreas.Add(new ChartArea("Re"));
			chartIm.ChartAreas.Add(new ChartArea("Im"));

			chartRe.Series.Add(new Series("ser1"));
			chartRe.Series["ser1"].ChartArea = "Re";
			chartRe.Series["ser1"].ChartType = SeriesChartType.Line;

			chartIm.Series.Add(new Series("ser1"));
			chartIm.Series["ser1"].ChartArea = "Im";
			chartIm.Series["ser1"].ChartType = SeriesChartType.Line;
			chartIm.ChartAreas["Im"].AxisY.IsReversed = true;


			chartRe.Series.Add(new Series("0"));
			chartRe.Series[0].ChartArea = "Re";
			chartRe.Series[0].ChartType = SeriesChartType.Line;
			chartIm.Series.Add(new Series("0"));
			chartIm.Series[0].ChartArea = "Im";
			chartIm.Series[0].ChartType = SeriesChartType.Line;


			string[] axisX1 = new string[2];
			double[] axisY1 = new double[2];
			axisX1[0] = "-1";
			axisX1[1] = "0";
			axisY1[0] = 0;
			axisY1[1] = 0;

			chartIm.Series[0].Points.DataBindXY(axisX1, axisY1);
			chartRe.Series[0].Points.DataBindXY(axisX1, axisY1);

			string[] axisX = new string[disp.Length];
			double[] axisYIm = new double[disp.Length];
			double[] axisYRe = new double[disp.Length];

			for (int i = 0; i < disp.Length; i++)
			{
				axisX[i] = disp[i].k.ToString();
				axisYIm[i] = disp[i].y.Im();
				axisYRe[i] = disp[i].y.Re();
			}

			chartIm.Series["ser1"].Points.DataBindXY(axisX, axisYIm);
			chartRe.Series["ser1"].Points.DataBindXY(axisX, axisYRe);
			chartIm.ChartAreas["Im"].AxisY.Title = "Im(y) - propagation constant";
			chartIm.ChartAreas["Im"].AxisX.Title = "k - wavenumber";
			chartRe.ChartAreas["Re"].AxisY.Title = "Im(y) - propagation constant";
			chartRe.ChartAreas["Re"].AxisX.Title = "k - wavenumber";
		}
		void _f1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_pr.isExit = false;
			this.Close();
		}
		#endregion
		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			_pr.isExit = true;
			this.Close();
		}
		#region "Coordinates Hint"
		private void chartRe_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!clickPosition.IsEmpty && e.Location != clickPosition)
			{
				tooltip.RemoveAll();
				clickPosition = new System.Drawing.Point();
			}
		}

		private void chartRe_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			var pos = e.Location;
			clickPosition = pos;
			var res = chartRe.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);

			foreach (var result in res)
			{
				if (result.ChartElementType == ChartElementType.PlottingArea)
				{
					var xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
					var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

					tooltip.Show("k = " + xVal + ", Re(y) = " + yVal, this.chartRe, e.Location.X, e.Location.Y - 10);
				}
			}
		}

		private void chartIm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!clickPosition.IsEmpty && e.Location != clickPosition)
			{
				tooltip.RemoveAll();
				clickPosition = new System.Drawing.Point();
			}
		}

		private void chartIm_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			var pos = e.Location;
			clickPosition = pos;
			var res = chartIm.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);

			foreach (var result in res)
			{
				if (result.ChartElementType == ChartElementType.PlottingArea)
				{
					var xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
					var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

					tooltip.Show("k = " + xVal + ", Re(y) = " + yVal, this.chartIm, e.Location.X, e.Location.Y - 10);
				}
			}
		}
		#endregion
		private void btnSaveRes_Click(object sender, RoutedEventArgs e)
		{
			sfd.ShowDialog();
			string activeDir = "";
			if (sfd.FileName.Length != 0) activeDir = sfd.FileName;
			else
			{
				activeDir = @"";
				MessageBox.Show("Characteristics are saved to program folder", "Warning");
			}
			string subPath = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();
			string newPath = System.IO.Path.Combine(activeDir, subPath);
			string newPathData = "";
			string newPathGraphRe = "";
			string newPathGraphIm = "";

			System.IO.Directory.CreateDirectory(newPath);
			string fileName = "characteristics.csv";

			newPathData = System.IO.Path.Combine(newPath, fileName);
			newPathGraphIm = System.IO.Path.Combine(newPath, "Im.png");
			newPathGraphRe = System.IO.Path.Combine(newPath, "Re.png");

			chartIm.SaveImage(newPathGraphIm, ChartImageFormat.Png);
			chartRe.SaveImage(newPathGraphRe, ChartImageFormat.Png);

			StreamWriter strwr = new StreamWriter(newPathData);

			strwr.WriteLine(_pr.dt.calculatingtype + ",Mode number," + _pr.dt.mode);
			strwr.WriteLine("Initial layer's characteristics");
			string R = "";
			string E = "";
			R = "Radius,";
			E = "Permittivity,";
			for (int i = 0; i < _pr.dt.Layers.Length; i++)
			{
				R += _pr.dt.Layers[i].R.ToString() + ",";
				E += _pr.dt.Layers[i].perm.ToString() + ",";
			}
			strwr.WriteLine(R);
			strwr.WriteLine(E);
			strwr.WriteLine("Wavenumber changes:,Number os steps,"+_pr.dt.stepWNN+",Size of step," + _pr.dt.stepWNS);
			if (_crit != null)
			{
				strwr.WriteLine("Step of changes of radius," + _pr.dt.stepRS);
				strwr.WriteLine("R,k,y");
				for (int i = 0; i < _crit.Length; i++)
				{
					for (int j = 0; j < _crit[i].D.Length; j++)
					{
						string str = _crit[i].R.ToString() + "," + _crit[i].D[j].k + "," + _crit[i].D[j].y.ToString();
						strwr.WriteLine(str);
					}
				}
			}
			if (_disp != null)
			{
				strwr.WriteLine("k,y");
				for (int i = 0; i < _disp.Length; i++)
				{
					string str = _disp[i].k.ToString() + "," + _disp[i].y.ToString();
					strwr.WriteLine(str);
				}
			}
			strwr.Close();
		}
		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			_pr.isExit = false;
			this.Close();
		}
		private void Window_Closed(object sender, EventArgs e)
		{
			if (!_pr.isExit)
				_pr.isExit = false;
		}
	}
}
