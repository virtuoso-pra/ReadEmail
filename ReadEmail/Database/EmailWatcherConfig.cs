//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReadEmail.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailWatcherConfig
    {
        public int Id { get; set; }
        public string targetEmailId { get; set; }
        public string password { get; set; }
        public string serverName { get; set; }
        public int refreshRateInSeconds { get; set; }
        public string targetFolder { get; set; }
        public int retainPeriodInDays { get; set; }
    }
}
