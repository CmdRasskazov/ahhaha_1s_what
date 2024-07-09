using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pr_5_sharp
{
    public partial class Form1 : Form
    {
        DataTable myTable;
        public Form1()
        {
            InitializeComponent();
            myTable = companyDataSet.Tables[0];
        }


        void LookAt(IEnumerable<DataRow> value)
        {
            //dataGridView2.DataSource = null;
            //dataGridView2.Columns.Add("Column1", "Имя");
            //dataGridView2.Columns.Add("Column2", "Должность");
            //dataGridView2.Columns.Add("Column3", "ДатаРождения");
            //dataGridView2.Columns.Add("Column4", "Оклад");

            //IEnumerable<DataRow> myQuery = myTable.AsEnumerable();
            //foreach (DataRow tmp in value)
            //{
            //    dataGridView2.Rows.Add(tmp.Field<string>("Имя"),
            //    tmp.Field<string>("Должность"),
            //    tmp.Field<DateTime>("ДатаРождения"),
            //    tmp.Field<int>("Оклад"));
            //}


            var ForLook = value.AsEnumerable().Select(t => new
            {
                Name = t.Field<string>("Имя"),
                Job = t.Field<string>("Должность"),
                DateBirth = t.Field<DateTime>("ДатаРождения"),
                Salary = t.Field<int>("Оклад"),

            }).ToList();
            dataGridView2.DataSource = ForLook;
        }
        public static int ReturnAge(DateTime dateBirth)
        {
            int age = (int)(DateTime.Now - dateBirth).TotalDays / 365;
            return age;
            
        }
        //• средний оклад по конторе;
        //• сотрудника по имени;
        //• сотрудников по должности;
        //• сотрудников пенсионного возраста;
        //• сотрудников, чей оклад выше среднего по конторе;
        //• сотрудников моложе 30-ти лет на указанной должности.

        //Используйте LINQ to DataSet, чтобы:
        //• сортировать список по фамилии;
        //• сортировать список по убыванию оклада;
        //• группировать список по должностям.
        //• найти средний оклад по должностям.
        //сформировать справку об окладах в виде: фамилия – оклад с сортировкой по убыванию

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "companyDataSet.Сотрудники". При необходимости она может быть перемещена или удалена.
            this.сотрудникиTableAdapter.Fill(this.companyDataSet.Сотрудники);

            var MySelectName = myTable.AsEnumerable().Select(t => t.Field<string>("Имя")).Distinct();
            comboBox1.DataSource = MySelectName.ToList();

            var MySelectJob = myTable.AsEnumerable().Select(t => t.Field<string>("Должность")).Distinct();
            comboBox2.DataSource = MySelectJob.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();
            if (listBox1.SelectedItem != null)
            {
                switch(listBox1.SelectedItem.ToString())
                {
                    case "Средний оклад по конторе":
                        
                        var average_salary = myTable.AsEnumerable().Average(t => t.Field<int>("Оклад"));
                        textBox2.Text = Math.Round( average_salary, 0).ToString();
                        break;

                    case "Сотрудника по имени":
                        label1.Text = "Сотрудники по имени: " + comboBox1.Text;
                        IEnumerable<DataRow> query1 =
                            from t in myTable.AsEnumerable()
                            where (t.Field<string>("Имя").Contains(comboBox1.Text))
                            select t;
                        LookAt(query1);
                        break;

                    case "Сотрудников по должности":
                        label1.Text = "Сотрудники по должности: " + comboBox2.Text;
                        IEnumerable<DataRow> query2 =
                            from t in myTable.AsEnumerable()
                            where (t.Field<string>("Должность").Contains(comboBox2.Text))
                            select t;
                        LookAt(query2);
                        break;

                    case "Сотрудников, чей оклад выше среднего по конторе":
                        label1.Text = "Сотрудники, чей оклад выше среднего по конторе: ";
                        double sum = myTable.AsEnumerable().Average(order => order.Field<int>("Оклад"));
                        IEnumerable<DataRow> query3 = (from t in myTable.AsEnumerable()
                                                       where t.Field<int>("Оклад") > sum
                                                       select t);
                        LookAt(query3);
                        break;

                    case "Сотрудников пенсионного возраста":
                        label1.Text = "Сотрудники пенсионного возраста: ";
                        IEnumerable<DataRow> query0 = (from t in myTable.AsEnumerable()
                                                       where ReturnAge( t.Field<DateTime>("ДатаРождения")) > 65
                                                       select t);

                        LookAt(query0);
                        break;

                    case "Сотрудников моложе 30-ти лет на указанной должности":
                        label1.Text = "Сотрудники моложе 30-ти лет на должности:  " + comboBox2.Text;
                        IEnumerable<DataRow> query4 = (from t in myTable.AsEnumerable()
                                                       where t.Field<string>("Должность").Contains(comboBox2.Text) && ReturnAge(t.Field<DateTime>("ДатаРождения")) < 30
                                                       select t);
                        LookAt(query4);
                        break;

                    case "Сортировать список по имени":
                        label1.Text = "Сортировка по имени ";
                        IEnumerable<DataRow> query5 = myTable.AsEnumerable().OrderBy(t => t.Field<string>("Имя"));
                        LookAt(query5);
                        break;

                    case "Сортировать список по убыванию оклада":
                        label1.Text = "Сортирока по убыванию оклада ";
                        IEnumerable<DataRow> query6 = myTable.AsEnumerable().OrderByDescending(t => t.Field<int>("Оклад"));
                        LookAt(query6);
                        break;

                    case "Группировать список по должностям":
                        label1.Text = "Группировка по должностям ";
                        var query7 = myTable.AsEnumerable().GroupBy(t => t.Field<string>("Должность")).Select(g => new
                        {
                            Job = g.Key,
                            Total = g.Count(),
                            Employees = string.Join(", ", g.Select(p => p.Field<string>("Имя")))
                        }).ToList();
                        dataGridView2.DataSource = query7;
                        break;

                    case "Средний оклад по должностям":
                        label1.Text = "Средний оклад по должностям ";
                        var query8 = myTable.AsEnumerable().GroupBy(t => t.Field<string>("Должность")).Select(g => new
                        {
                            Job = g.Key,
                            Total = g.Count(),
                            Avg_Salary = Math.Round(g.Average(t => t.Field<int>("Оклад")))
                        }).ToList();
                        dataGridView2.DataSource = query8;
                        break;

                    case "Справка об окладах":
                        label1.Text = "Справка о доходах ";
                        var query9 = myTable.AsEnumerable().Select(t => new
                        {
                            Name = t.Field<string>("Имя"),
                            Salary = t.Field<int>("Оклад"),
                        }).OrderByDescending(p => p.Salary).ToList();
                        dataGridView2.DataSource = query9;
                        break;

                    default:
                        break;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;

            switch (listBox1.SelectedItem.ToString())
            {
                case "Сотрудника по имени":
                    comboBox1.Enabled = true;
                    break;

                case "Сотрудников моложе 30-ти лет на указанной должности":
                case "Сотрудников по должности":
                    comboBox2.Enabled = true;
                    break;
            }
        }
    }
}
