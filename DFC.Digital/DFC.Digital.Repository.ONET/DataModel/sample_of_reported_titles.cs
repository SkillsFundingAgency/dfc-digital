//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DFC.Digital.Repository.ONET.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class sample_of_reported_titles
    {
        public string onetsoc_code { get; set; }
        public string reported_job_title { get; set; }
        public string shown_in_my_next_move { get; set; }
    
        public virtual occupation_data occupation_data { get; set; }
    }
}
