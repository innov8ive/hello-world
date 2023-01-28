using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data;
using System.Data.Linq.Mapping;

namespace WebApplication1
{
    public class SetuInfoDL
    {

        #region Private Members
        private string connectionString;
        #endregion

        #region Constructor
        public SetuInfoDL(string conString)
        {
            connectionString = conString;
        }
        #endregion

        #region Main Methods
        public SetuInfo Load(int ID)
        {
            SqlConnection SqlCon = new SqlConnection(connectionString);
            SetuInfo objSetuInfo = new SetuInfo();
            var dc = new DataContext(SqlCon);
            try
            {
                //Get SetuInfo
                var resultSetuInfo = dc.ExecuteQuery<SetuInfo>("exec Get_SetuInfo {0}", ID).ToList();
                if (resultSetuInfo.Count > 0)
                {
                    objSetuInfo = resultSetuInfo[0];
                }
                dc.Dispose();
                return objSetuInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
                SqlCon.Dispose();
            }
        }

        public SetuInfo Load(string platformBillId)
        {
            platformBillId = platformBillId.Replace("'", "''");
            SqlConnection SqlCon = new SqlConnection(connectionString);
            SetuInfo objSetuInfo = new SetuInfo();
            var dc = new DataContext(SqlCon);
            try
            {
                //Get SetuInfo
                var resultSetuInfo = dc.ExecuteQuery<SetuInfo>("Select * from SetuInfo where PlatformBillID={0}", platformBillId).ToList();
                if (resultSetuInfo.Count > 0)
                {
                    objSetuInfo = resultSetuInfo[0];
                }
                dc.Dispose();
                return objSetuInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
                SqlCon.Dispose();
            }
        }
        public DataTable LoadAllSetuInfo()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("exec Get_SetuInfo", con);

            con.Open();
            dt.Load(cmd.ExecuteReader());
            con.Close();

            return dt;
        }
        public bool Update(SetuInfo objSetuInfo)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlTransaction trn = con.BeginTransaction();
            try
            {
                //update SetuInfo
                UpdateSetuInfo(objSetuInfo, trn);
                if (objSetuInfo.ID > 0)
                {

                    trn.Commit();
                }
                return true;
            }
            catch
            {
                trn.Rollback();
                return false;
            }
            finally
            {
                con.Dispose();
            }
        }
        public bool Delete(int ID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlTransaction trn = con.BeginTransaction();
            try
            {
                //Delete SetuInfo
                DeleteSetuInfo(ID, trn);
                trn.Commit();
                return true;
            }
            catch
            {
                trn.Rollback();
                return false;
            }
            finally
            {
                con.Dispose();
            }
        }

        public bool UpdateSetuInfo(SetuInfo objSetuInfo, SqlTransaction trn)
        {
            SqlCommand cmd = new SqlCommand("Insert_Update_SetuInfo", trn.Connection);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Transaction = trn;
                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = objSetuInfo.Amount;
                cmd.Parameters.Add("@BillerBillID", SqlDbType.VarChar, 50).Value = objSetuInfo.BillerBillID;
                cmd.Parameters.Add("@EduclatPaymentId", SqlDbType.Int).Value = objSetuInfo.EduclatPaymentId;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = objSetuInfo.ID;
                cmd.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@LastStatus", SqlDbType.VarChar, 50).Value = objSetuInfo.LastStatus;
                cmd.Parameters.Add("@LastStatusRecieved", SqlDbType.DateTime).Value = objSetuInfo.LastStatusRecieved;
                cmd.Parameters.Add("@LinkCreated", SqlDbType.DateTime).Value = objSetuInfo.LinkCreated;
                cmd.Parameters.Add("@LinkExpired", SqlDbType.DateTime).Value = objSetuInfo.LinkExpired;
                cmd.Parameters.Add("@PaymentLinkUPIID", SqlDbType.VarChar, 500).Value = objSetuInfo.PaymentLinkUPIID;
                cmd.Parameters.Add("@PaymentLinkUPILInk", SqlDbType.VarChar, 500).Value = objSetuInfo.PaymentLinkUPILInk;
                cmd.Parameters.Add("@PaymentNote", SqlDbType.VarChar, 500).Value = objSetuInfo.PaymentNote;
                cmd.Parameters.Add("@PlatformBillID", SqlDbType.VarChar, 50).Value = objSetuInfo.PlatformBillID;
                cmd.Parameters.Add("@StatusEventId", SqlDbType.VarChar, 50).Value = objSetuInfo.StatusEventId;

                cmd.ExecuteNonQuery();

                //after updating the SetuInfo, update ID
                objSetuInfo.ID = Convert.ToInt32(cmd.Parameters["@ID"].Value);

                return true;
            }
            catch
            {
                trn.Rollback();
                return false;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        public bool DeleteSetuInfo(int ID, SqlTransaction trn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Delete from SetuInfo where ID=@ID", trn.Connection);
                cmd.Transaction = trn;

                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                cmd.ExecuteNonQuery();


                return true;
            }
            catch
            {
                trn.Rollback();
                return false;
            }
        }
        #endregion
    }
}
