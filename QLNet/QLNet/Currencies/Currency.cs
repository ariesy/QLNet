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

namespace QLNet.Currencies
{
	/// <summary>
	/// Currency specification
	/// </summary>
	public class Currency
	{
		private readonly string _name;
		private readonly string _code;
		private readonly int _numeric;
		private readonly string _symbol;
		private readonly string _fractionSymbol;
		private readonly int _fractionsPerUnit;
		private readonly Rounding _rounding;
		private readonly Currency _triangulated;
		private readonly string _formatString;

		/// <summary>
		/// currency name, e.g, "U.S. Dollar"
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// ISO 4217 three-letter code, e.g, "USD"
		/// </summary>
		public string code
		{
			get { return _code; }
		}

		/// <summary>
		/// ISO 4217 numeric code, e.g, "840"
		/// </summary>
		public int numericCode
		{
			get { return _numeric; }
		}

		/// <summary>
		/// symbol, e.g, "$"
		/// </summary>
		public string symbol
		{
			get { return _symbol; }
		}

		/// <summary>
		/// fraction symbol, e.g, "¢"
		/// </summary>
		public string fractionSymbol
		{
			get { return _fractionSymbol; }
		}

		/// <summary>
		/// number of fractionary parts in a unit, e.g, 100
		/// </summary>
		public int fractionsPerUnit
		{
			get { return _fractionsPerUnit; }
		}

		/// <summary>
		/// rounding convention
		/// </summary>
		public Rounding rounding
		{
			get { return _rounding; }
		}

		/// <summary>
		/// currency used for triangulated exchange when required
		/// </summary>
		public Currency triangulationCurrency
		{
			get { return _triangulated; }
		}

		/// <summary>
		/// The format will be fed three positional parameters, namely, value, code, and symbol, in this order.
		/// </summary>
		public string format
		{
			get { return _formatString; }
		}

		/// <summary>
		/// Instances built via this constructor have undefined behavior. Such instances can only act as placeholders
		/// and must be reassigned to a valid currency before being used.
		/// </summary>
		public Currency()
		{
		}

		public Currency(string name, string code, int numericCode, string symbol, string fractionSymbol, int fractionsPerUnit, Rounding rounding, string formatString)
			: this(name, code, numericCode, symbol, fractionSymbol, fractionsPerUnit, rounding, formatString, new Currency())
		{
		}

		public Currency(string name, string code, int numericCode, string symbol, string fractionSymbol, int fractionsPerUnit, Rounding rounding, string formatString, Currency triangulationCurrency)
		{
			_name = name;
			_code = code;
			_numeric = numericCode;
			_symbol = symbol;
			_fractionSymbol = fractionSymbol;
			_fractionsPerUnit = fractionsPerUnit;
			_rounding = rounding;
			_triangulated = triangulationCurrency;
			_formatString = formatString;
		}

		[Obsolete("Use IsEmpty property instead.")]
		public bool empty()
		{
			return (_name == null);
		}

		public bool IsEmpty
		{
			get { return _name == null; }
		}

		public override string ToString() { return code; }

		public bool Equals(Currency other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other._name, _name) && Equals(other._code, _code);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (!obj.GetType().IsSubclassOf(typeof(Currency))) return false;
			return Equals((Currency)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((_name != null ? _name.GetHashCode() : 0) * 397) ^ (_code != null ? _code.GetHashCode() : 0);
			}
		}

		public static bool operator ==(Currency c1, Currency c2)
		{
			return Equals(c1, c2);
		}

		public static bool operator !=(Currency c1, Currency c2)
		{
			return !Equals(c1, c2);
		}

		public static Money operator *(double value, Currency c)
		{
			return new Money(value, c);
		}
	}
}
