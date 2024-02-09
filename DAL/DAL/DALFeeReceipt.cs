using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALFeeReceipt
        {
            private CombineGymEntities db = new CombineGymEntities();

        //public List<spFeeReceiptList_Result> FeeReceiptListResult(Nullable<int> branchId, Nullable<int> FeeReceiptId, string name, string phone, Nullable<Int16> status, Nullable<Int16> gender, Nullable<Int16> shift)
        //{
        //    try
        //    {
        //        var EmployeeListResult = db.spFeeReceiptList(branchId, FeeReceiptId, name, phone, status, gender, shift).ToList();
        //        return EmployeeListResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public decimal GetStdentPrevBal(Nullable<int> branchId, Nullable<int> studentId, int feeMonth, int feeYear)
        {
            try
            {
                decimal? StdentPrevBalResult = db.spGetStdentPrevBal(branchId, studentId, feeMonth, feeYear).FirstOrDefault();
                if (StdentPrevBalResult == null)
                    StdentPrevBalResult = 0; 
                return StdentPrevBalResult.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<spFeeReceiptHistory_Result> FeeReceiptHistoryResult(Nullable<int> branchId, Nullable<int> studentId )
        {
            try
            {
                var feeReceiptHistoryResult = db.spFeeReceiptHistory(branchId,studentId).ToList();
                return feeReceiptHistoryResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<spFeeReceiptHistorySearch_Result> FeeReceiptHistorySearchResult(Nullable<int> branchId, Nullable<int> studentId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            try
            {
                var feeReceiptHistorySearchResult = db.spFeeReceiptHistorySearch(branchId, studentId,fromDate, toDate).ToList();
                return feeReceiptHistorySearchResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FeeReceipt FeeReceiptGet(Int64 FeeReceiptId)
        {
            try
            {
                var FeeReceipt = db.FeeReceipts.Where(x => x.FeeReceiptId == FeeReceiptId).FirstOrDefault();
                return FeeReceipt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsFeeReceiptEditable(int studentId, Int32 CurrentFeeReceiptId)
        {
            try
            {
                bool isEditable = false;
                int feeReceiptId = db.FeeReceipts.Where(x => x.StudentId == studentId).Max(x => x.FeeReceiptId);
                if(CurrentFeeReceiptId == feeReceiptId)
                {
                    isEditable = true;
                }
                else
                {
                    isEditable = false;
                }
                return isEditable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal FeeReceiptGetStudentPreviousBalance(Int64 studentId, Int64 branchId)
        {
            try
            {
                decimal previousBalance;
                   FeeReceipt feeReceipt = db.FeeReceipts.Where(x => x.BranchId == branchId && x.StudentId == studentId).OrderByDescending(x => x.FeeYear).OrderByDescending(x => x.FeeYear).FirstOrDefault();
                if(feeReceipt == null)
                {
                    previousBalance = 0;
                }
                else
                {
                    previousBalance = feeReceipt.BalanceAmount.Value;
                }

                return previousBalance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool FeeReceiptDelete(Int64 FeeReceiptId)
        {
            try
            {
                var FeeReceipt = db.FeeReceipts.Where(x => x.FeeReceiptId == FeeReceiptId).FirstOrDefault();
                db.FeeReceipts.Remove(FeeReceipt);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int IsDBStatus(Nullable<int> feeReceiptId, Nullable<int> branchId, Nullable<int> studentId, Nullable<int> feeYear, Nullable<int> feeMonh)
        {
            try
            {
                //  return 
                //1 = update, 
                //2 = duplicate, 
                //3 = previous entry

                int dbStatus = 0;

                FeeReceipt fee = null;
                if (feeReceiptId != null && feeReceiptId != 0)
                {
                    fee = db.FeeReceipts.Where(x => x.FeeReceiptId == feeReceiptId && x.BranchId == branchId && x.StudentId == studentId && x.FeeYear == feeYear && x.FeeMonth == feeMonh).FirstOrDefault();
                    if (fee != null)
                    {
                        dbStatus = 1;
                    }
                }
                else
                {
                    fee = db.FeeReceipts.Where(x => x.BranchId == branchId && x.StudentId == studentId && x.FeeYear == feeYear && x.FeeMonth == feeMonh).FirstOrDefault();
                    if (fee != null)
                    {
                        dbStatus = 2;
                    }
                }

                return dbStatus;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public FeeReceipt FeeReceiptSave(FeeReceipt FeeReceipt)
        {
            try
            {
                if (FeeReceipt != null)
                {
                    //db.FeeReceiptes.AddOrUpdate(FeeReceipt);
                    if (FeeReceipt.FeeReceiptId == 0)
                    {
                        db.Entry(FeeReceipt).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        db.Entry(FeeReceipt).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();
                    return FeeReceipt;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
