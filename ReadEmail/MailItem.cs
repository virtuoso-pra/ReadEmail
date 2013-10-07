using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Exchange.WebServices.Data;
using System.Text;

namespace ReadEmail
{
    class MailItem
    {
        public string From;
        public string[] Recipients;
        public string Subject;
        public string Body;
        public List<Attachment> attachment;
        public EmailMessage message;
        public string strFileName;
        public int intFileSize;
        public string strFiletype;
        public byte[] byteContent;
    }
}
