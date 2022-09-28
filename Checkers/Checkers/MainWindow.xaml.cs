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
            this.button.Click += owner.game.Select_Field;
            owner.board_grid.Children.Add(this.button);

            Grid.SetRow(this.button, row);
            Grid.SetColumn(this.button, column);
        }

        public void Set_State()
        {
            this.image.Source = owner.game.board[coords[0], coords[1]] switch
            {
                0 => Game.empty,
                1 => Game.white_checker,
                -1 => Game.black_checker,
                2 => Game.white_king,
                -2 => Game.black_king,
                _ => null,
            };
        }
    }

    public class Game
    {
        public MainWindow owner;
        public Field[,] fields = new Field[8, 8];
        public Button selected_field;

        public int turn = 1;

        public int[] selected_coords = new int[2];
        public int[,] board = new int[8, 8]
        {
            { 1, 0, 1, 0, 1, 0, 1, 0 },
            { 0, 1, 0, 1, 0, 1, 0, 1 },
            { 2, 0, 1, 0, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0,-2, 0,-1, 0,-1, 0,-1 },
            { -1,0,-1, 0,-1, 0,-1, 0 },
            { 0,-1, 0,-1, 0,-1, 0,-1 },
        };

        public static string path = "C:\\Users\\Overlord\\git\\checkers\\Checkers\\Images\\";
        public static BitmapImage white_checker = new BitmapImage(new Uri($"{path}white_checker.png"));
        public static BitmapImage white_king = new BitmapImage(new Uri($"{path}white_king.png"));
        public static BitmapImage black_checker = new BitmapImage(new Uri($"{path}black_checker.png"));
        public static BitmapImage black_king = new BitmapImage(new Uri($"{path}black_king.png"));
        public static BitmapImage empty = new BitmapImage(new Uri($"{path}empty.png"));

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

        public void Select_Field(object sender, RoutedEventArgs e)
        {
            bool is_empty = true;
            if (this.selected_field is not null)
            {
                this.selected_field.Background = new SolidColorBrush(Color.FromArgb(255, 169, 75, 0));
                is_empty = false;
            }

            selected_field = (Button)sender;

            int row, column;
            (row, column) = owner.Get_Coords(this.selected_field);

            if (this.board[row, column] != 0)
            {
                this.selected_field.Background = new SolidColorBrush(Color.FromArgb(255, 167, 139, 113));
                this.selected_coords[0] = row;
                this.selected_coords[1] = column;
            }
            else
            {
                if (!is_empty)
                {
                    this.Make_Move(selected_coords, row, column);
                }

                this.selected_field = null;
            }
        }

        public void Make_Move(int[] selected_coords, int row, int column)
        {
            int color = this.board[selected_coords[0], selected_coords[1]] / Math.Abs(this.board[selected_coords[0], selected_coords[1]]);

            int delta_row = row - selected_coords[0];
            int delta_column = column - selected_coords[1];

            int figure = Math.Abs(this.board[selected_coords[0], selected_coords[1]]);

            if (color != this.turn || Math.Abs(delta_row) != Math.Abs(delta_column)) return;

            if (figure == 1)
            {
                if (Math.Abs(delta_column) == 1 && delta_row == color) ;
                else if (Math.Abs(delta_column) == 2 && this.board[(row + selected_coords[0]) / 2, (column + selected_coords[1]) / 2] * color < 0)
                {
                    this.board[(row + selected_coords[0]) / 2, (column + selected_coords[1]) / 2] = 0;
                }
                else return;
            }
            else if (figure == 2)
            {
                int count = 0;
                for (int i = selected_coords[0], j = selected_coords[1]; i != row; i += Math.Abs(delta_row) / delta_row, j += Math.Abs(delta_column) / delta_column)
                {
                    if (this.board[i, j] * color > 0)
                        if (i != selected_coords[0] && j != selected_coords[1]) return;
                    else if (this.board[i, j] * color < 0)
                        count++;
                }

                if (count == 1 || count == 0)
                {
                    for (int i = selected_coords[0], j = selected_coords[1]; i != row; i += Math.Abs(delta_row) / delta_row, j += Math.Abs(delta_column) / delta_column)
                    {
                        if (i != selected_coords[0] && j != selected_coords[1]) this.board[i, j] = 0;
                    }
                }
                else
                    return;

            }
                this.turn *= -1;
                this.board[row, column] = color * figure;
                this.board[selected_coords[0], selected_coords[1]] = 0;
                this.Set_Position();
        }
    }

    public partial class MainWindow : Window
    {
        public string PlayerName;

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

        public Tuple<int, int> Get_Coords(Button field)
        {
            int row = Grid.GetRow(field);
            int column = Grid.GetColumn(field);

            return new Tuple<int, int>(row, column);
        }
    }
}
