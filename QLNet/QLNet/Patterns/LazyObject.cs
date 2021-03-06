/*
 Copyright (C) 2008 Siarhei Novik (snovik@gmail.com)
  
 This file is part of QLNet Project http://www.qlnet.org

 QLNet is free software: you can redistribute it and/or modify it
 under the terms of the QLNet license.  You should have received a
 copy of the license along with this program; if not, license is  
 available online at <http://trac2.assembla.com/QLNet/wiki/License>.
  
 QLNet is a based on QuantLib, a free-software/open-source library
 for financial quantitative analysts and developers - http://quantlib.org/
 The QuantLib license is available online at http://quantlib.org/license.shtml.
 
 This program is distributed in the hope that it will be useful, but WITHOUT
 ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 FOR A PARTICULAR PURPOSE.  See the license for more details.
*/

using System;

namespace QLNet.Patterns
{
	/// <summary>
	/// Framework for calculation on demand and result caching.
	/// </summary>
	public abstract class LazyObject : DefaultObservable, IObserver
	{
		public bool Calculated { get; protected set; }
		public bool Frozen { get; protected set; }

		public virtual void update()
		{
			if (!Frozen && Calculated)
			{
				notifyObservers();
			}

			Calculated = false;
		}

		/// <summary>
		/// This method forces recalculation of any results which would otherwise be cached.
		/// </summary>
		/// <remarks>
		/// Explicit invocation of this method is not necessary if the object registered itself 
		/// as observer with the structures on which such results depend. 
		/// It is strongly advised to follow this policy when possible.
		/// </remarks>
		public virtual void recalculate()
		{
			bool wasFrozen = Frozen;
			
			Calculated = false;
			Frozen = false;
			
			try
			{
				calculate();
			}
			catch
			{
				Frozen = wasFrozen;
				notifyObservers();
				throw;
			}

			Frozen = wasFrozen;
			
			notifyObservers();
		}

		/// <summary>
		/// This method constrains the object to return the presently cached results 
		/// on successive invocations, even if arguments upon which they depend should change.
		/// </summary>
		public void freeze()
		{
			Frozen = true;
		}

		/// <summary>
		/// This method reverts the effect of the <see cref="unfreeze"/> method, thus re-enabling recalculations.
		/// </summary>
		public void unfreeze()
		{
			Frozen = false;
			notifyObservers();
		}

		/// <summary>
		/// This method performs all needed calculations by calling the <see cref="performCalculations"/> method.
		/// 
		/// Objects cache the results of the previous calculation. Such results will be returned upon
		/// later invocations of <see cref="calculate"/>. 
		/// 
		/// When the results depend on arguments which could change between invocations, 
		/// the lazy object must register itself as observer of such objects for the 
		/// calculations to be performed again when they change.
		/// 
		/// Should this method be redefined in derived classes, <see cref="calculate"/> should be called
		/// in the overriding method.
		/// </summary>
		protected virtual void calculate()
		{
			if (!Calculated && !Frozen)
			{
				// prevent infinite recursion in case of bootstrapping
				Calculated = true;   
				try
				{
					performCalculations();
				}
				catch
				{
					Calculated = false;
					throw;
				}
			}
		}

		/// <summary>
		/// This method must implement any calculations which must be 
		/// (re)done in order to calculate the desired results.
		/// </summary>
		protected virtual void performCalculations()
		{
			throw new NotImplementedException();
		}
	}
}
