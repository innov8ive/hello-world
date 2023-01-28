using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    [Serializable]
    public class SetuInfoBL
    {

        #region Declaration
        private string connectionString;
        SetuInfo _SetuInfo;
        public SetuInfo Data
        {
            get { return _SetuInfo; }
            set { _SetuInfo = value; }
        }
        public bool IsNew
        {
            get { return (_SetuInfo.ID <= 0 || _SetuInfo.ID == null); }
        }
        #endregion

        #region Constructor
        public SetuInfoBL(string conString)
        {
            connectionString = conString;
        }
        #endregion

        #region Main Methods
        private SetuInfoDL CreateDL()
        {
            return new SetuInfoDL(connectionString);
        }
        public void New()
        {
            _SetuInfo = new SetuInfo();
        }
        public void Load(int ID)
        {
            var SetuInfoObj = this.CreateDL();
            _SetuInfo = ID <= 0 ? SetuInfoObj.Load(-1) : SetuInfoObj.Load(ID);
        }
        public void Load(string PlatFormBillID)
        {
            var SetuInfoObj = this.CreateDL();
            _SetuInfo = SetuInfoObj.Load(PlatFormBillID);
        }
        public DataTable LoadAllSetuInfo()
        {
            var SetuInfoDLObj = CreateDL();
            return SetuInfoDLObj.LoadAllSetuInfo();
        }
        public bool Update()
        {
            var SetuInfoDLObj = CreateDL();
            return SetuInfoDLObj.Update(this.Data);
        }
        public bool Delete(int ID)
        {
            var SetuInfoDLObj = CreateDL();
            return SetuInfoDLObj.Delete(ID);
        }
        #endregion
    }
}
