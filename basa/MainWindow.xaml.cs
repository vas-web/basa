using MySqlConnector;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace basa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private const string connectionString =
            "server=192.168.0.111;" + 
            "port=3306;" +
            "user=app;" +
            "password=PaSSw0r6;" +
            "Database=MyFirstShema;" +
            "Pipelining = False;" +
            "ConnectionProtocol=Socket;";
        private void FillDataGrid(string query, DataGrid dataGrid)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
                connection.Close();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                
                FillDataGrid("SELECT * FROM Аудитория", Auditoria);
                FillDataGrid("SELECT * FROM Группа", Gruppa);
                FillDataGrid("SELECT * FROM Дисциплина", Disciplina);
                FillDataGrid("SELECT * FROM Преподаватель", Prepod);

                string query = "SELECT e.Неделя, e.День, e.Время, a.№группа, b.НомерАудитория, " +
                                "c.НазваниеДисциплины, d.ФИОПреподаватель " +
                                "FROM Группа AS a, Аудитория AS b, Дисциплина AS c, " +
                                "Преподаватель AS d, Расписание AS e " +
                                "WHERE e.idГруппа=a.idГруппа AND e.idАудитория=b.idАудитория AND " +
                                "e.idДисциплина=c.idДисциплина AND e.idПреподаватель=d.idПреподаватель;";
                FillDataGrid(query, Raspisanie);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            
        }
        public void tabb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           switch (tabb.SelectedIndex)
            {
                case 0:
                    GB1.Visibility = Visibility.Visible;
                    GB2.Visibility = Visibility.Hidden;
                    GB3.Visibility = Visibility.Hidden;
                    GB4.Visibility = Visibility.Hidden;
                    GB5.Visibility = Visibility.Hidden;
                break;
                case 1:
                    GB1.Visibility = Visibility.Hidden;
                    GB2.Visibility = Visibility.Visible;
                    GB3.Visibility = Visibility.Hidden;
                    GB4.Visibility = Visibility.Hidden;
                    GB5.Visibility = Visibility.Hidden;
                break;
                case 2:
                    GB1.Visibility = Visibility.Hidden;
                    GB2.Visibility = Visibility.Hidden;
                    GB3.Visibility = Visibility.Visible;
                    GB4.Visibility = Visibility.Hidden;
                    GB5.Visibility = Visibility.Hidden;
                break;
                case 3:
                    GB1.Visibility = Visibility.Hidden;
                    GB2.Visibility = Visibility.Hidden;
                    GB3.Visibility = Visibility.Hidden;
                    GB4.Visibility = Visibility.Visible;
                    GB5.Visibility = Visibility.Hidden;
                break;
                case 4:
                    GB1.Visibility = Visibility.Hidden;
                    GB2.Visibility = Visibility.Hidden;
                    GB3.Visibility = Visibility.Hidden;
                    GB4.Visibility = Visibility.Hidden;
                    GB5.Visibility = Visibility.Visible;
                break;
            

            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO Аудитория (НомерАудитория, ТипАудитория, Вместимость) VALUES (@value2, @value3, @value4);";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@value2", tb12.Text);
                command.Parameters.AddWithValue("@value3", tb13.Text);
                command.Parameters.AddWithValue("@value4", tb14.Text);
                
                command.ExecuteNonQuery();
                FillDataGrid("SELECT * FROM Аудитория", Auditoria);
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message, "Ошибка");
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in Auditoria.SelectedItems)
                {
                    DataRowView row = (DataRowView)item;
                    int id = Convert.ToInt32(row["idАудитория"]);

                    string query = $"DELETE FROM Аудитория WHERE idАудитория = {id}";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, connection);

                        cmd.ExecuteNonQuery();
                    }
                }
                FillDataGrid("SELECT * FROM Аудитория", Auditoria);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in Gruppa.SelectedItems)
                {
                    DataRowView row = (DataRowView)item;
                    int id = Convert.ToInt32(row["idГруппа"]);

                    string query = $"DELETE FROM Группа WHERE idГруппа = {id}";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, connection);

                        cmd.ExecuteNonQuery();
                    }
                }
                FillDataGrid("SELECT * FROM Группа", Gruppa);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO Группа (№Группа, Специальность, Факультет, ЧислоСтудентов, ФИОСтароста) VALUES (@value1, @value2, @value3, @value4, @value5);";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@value1", tb21.Text);
                command.Parameters.AddWithValue("@value2", tb22.Text);
                command.Parameters.AddWithValue("@value3", tb23.Text);
                command.Parameters.AddWithValue("@value4", tb24.Text);
                command.Parameters.AddWithValue("@value5", tb25.Text);

                command.ExecuteNonQuery();
                FillDataGrid("SELECT * FROM Группа", Gruppa);
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message, "Ошибка");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO Дисциплина (ШифрДисциплины, НазваниеДисциплины, idПреподаватель) VALUES (@value1, @value2, @value3);";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@value1", tb31.Text);
                command.Parameters.AddWithValue("@value2", tb32.Text);
                command.Parameters.AddWithValue("@value3", tb33.Text);


                command.ExecuteNonQuery();
                FillDataGrid("SELECT * FROM Дисциплина", Disciplina);
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message, "Ошибка");
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in Disciplina.SelectedItems)
                {
                    DataRowView row = (DataRowView)item;
                    int id = Convert.ToInt32(row["idДисциплина"]);

                    string query = $"DELETE FROM Дисциплина WHERE idДисциплина = {id}";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, connection);

                        cmd.ExecuteNonQuery();
                    }
                }
                FillDataGrid("SELECT * FROM Дисциплина", Disciplina);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO Преподаватель (ФИОПреподаватель, Звание, Должность, Кафедра) VALUES (@value1, @value2, @value3, @value4);";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@value1", tb41.Text);
                command.Parameters.AddWithValue("@value2", tb42.Text);
                command.Parameters.AddWithValue("@value3", tb43.Text);
                command.Parameters.AddWithValue("@value4", tb43.Text);


                command.ExecuteNonQuery();
                FillDataGrid("SELECT * FROM Преподаватель", Prepod);
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message, "Ошибка");
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in Prepod.SelectedItems)
                {
                    DataRowView row = (DataRowView)item;
                    int id = Convert.ToInt32(row["idПреподаватель"]);

                    string query = $"DELETE FROM Преподаватель WHERE idПреподаватель = {id}";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, connection);

                        cmd.ExecuteNonQuery();
                    }
                }
                FillDataGrid("SELECT * FROM Преподаватель", Prepod);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO Расписание (idГруппа, idДисциплина, idАудитория, idПреподаватель, Время, День, Неделя) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7);";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@value1", tb51.Text);
                command.Parameters.AddWithValue("@value2", tb52.Text);
                command.Parameters.AddWithValue("@value3", tb53.Text);
                command.Parameters.AddWithValue("@value4", tb54.Text);
                command.Parameters.AddWithValue("@value5", tb55.Text);
                command.Parameters.AddWithValue("@value6", tb56.Text);
                command.Parameters.AddWithValue("@value7", tb57.Text);


                command.ExecuteNonQuery();
                FillDataGrid("SELECT e.idРасписание, e.Неделя, e.День, e.Время, a.№группа, b.НомерАудитория, c.НазваниеДисциплины, d.ФИОПреподаватель FROM Группа AS a, Аудитория AS b, Дисциплина AS c, Преподаватель AS d, Расписание AS e WHERE e.idГруппа=a.idГруппа AND e.idАудитория=b.idАудитория AND e.idДисциплина=c.idДисциплина AND e.idПреподаватель=d.idПреподаватель;", Raspisanie);
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Произошла ошибка при выполнении запроса: " + ex.Message, "Ошибка");
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in Raspisanie.SelectedItems)
                {
                    DataRowView row = (DataRowView)item;
                    int id = Convert.ToInt32(row["idРасписание"]);

                    string query = $"DELETE FROM Расписание WHERE idРасписание = {id}";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {   
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, connection);

                        cmd.ExecuteNonQuery();
                    }
                }
                FillDataGrid("SELECT * FROM Расписание", Raspisanie);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
    }

}