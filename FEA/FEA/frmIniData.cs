using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
			n = 9;
			Matrix A = new Matrix(n);
			A.SetA(n, 1, 10, 1, 0.000001, 0.43);
			dataGridView1.ColumnCount = 3*n-2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView1.Columns[i].Name = Convert.ToString(i);
			
			for (int i = 0; i < A.Rows(); i++)
			{
				string[] str = new string[3 * n - 2];
				for (int j = 0; j < A.Cols(); j++)
					str[j] = Convert.ToString(Math.Round(A.matrix[i, j],4));
				dataGridView1.Rows.Add(str);
			}

			Matrix B = new Matrix(n);
			B.SetB(n, 1, 10, 1, 0.000001, 0.43);
			dataGridView2.ColumnCount = 3 * n - 2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < B.Rows(); i++)
			{
				string[] str = new string[3 * n - 2];
				for (int j = 0; j < B.Cols(); j++)
					str[j] = Convert.ToString(Math.Round(B.matrix[i, j], 4));
				dataGridView2.Rows.Add(str);
			}
		}
    }
}
