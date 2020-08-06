using FastMember;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicTest
{
    public partial class Form1 : Form
    {
        delegate DataTable meDel(string a, char b);
        delegate List<WaterObject> delGetWaterObjList(string flowsID, char exp);
        
        ComboBox chosenCB;
        List<WaterObject> waterObjects;
        public Form1()
        {
            InitializeComponent();
            FillVOCombobox();
            FillPnablCombobox();
            FillMainBassCombobox();
            GetWaterObjList(firstCB.SelectedValue.ToString(), '2');
            listBox1.DisplayMember = "nwo";
            listBox1.ValueMember = "id";
            listBox2.DisplayMember = "nwo";
            listBox2.ValueMember = "id";
        }
        private void button1_Click(object sender, EventArgs e)
        {                       
            List<string> idList = new List<string>();           
            for (int i=0; i< listBox2.Items.Count; i++)
            {
                WaterObject wo = (WaterObject)listBox2.Items[i];
                idList.Add(wo.GetID().ToString());               
            }
           DataGetter dg = new DataGetter();           
            dataGridView1.DataSource =dg.GetFromKPX(idList);
        }

        private void GetWaterObjList(string flowsId, char exp)//Add selected water objects to listbox;
        {
            waterObjects = DataGetter.GetWaterObjList(flowsId, exp);
            foreach (WaterObject waterObject in waterObjects)
            {
                listBox1.Items.Add(new WaterObject {nwo = waterObject.GetName(), id = waterObject.GetID()});
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SelectParameters();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DisableCombobox(comboBox2);
            FillPnablCombobox();
        }
        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            DisableCombobox(comboBox3);
        }
        private void SelectParameters()//Select water object from databse by it`s name and region name; If any of parameters is not set, get all water object;
        {
            string vo = comboBox2.SelectedValue.ToString();
            string pnabl = comboBox3.SelectedValue.ToString();
            if (checkBox1.Checked)
            {
                vo = "%";
            }
            if (checkBox2.Checked)
            {
                pnabl = "%";
            }
            string query = "select BAS, TRIM(VO), PNABL, PRIV1 from KPH2012.KPH_NEW where VO like'" + vo + "' and Pnabl like'" + pnabl + "'";
            DataGetter dg = new DataGetter();
            dataGridView1.DataSource = dg.CreateDataTable(query);
        }

        private void FillMainBassCombobox()//Select all availble river basin names;
        {
            firstCB.DataSource = DataGetter.GetWaterObjWithAnyID();
            firstCB.DisplayMember = "NWO";
            firstCB.ValueMember = "ID";
            firstCB.BindingContext = this.BindingContext;
        }
        
        private void FillSubBassCombobox(ComboBox cb, delGetWaterObjList a, string id, char exp)//Select all availble river basin, belongs to main river basin;
        {
            DataGetter dg = new DataGetter();

            List<WaterObject> woList=a(id, exp);
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(woList))
            {
                table.Load(reader);
            }
            cb.DataSource = table;
            cb.DisplayMember = "NWO";
            cb.ValueMember = "ID";
            cb.BindingContext = this.BindingContext;
        }

        private void FillVOCombobox()//Select avalible water object names  
        {
            string pnabl = "%";
            DataGetter dg = new DataGetter();
            comboBox2.DataSource = dg.GetAllWaterObj().Tables["WaterObj"];
            comboBox2.DisplayMember = "VO";
            comboBox2.ValueMember = "VO";
            comboBox2.BindingContext = this.BindingContext;
        }
        private void FillPnablCombobox()//Select avalible region names  
        {
            string vo = comboBox2.SelectedValue.ToString();
            if (checkBox1.Checked)
            {
                vo = "%";
            }
            DataGetter dg = new DataGetter();
            comboBox3.DataSource = dg.GetObservationPoint(vo).Tables["Pnabl1"];
            comboBox3.DisplayMember = "Pnabl";
            comboBox3.ValueMember = "Pnabl";
            comboBox3.BindingContext = this.BindingContext;
        }

        private void DisableCombobox(ComboBox comboBox)
        {
            if (comboBox.Enabled)
            {
                comboBox.Enabled = false;
            }
            else
            {
                comboBox.Enabled = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPnablCombobox();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)//1st
        {
            chosenCB = firstCB;
            ClearCB(secondCB);
            if ((firstCB.SelectedValue != null) && firstCB.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                ClearChosenWaterObjs();
                GetWaterObjList(firstCB.SelectedValue.ToString(), '2');
            }
        }

        private void ClearChosenWaterObjs()
        {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
        }

        private void comboBox4_DropDown(object sender, EventArgs e)//2
        {
            FillSubBassCombobox(secondCB, DataGetter.GetWaterObjList, firstCB.SelectedValue.ToString(), '2');
        }

        private void comboBox5_DropDown(object sender, EventArgs e)//3
        {
            if (secondCB.SelectedValue != null)
            {
                FillSubBassCombobox(thirdCB, DataGetter.GetWaterObjList, secondCB.SelectedValue.ToString(), '3');
                ClearChosenWaterObjs();
                GetWaterObjList(firstCB.SelectedValue.ToString(), '3');
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)//2
        {
            ClearCB(thirdCB);
            chosenCB = secondCB;
            if ((secondCB.SelectedValue != null) && (secondCB.SelectedValue.ToString() != "System.Data.DataRowView"))
            {
                ClearChosenWaterObjs();
                GetWaterObjList(secondCB.SelectedValue.ToString(), '3');
            }
        }

        private void ClearCB(ComboBox cb)
        {
            cb.DataSource = null;
            cb.Items.Clear();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)//3
        {
            ClearCB(fourthCB);
            chosenCB = thirdCB;
            if ((thirdCB.SelectedValue != null) && thirdCB.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                ClearChosenWaterObjs();
                GetWaterObjList(thirdCB.SelectedValue.ToString(), '4');
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (thirdCB.SelectedValue != null)
                FillSubBassCombobox(fourthCB, DataGetter.GetWaterObjList, thirdCB.SelectedValue.ToString(), '4');
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//4
        {
            chosenCB = fourthCB;
            ClearCB(fifthCB);
            if ((fourthCB.SelectedValue != null) && fourthCB.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                ClearChosenWaterObjs();
                GetWaterObjList(fourthCB.SelectedValue.ToString(), '5');
            }
        }

        private void button3_Click(object sender, EventArgs e)//Add to list
        {
            if (listBox1.Items.Count > 0)
            {
                if (listBox1.SelectedItem != null)
                {
                    listBox2.Items.Add(listBox1.SelectedItem);
                    listBox1.Items.Remove(listBox1.SelectedItem);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)//Remove from list
        {
            if (listBox2.SelectedItem != null)
            {
                listBox1.Items.Add(listBox2.SelectedItem);               
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }
    }
}