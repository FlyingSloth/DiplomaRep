﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace FEA
{
    public partial class frmIniData : Form
    {
        public frmIniData()
        {
            InitializeComponent();
        }
		private int n;
		/*
		public void FF()
		{
			n = 200;
			int N = 100;

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			Matrix A = new Matrix();
			A.SetA(n, 1, 10, 1, 0.43);
			Matrix B = new Matrix();
			B.SetB(n, 1, 10, 1, 0.43);
			Matrix C = new Matrix();
			Complex[] E1 = new Complex[21];
			C.Eigen(B.Invert() * A, out E1);
			sw.Stop();
			TimeSpan ts;
			ts = sw.Elapsed;

			for (int i = 0; i < 21; i++)
			{
				string str;
				str = E1[i].ToString() + "\n";
				richTextBox1.Text += str;
				//dataGridView1.Rows.Add(str);
			}
			this.textBox1.Text = ts.ToString();
		}
		public void SF()
		{
			n = 200;
			int N = 100;
			System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
			sw1.Start();

			Matrix A1 = new Matrix();
			A1.SetA(n, 1, 10, 1, 0.43);
			Matrix B1 = new Matrix();
			B1.SetB(n, 1, 10, 1, 0.43);
			Matrix C1 = new Matrix();
			Complex[] E2 = new Complex[21];
			E2 = A1.eige(B1);
			sw1.Stop();
			TimeSpan ts1;
			ts1 = sw1.Elapsed;

			dataGridView2.RowCount = 1;
			for (int i = 0; i < 21; i++)
			{
				string str;
				str = E2[i].ToString() + "\n";
				//richTextBox2.Text += str;
				dataGridView2.Rows.Add(str);
			}
			this.textBox2.Text = ts1.ToString();
		}
		*/
		private void button1_Click(object sender, EventArgs e)
		{
			//dataGridView1.RowCount = 1;
			//dataGridView2.RowCount = 1;
			//Thread ft = new Thread(new ThreadStart(FF));
			//Thread st = new Thread(new ThreadStart(SF));
			//ft.Start();
			//st.Start();
			//ft.Join();
			//st.Join();


			
			n = 200;
            int N = 100;
			
			System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
			sw1.Start();
			
            WorkObject obj = new WorkObject();
            obj.dispchar = obj.dispersion(n, N, 0.01, 1, 10, 0.7);
			/*
			Matrix A1 = new Matrix();
			A1.SetA(n, 1, 10, 1, 0.43);
			Matrix B1 = new Matrix();
			B1.SetB(n, 1, 10, 1, 0.43);
			Matrix C1 = new Matrix();
			Complex[] E2 = new Complex[21];
			E2 = A1.eige(B1);*/
			


			dataGridView2.ColumnCount = 2;
			dataGridView2.Columns[0].Name = "k";
			dataGridView2.Columns[0].Name = "y";

			for (int i = 0; i < N; i++)
			//Parallel.For(0, N, (i, loopState) =>
			{
				string[] str = new string[2];
				str[0] = obj.dispchar[i].k.ToString();
				str[1] = obj.dispchar[i].y.ToString();
				dataGridView2.Rows.Add(str);
			}
			sw1.Stop();
			TimeSpan ts1;
			ts1 = sw1.Elapsed;
			this.textBox2.Text = ts1.ToString();

			#region "Comments"
			/*
            dataGridView2.RowCount = 1;

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			
            //WorkObject obj = new WorkObject();
            //obj.dispchar = obj.dispersion(n, N, 0.01, 1, 10, 0.7);
			Matrix A = new Matrix();
			A.SetA(n, 1, 10, 1, 0.43);
			Matrix B = new Matrix();
			B.SetB(n, 1, 10, 1, 0.43);
			Matrix C = new Matrix();
			Complex[] E1 = new Complex[21];
			C.Eigen(B.Invert() * A, out E1);
			sw.Stop();
			TimeSpan ts;
			ts = sw.Elapsed;
			

			dataGridView1.RowCount = 1;
			for (int i = 0; i < 21; i++)
			{
				//string[] str = new string[3 * n - 2];
				string str;
				//for (int j = 0; j < A.Cols(); j++)
					str = E1[i].ToString();
				dataGridView1.Rows.Add(str);
			}
			this.textBox1.Text = ts.ToString();
			*/
			/*
            dataGridView2.ColumnCount = 2;
            dataGridView2.Columns[0].Name = "k";
            dataGridView2.Columns[0].Name = "y";

            for (int i = 0; i < N; i++)
            {
                string[] str = new string[2];
                str[0] = obj.dispchar[i].k.ToString();
                str[1] = obj.dispchar[i].y.ToString();
                dataGridView2.Rows.Add(str);
            }
			*/
			//if (n < 200) n = 200;
			//Matrix A = new Matrix(n);
			//A.SetA(n, 1, 10, 1, 0.43);

			/*
            dataGridView1.ColumnCount = 3*n-2;
            
            for (int cou = 0; cou < A.Rows(); cou++)
            {
                dataGridView1.Columns[cou].Name = Convert.ToString(cou);
            }

            //Parallel.For(0, A.Rows(), delegate(int i)
            for (int i = 0; i < A.Rows(); i++ )
            {
                string[] str = new string[3 * n - 2];
                for (int j = 0; j < A.Cols(); j++)
                    str[j] = Convert.ToString(Math.Round(A.matrix[i, j], 4));
                dataGridView1.Rows.Add(str);
            }
            */
			//Matrix B = new Matrix(n);
			//B.SetB(n, 1, 10, 1, 0.43);
			/*
			Matrix AC = new Matrix(n);
			AC.Copy(A);
			Matrix AC1 = new Matrix(n);
			AC1 = B.Invert() * A;
			Matrix AC2 = new Matrix(n);
			AC2.Copy(A);
			*/

			//Complex[] E = new Complex[21];
			
			//Matrix trid = new Matrix(3*n-2,1);

			//A.SetEigenvalues(A.eige(B));
			/*
			E = A.eige(B);

			dataGridView2.ColumnCount = 1;
			for (int i = 0; i < 1; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < 21; i++)
			{
				string str;
				str = E[i].ToString();
				dataGridView2.Rows.Add(str);
			}
			*/
			/*
			AC2 = AC2.TriDiagonal(ref AC1, ref E);

			dataGridView2.ColumnCount = 3 * n - 2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < AC2.Rows(); i++)
			{
				string[] str = new string[AC1.Cols()];
				for (int j = 0; j < AC2.Cols(); j++)
					str[j] += AC2.matrix[i, j].ToString();
				dataGridView2.Rows.Add(str);
			}

			for (int i = 0; i < AC1.Rows(); i++)
			{
				trid.matrix[i, 0] = AC1.matrix[i, i];
			}

			Matrix resmult = new Matrix(3*n-2,3*n-2);
			resmult = resmult.tqli(ref AC1, ref trid, E);//TriDiagonal(ref AC1, ref E);

			dataGridView3.ColumnCount = 3 * n - 2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView3.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < resmult.Rows(); i++)
			{
				string[] str = new string[resmult.Cols()];
				for (int j = 0; j < resmult.Cols(); j++)
					str[j] += resmult.matrix[i, j].ToString();
				dataGridView3.Rows.Add(str);
			}
			*/
			/*
			Matrix resmult = new Matrix(n);

			Matrix B = new Matrix(n);
			B.SetB(n, 1, 10, 1, 0.43);

			resmult = B.Invert() * A;
			dataGridView2.ColumnCount = 3 * n - 2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < resmult.Rows(); i++)
			{
				string[] str = new string[3 * n - 2];
				for (int j = 0; j < resmult.Cols(); j++)
					str[j] = Convert.ToString(Math.Round(resmult.matrix[i, j], 4));
				dataGridView2.Rows.Add(str);
			}
			*/

			/*
			Matrix test1 = new Matrix(n);
			test1.setTest1(n);
			Matrix test2 = new Matrix(n);
			test2.setTest2(n);
			Matrix resmult = new Matrix(n);

			resmult = test1 * test2;
			{
				dataGridView1.ColumnCount = n;
				for (int i = 0; i < n; i++)
					dataGridView1.Columns[i].Name = Convert.ToString(i);

				for (int i = 0; i < n; i++)
				{
					string[] str = new string[n];
					for (int j = 0; j < n; j++)
						str[j] = Convert.ToString(Math.Round(2*test1.matrix[i, j].ToDouble(), 4));
					dataGridView1.Rows.Add(str);
				}
			}
			
			dataGridView2.ColumnCount = n;
			for (int i = 0; i < n; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < n; i++)
			{
				string[] str = new string[n];
				for (int j = 0; j < n; j++)
					str[j] = Convert.ToString(Math.Round(resmult.matrix[i, j].ToDouble(), 4));
				dataGridView2.Rows.Add(str);
			}
			*/
			#endregion
		}

		private void button2_Click(object sender, EventArgs e)
		{
			textBox1.Text = "";
			Complex c1 = new Complex(1, 4.899);
			Complex c2 = new Complex(4, 3);
			//if (c1 > c2) textBox1.Text = "c1>c2";
			//else textBox1.Text = "c1<c2";
			textBox1.Text = Convert.ToString(c1.Arg()) + "  " + Convert.ToString(c2.Arg());
		}
    }
}
