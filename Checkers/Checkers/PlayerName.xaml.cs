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
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Логика взаимодействия для PlayerName.xaml
    /// </summary>
    public partial class PlayerName : Window
    {
        public PlayerName()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            TextBox2.Text = "Player";
            TextBox2.Focus();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Name
        {
            get { return TextBox2.Text; }
        }
    }
}
