using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using System;

namespace FitnessCenter_Zhukov;

public partial class CRUDForm : Window
{
    private List<Clients> _Clients;
    private Clients CurrentClient;
    public CRUDForm(Clients currentClient, List<Clients> _clients)
    {
        InitializeComponent();
        CurrentClient = currentClient;
        this.DataContext = currentClient;
        _Clients = _clients;
    }
    private MySqlConnection conn;
    string connStr = "server=10.10.1.24;database=abd1_11;port=3306;User Id=user_01;password=user01pro";

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var usr = _Clients.FirstOrDefault(x => x.ID_клиента == CurrentClient.ID_клиента);
        if (usr == null)
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                string add = "INSERT INTO Клиенты(Фамилия, Имя, Номер_телефона, Дата_рожджения, Пол, Скидка) VALUES ('" + Fam.Text + "', '" + NameClient.Text + "', '" + Telephone.Text + "''" + DateBirth.Text + "', '" + Pol.Text + "', '" + Discount.Text + "');";
                MySqlCommand cmd = new MySqlCommand(add, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception);
            }
        }
        else
        {
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                string upd = "UPDATE заказы SET Фамилия = '" + Fam.Text + "', Имя = '" +  NameClient.Text + "', Номер_телефона = '" + Telephone.Text + "', Дата_рожджения = '" +  DateBirth.Text + "', Пол = '" + Pol.Text + "', Скидка = '" + Discount.Text + "'  WHERE ID_клиента = " + Convert.ToInt32(ID.Text) + ";";
                MySqlCommand cmd = new MySqlCommand(upd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.Write("Error" + exception);
            }
        }
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        ClientsForm back = new ClientsForm();
        this.Close();
        back.Show(); 
    }
}