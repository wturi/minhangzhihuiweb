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
    
    public partial class tbRepair
    {
        public string RepairID { get; set; }
        public Nullable<int> RepairType { get; set; }
        public string RepairTitle { get; set; }
        public string RepairContent { get; set; }
        public Nullable<int> StateCode { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}