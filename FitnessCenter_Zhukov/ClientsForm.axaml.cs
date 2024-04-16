using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace FitnessCenter_Zhukov;

public partial class ClientsForm : Window
{
    public ClientsForm()
    {
        InitializeComponent();
        ShowTable(fullTable);
        FillCMB();
    }
    
    private List<Clients> _client;
    string fullTable = "SELECT * FROM Клиенты";
    string connStr = "server=10.10.1.24;database=abd1_11;port=3306;User Id=user_01;password=user01pro";
    private MySqlConnection conn;
        
    public void ShowTable(string sql)
    {
        _client = new List<Clients>();
        conn = new MySqlConnection(connStr);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var clients = new Clients()
            {
                ID_клиента = reader.GetInt32("ID_клиента"),
                Фамилия = reader.GetString("Фамилия"),
                Имя = reader.GetString("Имя"),
                Номер_телефона = reader.GetString("Номер_телефона"),
                Дата_рождения = reader.GetDateTime("Дата_рождения"),
                Пол = reader.GetString("Пол"),
                Скидка = reader.GetString("Скидка")
            };
            _client.Add(clients);
        }
        conn.Close();
        DataGrid.ItemsSource = _client;
    }
    
    private void SearchFio(object? sender, TextChangedEventArgs e)
    {
        var fam = _client;
        fam = fam.Where(x => x.Фамилия.Contains(Search_FIO.Text)).ToList();
        DataGrid.ItemsSource = fam;
    }

    private void Reset_OnClick(object? sender, RoutedEventArgs e)
    {
        ShowTable(fullTable);
        Search_FIO.Text = string.Empty;
    }
    
    private void AddData(object? sender, RoutedEventArgs e)
    {
        Clients newClient = new Clients();
        CRUDForm add = new CRUDForm(newClient, _client);
        add.Show();
        this.Close();
    }

    private void Edit(object? sender, RoutedEventArgs e)
    {
        Clients currentClient = DataGrid.SelectedItem as Clients;
        if (currentClient == null)
            return;
        CRUDForm edit = new CRUDForm(currentClient, _client);
        edit.Title = "Форма редактирования данных";
        edit.TitleBlock.Text = "Редатирование данных";
        edit.Show();
        this.Close();
    }
    
    private void Del(object? sender, RoutedEventArgs e)
    {
        try
        {
            Clients usr = DataGrid.SelectedItem as Clients;
            if (usr == null)
            {
                return;
            }
            conn = new MySqlConnection(connStr);
            conn.Open();
            string sql = "DELETE FROM Клиенты WHERE ID_клиента = " + usr.ID_клиента;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            _client.Remove(usr);
            ShowTable(fullTable);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void CmbPol(object? sender, SelectionChangedEventArgs e)
    {
        var genderComboBox = (ComboBox)sender;
        var currentGender = genderComboBox.SelectedItem as Clients;
        var filteredUsers = _client
            .Where(x => x.Пол == currentGender.Пол)
            .ToList();
        DataGrid.ItemsSource = filteredUsers;
    }
    
    public void FillCMB()
    {
        _client = new List<Clients>();
        conn = new MySqlConnection(connStr);
        conn.Open();
        MySqlCommand command = new MySqlCommand("select * from Клиенты", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentGender = new Clients()
            {
                ID_клиента = reader.GetInt32("ID_клиента"),
                Фамилия = reader.GetString("Фамилия"),
                Имя = reader.GetString("Имя"),
                Номер_телефона = reader.GetString("Номер_телефона"),
                Дата_рождения = reader.GetDateTime("Дата_рождения"),
                Пол = reader.GetString("Пол"),
                Скидка = reader.GetString("Скидка")
            };
            _client.Add(currentGender);
        }
        conn.Close();
        var genderComboBox = this.Find<ComboBox>("CmbPol");
        genderComboBox.ItemsSource = _client.DistinctBy(s => s.Пол);
    }

    private void SortAscending(object sender, RoutedEventArgs e)
    {

        var sortedItems = DataGrid.ItemsSource.Cast<Clients>().OrderBy(s => s.Имя).ToList();
        DataGrid.ItemsSource = sortedItems;
    }

    private void SortDescending(object sender, RoutedEventArgs e)
    {
        var sortedItems = DataGrid.ItemsSource.Cast<Clients>().OrderByDescending(s => s.Имя).ToList();
        DataGrid.ItemsSource = sortedItems;
    }

    public void Exit_Program(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private void LogOut(object? sender, RoutedEventArgs e)
    {
        MainWindow logout = new MainWindow();
        logout.Show();
        this.Close();
    }
}