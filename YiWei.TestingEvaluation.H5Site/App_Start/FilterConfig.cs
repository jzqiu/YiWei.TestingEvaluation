﻿using System.Web;
using System.Web.Mvc;

namespace YiWei.TestingEvaluation.H5Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}