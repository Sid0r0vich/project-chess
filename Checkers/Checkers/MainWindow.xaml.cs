using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Checkers
{
    public class Field
    {
        public Button button = new Button();
        public Image image = new Image();

        public MainWindow owner;
        public int[] coords;

        public Field(MainWindow owner, int row, int column)
        {
            this.owner = owner;
            this.coords = new int[2] { row, column };

            this.button.Background = new SolidColorBrush(Color.FromArgb(255, 169, 75, 0));
            this.image.Margin = new Thickness(7, 7, 7, 7);

            this.button.Content = this.image;
            this.button.Click += owner.Select_Field;
            owner.board_grid.Children.Add(this.button);

            Grid.SetRow(this.button, row);
            Grid.SetColumn(this.button, column);
        }

        public void Set_State()
        {
            this.image.Source = owner.board[coords[0], coords[1]] switch
            {
                1 => MainWindow.white_checker,
                -1 => MainWindow.black_checker,
                0 => MainWindow.empty,
                _ => null,
            };
        }

        public void Make_Move()
        {

        }
    }

    public class Game
    {
        public MainWindow owner;
        public Field[,] fields = new Field[8, 8];

        public Game(MainWindow owner)
        {
            this.owner = owner;
        }

        public void Create_Board()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        Field field = new Field(this.owner, i, j);
                        fields[i, j] = field;
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
                        fields[i, j].Set_State();
                    }
                }
            }
        }
    }

    public partial class MainWindow : Window
    {
        public string PlayerName;

        public static string path = "C:\\Users\\Overlord\\git\\checkers\\Checkers\\Images\\";
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

        public Button selected_field;
        public short[] selected_coords = new short[2];

        public short turn = 1;

        public Game game;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            game = new Game(this);
            game.Create_Board();
        }

        public void Get_Name(object sender, RoutedEventArgs e)
        {
            PlayerName WindowName = new PlayerName();

            if (WindowName.ShowDialog() == true)
            {
                if (WindowName.Name != "")
                {
                    this.PlayerName = WindowName.Name;

                    TextBlock TextName = new TextBlock()
                    {
                        Height = 50,
                        Text = $"{this.PlayerName}",
                        FontSize = 30,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(0, 20, 0, 0)
                    };

                    GridInfo.Children.Remove(ButtonPlay);
                    GridInfo.Children.Add(TextName);

                    game.Set_Position();
                }
                else
                    MessageBox.Show("Please, enter your name!");
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

            if (color != this.turn) return;

            if (Math.Abs(column - selected_coords[1]) == 1 && row - selected_coords[0] == color) ;
            else if (Math.Abs(column - selected_coords[1]) == 2 && Math.Abs(row - selected_coords[0]) == 2 &&
                this.board[(row + selected_coords[0]) / 2, (column + selected_coords[1]) / 2] == -color)
            {
                this.board[(row + selected_coords[0]) / 2, (column + selected_coords[1]) / 2] = 0;
            }
            else return;

            this.turn *= -1;
            this.board[row, column] = color;
            this.board[selected_coords[0], selected_coords[1]] = 0;
            game.Set_Position();
        }
    }
}
