/* ====================================================================
   Copyright (C) 2004-2008  fyiReporting Software, LLC
   Copyright (C) 2011  Peter Gill <peter@majorsilence.com>

   This file is part of the fyiReporting RDL project.
	
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.


   For additional information, email info@fyireporting.com or visit
   the website www.fyiReporting.com.
*/
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using RdlEngine.Resources;
using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// The Financial class holds a number of static functions relating to financial
	/// calculations.
	/// 
	///	Note: many of the financial functions use the following function as their basis
	///	 pv*(1+rate)^nper+pmt*(1+rate*type)*((1+rate)^nper-1)/rate)+fv=0
	///	 if rate = 0
	///	 (pmt*nper)+pv+fv=0
	/// </summary>
	sealed internal class Financial
	{
		/// <summary>
		/// Double declining balance depreciation (when factor = 2).  Other factors may be specified.
		/// </summary>
		/// <param name="cost">Initial cost of asset</param>
		/// <param name="salvage">Salvage value of asset at end of depreciation</param>
		/// <param name="life">Number of periods over which to depreciate the asset.  AKA useful life</param>
		/// <param name="period">The period for which you want to know the depreciation amount.</param>
		/// <returns></returns>
		static public double DDB(double cost, double salvage, int life, int period)
		{
			return DDB(cost, salvage, life, period, 2);
		}

		/// <summary>
		/// Double declining balance depreciation (when factor = 2).  Other factors may be specified.
		/// </summary>
		/// <param name="cost">Initial cost of asset</param>
		/// <param name="salvage">Salvage value of asset at end of depreciation</param>
		/// <param name="life">Number of periods over which to depreciate the asset.  AKA useful life</param>
		/// <param name="period">The period for which you want to know the depreciation amount.</param>
		/// <param name="factor">The rate at which the balance declines.  Defaults to 2 (double declining) when omitted.</param>
		/// <returns></returns>
		static public double DDB(double cost, double salvage, int life, int period, double factor)
		{
			if (period > life || period < 1 || life < 1)	// invalid arguments
				return double.NaN;

			double depreciation=0;
			for (int i=1; i < period; i++)
			{
				depreciation += (cost - depreciation)*factor/life;	
			}
			if (period == life)
				return cost - salvage - depreciation;		// for last year we force the depreciation so that cost - total depreciation = salvage
			else
				return (cost - depreciation)*factor/life;
		}

		/// <summary>
		/// Returns the future value of an investment when using periodic, constant payments and 
		/// constant interest rate.
		/// </summary>
		/// <param name="rate">Interest rate per period</param>
		/// <param name="periods">Total number of payment periods</param>
		/// <param name="pmt">Amount of payment each period</param>
		/// <param name="presentValue">Lump sum amount that a series of payments is worth now</param>
		/// <param name="endOfPeriod">Specify true if payments are due at end of period, otherwise false</param>
		/// <returns></returns>
		static public double FV(double rate, int periods, double pmt, double presentValue, bool endOfPeriod)
		{
			int type = endOfPeriod? 0: 1;	

			double fv;
			if (rate == 0)
				fv = -(pmt*periods+presentValue);
			else
			{
				double temp = Math.Pow(1+rate, periods);
				fv = -(presentValue*temp + pmt*(1+rate*type)*((temp -1)/rate));
			}

			return fv;
		}
		
		/// <summary>
		/// Returns the interest payment portion of a payment given a particular period.
		/// </summary>
		/// <param name="rate">Interest rate per period</param>
		/// <param name="period">Period for which you want the interest payment.</param>
		/// <param name="periods">Total number of payment periods</param>
		/// <param name="presentValue">Lump sum amount that a series of payments is worth now</param>
		/// <param name="futureValue">Cash balance you want to attain after last payment</param>
		/// <param name="endOfPeriod">Specify true if payments are due at end of period, otherwise false</param>
		/// <returns></returns>
		static public double IPmt(double rate, int period, int periods, double presentValue, double futureValue, bool endOfPeriod)
		{
			// TODO -- routine needs more work.   off when endOfPeriod is false; must be more direct way to solve
			if (!endOfPeriod)
				throw new Exception(Strings.Financial_Error_IPmtNotSupportPayments);

			if (period < 0 || period > periods)
				return double.NaN;
			if (!endOfPeriod)
				period--;

			double pmt = -Pmt(rate, periods, presentValue, futureValue, endOfPeriod);
			double prin = presentValue; 
			double interest=0;
			for (int i=0; i < period; i++)
			{
				interest = rate*prin;
				prin = prin - pmt + interest;
			}
			return -interest;
		}
		
		/// <summary>
		/// Returns the number of periods for an investment.
		/// </summary>
		/// <param name="rate">Interest rate per period</param>
		/// <param name="pmt">Amount of payment each period</param>
		/// <param name="presentValue">Lump sum amount that a series of payments is worth now</param>
		/// <param name="futureValue">Future value or cash balance you want after last payment</param>
		/// <param name="endOfPeriod">Specify true if payments are due at end of period, otherwise false</param>
		/// <returns></returns>
		static public double NPer(double rate, double pmt, double presentValue, double futureValue, bool endOfPeriod)
		{
			if (Math.Abs(pmt) < double.Epsilon)
				return double.NaN;

			int type = endOfPeriod? 0: 1;	

			double nper;
			if (rate == 0)
				nper = -(presentValue + futureValue)/pmt;
			else
			{
				double r1 = 1 + rate*type;
				double pmtr1 = pmt * r1 / rate;
				double y = (pmtr1 - futureValue) / (presentValue + pmtr1);

				nper = Math.Log(y) / Math.Log(1 + rate);
			}

			return nper;
		}
		/// <summary>
		/// Returns the periodic payment for an annuity using constant payments and 
		/// constant interest rate.
		/// </summary>
		/// <param name="rate">Interest rate per period</param>
		/// <param name="periods">Total number of payment periods</param>
		/// <param name="presentValue">Lump sum amount that a series of payments is worth now</param>
		/// <param name="futureValue">Cash balance you want to attain after last payment</param>
		/// <param name="endOfPeriod">Specify true if payments are due at end of period, otherwise false</param>
		/// <returns></returns>
		static public double Pmt(double rate, int periods, double presentValue, double futureValue, bool endOfPeriod)
		{
			if (periods < 1)
				return double.NaN;

			int type = endOfPeriod? 0: 1;	

			double pmt;
			if (rate == 0)
				pmt = -(presentValue + futureValue)/periods;
			else
			{
				double temp = Math.Pow(1+rate, periods);
				pmt = -(presentValue*temp + futureValue) / ((1+rate*type)*(temp-1)/rate);
			}

			return pmt;
		}
		/// <summary>
		/// Returns the present value of an investment.  The present value is the total
		/// amount that a series of future payments is worth now. 
		/// </summary>
		/// <param name="rate">Interest rate per period</param>
		/// <param name="periods">Total number of payment periods</param>
		/// <param name="pmt">Amount of payment each period</param>
		/// <param name="futureValue">Cash balance you want to attain after last payment is made</param>
		/// <param name="endOfPeriod">Specify true if payments are due at end of period, otherwise false</param>
		/// <returns></returns>
		static public double PV(double rate, int periods, double pmt, double futureValue, bool endOfPeriod)
		{
			int type = endOfPeriod? 0: 1;	

			double pv;
			if (rate == 0)
				pv = -(pmt*periods+futureValue);
			else
			{
				double temp = Math.Pow(1+rate, periods);
				pv = -(pmt*(1+rate*type)*((temp-1)/rate) + futureValue)/temp;
			}

			return pv;
		}
		/// <summary>
		/// Returns the interest rate per period for an annuity.  This routine uses an
		/// iterative approach to solving for rate.   If after 30 iterations the answer
		/// doesn't converge to within 0.0000001 then double.NAN is returned.
		/// </summary>
		/// <param name="periods">Total number of payment periods</param>
		/// <param name="pmt">Amount of payment each period</param>
		/// <param name="presentValue">Lump sum amount that a series of payments is worth now</param>
		/// <param name="futureValue">Cash balance you want to attain after last payment</param>
		/// <param name="endOfPeriod">Specify true if payments are due at end of period, otherwise false</param>
		/// <param name="guess">Your guess as to what the interest rate will be.</param>
		/// <returns></returns>
		static public double Rate(int periods, double pmt, double presentValue, double futureValue, bool endOfPeriod, double guess)
		{
			// TODO:  should be better algorithm: linear interpolation, Newton-Raphson approximation???
			int type = endOfPeriod? 0: 1;	
			// Set the lower bound
			double gLower= guess > .1? guess-.1: 0;	
			double power2=.1;
			while (RateGuess(periods, pmt, presentValue, futureValue, type, gLower) > 0)
			{
				gLower -= power2;
				power2 *= 2;
			}
			// Set the upper bound
			double gUpper= guess+.1;
			power2=.1;
			while (RateGuess(periods, pmt, presentValue, futureValue, type, gUpper) < 0)
			{
				gUpper += power2;
				power2 *= 2;
			}
			double z;
			double incr;
			double newguess;
			for (int i= 0; i<30; i++)
			{
				z = RateGuess(periods,pmt,presentValue,futureValue, type, guess);

				if (z > 0)
				{
					gUpper = guess;
					incr = (guess - gLower)/2;
					newguess = guess - incr;
				}
				else
				{
					gLower = guess;
					incr = (gUpper - guess)/2;
					newguess = guess + incr;
				}
				if (incr < 0.0000001)		// Is the difference within our margin of error?
					return guess;
				guess = newguess;
			}
			return double.NaN;			// we didn't converge
		}

		static private double RateGuess(int periods, double pmt, double pv, double fv, int type, double rate)
		{
			// When the guess is close the result should almost be 0
			if (rate == 0)
				return pmt*periods + pv + fv;
			double temp = Math.Pow(1+rate, periods);
			return pv*temp + pmt*(1 + rate*type)*((temp - 1)/rate) + fv;
		}

		/// <summary>
		/// SLN returns the straight line depreciation for an asset for a single period.
		/// </summary>
		/// <param name="cost">Initial cost of asset</param>
		/// <param name="salvage">Salvage value of asset at end of depreciation</param>
		/// <param name="life">Number of periods over which to depreciate the asset. AKA useful life</param>
		/// <returns></returns>
		static public double SLN(double cost, double salvage, double life)
		{
			if (life == 0)
				return double.NaN;
			return (cost - salvage) / life;
		}
		/// <summary>
		/// Sum of years digits depreciation.  An asset often loses more of its value early in its lifetime. 
		/// SYD has this behavior.
		/// </summary>
		/// <param name="cost">Initial cost of asset</param>
		/// <param name="salvage">Salvage value of asset at end of depreciation</param>
		/// <param name="life">Number of periods over which to depreciate the asset.  AKA useful life</param>
		/// <param name="period">The period for which you want to know the depreciation amount.</param>
		/// <returns></returns>
		static public double SYD(double cost, double salvage, int life, int period)
		{
			int sumOfPeriods;
			if (life % 2 == 0)						// sum = n/2 * (n+1) when even
				sumOfPeriods = life/2 * (life + 1);
			else									// sum = (n+1)/2 * n when odd
				sumOfPeriods = (life+1)/2 * life;

			return ((life + 1 - period) * (cost - salvage)) / sumOfPeriods;
		}
	}
}
