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
		public Results()
		{
			InitializeComponent();
		}

		public Results(Progress pr, WorkObject.CRIT[] crit, bool isCond)
		{
			InitializeComponent();
			_pr = pr;

			if (isCond)
			{
				chartRe.ChartAreas.Add(new ChartArea("Re"));
				chartIm.ChartAreas.Add(new ChartArea("Im"));

				chartIm.ChartAreas["Im"].AxisY.IsReversed = true;

				chartRe.Series.Add(new Series("ser1"));
				chartRe.Series["ser1"].ChartArea = "Re";
				chartRe.Series["ser1"].ChartType = SeriesChartType.Line;

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
			}
			else
			{
				//chartRe.ChartAreas.Add(new ChartArea("Re"));
				//chartIm.ChartAreas.Add(new ChartArea("Im"));

				//chartIm.ChartAreas["Im"].AxisY.IsReversed = true;

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

					//chartIm.Series[i].Label = crit[i].R.ToString();
					//chartRe.Series[i].Label = crit[i].R.ToString();
					chartIm.ChartAreas[i.ToString()].AxisX.Title = "R = " + crit[i].R.ToString();
					chartRe.ChartAreas[i.ToString()].AxisX.Title = "R = " + crit[i].R.ToString();

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
		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
