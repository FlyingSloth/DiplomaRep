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
using System.Globalization;
using System.ComponentModel;

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
		System.Drawing.Point? clickPosition = null;
		System.Windows.Forms.FolderBrowserDialog sfd = new System.Windows.Forms.FolderBrowserDialog();

		#region Constructors
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
				this.Title = "Критические условия";
				chartIm.ChartAreas.Add(new ChartArea("Im"));

				chartIm.ChartAreas["Im"].AxisY.IsReversed = true;
				chartIm.ChartAreas["Im"].AxisX.IsReversed = true;

				chartIm.Series.Add(new Series("ser1"));
				chartIm.Series["ser1"].ChartArea = "Im";
				chartIm.Series["ser1"].ChartType = SeriesChartType.Line;
				chartIm.Series.Add(new Series("ser2"));
				chartIm.Series["ser2"].ChartArea = "Im";
				chartIm.Series["ser2"].ChartType = SeriesChartType.Line;

				chartIm.ChartAreas["Im"].AxisX.Interval = 2;
				chartRe.ChartAreas["Re"].AxisX.Interval = 2;

				string[] axisX = new string[crit.Length];
				double[] axisYIm1 = new double[crit.Length];
				double[] axisYIm2 = new double[crit.Length];
				for (int i = 0; i < crit.Length; i++)
				{
					axisX[i] = crit[i].D[0].k.ToString();
					axisYIm1[i] = crit[i].D[0].y1.Im();
					axisYIm2[i] = crit[i].D[1].y1.Im();
				}
				chartIm.Series["ser1"].Points.DataBindXY(axisX, axisYIm1);
				chartIm.Series["ser2"].Points.DataBindXY(axisX, axisYIm2);

				chartIm.Series["ser1"].ToolTip = "k=#VALX, Im(y)=#VALY";
				chartIm.Series["ser2"].ToolTip = "k=#VALX, Im(y)=#VALY";
				for (int i = 0; i < crit.Length; i++)
				{
					chartIm.Series["ser1"].Points[i].Label = crit[i].R.ToString();
					chartIm.Series["ser2"].Points[i].Label = crit[i].R.ToString();
				}
				chartIm.ChartAreas["Im"].AxisY.Title = "Im(y) - постоянная распространения";
				chartIm.ChartAreas["Im"].AxisX.Title = "k - волновое число";
			}
			else
			{
				this.Title = "Критические значения";
				string[][] axisX = new string[crit.Length][];
				double[][] axisYIm = new double[crit.Length][];
				double[][] axisYRe = new double[crit.Length][];
				for (int i = 0; i < crit.Length; i++)
				{
					chartRe.ChartAreas.Add(new ChartArea(i.ToString()));
					chartIm.ChartAreas.Add(new ChartArea(i.ToString()));

					chartIm.ChartAreas[i.ToString()].AxisY.IsReversed = true;

					chartIm.ChartAreas[i.ToString()].AxisX.Interval = 2;
					chartRe.ChartAreas[i.ToString()].AxisX.Interval = 2;
					
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
						axisYIm[i][j] = crit[i].D[j].y1.Im();
						axisYRe[i][j] = crit[i].D[j].y1.Re();
					}
					chartIm.Series[i].Points.DataBindXY(axisX[i], axisYIm[i]);
					chartRe.Series[i].Points.DataBindXY(axisX[i], axisYRe[i]);
					chartIm.Series[i].ToolTip = "k=#VALX, Im(y)=#VALY";
					chartRe.Series[i].ToolTip = "k=#VALX, Re(y)=#VALY";
					chartIm.ChartAreas[i.ToString()].AxisY.Title = "Im(y) - постоянная распространения";
					chartRe.ChartAreas[i.ToString()].AxisY.Title = "Re(y) - постоянная распространения";
					chartIm.ChartAreas[i.ToString()].AxisX.Title = "k - волновое число, R = " + crit[i].R.ToString();
					chartRe.ChartAreas[i.ToString()].AxisX.Title = "k - волновое число, R = " + crit[i].R.ToString();
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

			this.Title = "Дисперсионные характеристики";
			chartRe.ChartAreas.Add(new ChartArea("Re"));
			chartIm.ChartAreas.Add(new ChartArea("Im"));

			chartRe.Series.Add(new Series("ser1"));
			chartRe.Series["ser1"].ChartArea = "Re";
			chartRe.Series["ser1"].ChartType = SeriesChartType.Line;

			chartIm.Series.Add(new Series("ser1"));
			chartIm.Series["ser1"].ChartArea = "Im";
			chartIm.Series["ser1"].ChartType = SeriesChartType.Line;
			chartIm.ChartAreas["Im"].AxisY.IsReversed = true;

            chartRe.Series.Add(new Series("ser2"));
            chartRe.Series["ser2"].ChartArea = "Re";
            chartRe.Series["ser2"].ChartType = SeriesChartType.Line;

            chartIm.Series.Add(new Series("ser2"));
            chartIm.Series["ser2"].ChartArea = "Im";
            chartIm.Series["ser2"].ChartType = SeriesChartType.Line;
            chartIm.ChartAreas["Im"].AxisY.IsReversed = true;

			chartRe.Series.Add(new Series("0"));
			chartRe.Series[0].ChartArea = "Re";
			chartRe.Series[0].ChartType = SeriesChartType.Line;
			chartIm.Series.Add(new Series("0"));
			chartIm.Series[0].ChartArea = "Im";
			chartIm.Series[0].ChartType = SeriesChartType.Line;
			
			chartIm.ChartAreas["Im"].AxisX.Interval = 2;
			chartRe.ChartAreas["Re"].AxisX.Interval = 2;
			
			string[] axisX = new string[disp.Length];
			double[] axisY1Im = new double[disp.Length];
			double[] axisY1Re = new double[disp.Length];
            double[] axisY2Im = new double[disp.Length];
            double[] axisY2Re = new double[disp.Length];

			for (int i = 0; i < disp.Length; i++)
			{
				axisX[i] = disp[i].k.ToString();
				axisY1Im[i] = disp[i].y1.Im();
				axisY1Re[i] = disp[i].y1.Re();
                axisY2Im[i] = disp[i].y2.Im();
                axisY2Re[i] = disp[i].y2.Re();
			}

			chartIm.Series["ser1"].Points.DataBindXY(axisX, axisY1Im);
			chartRe.Series["ser1"].Points.DataBindXY(axisX, axisY1Re);
            chartIm.Series["ser2"].Points.DataBindXY(axisX, axisY2Im);
            chartRe.Series["ser2"].Points.DataBindXY(axisX, axisY2Re);

			chartIm.Series["ser1"].ToolTip = "k=#VALX, Im(y)=#VALY";
			chartRe.Series["ser1"].ToolTip = "k=#VALX, Re(y)=#VALY";
            chartIm.Series["ser2"].ToolTip = "k=#VALX, Im(y)=#VALY";
            chartRe.Series["ser2"].ToolTip = "k=#VALX, Re(y)=#VALY";

			chartIm.ChartAreas["Im"].AxisY.Title = "Im(y) - постоянная распространения";
			chartIm.ChartAreas["Im"].AxisX.Title = "k - волновое число";
			chartRe.ChartAreas["Re"].AxisY.Title = "Re(y) - постоянная распространения";
			chartRe.ChartAreas["Re"].AxisX.Title = "k - волновое число";
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
		private void btnSaveRes_Click(object sender, RoutedEventArgs e)
		{
			/*
            string activeDir = "";
			if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				activeDir = sfd.SelectedPath;
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

				StreamWriter strwr = new StreamWriter(newPathData, false, Encoding.Unicode);

				strwr.WriteLine(_pr.dt.calculatingtype + ",Номер моды," + _pr.dt.mode);
				strwr.WriteLine("Начальные характеристики волновода");
				string R = "";
				string E = "";
				R = "Радиус,";
				E = "Проницаемость,";
				for (int i = 0; i < _pr.dt.Layers.Length; i++)
				{
					R += _pr.dt.Layers[i].R.ToString() + ",";
					E += _pr.dt.Layers[i].perm.ToString() + ",";
				}
				strwr.WriteLine(Convert.ToString(R,new CultureInfo("ru-Ru")));
				strwr.WriteLine(E);
				strwr.WriteLine("Изменения волновода:,Число шагов волнового числа k," + _pr.dt.stepWNN + ",Шаг k," + _pr.dt.stepWNS);
				if (_crit != null)
				{
					strwr.WriteLine("Шаг изменения радиуса," + _pr.dt.stepRS);
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
             * */
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
