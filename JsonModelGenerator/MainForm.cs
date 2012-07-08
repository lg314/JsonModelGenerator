using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
namespace JsonModelGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    getJsonClient<AddRelationJsonClient>(@"C:\Users\Administrator\SkyDrive\OtransModel");
        //    getJsonClient<ComfirmTrustJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<LogChallengeJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<LogInJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<LogOutJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<RegisterJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<RemoveRelationJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<TrustRemoveJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<TrustRequestJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<UpdateLocationJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //    getJsonClient<ViewLocationJsonClient>(@"c:\users\administrator\skydrive\otransmodel");
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    getJsonServer<GetRelationJsonServer>(@"C:\Users\Administrator\SkyDrive\OtransModel");
        //    getJsonServer<LogChallengeJsonServer>(@"C:\Users\Administrator\SkyDrive\OtransModel");
        //    getJsonServer<MemberJsonServer>(@"C:\Users\Administrator\SkyDrive\OtransModel");
        //    getJsonServer<MyTrustRequestDetailJsonServer>(@"C:\Users\Administrator\SkyDrive\OtransModel");
        //    getJsonServer<MyTrustRequestJsonServer>(@"C:\Users\Administrator\SkyDrive\OtransModel");
        //    getJsonServer<ViewLocationJsonServer>(@"C:\Users\Administrator\SkyDrive\OtransModel");
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            CodeParameter cgp = new CodeParameter()
            {
                ServerFunction = "getObjectWithData",
                ClientFunction = "jsonData",
                Folder = @"c:\Model",
                Prefix = "OTrans",
                ClientProtocal = "JsonClient",
                ServerProtocal = "JsonServer"
            };
            CodeFactory.Generate(cgp, typeof(TestModel), JsonModelType.ServerClient);

        }

    }

    public class CheckItem
    {
        public JsonModelType JsonModelType { get; set; }
        public Type Type { get; set; }
        public override string ToString()
        {
            return Type.ToString();
        }
    }


}
