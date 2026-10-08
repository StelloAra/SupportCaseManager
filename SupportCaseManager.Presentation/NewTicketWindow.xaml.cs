using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManager.Application.Services;
using SupportCaseManager.Domain.Enums;
using System;

namespace SupportCaseManager.Presentation
{
    public sealed partial class NewTicketWindow : Window
    {
        private readonly TicketService _ticketService;
        private readonly CustomerService _customerService;
        public event EventHandler? TicketCreated;

        public NewTicketWindow(
            TicketService ticketService,
            CustomerService customerService)
        {
            _ticketService = ticketService;
            _customerService = customerService;

            InitializeComponent();

            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = _customerService.GetAllCustomers();

            CustomerComboBox.ItemsSource = customers;
            CustomerComboBox.DisplayMemberPath = "Name.Value";
            CustomerComboBox.SelectedValuePath = "Id";
        }

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                if (CustomerComboBox.SelectedValue is not Guid customerId)
                {
                    ShowError("Du måste välja en kund.");
                    return;
                }

                var selectedPriority =
                    (PriorityComboBox.SelectedItem as ComboBoxItem)
                    ?.Content?
                    .ToString();

                if (!Enum.TryParse<TicketPriority>(
                    selectedPriority,
                    out var priority))
                {
                    ShowError("Du måste välja en giltig prioritet.");
                    return;
                }

                _ticketService.CreateTicket(
                    TitleTextBox.Text,
                    DescriptionTextBox.Text,
                    customerId,
                    priority);

                TicketCreated?.Invoke(this, EventArgs.Empty);

                Close();
            }
            catch (ArgumentException ex)
            {
                ShowError(ex.Message);
            }
            catch (InvalidOperationException ex)
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