using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FEA;
using ei;

namespace FEA
{
    public class WorkObject
	{
		#region "Structs of characteristics and critical values"
		public struct DISP
        {
            public double k;
            public Complex y;
        };

		public struct CRIT
		{
			public double? R;
			public DISP[] D;
		};

		public struct LAY
		{
			public double perm;
			public double R;
		};

		#endregion
		public DISP[] dispchar; //dispersion characteristics of waveguide
		#region "Dispersion characteristics"
		private Complex[] eigen(int fen, double kc, int mc, LAY[] L)
        {
			Matrix A = new Matrix(fen);
            A.SetA(fen, kc, mc, L);
            Matrix B = new Matrix(fen);
            B.SetB(fen, kc,  mc, L);
            return A.eige(B);
        }
		#region "Timing"
		long timeleft = 0;
		long temptime = 0;
		long firsttime = 0;
		public string TimeLeft()
		{
			return timeleft.ToString();
		}
		#endregion
		/// <summary>
        /// Dispersion characteristics
        /// </summary>
        /// <param name="fe">Number of finite elements</param>
        /// <param name="Nsteps">Number of steps</param>
        /// <param name="step">Step</param>
        /// <param name="R">Radius of layer</param>
        public DISP[] dispersion(int fe, int Nsteps, double step, int mode, LAY[] L, ref System.ComponentModel.BackgroundWorker bg, ref int iniProgress, int coef, bool isChecked)
        {
			dispchar = new DISP[Nsteps+1];
            Complex[] E1 = new Complex[21];
            Complex zeroValue = new Complex();
            int minN = 0;
			int progress = 0;

            E1 = eigen(fe, 0, mode, L);
            zeroValue = E1[mode-1];
            dispchar[0].k = 0;
            dispchar[0].y = zeroValue;
            for (int i1 = 1; i1 < Nsteps + 1; i1++)
            {
				Stopwatch sw = new Stopwatch();
				sw.Start();
				
				Complex[] E2 = new Complex[21];
                Complex[] buf = new Complex[21];
                Complex[] tempbuf = new Complex[21];
				E2 = eigen(fe, step * i1, mode, L);

                for (int i2 = 0; i2 < 21; i2++)
                    buf[i2] = new Complex(Math.Abs(zeroValue.Re() - E2[i2].Re()), Math.Abs(zeroValue.Im() - E2[i2].Im()));

                for (int i = 0; i < 21; i++) tempbuf[i] = buf[i];
                zeroValue.quickSort(ref tempbuf,0,20);
                Complex minVal = tempbuf[0];

                for (int i3 = 0; i3 < 21; i3++)
                {
                    if (buf[i3] == minVal)
                        minN = i3;
                }
                dispchar[i1].k = step * i1;
                dispchar[i1].y = E2[minN];
                zeroValue = E2[minN];

				if (sw.IsRunning)
					sw.Stop();
				if (!isChecked && i1 == 1)
				{
					firsttime = (int)((sw.ElapsedMilliseconds)/1000);
					timeleft = (int)((Nsteps-1) * firsttime);
					if (coef != 0)  timeleft *= coef;
				}
				temptime = (int)((sw.ElapsedMilliseconds) / 1000);
				if (coef != 0)
				{
					progress = (int)(Convert.ToDouble(i1)/Nsteps*100)/coef+iniProgress;
					
				}
				bg.ReportProgress(progress, (object)timeleft);
				timeleft -= temptime;
            }
			iniProgress = progress;
            return dispchar;
		}
		#endregion
		#region "Critical values and conditions"
		/// <summary>
		/// Function searches critical values and condition. Result depends on param "isCond"
		/// </summary>
		/// <param name="fe">#finite elements</param>
		/// <param name="Cstep">Step for radius checking</param>
		/// <param name="Nsteps">Step of wave number</param>
		/// <param name="step">Size of step of wave number</param>
		/// <param name="mode">Mode #</param>
		/// <param name="ec">Permittivity</param>
		/// <param name="isCond">Choise of return</param>
		/// <returns>
		/// If isCond = true: function returns critical conditions, values that are the first and the last complex numbers in all
		/// dispersion characteristics
		/// If isCond = false: function returns critical values: all the numbers between critical(including them)
		/// </returns>
		public CRIT[] Crit(int fe, double Cstep, int Nsteps, double step, int mode, LAY[] L, ref System.ComponentModel.BackgroundWorker bg, bool isCond = true)
		{
			if (L.Length == 2)
			{
				int N = Convert.ToInt32(1 / Cstep);
				LAY[] bufL = new LAY[L.Length];
				bufL = L;
				CRIT[] buf = new CRIT[N];
				CRIT[] precrit = new CRIT[N];

				for (int i = 0; i < N; i++)
				{
					precrit[i].D = new DISP[2];
				}

				CRIT[] critVal = new CRIT[N];

				bool isChecked = false;
				int iniProgress = 0;
				int coef = N;
				//all the dispersion characteristics for every radius
				for (int i = 0; i < N; i++)
				{
					buf[i].R = i * Cstep;
					bufL[0].R = i * Cstep;

					if (i == 0) isChecked = false;
					else isChecked = true;

					buf[i].D = dispersion(fe, Nsteps, step, mode, bufL, ref bg, ref iniProgress, coef, isChecked);
				}
				#region "Filling critical values and conditions"
				for (int i = 0; i < N; i++)
				{
					int Beg = 0, End = 0;
					for (int ii = 0; ii < Nsteps; ii++)
					{
						if (buf[i].D[ii].y.isComplex())
						{
							precrit[i].R = buf[i].R;
							precrit[i].D[0] = buf[i].D[ii];
							Beg = ii;
							break;
						}
					}
					if (Beg != 0)
					{
						for (int ii = Nsteps - 1; ii > 1; ii--)
						{
							if (buf[i].D[ii].y.isComplex())
							{
								precrit[i].R = buf[i].R;
								precrit[i].D[1] = buf[i].D[ii];
								End = ii;
								break;
							}
						}
					}

					if (Beg != 0 && End != 0 && !isCond)
					{
						critVal[i].D = new DISP[End - Beg + 1];
						critVal[i].R = buf[i].R;
						for (int ii = Beg; ii <= End; ii++)
						{

							critVal[i].D[ii - Beg] = buf[i].D[ii];
						}
					}
				}
				if (!isCond)
				{

					int counter = 0;
					CRIT[] bufcrit = new CRIT[N];
					for (int j = 0; j < N; j++)
					{
						if (!isNull(critVal[j].R))
						{
							bufcrit[counter] = critVal[j];
							counter++;
						}
					}
					critVal = new CRIT[counter];
					for (int j = 0; j < counter; j++)
						critVal[j] = bufcrit[j];
					return critVal;
				}
				else
				{
					//deleting "null" in precrit
					//TODO: починить смещение
					int counter = 0;
					CRIT[] bufcrit = new CRIT[N];
					for (int i = 0; i < N; i++)
					{
						if (!isNull(precrit[i].R))
						{
							bufcrit[counter] = precrit[i];
							counter++;
						}
					}

					CRIT[] critCond = new CRIT[counter];
					//final array of critical conditions
					for (int i = 0; i < counter; i++)
					{
						critCond[i] = bufcrit[i];
					}
					/*
					#region "Filling critical conditions"
					CRIT[] critCond = new CRIT[2 * counter];
					for (int i = 0; i < 2 * counter; i++)
					{
						critCond[i].D = new DISP[1];
					}
					for (int i = 0; i < counter; i++)
					{
						critCond[i].R = bufcrit[i].R;
						critCond[i].D[0].k = bufcrit[i].D[0].k;
						critCond[i].D[0].y = new Complex(0.0, bufcrit[i].D[0].y.Im());
					}
					for (int i = counter; i < 2 * counter; i++)
					{
						critCond[i].R = bufcrit[i - counter].R;
						critCond[i].D[0].k = bufcrit[i - counter].D[1].k;
						critCond[i].D[0].y = new Complex(0.0, bufcrit[i - counter].D[1].y.Im());
					}
					#endregion
					 */
				#endregion
					return critCond;
				}
			}
			else
			{
				CRIT[] c = new CRIT[1];
				c[0].R = 0.0;
				c[0].D = new DISP[1];
				c[0].D[0].k = 0.0;
				c[0].D[0].y = new Complex();
				return c;
			}
		}
		#endregion
		public bool isNull<T>(T value)
		{
			return object.Equals(value, default(T));
		}
	}
}
