//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MinHangWisdomParkWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class mtRfidCode
    {
        public mtRfidCode()
        {
            this.tbIORecord = new HashSet<tbIORecord>();
            this.tbOwnerRfid = new HashSet<tbOwnerRfid>();
        }
    
        public int AutoId { get; set; }
        public string RfidCode { get; set; }
        public string TID { get; set; }
        public string UID { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    
        public virtual ICollection<tbIORecord> tbIORecord { get; set; }
        public virtual ICollection<tbOwnerRfid> tbOwnerRfid { get; set; }
    }
}
