using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kuzmin_GlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> Agents = new List<Agent>();
        List<Agent> TableList;

        public AgentPage()
        {
            InitializeComponent();
            var currentAgents = Kuzmin_GlazkiEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgents;
            ComboType.SelectedIndex = 0;
            Filter.SelectedIndex = 0;

            UpdateAgents();
        }

        private void UpdateAgents()
        {
            var currentAgents = Kuzmin_GlazkiEntities.GetContext().Agent.ToList();

            if (ComboType.SelectedIndex == 0)
            {
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }

            if (Filter.SelectedIndex == 0)
            {
                currentAgents = currentAgents.Where(p => (p.GetAgentType.Contains("МФО") || p.GetAgentType.Contains("ООО") || p.GetAgentType.Contains("ОАО") || p.GetAgentType.Contains("ЗАО") || p.GetAgentType.Contains("ПАО") || p.GetAgentType.Contains("МКК"))).ToList();
            }
            if (Filter.SelectedIndex == 1)
            {
                currentAgents = currentAgents.Where(p => p.GetAgentType.Contains("МФО")).ToList();
            }
            if (Filter.SelectedIndex == 2)
            {
                currentAgents = currentAgents.Where(p => (p.GetAgentType.Contains("ООО"))).ToList();
            }
            if (Filter.SelectedIndex == 3)
            {
                currentAgents = currentAgents.Where(p => (p.GetAgentType.Contains("ЗАО"))).ToList();
            }
            if (Filter.SelectedIndex == 4)
            {
                currentAgents = currentAgents.Where(p => (p.GetAgentType.Contains("МКК"))).ToList();
            }
            if (Filter.SelectedIndex == 5)
            {
                currentAgents = currentAgents.Where(p => (p.GetAgentType.Contains("ОАО"))).ToList();
            }
            if (Filter.SelectedIndex == 6)
            {
                currentAgents = currentAgents.Where(p => (p.GetAgentType.Contains("ПАО"))).ToList();
            }

            AgentListView.ItemsSource = currentAgents;
            TableList = currentAgents;
            ChangePage(0, 0);

            string searchText = TBoxSearch.Text.ToLower()
                                 .Replace(" ", "")
                                 .Replace("(", "")
                                 .Replace(")", "")
                                 .Replace("-", "");

            currentAgents = currentAgents
                .Where(p => p.Email.ToLower().Replace(" ", "").Replace(".", "").Replace("@", "").Contains(searchText) ||
                            p.Title.ToLower().Replace(" ", "").Replace("!", "").Replace("@", "").Contains(searchText) ||
                            p.Phone.ToLower().Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Contains(searchText))
                .ToList();
            AgentListView.ItemsSource = currentAgents.ToList();
        }

        

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // серч
            UpdateAgents();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // сортировка (оао ооо)
            UpdateAgents();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // фильтрация по приоритету 
            UpdateAgents();
        }

        public void ChangePage(int direction, int? selectedPage)
        {
            Agents.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;

            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;

                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        Agents.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                Agents.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                Agents.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();

                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = $" из {CountRecords.ToString()}";

                AgentListView.ItemsSource = Agents;

                AgentListView.Items.Refresh();
            }
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}