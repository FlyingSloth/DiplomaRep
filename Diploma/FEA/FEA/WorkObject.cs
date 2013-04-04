using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using FEA;
using ei;

namespace FEA
{
    public class WorkObject
	{
		#region Structs of characteristics and critical values
		public struct DISP
        {
            public double k;
            public Complex y1;
            public Complex y2;
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
		#region Dispersion characteristics
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
        public DISP[] dispersion(int fe, int Nsteps, double step, int mode, LAY[] L, int[] curves, ref System.ComponentModel.BackgroundWorker bg, ref int iniProgress, int coef, bool isChecked)
        {
			dispchar = new DISP[Nsteps+1];
            Complex[] E1 = new Complex[21];
            Complex firstAbsValue = new Complex();
            Complex secondAbsValue = new Complex();
            int firstMinN = 0, secondMinN = 0;
			int progress = 0;

            E1 = eigen(fe, 0, mode, L);
            firstAbsValue = E1[curves[0]-1];
            secondAbsValue = E1[curves[1]-1];
            dispchar[0].k = 0;
            dispchar[0].y1 = firstAbsValue;
            dispchar[0].y2 = secondAbsValue;
            for (int i1 = 1; i1 < Nsteps + 1; i1++)
            {
				Stopwatch sw = new Stopwatch();
				sw.Start();
				
				Complex[] E2 = new Complex[21];
                Complex[] firstbuf = new Complex[21];
                Complex[] firsttempbuf = new Complex[21];
                Complex[] secondbuf = new Complex[21];
                Complex[] secondtempbuf = new Complex[21];
				E2 = eigen(fe, step * i1, mode, L);

				for (int i2 = 0; i2 < 21; i2++)
				{
					Complex firsttemp =  new Complex(Math.Abs(firstAbsValue.Re() - E2[i2].Re()), Math.Abs(firstAbsValue.Im() - E2[i2].Im()));
                    Complex secondtemp = new Complex(Math.Abs(secondAbsValue.Re() - E2[i2].Re()), Math.Abs(secondAbsValue.Im() - E2[i2].Im()));

                    firstbuf[i2] = new Complex(firsttemp.Abs());
                    secondbuf[i2] = new Complex(secondtemp.Abs());
				}

                for (int i = 0; i < 21; i++)
                {
                    firsttempbuf[i] = firstbuf[i];
                    secondtempbuf[i] = secondbuf[i];
                }
                firstAbsValue.quickSort(ref firsttempbuf, 0, 20);
                firstAbsValue.quickSort(ref secondtempbuf, 0, 20);
                Complex firstMinVal = firsttempbuf[0];
                Complex secondMinVal = secondtempbuf[0];

                for (int i3 = 0; i3 < 21; i3++)
                {
                    if (firstbuf[i3] == firstMinVal)
					{
						firstMinN = i3;
						break;
					}
                }
                for (int i3 = 0; i3 < 21; i3++)
                {
                    if (secondbuf[i3] == secondMinVal)
                    {
                        secondMinN = i3;
                        break;
                    }
                }
                dispchar[i1].k = step * i1;
                dispchar[i1].y1 = E2[firstMinN];
                dispchar[i1].y2 = E2[secondMinN];
                firstAbsValue = E2[firstMinN];
                secondAbsValue = E2[secondMinN];

				if (sw.IsRunning)
					sw.Stop();
				if (!isChecked && i1 == 1)
				{
					firsttime = (int)((sw.ElapsedMilliseconds)/1000);
					timeleft = (int)((Nsteps) * firsttime);
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
		#region Critical values and conditions
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
		public CRIT[] Crit(int fe, double Cstep, int Nsteps, double step, int mode, LAY[] L, int[] curves, ref System.ComponentModel.BackgroundWorker bg, bool isCond = true)
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
					precrit[i].D = new DISP[1];
				}

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

					buf[i].D = dispersion(fe, Nsteps, step, mode, bufL, curves, ref bg, ref iniProgress, coef, isChecked);
				}
				#region Filling critical values and conditions
				for (int i = 0; i < N; i++)
				{
					int Beg = 0, End = 0;
                    //выполнять проверки на то, край какой кривой является граничным условием
					for (int ii = 0; ii < Nsteps; ii++)
					{
                        if (buf[i].D[ii].y1.isComplex() || buf[i].D[ii].y1.isComplex())
						{
							precrit[i].R = buf[i].R;
                            precrit[i].D[0].y1 = (buf[i].D[ii].y1.isComplex()) ? buf[i].D[ii].y1 : buf[i].D[ii].y2;
							Beg = ii;
							break;
						}
					}
					if (Beg != 0)
					{
						for (int ii = Nsteps - 1; ii > 1; ii--)
						{
							if (buf[i].D[ii].y1.isComplex()) 
							{
								precrit[i].R = buf[i].R;
                                precrit[i].D[0].y2 = (buf[i].D[ii].y1.isComplex()) ? buf[i].D[ii].y1 : buf[i].D[ii].y2;
								End = ii;
								break;
							}
						}
					}
				}
				
				{
					//deleting "null" in precrit
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
				c[0].D[0].y1 = new Complex();
                c[0].D[0].y2 = new Complex();
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
