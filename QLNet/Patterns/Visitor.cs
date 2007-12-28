/*
 This file is part of QLNet Project http://trac2.assembla.com/QLNet
 
 QLNet is a porting of QuantLib, a free-software/open-source library
 for financial quantitative analysts and developers - http://quantlib.org/
 The license is available online at http://quantlib.org/license.shtml.
 
 This program is distributed in the hope that it will be useful, but WITHOUT
 ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 FOR A PARTICULAR PURPOSE.  See the license for more details.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace QLNet
{
   public class AcyclicVisitor
   {
      public void Dispose()
      {
      }
   }

   //! %Visitor for a specific class
   public class Visitor<T>
   {
      public void Dispose()
      {
      }
      public void visit(ref T NamelessParameter){}
   }
}
