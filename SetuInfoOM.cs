using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data;
using System.Data.Linq.Mapping;
using System.Collections;

namespace WebApplication1
{

    [Serializable]
    [Table(Name = "dbo.SetuInfo")]
    public class SetuInfo
    {
        private System.Nullable<decimal> _Amount;
        private string _BillerBillID;
        private System.Nullable<int> _EduclatPaymentId;
        private System.Nullable<int> _ID;
        private string _LastStatus;
        private System.Nullable<DateTime> _LastStatusRecieved;
        private System.Nullable<DateTime> _LinkCreated;
        private System.Nullable<DateTime> _LinkExpired;
        private string _PaymentLinkUPIID;
        private string _PaymentLinkUPILInk;
        private string _PaymentNote;
        private string _PlatformBillID;
        private string _StatusEventId;



        [Column(Storage = "_Amount")]
        public System.Nullable<decimal> Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }

        [Column(Storage = "_BillerBillID")]
        public string BillerBillID
        {
            get
            {
                return _BillerBillID;
            }
            set
            {
                _BillerBillID = value;
            }
        }

        [Column(Storage = "_EduclatPaymentId")]
        public System.Nullable<int> EduclatPaymentId
        {
            get
            {
                return _EduclatPaymentId;
            }
            set
            {
                _EduclatPaymentId = value;
            }
        }

        [Column(Storage = "_ID")]
        public System.Nullable<int> ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        [Column(Storage = "_LastStatus")]
        public string LastStatus
        {
            get
            {
                return _LastStatus;
            }
            set
            {
                _LastStatus = value;
            }
        }

        [Column(Storage = "_LastStatusRecieved")]
        public System.Nullable<DateTime> LastStatusRecieved
        {
            get
            {
                return _LastStatusRecieved;
            }
            set
            {
                _LastStatusRecieved = value;
            }
        }

        [Column(Storage = "_LinkCreated")]
        public System.Nullable<DateTime> LinkCreated
        {
            get
            {
                return _LinkCreated;
            }
            set
            {
                _LinkCreated = value;
            }
        }

        [Column(Storage = "_LinkExpired")]
        public System.Nullable<DateTime> LinkExpired
        {
            get
            {
                return _LinkExpired;
            }
            set
            {
                _LinkExpired = value;
            }
        }

        [Column(Storage = "_PaymentLinkUPIID")]
        public string PaymentLinkUPIID
        {
            get
            {
                return _PaymentLinkUPIID;
            }
            set
            {
                _PaymentLinkUPIID = value;
            }
        }

        [Column(Storage = "_PaymentLinkUPILInk")]
        public string PaymentLinkUPILInk
        {
            get
            {
                return _PaymentLinkUPILInk;
            }
            set
            {
                _PaymentLinkUPILInk = value;
            }
        }

        [Column(Storage = "_PaymentNote")]
        public string PaymentNote
        {
            get
            {
                return _PaymentNote;
            }
            set
            {
                _PaymentNote = value;
            }
        }

        [Column(Storage = "_PlatformBillID")]
        public string PlatformBillID
        {
            get
            {
                return _PlatformBillID;
            }
            set
            {
                _PlatformBillID = value;
            }
        }

        [Column(Storage = "_StatusEventId")]
        public string StatusEventId
        {
            get
            {
                return _StatusEventId;
            }
            set
            {
                _StatusEventId = value;
            }
        }
    }


}
