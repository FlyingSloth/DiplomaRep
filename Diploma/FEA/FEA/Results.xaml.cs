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
        WorkObject.CRIT[] bufcrit;
        WorkObject.DISP[] bufdisp;

		System.Windows.Forms.ToolTip tooltip = new System.Windows.Forms.ToolTip();
		System.Drawing.Point? clickPosition = null;
		System.Windows.Forms.FolderBrowserDialog sfd = new System.Windows.Forms.FolderBrowserDialog();

		#region Constructors
		public Results()
		{
			InitializeComponent();
		}
		public Results(Progress pr, WorkObject.CRIT[] crit, bool isQuad)
		{
			
            InitializeComponent();
			_pr = pr;
			_pr._f1.Show();
			_pr._f1._res = this;
			_pr._f1.Closing += new System.ComponentModel.CancelEventHandler(_f1_Closing);
			_crit = crit;
			
			this.Title = "Критические условия. ";
            if (isQuad) this.Title += "Квадратичные оси";
			chartRe.ChartAreas.Add(new ChartArea("Im"));

			chartRe.ChartAreas["Im"].AxisY.IsReversed = true;
			chartRe.ChartAreas["Im"].AxisX.IsReversed = true;

			chartRe.Series.Add(new Series("ser1"));
			chartRe.Series["ser1"].ChartArea = "Im";
			chartRe.Series["ser1"].ChartType = SeriesChartType.Line;
			chartRe.Series.Add(new Series("ser2"));
			chartRe.Series["ser2"].ChartArea = "Im";
			chartRe.Series["ser2"].ChartType = SeriesChartType.Line;

			chartRe.ChartAreas["Im"].AxisX.Interval = 2;
				
			string[] axisX = new string[crit.Length];
			double[] axisYIm1 = new double[crit.Length];
			double[] axisYIm2 = new double[crit.Length];

            
            bufcrit = new WorkObject.CRIT[crit.Length];
            for (int i = 0; i < crit.Length; i++)
            {
                bufcrit[i].D = new WorkObject.DISP[1];
            }

            for (int i = 0; i < crit.Length; i++)
            {
                bufcrit[i].R = crit[i].R;
                bufcrit[i].D[0].k = crit[i].D[0].k;
                if (isQuad)
                {
                    bufcrit[i].D[0].k = crit[i].D[0].k * crit[i].D[0].k;
                    bufcrit[i].D[0].y1 = crit[i].D[0].y1.Pow(2);
                    bufcrit[i].D[0].y2 = crit[i].D[0].y2.Pow(2);
                }
                else
                {
                    bufcrit[i].D[0].y1 = crit[i].D[0].y1;
                    bufcrit[i].D[0].y2 = crit[i].D[0].y2;
                }
            }
            
			for (int i = 0; i < crit.Length; i++)
			{
				axisX[i] = crit[i].D[0].k.ToString();
				axisYIm1[i] = bufcrit[i].D[0].y1.Im();
				axisYIm2[i] = bufcrit[i].D[0].y2.Im();
			}
			chartRe.Series["ser1"].Points.DataBindXY(axisX, axisYIm1);
			chartRe.Series["ser2"].Points.DataBindXY(axisX, axisYIm2);

			chartRe.Series["ser1"].ToolTip = "k=#VALX, Im(y)=#VALY";
			chartRe.Series["ser2"].ToolTip = "k=#VALX, Im(y)=#VALY";
			for (int i = 0; i < crit.Length; i++)
			{
				chartRe.Series["ser1"].Points[i].Label = bufcrit[i].R.ToString();
				chartRe.Series["ser2"].Points[i].Label = bufcrit[i].R.ToString();
			}
			chartRe.ChartAreas["Im"].AxisX.Title = "k - волновое число";
            chartRe.ChartAreas["Im"].AxisY.Title = "Im(y) - постоянная распространения";
		}

        public Results(Progress pr, WorkObject.DISP[] disp, bool isQuad)
		{
			InitializeComponent();
			_pr = pr;
			_pr._f1.Show();
			_pr._f1._res = this;
			_pr._f1.Closing += new System.ComponentModel.CancelEventHandler(_f1_Closing);
			_disp = disp;

			this.Title = "Дисперсионные характеристики. ";
            if (isQuad) this.Title += "Квадратичные оси";
			chartRe.ChartAreas.Add(new ChartArea("Re"));
			chartRe.ChartAreas.Add(new ChartArea("Im"));

			chartRe.Series.Add(new Series("ser1"));
			chartRe.Series["ser1"].ChartArea = "Re";
			chartRe.Series["ser1"].ChartType = SeriesChartType.Line;
            chartRe.Series.Add(new Series("ser11"));
            chartRe.Series["ser11"].ChartArea = "Re";
            chartRe.Series["ser11"].ChartType = SeriesChartType.Line;
            
            chartRe.Series.Add(new Series("ser2"));
            chartRe.Series["ser2"].ChartArea = "Im";
            chartRe.Series["ser2"].ChartType = SeriesChartType.Line;
            chartRe.ChartAreas["Im"].AxisY.IsReversed = true;
            chartRe.Series.Add(new Series("ser21"));
            chartRe.Series["ser21"].ChartArea = "Im";
            chartRe.Series["ser21"].ChartType = SeriesChartType.Line;
            
			chartRe.ChartAreas["Im"].AxisX.Interval = 2;
			chartRe.ChartAreas["Re"].AxisX.Interval = 2;
			
			string[] axisX = new string[disp.Length];
			double[] axisY1Im = new double[disp.Length];
			double[] axisY1Re = new double[disp.Length];
            double[] axisY2Im = new double[disp.Length];
            double[] axisY2Re = new double[disp.Length];

            bufdisp = new WorkObject.DISP[disp.Length];
            for (int i = 0; i < disp.Length; i++)
            {
                
                if (isQuad)
                {
                    bufdisp[i].k = disp[i].k * disp[i].k;
                    bufdisp[i].y1 = disp[i].y1.Pow(2);
                    bufdisp[i].y2 = disp[i].y2.Pow(2);
                }
                else
                {
                    bufdisp[i].k = disp[i].k;
                    bufdisp[i].y1 = disp[i].y1;
                    bufdisp[i].y2 = disp[i].y2;
                }
            }

			for (int i = 0; i < disp.Length; i++)
			{
				axisX[i] = bufdisp[i].k.ToString();
                axisY1Im[i] = bufdisp[i].y1.Im();
                axisY1Re[i] = bufdisp[i].y1.Re();
                axisY2Im[i] = bufdisp[i].y2.Im();
                axisY2Re[i] = bufdisp[i].y2.Re();
			}

			chartRe.Series["ser1"].Points.DataBindXY(axisX, axisY1Re);
            chartRe.Series["ser2"].Points.DataBindXY(axisX, axisY1Im);
            chartRe.Series["ser11"].Points.DataBindXY(axisX, axisY2Re);
			chartRe.Series["ser21"].Points.DataBindXY(axisX, axisY2Im);
            

			
			chartRe.Series["ser1"].ToolTip = "k=#VALX, Re(y)=#VALY";
            chartRe.Series["ser2"].ToolTip = "k=#VALX, Im(y)=#VALY";
            chartRe.Series["ser11"].ToolTip = "k=#VALX, Re(y)=#VALY";
            chartRe.Series["ser21"].ToolTip = "k=#VALX, Im(y)=#VALY";

			chartRe.ChartAreas["Im"].AxisY.Title = "Im(y) - постоянная распространения";
			chartRe.ChartAreas["Im"].AxisX.Title = "k - волновое число";
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
            string delim = "\t";
            string activeDir = "";
			if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				activeDir = sfd.SelectedPath;
				string subPath = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();
				string newPath = System.IO.Path.Combine(activeDir, subPath);
				string newPathData = "";
				string newPathGraphRe = "";

				System.IO.Directory.CreateDirectory(newPath);
				string fileName = "characteristics.tsv";

				newPathData = System.IO.Path.Combine(newPath, fileName);
				newPathGraphRe = System.IO.Path.Combine(newPath, _pr.dt.calculatingtype+".png");

				chartRe.SaveImage(newPathGraphRe, ChartImageFormat.Png);

				StreamWriter strwr = new StreamWriter(newPathData, false, Encoding.Unicode);

                strwr.WriteLine(_pr.dt.calculatingtype + delim + "Номер моды:" + delim + _pr.dt.mode + delim + "Номера кривых:" + delim + _pr.dt.curves[0] + delim + _pr.dt.curves[1]);
				strwr.WriteLine("Начальные характеристики волновода");
				string R = "";
				string E = "";
				R = "Радиус:"+ delim;
				E = "Проницаемость:"+ delim;
				for (int i = 0; i < _pr.dt.Layers.Length; i++)
				{
					R += _pr.dt.Layers[i].R.ToString() + delim;
					E += _pr.dt.Layers[i].perm.ToString()+ delim;
				}
				strwr.WriteLine(Convert.ToString(R,new CultureInfo("ru-Ru")));
                strwr.WriteLine(Convert.ToString(E, new CultureInfo("ru-Ru")));
                strwr.WriteLine("Изменения волновода:");
                strwr.WriteLine("Число шагов волнового числа k:" + delim + _pr.dt.stepWNN + delim + "Шаг k:" + delim +_pr.dt.stepWNS);
				if (_crit != null)
				{
                    strwr.WriteLine("Шаг изменения радиуса" + delim +_pr.dt.stepRS);
					strwr.WriteLine("R" + delim +"k"+ delim + "y");
					for (int i = 0; i < bufcrit.Length; i++)
					{
						for (int j = 0; j < bufcrit[i].D.Length; j++)
						{
                            string str = bufcrit[i].R.ToString() + delim + bufcrit[i].D[j].k + delim + bufcrit[i].D[j].y1.ToString();
							strwr.WriteLine(str);
						}
					}
				}
				if (_disp != null)
				{
                    strwr.WriteLine("k" + delim + "y1" + delim + "y2");
					for (int i = 0; i < bufdisp.Length; i++)
					{
                        string str = bufdisp[i].k.ToString() + delim + bufdisp[i].y1.ToString() + delim + bufdisp[i].y2.ToString();
						strwr.WriteLine(str);
					}
				}
				strwr.Close();
			}
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
