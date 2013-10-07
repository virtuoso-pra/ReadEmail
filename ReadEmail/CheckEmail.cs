using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Configuration;
using System.ServiceProcess;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.Office.Interop.Excel;
using System.Text;

namespace ReadEmail
{
    public partial class CheckEmail : ServiceBase
    {
        System.Timers.Timer timer;
        public CheckEmail()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // CheckMail();
            timer = new System.Timers.Timer();
            timer.Interval = 60000D;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            timer.Start();
        }

        public void _CheckMail()
        {
            CheckMail();
            timer = new System.Timers.Timer();
            timer.Interval = 60000D;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            timer.Start();
        }

        protected void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckMail();
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }

        public void CheckMail()
        {
            StreamWriter st = new StreamWriter("C:\\Test\\_testings.txt");
            string emailId = "encoredev@virtuoso.com";// ConfigurationManager.AppSettings["UserName"].ToString();
            if (emailId != string.Empty)
            {
                st.WriteLine(DateTime.Now + " " + emailId);
                st.Close();
                ExchangeService service = new ExchangeService();
                service.Credentials = new WebCredentials(emailId, "Sea2013");
                service.UseDefaultCredentials = false;
                service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
                Folder inbox = Folder.Bind(service, WellKnownFolderName.Inbox);
                SearchFilter sf = new SearchFilter.SearchFilterCollection(LogicalOperator.And, new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
                if (inbox.UnreadCount > 0)
                {
                    ItemView view = new ItemView(inbox.UnreadCount);
                    FindItemsResults<Item> findResults = inbox.FindItems(sf, view);
                    PropertySet itempropertyset = new PropertySet(BasePropertySet.FirstClassProperties, EmailMessageSchema.From, EmailMessageSchema.ToRecipients);
                    itempropertyset.RequestedBodyType = BodyType.Text;
                    //itemview.PropertySet = itempropertyset;
                    //inbox.UnreadCount
                    ServiceResponseCollection<GetItemResponse> items = service.BindToItems(findResults.Select(item => item.Id), itempropertyset);
                    MailItem[] msit = getMailItem(items, service);

                    foreach (MailItem item in msit)
                    {
                        item.message.IsRead = true;
                        item.message.Update(ConflictResolutionMode.AlwaysOverwrite);
                        foreach (Attachment attachment in item.attachment)
                        {
                            if (attachment is FileAttachment)
                            {

                                FileAttachment fileAttachment = attachment as FileAttachment;

                                //fileAttachment.Load(@"C:\\Test\\" + fileAttachment.Name);'
                                FileStream theStream = new FileStream("C:\\Test\\" + fileAttachment.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                fileAttachment.Load(theStream);
                                byte[] fileContents;
                                MemoryStream memStream = new MemoryStream();
                                theStream.CopyTo(memStream);
                                fileContents = memStream.GetBuffer();
                                theStream.Close();
                                theStream.Dispose();
                                Console.WriteLine("Attachment name: " + fileAttachment.Name + fileAttachment.Content + fileAttachment.ContentType + fileAttachment.Size);
                            }
                        }
                    }                   
                }
                DeleteMail();
            }
        }

        public void DeleteMail()
        {
            ExchangeService service = new ExchangeService();
            string emailId = "encoredev@virtuoso.com"; //  string filesPath5 = ConfigurationManager.AppSettings["UserName"].ToString();
            if (emailId != string.Empty)
            {
                service.Credentials = new WebCredentials(emailId, "Sea2013");
                service.UseDefaultCredentials = false;
                service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
                DateTime searchdate = DateTime.Now;
                searchdate = searchdate.AddDays(-4);

                SearchFilter greaterthanfilter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, searchdate);
                SearchFilter lessthanfilter = new SearchFilter.IsLessThan(ItemSchema.DateTimeReceived, searchdate);
                SearchFilter filter = new SearchFilter.SearchFilterCollection(LogicalOperator.Or, lessthanfilter);
                Folder folder = Folder.Bind(service, WellKnownFolderName.Inbox);              
                //Or the folder you want to search in
                FindItemsResults<Item> results = folder.FindItems(filter, new ItemView(folder.TotalCount));
                foreach (Item i in results.Items)
                {
                    i.Delete(DeleteMode.MoveToDeletedItems);
                }
            }

        }

        private MailItem[] getMailItem(ServiceResponseCollection<GetItemResponse> items, ExchangeService service)
        {
            return items.Select(item =>
            {
                return new MailItem()
                {
                    message = EmailMessage.Bind(service, item.Item.Id, new PropertySet(BasePropertySet.IdOnly, ItemSchema.Attachments, ItemSchema.HasAttachments)),
                    From = ((Microsoft.Exchange.WebServices.Data.EmailAddress)item.Item[EmailMessageSchema.From]).Address,
                    Recipients = ((Microsoft.Exchange.WebServices.Data.EmailAddressCollection)item.Item[EmailMessageSchema.ToRecipients]).Select(recipient => recipient.Address).ToArray(),
                    Subject = item.Item.Subject,
                    Body = item.Item.Body.ToString(),
                    attachment = item.Item.Attachments.ToList(),
                };
            }).ToArray();
        }

    }
}
