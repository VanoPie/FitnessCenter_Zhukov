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

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    string connectionString = "server=10.10.1.24;database=abd1_11;port=3306;User Id=user_01;password=user01pro";
    
    public void Authorization(object? sender, RoutedEventArgs e)
    {
        string telephone = Telephone.Text;
        string password = Password.Text;
        bool isUser = IsUserValidClient(telephone, password);
        bool isEmployee = IsUserValidEmployee(telephone, password);
        if (isUser || isEmployee)
        {
            if (isUser)
            {
                ClientsForm client = new ClientsForm();
                client.Add.IsVisible = false;
                client.Update.IsVisible = false;
                client.Delete.IsVisible = false;
                Hide();
                client.Show();
            }
            else if (isEmployee)
            {
                ClientsForm employee = new ClientsForm();
                employee.Add.IsVisible = true;
                employee.Update.IsVisible = true;
                employee.Delete.IsVisible = true;
                employee.Title = "Форма просмотра записей (права администратора)";
                Hide();
                employee.Show();
            } 
        }
        else
        {
            Console.WriteLine("Неверный логин или пароль");
        }
    }
    
    private bool IsUserValidEmployee(string telephone, string password)
        {
            bool isValid = false;
        
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Администраторы WHERE Телефон_администратора = @Telephone AND Пароль_сотрудника= @Password";
        
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Telephone", telephone);
                    command.Parameters.AddWithValue("@Password", password);
        
                    connection.Open();
        
                    object result = command.ExecuteScalar();
                    int count = Convert.ToInt32(result);
        
                    if (count == 1)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
    private bool IsUserValidClient(string telephone, string password) 
    {
        bool isValid = false;
    
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT COUNT(1) FROM Пользователи WHERE Телефон_клиента = @Telephone AND Пароль_клиента= @Password";
    
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Telephone", telephone);
                command.Parameters.AddWithValue("@Password", password);
    
                connection.Open();
    
                object result = command.ExecuteScalar();
                int count = Convert.ToInt32(result);
    
                if (count == 1)
                {
                    isValid = true;
                }
            }
        }
        return isValid;
    }

    private void Registration(object? sender, RoutedEventArgs e)
    {
        RegisterForm reg = new RegisterForm();
        Hide();
        reg.Show();
    }
    public void Exit_Program(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}