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
    
    public partial class Jokul_Passport_Friend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendUserId { get; set; }
        public string FriendUsername { get; set; }
        public System.DateTime DateTime { get; set; }
    }
}
