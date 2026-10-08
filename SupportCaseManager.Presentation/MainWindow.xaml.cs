using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManager.Application.Models;
using SupportCaseManager.Application.Services;
using SupportCaseManager.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SupportCaseManager.Presentation
{
    public sealed partial class MainWindow : Window
    {
        private readonly TicketQueryService _ticketQueryService;
        private readonly CustomerService _customerService;
        private readonly NewTicketWindow _newTicketWindow;
        private readonly TicketService _ticketService;

        private bool _isInitialized;

        public MainWindow(
            TicketQueryService ticketQueryService,
            CustomerService customerService,
            NewTicketWindow newTicketWindow, 
            TicketService ticketService)
        {
            _ticketQueryService = ticketQueryService;
            _customerService = customerService;
            _newTicketWindow = newTicketWindow;
            _ticketService = ticketService;

            _newTicketWindow.TicketCreated += NewTicketWindow_TicketCreated;

            InitializeComponent();

            _isInitialized = true;

            LoadTickets();
        }

        private void NewTicketWindow_TicketCreated(object? sender, EventArgs e)
        {
            LoadTickets();
        }

        private void LoadTickets()
        {
            ApplyFilters();
        }

        private void SearchTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (!_isInitialized)
                return;

            ApplyFilters();
        }

        private void StatusComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!_isInitialized)
                return;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var searchTerm = SearchTextBox.Text;

            var selectedItem =
                StatusComboBox.SelectedItem as ComboBoxItem;

            var selectedStatus =
                selectedItem?.Content?.ToString();

            IReadOnlyList<TicketOverviewModel> tickets;

            if (selectedStatus == "Alla")
            {
                tickets = _ticketQueryService.SearchTickets(searchTerm);
            }
            else if (Enum.TryParse<TicketStatus>(
                selectedStatus,
                out var status))
            {
                tickets = _ticketQueryService.SearchAndFilterTickets(
                    searchTerm,
                    status);
            }
            else
            {
                tickets = _ticketQueryService.SearchTickets(searchTerm);
            }

            TicketListView.ItemsSource = tickets;
        }

        private void NewCustomerButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var window = new NewCustomerWindow(_customerService);

            window.Activate();
        }

        private void NewTicketButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            _newTicketWindow.Activate();
        }

        private void TicketListView_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (TicketListView.SelectedItem is not TicketOverviewModel selectedTicket)
                return;

            var ticket = _ticketService.GetTicket(selectedTicket.Id);
            if (ticket == null)
                return;

            var window = new TicketDetailWindow(ticket, _customerService);

            window.Activate();

        }
    }
}