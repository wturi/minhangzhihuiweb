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
    
    public partial class tbCheckInOuts
    {
        public string CheckID { get; set; }
        public int Version { get; set; }
        public int SortNo { get; set; }
        public string UseType { get; set; }
        public string ObjectId { get; set; }
        public Nullable<int> ObjectVersion { get; set; }
        public string Memo { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
