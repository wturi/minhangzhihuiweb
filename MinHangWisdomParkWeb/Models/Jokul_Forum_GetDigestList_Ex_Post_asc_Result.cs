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
    
    public partial class Jokul_Forum_GetDigestList_Ex_Post_asc_Result
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public int TopicTypeId { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
        public int ReplayCount { get; set; }
        public int TodayReplayCount { get; set; }
        public int Attachment { get; set; }
        public int TagCount { get; set; }
        public int PostUserId { get; set; }
        public string PostUsername { get; set; }
        public System.DateTime PostDatetime { get; set; }
        public int LastPostId { get; set; }
        public System.DateTime LastPostDatetime { get; set; }
        public int LastPostUserId { get; set; }
        public string LastPostUsername { get; set; }
        public byte Digest { get; set; }
        public byte Top { get; set; }
        public byte Audit { get; set; }
        public byte Invisible { get; set; }
        public int PostSubTable { get; set; }
        public string HighLight { get; set; }
        public byte Close { get; set; }
        public int FormId { get; set; }
        public byte Ban { get; set; }
        public int LastModId { get; set; }
        public byte Cover { get; set; }
        public decimal Rate { get; set; }
        public string BoardName { get; set; }
    }
}
