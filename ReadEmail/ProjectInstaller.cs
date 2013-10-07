using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.IO;
using System.Configuration;
using System.Linq;


namespace ReadEmail
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string filesPath = Context.Parameters["UserName"];
            string filesPath2 = Context.Parameters["Pwd"];
            string filesPath3 = Context.Parameters["Domain"];
            string filesPath4 = Context.Parameters["Days"];
            string targetDirectory = Context.Parameters["targetdir"];
            string exePath = string.Format("{0}ReadEmail.exe", targetDirectory);

            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);

            config.AppSettings.Settings["UserName"].Value = filesPath;
            config.AppSettings.Settings["Password"].Value = filesPath2;
            config.AppSettings.Settings["Days"].Value = filesPath3;
            config.AppSettings.Settings["Domain"].Value = filesPath4;
            config.Save();
        }


        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            new ServiceController(serviceInstaller1.ServiceName).Start();
            string filesPath = Context.Parameters["UserName"];
            string filesPath2 = Context.Parameters["Pwd"];
            string filesPath3 = Context.Parameters["Domain"];
            string filesPath4 = Context.Parameters["Days"];
            string targetDirectory = Context.Parameters["targetdir"];
            StreamWriter st = new StreamWriter("C:\\Test\\test.txt");
            st.Write(filesPath);
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            st.WriteLine(filesPath2);
            st.WriteLine(targetDirectory);
            st.Close();
            //Properties.Settings.Default.UserName = filesPath;
            //Properties.Settings.Default.Password = filesPath2;
            //Properties.Settings.Default.Days = Convert.ToInt16(filesPath3);
            //Properties.Settings.Default.Domain = filesPath4;
            //Properties.Settings.Default.Save();
            //KeyValueConfigurationCollection settings = config.AppSettings.Settings;
            //settings["UserName"].Value = filesPath;
            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            StreamWriter st1 = new StreamWriter("C:\\Test\\test2.txt");
            st1.WriteLine(filesPath3);
            st1.WriteLine(filesPath4);
            st1.Close();          
        }

        private void ProjectInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
