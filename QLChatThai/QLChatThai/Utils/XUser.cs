using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class XUser
{
    public static bool IsAuthenticated
    {
        get
        {
            var user = HttpContext.Current.Session["user"];
            return user != null;
        }
    }
}