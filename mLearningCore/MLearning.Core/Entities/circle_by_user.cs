//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MLearningDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class circle_by_user
    {
		[PrimaryKey, AutoIncrement]
		public int id_pk { get; set;}
        public int User_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public int owner_id { get; set; }
        public System.DateTime created_at { get; set; }
        public System.DateTime updated_at { get; set; }
    }
}
