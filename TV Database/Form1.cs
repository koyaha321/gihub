using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace TV_Database
{
    public partial class Form1 : Form
    {
        private string path;
        private List<Panel> panels = new List<Panel>(4);
        OleDbConnection connection;
        OleDbDataReader reader;
        string regAdress = "reg.on.tv.adr@gmail.com";
        string helpAdress = "help.on.tv.adr@gmail.com";
        string language = "rus";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            this.usersTableAdapter.Fill(this.databaseDataSet.Users);
            dataGridView2.DataSource = databaseDataSet.Tariffs;
            path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "\\Database.accdb";
            connection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Persist Security Info=False;");
            panels.Add(panel1);
            panels.Add(panel2);
            panels.Add(panel3);
            panels.Add(panel4);
            panels.Add(panel5);
            panels.Add(panel6);
            ShowPanel(1);
            label5.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ShowPanel(2);     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowPanel(4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            connection.Open();
            string login = textBox1.Text;
            string password = textBox2.Text;
            OleDbCommand findUser = new OleDbCommand($"SELECT User_login FROM Users WHERE User_login =\"{login}\";", connection);
            reader = findUser.ExecuteReader();
            if(reader.HasRows)
            {
                OleDbCommand checkPassword = new OleDbCommand($"SELECT User_password FROM Users WHERE User_login = \"{login}\";", connection);
                reader = checkPassword.ExecuteReader();               
                while (reader.Read())
                {
                    string checkedPassword = reader.GetValue(0).ToString();
                    if(checkedPassword == password)
                    {
                        string tariff = "";
                        string leftDays = "";
                        OleDbDataReader dataReader;
                        OleDbCommand getTariff = new OleDbCommand($"SELECT User_tariff FROM Users WHERE User_login = \"{login}\";", connection);
                        dataReader = getTariff.ExecuteReader();
                        while(dataReader.Read())
                        {
                            tariff = dataReader.GetValue(0).ToString();
                        }
                        OleDbCommand getDays = new OleDbCommand($"SELECT User_left_days FROM Users WHERE User_login = \"{login}\";", connection);
                        dataReader = getDays.ExecuteReader();
                        while (dataReader.Read())
                        {
                            leftDays= dataReader.GetValue(0).ToString();
                        }
                        textBox1.Text = string.Empty;
                        textBox2.Text = string.Empty;
                        label25.Text = "Логин: " + login;
                        label26.Text = "Пароль: " + password;
                        label27.Text = "Тариф: " + tariff;
                        label28.Text = "Осталось дней: " + leftDays;
                        label29.Text = login;
                        ShowPanel(6);
                        this.tariffsTableAdapter1.Fill(this.databaseDataSet.Tariffs);
                        label5.Visible = false;
                    }
                    else
                    {
                        label5.Visible = true;
                    }
                }
                
            }
            else
            {
                label5.Visible = true;
            }
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowPanel(3);
        }

        private void AddUser(string login,string password,string name,string surname)
        {

            MailAddress from = new MailAddress("reg.on.tv.adr@gmail.com", login);
            MailAddress to = new MailAddress("reg.on.tv.adr@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "New Registration";
            message.Body = $"Имя - {textBox3.Text}\n Фамилия - {textBox4.Text} \n Логин - {textBox5.Text} \n Пароль - {textBox6.Text}";
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("reg.on.tv.adr@gmail.com", "Qwerty123.");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(message);
            message.Dispose();
            /*   OleDbCommand findUser = new OleDbCommand($"SELECT User_login FROM Users WHERE User_login = \"{login}\";", connection);


               reader = findUser.ExecuteReader();
               if (reader.HasRows)
               {
                   label13.Text = "Пользователь с таким логином уже зарегистрирован";
               }
               else
               {

                     OleDbCommand addUser = new OleDbCommand($"INSERT INTO Users (User_name,User_surname,User_tariff,User_left_days,User_login,User_password) VALUES (\"{name}\",\"{surname}\",\"111\",2,\"{login}\",\"{password}\");", connection);
                     reader = addUser.ExecuteReader();

                     label13.Text = "Вы успешно зарегистрировались";
               }

               connection.Close();*/
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddUser(textBox5.Text,textBox6.Text,textBox3.Text,textBox4.Text);
        }
        private void ShowPanel(int number)
        {
            
            for(int i=0;i<panels.Count;i++)
            {
                if (i < number)
                {
                    panels[i].Show();
                }
                else
                {
                    panels[i].Hide();
                }
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox7.Text != string.Empty && textBox8.Text != string.Empty)
            {
                string login = textBox7.Text;
                string password = textBox8.Text;
                connection.Open();
                OleDbCommand checkAdmin = new OleDbCommand($"SELECT Admin_login FROM Admin WHERE Admin_login =\"{login}\";", connection);
                reader = checkAdmin.ExecuteReader();
                if(reader.HasRows)
                {
                    OleDbCommand checkPassword = new OleDbCommand($"SELECT Admin_password FROM Admin WHERE Admin_login = \"{login}\";", connection);
                    reader = checkPassword.ExecuteReader();
                    while(reader.Read())
                    {
                        if(password == reader.GetString(0))
                        {                          
                            ShowPanel(5);
                        }

                    }
                }
               
                connection.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
               
              connection.Open();
              string id = dataGridView1.SelectedCells[0].Value.ToString();
              OleDbCommand deleteUser = new OleDbCommand($"DELETE FROM Users WHERE ID={id};", connection);
              reader = deleteUser.ExecuteReader();           
              connection.Close();
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
            dataGridView1.Update();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

            connection.Open();
            string name = textBox9.Text;
            string surname = textBox10.Text;
            string tariff = comboBox1.Text;
            int days = Int32.Parse(textBox11.Text);
            string login = textBox12.Text;
            string password = textBox13.Text;
            OleDbCommand checkUser = new OleDbCommand($"SELECT User_login FROM Users WHERE User_login = \"{login}\";", connection);
            reader = checkUser.ExecuteReader();
            if (reader.HasRows)
            {
                label23.Visible = true;
            }
            else
            {

                OleDbDataAdapter adapter = new OleDbDataAdapter($"INSERT INTO Users (User_name,User_surname,User_tariff,User_left_days,User_login,User_password) VALUES" +
                    $" (\"{name}\",\"{surname}\",\"{tariff}\",{days},\"{login}\",\"{password}\");", connection);
                adapter.SelectCommand.ExecuteNonQuery();
                connection.Close();
                connection.Open();
                adapter = new OleDbDataAdapter("SELECT * FROM Users;", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.Refresh();

                MailAddress to = new MailAddress(login);
                MailAddress from = new MailAddress(regAdress);
                MailMessage message = new MailMessage(from, to);
                message.Body = $"Ваша заявка на регистрацию одобрена\n Ваш email для входа : {login}\n Ваш пароль для входа : {password}";
                message.Subject = "Ваша заявка на регистрацию одобрена!";
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("reg.on.tv.adr@gmail.com", "Qwerty123.");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);
                message.Dispose();
                label23.Visible = false;
            }
            connection.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                textBox14.Text = item.Cells[1].Value.ToString();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                textBox14.Text = item.Cells[1].Value.ToString();
                textBox15.Text = item.Cells[2].Value.ToString();
                textBox16.Text = item.Cells[4].Value.ToString();
                textBox17.Text = item.Cells[5].Value.ToString();
                textBox18.Text = item.Cells[6].Value.ToString();
                comboBox2.Text = item.Cells[3].Value.ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int id=0;
            string text = "";
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                id = Int32.Parse(item.Cells[0].Value.ToString());
            }
                if (textBox14.Text!=string.Empty&&textBox15.Text!=string.Empty&&textBox16.Text!=string.Empty&&textBox17.Text!=string.Empty&&textBox18.Text!=string.Empty)
            {
                connection.Open();
                OleDbCommand checkLogin = new OleDbCommand($"SELECT ID FROM Users WHERE User_login = \"{textBox17.Text}\";",connection);
                reader = checkLogin.ExecuteReader();
                while(reader.Read())
                {
                    text = reader.GetValue(0).ToString();
                }
                if (!reader.HasRows||text==id.ToString())
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter($"UPDATE Users SET User_name = \"{textBox14.Text}\",User_surname =\"{textBox15.Text}\"," +
                        $"User_tariff = \"{comboBox2.Text}\",User_left_days = {Int32.Parse(textBox16.Text)},User_login =\"{textBox17.Text}\",User_password = \"{textBox18.Text}\" WHERE ID = {id}; ", connection);
                    adapter.SelectCommand.ExecuteNonQuery();
                    connection.Close();
                    connection.Open();
                    adapter = new OleDbDataAdapter("SELECT * FROM Users;", connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                    dataGridView1.Refresh();
                    connection.Close();
                }
                else
                {
                    label16.Visible = true;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ShowPanel(1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ShowPanel(2);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ShowPanel(1);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ShowPanel(1);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ShowPanel(1);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            connection.Open();
            string tariff = ""; 
            foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)
            {
                tariff = item.Cells[1].Value.ToString();
            }
            OleDbCommand changeTariff = new OleDbCommand($"UPDATE Users SET User_tariff = \"{tariff}\",User_left_days = {30} WHERE User_login = \"{label29.Text}\";", connection);
            reader = changeTariff.ExecuteReader();
            label27.Text = "Тариф: " + tariff;
            label28.Text = "Осталось дней: " + 30;
            connection.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MailAddress from = new MailAddress(helpAdress);
            MailAddress to = new MailAddress(helpAdress);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Help Request";
            message.Body = textBox19.Text + $"\n {label29.Text}";
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(helpAdress, "Qwerty123.");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(message);
            message.Dispose();
            textBox19.Text = string.Empty;
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            if(language == "rus")
            {
                language = "ua";
                label1.Text = "Вас вітає мережа кабельного телебачення";
                label2.Text = "Будь ласка,оберіть шлях авторизації";
                button1.Text = "Користувач";
                button2.Text = "Адміністратор";
                button17.Text = "Змінити мову";
                label3.Text = "Електронна пошта";
                label4.Text = "Пароль";
                button3.Text = "Увійти";
                button4.Text = "Зареєструватися";
                label6.Text = "Ім'я";
                label7.Text = "Прізвище";
                label8.Text = "Електронна пошта";
                label9.Text = "Пароль";
                button5.Text = "Реєстрація";
                label10.Text = "Авторизуйтесь як адміністратор щоб продовжити працювати";
                label11.Text = "Логін";
                label12.Text = "Пароль";
                button6.Text = "Вхід";
                button8.Text = "Видалити користувача";
                label16.Text = "Користувач з таким логіном вже існує";
                label23.Text = "Такий користувач вже зареєстрован";
                label24.Text = "Особистий кабінет";
                label25.Text = "Логін:";
                label26.Text = "Пароль:";
                label27.Text = "Тариф:";
                label28.Text = "Залишилося днів:";
                button16.Text = "Відправити повідомлення у підтримку";
                button15.Text = "Сплатити обранний тариф";
                label5.Text = "Пошта або пароль невірні";
            }
            else
            {
                language = "rus";
                label1.Text = "Вас приветствует сеть кабельного телевидения";
                label2.Text = "Пожалуйста,выберите путь авторизации";
                button1.Text = "Пользователь";
                button2.Text = "Администратор";
                button17.Text = "Сменить язык";
                label3.Text = "Электронная почта";
                label4.Text = "Пароль";
                button3.Text = "Войти";
                button4.Text = "Зарегистрироваться";
                label6.Text = "Імя";
                label7.Text = "Фамилия";
                label8.Text = "Электронная почта";
                label9.Text = "Пароль";
                button5.Text = "Регистрация";
                label10.Text = "Авторизуйтесь как администратор чтобы продолжить работу";
                label11.Text = "Логин";
                label12.Text = "Пароль";
                button6.Text = "Вход";
                button8.Text = "Удалить пользователя";
                label16.Text = "Пользователь с таким логином уже существует";
                label23.Text = "Такой пользователь уже зарегистрирован";
                label24.Text = "Личный кабинет";
                label25.Text = "Логин:";
                label26.Text = "Пароль:";
                label27.Text = "Тариф:";
                label28.Text = "Осталось дней:";
                button16.Text = "Отправить сообщение в поддержку";
                button15.Text = "Оплатить выбранный тариф";
                label5.Text = "Почта или пароль неверные";
            }
        }
    }
 }

