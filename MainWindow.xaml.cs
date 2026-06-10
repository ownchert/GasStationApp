using System.Windows;
using GasStationApp.Pages;
using GasStationApp.Services;

namespace GasStationApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ShowCurrentUser();
            ApplyRoleAccess();
            OpenStartPage();
        }

        private void ShowCurrentUser()
        {
            string employeeName = string.IsNullOrWhiteSpace(CurrentSession.EmployeeName)
                ? CurrentSession.Login
                : CurrentSession.EmployeeName;

            TxtUserInfo.Text = employeeName;
            TxtRoleInfo.Text = "Роль: " + CurrentSession.RoleName;
        }

        private void ApplyRoleAccess()
        {
            if (CurrentSession.IsAdmin)
            {
                // Администратор видит всё.
                return;
            }

            if (CurrentSession.IsManager)
            {
                // Менеджер работает с поставками, складом и отчетами.
                BtnEmployees.Visibility = Visibility.Collapsed;
                return;
            }

            if (CurrentSession.IsCashier)
            {
                // Кассир видит продажи и остатки, но не управляет справочниками и поставками.
                BtnProductSupply.Visibility = Visibility.Collapsed;
                BtnFuelSupply.Visibility = Visibility.Collapsed;

                BtnSuppliers.Visibility = Visibility.Collapsed;
                BtnEmployees.Visibility = Visibility.Collapsed;

                BtnReports.Visibility = Visibility.Collapsed;
                BtnStockControl.Visibility = Visibility.Collapsed;

                BtnFuelColumns.Visibility = Visibility.Collapsed;
                return;
            }
        }

        private void OpenStartPage()
        {
            if (CurrentSession.IsCashier)
            {
                MainFrame.Navigate(new ProductSalePage());
            }
            else
            {
                MainFrame.Navigate(new ProductPage());
            }
        }

        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeePage());
        }

        private void BtnProduct_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductPage());
        }

        private void BtnFuel_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FuelPage());
        }

        private void BtnStockControl_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StockControlPage());
        }

        private void BtnSupplier_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SupplierPage());
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CheckPage());
        }

        private void BtnProductSupply_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SupplyPage());
        }

        private void BtnFuelSupply_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FuelSupplyPage());
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportPage());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.Clear();

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            Close();
        }
        private void BtnProductSale_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductSalePage());
        }

        private void BtnFuelSale_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FuelSalePage());
        }

        private void BtnFuelColumns_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FuelColumnPage());
        }
    }
}