using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElasticSearchFormApplication
{
    public class CRUDEmployee
    {
        public static DataTable getAllDocument()
        {
            DataTable dataTable = new DataTable("Employee");
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("address", typeof(string));
            dataTable.Columns.Add("MobileNo", typeof(string));
            var res = ConnectionToES.EsClient().Search<Employee>(s => s
            .Index("employee")
            .Type("doc")
            .From(0)
            .Size(1000)
            .Query(q => q.MatchAll()));
            foreach (var hit in res.Hits)
            {
                dataTable.Rows.Add(hit.Id.ToString(), Convert.ToString(hit.Source.name), Convert.ToString(hit.Source.address), Convert.ToString(hit.Source.MobileNo));
            }
            return dataTable;
        }
        public static bool insertEmployee(int ID, string Name, string Address, string MobileNO)
        {
            bool status;
            var myJson = new
            {
                name = Name,
                address = Address,
                MobileNo = MobileNO
            };
            var response = ConnectionToES.EsClient().Index(myJson, i => i.Index("employee").Type("doc").Id(ID));
            if (response.IsValid)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

        public static bool updateEmployee(int ID, string Name, string Address, string MobileNO)
        {
            bool status;
            var response = ConnectionToES.EsClient().Update<DocumentAttributes, UpdateDocumentAttributes>(ID, d => d
            .Index("employee")
            .Type("doc")
            .Doc(new UpdateDocumentAttributes
            {
                name = Name,
                address = Address,
                MobileNo = MobileNO
            }));
            if (response.IsValid)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

        public static bool deleteEmployee(string searchID)
        {
            bool status;
            var response = ConnectionToES.EsClient().Delete<DocumentAttributes>(searchID, d => d
            .Index("employee")
            .Type("doc"));
            if (response.IsValid)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        public static Tuple<string, string, string, string> GetEmployeeByID(string ID)
        {
            string id = "";
            string name = "";
            string address = "";
            string MobileNo = "";
            var response = ConnectionToES.EsClient().Search<DocumentAttributes>(s => s
            .Index("employee")
            .Type("doc")
            .Query(q => q.Term(t => t.Field("_id").Value(ID)))); //Search based on employeeID
            foreach (var hit in response.Hits)
            {
                id = hit.Id.ToString();
                name = Convert.ToString(hit.Source.name);
                address = Convert.ToString(hit.Source.address);
                MobileNo = Convert.ToString(hit.Source.MobileNo);
            }
            return Tuple.Create(id, name, address, MobileNo);
        }

        public static string ValidateEmployee(string name, string address, string MobileNo)
        {
            string msg = string.Empty;
            long ID = 0;
            if (String.IsNullOrWhiteSpace(name))
            {
                msg = "Please enter employee name. \n";
            }
            bool convert = long.TryParse(name, out ID);
            if (convert == true)
            {
                msg = msg + "Please enter a valid employee name.\n";
            }
            if (String.IsNullOrWhiteSpace(address))
            {
                msg = msg + "Please enter employee address.\n";
            }
            bool convert1 = long.TryParse(address, out ID);
            if (convert1 == true)
            {
                msg = msg + "Please enter a valid employee address.\n";
            }
            Regex validator = new Regex("(3|4|5|6|7|8|9){1}[0-9]{9}");
            string match = validator.Match(MobileNo).Value.ToString();
            if (match.Length == 10)
            {
            }
            else
            {
                msg = msg + "Please enter a valid mobile no.";
            }
            return msg;
        }
    }
}
