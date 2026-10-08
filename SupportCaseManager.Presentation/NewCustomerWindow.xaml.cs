using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManager.Application.Services;
using System;

namespace SupportCaseManager.Presentation
{
    public sealed partial class NewCustomerWindow : Window
    {
        private readonly CustomerService _customerService;

        public NewCustomerWindow(CustomerService customerService)
        {
            _customerService = customerService;

            InitializeComponent();
        }

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                _customerService.CreateCustomer(
                    NameTextBox.Text,
                    EmailTextBox.Text);

                Close();
            }
            catch (ArgumentException ex)
            {
                ShowError(ex.Message);
            }
        }

        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }

        private async void ShowError(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Fel",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = Content.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}