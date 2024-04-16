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

public partial class RegisterForm : Window
{
    public RegisterForm()
    {
        InitializeComponent();
    }
    private MySqlConnection conn;
    private string connStr = "server=10.10.1.24;database=abd1_11;port=3306;User Id=user_01;password=user01pro";
    
    private void Regist(object? sender, RoutedEventArgs e)
    {
        conn = new MySqlConnection(connStr);
        conn.Open();
        string regist = "INSERT INTO Клиенты(ФИО, Телефон, Пароль, Дата_рождения, Пол) VALUES ('" + FIO.Text + "', '" + Telephone.Text + "', '" + Password.Text + "', '" + DateBirth.Text + "', '" + Pol.Text + "');";
        MySqlCommand cmd = new MySqlCommand(regist, conn);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    private void MainFormGo(object? sender, RoutedEventArgs e)
    {
        MainWindow auth = new MainWindow();
        this.Close();
        auth.Show();
    }
}