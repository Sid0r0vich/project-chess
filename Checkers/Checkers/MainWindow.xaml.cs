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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Checkers
{
    public partial class MainWindow : Window
    {
        public string PlayerName;

        static string path = "C:\\Users\\Overlord\\git\\checkers\\Checkers\\Images\\";
        public static BitmapImage white_checker = new BitmapImage(new Uri($"{path}white_checker.png"));
        public static BitmapImage black_checker = new BitmapImage(new Uri($"{path}black_checker.png"));
        public static BitmapImage empty = new BitmapImage(new Uri($"{path}empty.png"));

        public short[,] board = new short[8, 8]
        {
            { 1, 0, 1, 0, 1, 0, 1, 0 },
            { 0, 1, 0, 1, 0, 1, 0, 1 },
            { 1, 0, 1, 0, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0,-1, 0,-1, 0,-1, 0,-1 },
            { -1,0,-1, 0,-1, 0,-1, 0 },
            { 0,-1, 0,-1, 0,-1, 0,-1 },
        };

        public System.Windows.Controls.Button[,] buttons = new System.Windows.Controls.Button[8, 8];

        public Button selected_field;
        public short[] selected_coords = new short[2];

        public MainWindow()
        {
            InitializeComponent();
            Create_Board();
        }
        public void Get_Name(object sender, RoutedEventArgs e)
        {
            PlayerName WindowName = new PlayerName();

            if (WindowName.ShowDialog() == true)
            {
                if (WindowName.Name != "")
                {
                    GridInfo.Children.Remove(ButtonPlay);
                    MessageBox.Show("Play!");
                    this.PlayerName = WindowName.Name;

                    TextBlock TextName = new TextBlock();

                    TextName.Height = 50;
                    TextName.Text = $"{this.PlayerName}";
                    TextName.FontSize = 30;
                    TextName.HorizontalAlignment = HorizontalAlignment.Center;
                    TextName.VerticalAlignment = VerticalAlignment.Top;
                    TextName.Margin = new Thickness(0, 20, 0, 0);
                    GridInfo.Children.Add(TextName);

                    Set_Position();
                }
                else
                    MessageBox.Show("Please, enter your name!");
            }
        }
        public void Create_Board()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        Button button = new Button();
                        button.Background = new SolidColorBrush(Color.FromArgb(255, 169, 75, 0));
                        button.Click += this.Select_Field;
                        board_grid.Children.Add(button);

                        Grid.SetRow(button, i);
                        Grid.SetColumn(button, j);

                        buttons[i, j] = button;
                    }
                }
            }
        }
        public void Set_Position()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        image.Margin = new Thickness(7, 7, 7, 7);
                        buttons[i,j].Content = image;

                        if (board[i, j] == 1)
                            image.Source = MainWindow.white_checker;
                        else if (board[i, j] == -1)
                            image.Source = MainWindow.black_checker;
                        else image.Source = MainWindow.empty;
                    }
                }
            }
        }
        public void Select_Field(object sender, RoutedEventArgs e)
        {
            bool is_empty = true;
            if (selected_field is not null)
            {
                selected_field.Background = new SolidColorBrush(Color.FromArgb(255, 169, 75, 0));
                is_empty = false;
            }

            selected_field = (Button)sender;

            short row = (short)Grid.GetRow(selected_field);
            short column = (short)Grid.GetColumn(selected_field);

            if (this.board[row, column] != 0)
            {
                selected_field.Background = new SolidColorBrush(Color.FromArgb(255, 167, 139, 113));
                selected_coords[0] = row;
                selected_coords[1] = column;
            }
            else
            {
                if (!is_empty)
                {
                    Make_Move(selected_coords, row, column);
                }
                
                selected_field = null;
            }
        }
        public void Make_Move(short[] selected_coords, short row, short column)
        {
            short color = this.board[selected_coords[0], selected_coords[1]];

            if (Math.Abs(column - selected_coords[1]) == 1 && row - selected_coords[0] == color)
            {
                this.board[row, column] = color;
                this.board[selected_coords[0], selected_coords[1]] = 0;
                Set_Position();
            }
            else if (Math.Abs(column - selected_coords[1]) == 2 && Math.Abs(row - selected_coords[0]) == 2 &&
                this.board[(row + selected_coords[0]) / 2, (column + selected_coords[1]) / 2] == -color)
            {
                this.board[row, column] = color;
                this.board[selected_coords[0], selected_coords[1]] = 0;
                this.board[(row + selected_coords[0]) / 2, (column + selected_coords[1]) / 2] = 0;
                Set_Position();
            }
        }
    }
}
