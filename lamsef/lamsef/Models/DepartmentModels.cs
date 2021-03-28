using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lamsef.Models
{
    public class DepartmentModels
    {
        public List<M_Department> DepartmentList { get; set; }
        public List<SearchEmployee> SearchResult { get; set; }
        public bool checkInputflag { get; set; } //Check input is valid or not valid. Return false if not valid
        public bool checkCodeflag { get; set; } // Check  Company Employee Code in database.

        public int SelectDepartmentId { get; set; }
    }

    public class M_Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class SearchEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyEmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public string RegionName { get; set; }

    }
}