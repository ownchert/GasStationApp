using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GasStationApp.Models;
using GasStationApp.Services;
using Microsoft.EntityFrameworkCore;

namespace GasStationApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            TxtLogin.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TryLogin();
            }
        }

        private void TryLogin()
        {
            TxtError.Text = "";

            string login = TxtLogin.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrWhiteSpace(login))
            {
                TxtError.Text = "Введите логин.";
                TxtLogin.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                TxtError.Text = "Введите пароль.";
                TxtPassword.Focus();
                return;
            }

            try
            {
                string passwordHash = GetSha256Hash(password);

                using (var db = new GasStationContext())
                {
                    var user = db.AppUser
                        .Include(u => u.Role)
                        .Include(u => u.Employee)
                        .FirstOrDefault(u =>
                            u.Login == login &&
                            u.PasswordHash == passwordHash &&
                            u.IsActive == true);

                    if (user == null)
                    {
                        TxtError.Text = "Неверный логин или пароль. Проверьте данные и попробуйте снова.";
                        TxtPassword.Clear();
                        TxtPassword.Focus();
                        return;
                    }

                    CurrentSession.SetUser(user);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    Close();
                }
            }
            catch (Exception ex)
            {
                TxtError.Text = "Ошибка подключения или авторизации: " + ex.Message;
            }
        }

        private string GetSha256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}