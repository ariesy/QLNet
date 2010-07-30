/*
 Copyright (C) 2008 Siarhei Novik (snovik@gmail.com)
 Copyright (C) 2008, 2009 , 2010  Andrea Maggiulli (a.maggiulli@gmail.com) 
  
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
using System.Collections.Generic;
using QLNet.Time;

namespace QLNet 
{
   public class FixedRateBond : Bond 
   {
      //! fixed-rate bond
      /*! \ingroup instruments

          \test calculations are tested by checking results against
                cached values.
      */
      public FixedRateBond(int settlementDays, double faceAmount, Schedule schedule, 
                           List<double> coupons,DayCounter accrualDayCounter)
            : this(settlementDays, faceAmount, schedule, coupons, accrualDayCounter,
				   BusinessDayConvention.Following, 100, null, new Calendar()) { }

      public FixedRateBond(int settlementDays, double faceAmount, Schedule schedule,
                           List<double> coupons, DayCounter accrualDayCounter, 
                           BusinessDayConvention paymentConvention,double redemption, Date issueDate)
         : this(settlementDays, faceAmount, schedule, coupons, accrualDayCounter,
                paymentConvention, redemption, issueDate, new Calendar()) { }

      //! simple annual compounding coupon rates      
      public FixedRateBond(int settlementDays, double faceAmount, Schedule schedule,List<double> coupons, 
                           DayCounter accrualDayCounter, BusinessDayConvention paymentConvention,
                           double redemption, Date issueDate,Calendar paymentCalendar)
         : base(settlementDays, paymentCalendar == new Calendar() ? schedule.calendar() : paymentCalendar, 
                issueDate) 
      {
         frequency_ = schedule.tenor().frequency();
         dayCounter_ = accrualDayCounter;
         maturityDate_ = schedule.endDate();

         cashflows_ = new FixedRateLeg(schedule)
            .withCouponRates(coupons, accrualDayCounter)
            .withPaymentCalendar(calendar_)
            .withNotionals(faceAmount)
            .withPaymentAdjustment(paymentConvention);

         addRedemptionsToCashflows(new List<double>() { redemption });

         if (cashflows().Count == 0)
            throw new ApplicationException("bond with no cashflows!");

         if (redemptions_.Count != 1)
            throw new ApplicationException("multiple redemptions created");
      }
      
      /*! simple annual compounding coupon rates
          with internal schedule calculation */
      public FixedRateBond(int settlementDays, 
                           Calendar calendar,
                           double faceAmount,
                           Date startDate,
                           Date maturityDate,
                           Period tenor,
                           List<double> coupons,
                           DayCounter accrualDayCounter,
                           BusinessDayConvention accrualConvention,
                           BusinessDayConvention paymentConvention,
                           double redemption,
                           Date issueDate,
                           Date stubDate,
                           DateGeneration.Rule rule,
                           bool endOfMonth,
                           Calendar paymentCalendar)
         : base(settlementDays, paymentCalendar == new Calendar() ? calendar : paymentCalendar, 
                issueDate) 
      {

         frequency_ = tenor.frequency();
         dayCounter_ = accrualDayCounter;
         maturityDate_     = maturityDate;

         Date firstDate, nextToLastDate;

         switch (rule) 
         {
         
            case DateGeneration.Rule.Backward:
				 firstDate = null;
               nextToLastDate = stubDate;
               break;

            case DateGeneration.Rule.Forward:
               firstDate = stubDate;
			   nextToLastDate = null;
               break;

            case DateGeneration.Rule.Zero:
            case DateGeneration.Rule.ThirdWednesday:
            case DateGeneration.Rule.Twentieth:
            case DateGeneration.Rule.TwentiethIMM:
               throw new ApplicationException("stub date (" + stubDate + ") not allowed with " + rule + " DateGeneration::Rule");
              
            default:
               throw new ApplicationException("unknown DateGeneration::Rule (" + rule + ")");
         }


         Schedule schedule = new Schedule(startDate, maturityDate_, tenor,
                                          calendar, accrualConvention, accrualConvention,
                                          rule, endOfMonth,
                                          firstDate, nextToLastDate);

            
         cashflows_ = new FixedRateLeg(schedule)
            .withCouponRates(coupons, accrualDayCounter)
            .withPaymentCalendar(calendar_)
            .withNotionals(faceAmount)
            .withPaymentAdjustment(paymentConvention);

         addRedemptionsToCashflows(new List<double>() { redemption });


         if (cashflows().Count == 0)
            throw new ApplicationException("bond with no cashflows!");
         
         if (redemptions_.Count != 1)
            throw new ApplicationException("multiple redemptions created");
      }

      //! generic compounding and frequency InterestRate coupons 
      public FixedRateBond(int settlementDays,
                           double faceAmount,
                           Schedule schedule,
                           List<InterestRate> coupons,
                           BusinessDayConvention paymentConvention,
                           double redemption,
                           Date issueDate)
         : this(settlementDays, faceAmount, schedule, coupons, paymentConvention, redemption, issueDate, new Calendar()) { }

      public FixedRateBond(int settlementDays,
                           double faceAmount,
                           Schedule schedule,
                           List<InterestRate> coupons,
                           BusinessDayConvention paymentConvention,
                           double redemption,
                           Date issueDate,
                           Calendar paymentCalendar)

         : base(settlementDays,paymentCalendar == new Calendar() ? schedule.calendar() : paymentCalendar,
                issueDate)
      {
      
         frequency_ = schedule.tenor().frequency();
         dayCounter_ = coupons[0].dayCounter();
         maturityDate_ = schedule.endDate();

        cashflows_ = new FixedRateLeg(schedule)
                    .withCouponRates(coupons)
                    .withPaymentCalendar(calendar_)
                    .withNotionals(faceAmount)
                    .withPaymentAdjustment(paymentConvention);

        addRedemptionsToCashflows(new List<double>() { redemption });


        if (cashflows().Count == 0)
         throw new ApplicationException("bond with no cashflows!");
        
        if (redemptions_.Count != 1)
           throw new ApplicationException("multiple redemptions created");
    }
      
      Frequency frequency() { return frequency_; }
      DayCounter dayCounter() { return dayCounter_; }

      protected Frequency frequency_;
      protected DayCounter dayCounter_;

   }
}
