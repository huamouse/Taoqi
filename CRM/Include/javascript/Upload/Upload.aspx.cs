using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Taoqi;
using System.Web.Configuration;

public partial class Upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string virtuURL = WebConfigurationManager.AppSettings["UploadPath_image"].Replace("~", Request.ApplicationPath);
        string basePath = Server.MapPath(virtuURL);
        string name = string.Empty;
        HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        string _tbName = System.Web.HttpContext.Current.Request["tbName"];
        string _tbFields = System.Web.HttpContext.Current.Request["tbFields"];
        string _ID = System.Web.HttpContext.Current.Request["tbId"];

        if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);

        if (files != null)
        {
            for (int i = 0; i < files.Count; i++)
            {
                string strFileName = Guid.NewGuid().ToString() + ".png";
                //将记录保存到数据库中
                if (_tbName == "Users")
                    DAL.UpdateTableByColumn(_tbName, _tbFields, strFileName, "ID", Security.USER_ID.ToString());
                else if (_tbName == "TQClient")
                    DAL.UpdateTableByColumn(_tbName, _tbFields, strFileName, "ID", Security.UserClientID);
                else if (_tbName == "TQMarketInformation")
                    DAL.UpdateTableByColumn(_tbName, _tbFields, strFileName, "ID", _ID);
                files[i].SaveAs(basePath + strFileName);
            }
        }
    }
}