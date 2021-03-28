using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace lamsef.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index(string button, Models.DepartmentModels ReadDepartmentModel, string SearchEmployeeName)
        {
            Models.DepartmentModels DepartmentModel = new Models.DepartmentModels();
            List<Models.M_Department> DepartmentList = new List<Models.M_Department>();

            DepartmentList = GetDepartmentList(DepartmentModel);

            if(button == "Search")
            {
                DepartmentModel.SearchResult = SearchResult(ReadDepartmentModel.SelectDepartmentId, SearchEmployeeName);
            }
            return View(DepartmentModel);
        }

        public ActionResult NewEmployee(string button, Models.DepartmentModels ReadDepartmentModel ,string NewEmployeeName, string NewCompanyEmployeeCode)
        {
            Models.DepartmentModels DepartmentModel = new Models.DepartmentModels();
            List<Models.M_Department> DepartmentList = new List<Models.M_Department>();


            DepartmentModel.checkCodeflag = true;
            DepartmentModel.checkInputflag = true;

            if (button == "Register")
            {
                if(ReadDepartmentModel.SelectDepartmentId != 0 && NewEmployeeName != "" && NewCompanyEmployeeCode !="")
                {
                    DepartmentModel.checkCodeflag = checkCompanyEmployeeCode(NewCompanyEmployeeCode);
                    if(DepartmentModel.checkCodeflag == true)
                    {
                        int RegisterId = GetRegisterNumber() + 1;
                        RegisterEmployee(RegisterId, ReadDepartmentModel.SelectDepartmentId, NewEmployeeName, NewCompanyEmployeeCode);
                        return RedirectToAction("Index", "Department");
                    }
                }
                else
                {
                    DepartmentModel.checkInputflag = false;
                }
            }

            DepartmentList = GetDepartmentList(DepartmentModel);
            return View(DepartmentModel);
        }

        public List<Models.M_Department> GetDepartmentList(Models.DepartmentModels DepartmentModel)
        {
            Modules.DBModules DB = new Modules.DBModules();
            Models.M_Department Department = new Models.M_Department();
            List<Models.M_Department> DepartmentList = new List<Models.M_Department>();
            SqlDataReader sqlRdr = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM M_Department");
            try
            {
                if (!DB.DB_Connect())
                {

                }
                if(DB.DB_SqlReader(strSql.ToString(), ref sqlRdr))
                {

                }
                while (sqlRdr.Read())
                {
                    Department = new Models.M_Department();

                    Department.DepartmentId = int.Parse(sqlRdr["DepartmentId"].ToString());
                    Department.DepartmentName = sqlRdr["DepartmentName"].ToString();

                    DepartmentList.Add(Department);
                }
                DepartmentModel.DepartmentList = DepartmentList;
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if(sqlRdr != null)
                {
                    if(!sqlRdr.IsClosed)
                    {
                        sqlRdr.Close();
                    }
                }
                DB.DB_Close();
            }
            return DepartmentList;
        }

        public List<Models.SearchEmployee> SearchResult(int SelectDepartmentId, string SearchEmployeeName)
        {
            bool sqlFlag = false;
            Modules.DBModules DB = new Modules.DBModules();
            Models.SearchEmployee SearchEmployee = new Models.SearchEmployee();
            List<Models.SearchEmployee> SearchEmployeeList = new List<Models.SearchEmployee>();
            SqlDataReader sqlRdr = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT M_Employee.EmployeeName, M_Employee.CompanyEmployeeCode, M_Department.DepartmentName, M_Department.RegionName");
            strSql.Append(" FROM M_Department");
            strSql.Append(" INNER JOIN M_Employee ON M_Department.DepartmentId=M_Employee.DepartmentId");

            // When the user select Department in Search condition.
            if (SelectDepartmentId != 0)
            {
                strSql.Append(" WHERE");
                strSql.Append(" M_Employee.DepartmentId = ");
                strSql.AppendFormat("{0}", SelectDepartmentId);
                sqlFlag = true;
            }

            if (SearchEmployeeName != "")
            {
                if(sqlFlag == true)
                {
                    strSql.Append(" AND");
                }
                if(SelectDepartmentId == 0)
                {
                    strSql.Append(" WHERE");
                }
                strSql.Append(" M_Employee.EmployeeName = ");
                strSql.AppendFormat("'{0}'", SearchEmployeeName);
            }
            try
            {
                if (!DB.DB_Connect())
                {

                }
                if (DB.DB_SqlReader(strSql.ToString(), ref sqlRdr))
                {

                }
                while (sqlRdr.Read())
                {
                    SearchEmployee = new Models.SearchEmployee();

                    SearchEmployee.EmployeeName = sqlRdr["EmployeeName"].ToString();

                    SearchEmployee.CompanyEmployeeCode = sqlRdr["CompanyEmployeeCode"].ToString();

                    SearchEmployee.DepartmentName = sqlRdr["DepartmentName"].ToString();

                    SearchEmployee.RegionName = sqlRdr["RegionName"].ToString();

                    SearchEmployeeList.Add(SearchEmployee);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sqlRdr != null)
                {
                    if (!sqlRdr.IsClosed)
                    {
                        sqlRdr.Close();
                    }
                }
                DB.DB_Close();
            }
            return SearchEmployeeList;
        }

        // Returns "False" If a Company Employee Code already exist otherwise return "True".
        public bool checkCompanyEmployeeCode(string NewCompanyEmployeeCode)
        {
            {
                Modules.DBModules DB = new Modules.DBModules();
                Models.SearchEmployee SearchEmployee = new Models.SearchEmployee();
                SqlDataReader sqlRdr = null;
                StringBuilder strSql = new StringBuilder();

                strSql.Append("SELECT * FROM M_Employee WHERE ");
                strSql.Append("CompanyEmployeeCode = ");
                strSql.AppendFormat("'{0}'", NewCompanyEmployeeCode);
                try
                {
                    if (!DB.DB_Connect())
                    {

                    }
                    if (DB.DB_SqlReader(strSql.ToString(), ref sqlRdr))
                    {

                    }
                    while (sqlRdr.Read())
                    {
                        SearchEmployee = new Models.SearchEmployee();

                        SearchEmployee.CompanyEmployeeCode = sqlRdr["CompanyEmployeeCode"].ToString();
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (sqlRdr != null)
                    {
                        if (!sqlRdr.IsClosed)
                        {
                            sqlRdr.Close();
                        }
                    }
                    DB.DB_Close();
                }
                if (SearchEmployee.CompanyEmployeeCode == null)
                {
                    return true;
                }
                return false;
            }
        }

        //Get the ID number in the database to register information for new employees.
        public int GetRegisterNumber()
        {
            Modules.DBModules DB = new Modules.DBModules();
            Models.SearchEmployee SearchEmployee = new Models.SearchEmployee();
            SqlDataReader sqlRdr = null;
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT EmployeeId FROM M_Employee");
            try
            {
                if (!DB.DB_Connect())
                {

                }
                if (DB.DB_SqlReader(strSql.ToString(), ref sqlRdr))
                {

                }
                while (sqlRdr.Read())
                {
                    SearchEmployee = new Models.SearchEmployee();

                    SearchEmployee.EmployeeId = int.Parse(sqlRdr["EmployeeId"].ToString());
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sqlRdr != null)
                {
                    if (!sqlRdr.IsClosed)
                    {
                        sqlRdr.Close();
                    }
                }
                DB.DB_Close();
            }
            return SearchEmployee.EmployeeId;
        }

        // Register new employee into the database
        public bool RegisterEmployee(int RegisterId, int SelectDepartmentId, string NewEmployeeName, string NewCompanyEmployeeCode)
        {
            Modules.DBModules DB = new Modules.DBModules();

            SqlDataReader sqlRdr = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO M_Employee (EmployeeId, EmployeeName, CompanyEmployeeCode, DepartmentId, IsDeleted, CreateBy, CreateOn,  UpdateBy, UpdateOn)");
            strSql.Append(" VALUES (");
            strSql.AppendFormat("'{0}'", RegisterId);
            strSql.Append(",");
            strSql.AppendFormat(" '{0}'", NewEmployeeName);
            strSql.Append(",");
            strSql.AppendFormat(" '{0}'", NewCompanyEmployeeCode);
            strSql.Append(",");
            strSql.AppendFormat(" '{0}'", SelectDepartmentId);
            strSql.Append(", '0'");
            strSql.Append(", 'Hai TV'");
            strSql.Append(", ");
            strSql.Append("'");
            strSql.Append(DateTime.Now.ToString());
            strSql.Append("'");
            strSql.Append(", 'Hai TV'");
            strSql.Append(", ");
            strSql.Append("'");
            strSql.Append(DateTime.Now.ToString());
            strSql.Append("')");

            try
            {
                if (!DB.DB_Connect()) { }
                if (DB.DB_SqlReader(strSql.ToString(), ref sqlRdr)) { }
                while (sqlRdr.Read()){ }
            }
            catch (Exception ex) {}
            finally
            {
                if (sqlRdr != null)
                {
                    if (!sqlRdr.IsClosed)
                    {
                        sqlRdr.Close();
                    }
                }
                DB.DB_Close();
            }
            return true;
        }
    }
}