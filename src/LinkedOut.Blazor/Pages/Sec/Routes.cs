using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedOut.Blazor.Pages.Sec
{
    public static class Routes
    {

        public static string _jobSearchIdToken = "{jobSearchId:int}";
        public static readonly string JobSearch = "";

        public static string MakeJobSearchUrl(int id)
        {
            return $"/sec/jobsearch/{id}";
        }


    }

    public class Route
    {

    }

    public class RouteToken<T>
    {

    }
}
