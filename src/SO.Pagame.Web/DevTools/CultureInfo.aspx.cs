﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SO.DevTools
{
    public partial class CultureInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Request.IsLocal)
            {
                Response.Status = "403 Forbidden";
                Response.End();
            }
        }
    }
}