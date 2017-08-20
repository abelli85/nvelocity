using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WalkReader.Model
{
    public class Meter : ICloneable
    {

        public int id { set; get; }

        //ROUTE INFO
        public int routeId { set; get; }
        public int sequence { set; get; }
        //SITE INFO
        public String siteId { set; get; }
        public String siteNo { set; get; }
        public String street { set; get; }
        public String stN1 { set; get; }
        public String stN2 { set; get; }
        public String city { set; get; }
        public String zip { set; get; }
        public String siteRemark { set; get; }
        public String meterLocation { set; get; }
        public double latitude { set; get; }
        public double longitude { set; get; }
        //CUSTOMER INFO
        public String customerName { set; get; }
        public String customerInfo { set; get; }
        //HISTORY
        public String exMeterId { set; get; }
        public DateTime? exDate { set; get; }

        public string exDateStr
        {
            get
            {
                return exDate == null ? "" : string.Format(ISO_DATE_FORMAT, exDate);
            }
        }

        public int exValue { set; get; }
        //AMR CONFIGURATION

        public string amrChannel
        {
            set
            {
                _amrChannelObj = Helper.GetEnum<AmrChannel>(value, AmrChannel.SENSUS_RF);
            }

            get
            {
                return _amrChannelObj.GetDescription();
            }
        }

        private AmrChannel _amrChannelObj = AmrChannel.SENSUS_RF;

        public String amrDevice { set; get; }
        public String amrMeterId { set; get; }
        //SENSUS RF
        public long srfAddress { set; get; }
        public String srfKey { set; get; }
        //READING
        public DateTime? date { set; get; }

        public string dateStr
        {
            get { return date == null ? "" : string.Format(ISO_DATE_FORMAT, date); }
        }

        public String meterId { set; get; }

        public string readoutStatus
        {
            set
            {
                _readoutStatus = Helper.GetEnum<ReadoutStatus>(value, ReadoutStatus.MANUAL_READ);
            }
            get
            {
                return _readoutStatus.GetDescription();
            }
        } //todo add values to enum, F1 -> HH Database/Directory Stucture

        private ReadoutStatus _readoutStatus = ReadoutStatus.MANUAL_READ;
        private readonly string ISO_DATE_FORMAT = "{0:yyyy-MM-dd'T'HH:mm:ss'Z'}";

        public int value { set; get; }
        public String unit { set; get; }
        public String comment { set; get; }
        public int alarm { set; get; }
        public String routeName { set; get; }
        public DateTime timestamp { set; get; }

        public string timestampStr
        {
            get { return timestamp == null ? "" : string.Format(ISO_DATE_FORMAT, timestamp); }
        }

        public string login { set; get; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public enum ReadoutStatus
    {
        //todo add more?
        [Description("MANUAL_READ")]
        MANUAL_READ,

        [Description("NOT_READ")]
        NOT_READ,

        [Description("AUTOMATIC_READ")]
        AUTOMATIC_READ,

        [Description("AUTOMATIC_READ_ALARM")]
        AUTOMATIC_READ_ALARM
    }

    public enum AmrChannel
    {
        [Description(("MANUAL"))]
        MANUAL,

        [Description("SENSUS_RF")]
        SENSUS_RF
    }
}
