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

		struct DS
		{
			string[] x;
			double[] y;
		}
		WorkObject.CRIT[] _crit;
		WorkObject.DISP[] _disp;
		bool _isCond = false;

		System.Windows.Forms.ToolTip tooltip = new System.Windows.Forms.ToolTip();
		System.Drawing.Point clickPosition = new System.Drawing.Point();

		#region "Constructors"
		public Results()
		{
			InitializeComponent();
		}
		public Results(Progress pr, WorkObject.CRIT[] crit, bool isCond)
		{
			InitializeComponent();
			_pr = pr;
			_crit = crit;
			_isCond = isCond;

			if (isCond)
			{
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
			}
			else
			{
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
				}

			}
		}
		public Results(Progress pr, WorkObject.DISP[] disp)
		{
			InitializeComponent();
			_pr = pr;
			_disp = disp;

			chartRe.ChartAreas.Add(new ChartArea("Re"));
			chartIm.ChartAreas.Add(new ChartArea("Im"));

			chartRe.Series.Add(new Series("ser1"));
			chartRe.Series["ser1"].ChartArea = "Re";
			chartRe.Series["ser1"].ChartType = SeriesChartType.Line;

			chartIm.Series.Add(new Series("ser1"));
			chartIm.Series["ser1"].ChartArea = "Im";
			chartIm.Series["ser1"].ChartType = SeriesChartType.Line;
			chartIm.ChartAreas["Im"].AxisY.IsReversed = true;

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
		}
		#endregion
		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
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
			string activeDir = @"";
			string subPath = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + " " + _pr.characteristics;
			string newPath = System.IO.Path.Combine(activeDir, subPath);
			string newPathData = "";
			string newPathGraphRe = "";
			string newPathGraphIm = "";

			System.IO.Directory.CreateDirectory(newPath);
			string fileName = "characteristics.txt";

			newPathData = System.IO.Path.Combine(newPath, fileName);
			newPathGraphIm = System.IO.Path.Combine(newPath, "Im.png");
			newPathGraphRe = System.IO.Path.Combine(newPath, "Re.png");

			chartIm.SaveImage(newPathGraphIm, ChartImageFormat.Png);
			chartRe.SaveImage(newPathGraphRe, ChartImageFormat.Png);

			StreamWriter strwr = new StreamWriter(newPathData);

			if (_crit != null)
			{
				strwr.WriteLine("  R      k      y  ");
				for (int i = 0; i < _crit.Length; i++)
				{
					for (int j = 0; j < _crit[i].D.Length; j++)
					{
						string str = _crit[i].R.ToString() + "     " + _crit[i].D[j].k + "     " + _crit[i].D[j].y;
						strwr.WriteLine(str);
					}
				}
			}
			if (_disp != null)
			{
				strwr.WriteLine("  k      y  ");
				for (int i = 0; i < _disp.Length; i++)
				{
					string str = _disp[i].k.ToString() + "     " + _disp[i].y.ToString();
					strwr.WriteLine(str);
				}
			}
			strwr.Close();
		}
	}
}
