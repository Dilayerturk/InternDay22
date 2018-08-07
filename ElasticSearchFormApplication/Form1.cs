using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElasticSearchFormApplication
{
    public partial class elasticSearchForm : MetroFramework.Forms.MetroForm
    {
        public elasticSearchForm()
        {
            InitializeComponent();
        }

        string documentID = "";
        int id;
        string name = "";
        string address = "";
        string MobileNo = "";

        private void btnGetData_Click(object sender, EventArgs e)
        {
            DataTable dt = CRUDEmployee.getAllDocument();
            gridView.DataSource = dt;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            bool status = false;
            long ID = 0;
            bool convert = long.TryParse(txtId.Text, out ID);
            if (convert == true)
            {
                string msg = CRUDEmployee.ValidateEmployee(txtName.Text, txtAddress.Text, txtNumber.Text);
                if (msg == string.Empty)
                {
                    id = Convert.ToInt32(txtId.Text);
                    name = txtName.Text;
                    address = txtAddress.Text;
                    MobileNo = txtNumber.Text;
                    status = CRUDEmployee.insertEmployee(id, name, address, MobileNo);
                    if (status == true)
                    {
                        MessageBox.Show("Document Inserted/ Indexed Successfully");
                        btnGetData_Click(sender, e);
                        txtId.Text = "";
                        txtName.Text = "";
                        txtAddress.Text = "";
                        txtNumber.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Error! Occured During Document Insert/ Index!");
                    }
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid employee ID.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
                bool status;
                long ID = 0;
                bool convert = long.TryParse(txtId.Text, out ID);
                if (convert == true)
                {
                    id = Convert.ToInt32(txtId.Text);
                    name = txtName.Text;
                    address = txtAddress.Text;
                    MobileNo = txtNumber.Text;
                    status = CRUDEmployee.updateEmployee(id, name, address, MobileNo);
                    if (status == true)
                    {
                        MessageBox.Show("Document Updated Successfully");
                        btnGetData_Click(sender, e);
                        txtId.Text = "";
                        txtName.Text = "";
                        txtAddress.Text = "";
                        txtNumber.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Error! Occured During Document Update!");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid employee ID.");
                }
            }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool status;
            documentID = txtId.Text;
            long ID = 0;
            bool convert = long.TryParse(documentID, out ID);
            if (convert == true)
            {
                status = CRUDEmployee.deleteEmployee(ID.ToString());
                if (status == true)
                {
                    MessageBox.Show("Document Deleted Successfully");
                    btnGetData_Click(sender, e);
                    txtId.Text = "";
                }
                else
                {
                    MessageBox.Show("Error Occured During Document Delete!");
                }
            }
            else
            {
                MessageBox.Show("Please enter a employee id.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            long ID = 0;
            bool status = long.TryParse(txtSearch.Text, out ID);
            if (status == true)
            {
                var empItem = CRUDEmployee.GetEmployeeByID(ID.ToString());
                txtId.Text = empItem.Item1;
                txtName.Text = empItem.Item2;
                txtAddress.Text = empItem.Item3;
                txtNumber.Text = empItem.Item4;
            }
            else
            {
                MessageBox.Show("Please enter a valid employee ID.");
            }
        }
    }
    
    
}
