using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace FEA
{
    public partial class frmIniData : Form
    {
        public frmIniData()
        {
            InitializeComponent();
        }
		private int n;

		private void button1_Click(object sender, EventArgs e)
		{
			n = 200;

			//if (n < 200) n = 200;
			Matrix A = new Matrix(n);
			A.SetA(n, 1, 10, 1, 0.43);

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
			Matrix B = new Matrix(n);
			B.SetB(n, 1, 10, 1, 0.43);
			/*
			Matrix AC = new Matrix(n);
			AC.Copy(A);
			Matrix AC1 = new Matrix(n);
			AC1 = B.Invert() * A;
			Matrix AC2 = new Matrix(n);
			AC2.Copy(A);
			*/

			Complex[] E = new Complex[21];
			
			//Matrix trid = new Matrix(3*n-2,1);

			//A.SetEigenvalues(A.eige(B));
			
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
					str[j] = Convert.ToString(Math.Round(resmult.matrix[i, j].ToDouble(), 4));
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
