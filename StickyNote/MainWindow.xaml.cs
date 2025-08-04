using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace StickyNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private bool _openPanel = false; 

        public bool OpenPanel
        {
            get => _openPanel; 
            set => OnPropertyChanged(ref _openPanel, value);
        }

        private Brush _backgroundColor = Brushes.Yellow;

        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set => OnPropertyChanged(ref _backgroundColor, value);
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.MouseDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed && e.OriginalSource is not Button)
                {
                    this.DragMove();
                }
            };
        }

        public MainWindow(Brush backgroundColor) : this()
        {
            BackgroundColor = backgroundColor.Clone();
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            BackgroundColor = button.Background;
            OpenPanel = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void BtnChangeColor_Click(object sender, RoutedEventArgs e)
        {
            OpenPanel = true;
        }

        private void BtnNewNote_Click(object sender, RoutedEventArgs e)
        {
            var note = new MainWindow(BackgroundColor);
            note.Show();
        }

        private void BtnBold_Click(object sender, RoutedEventArgs e)
        {
            txtBox.FontWeight = txtBox.FontWeight != FontWeights.Bold ? FontWeights.Bold : FontWeights.Normal;   
        }

        private void BtnItalic_Click(object sender, RoutedEventArgs e)
        {
            txtBox.FontStyle = txtBox.FontStyle != FontStyles.Italic ? FontStyles.Italic : FontStyles.Normal;
        }

        private void BtnUnderline_Click(object sender, RoutedEventArgs e)
        {
            txtBox.TextDecorations = txtBox.TextDecorations != TextDecorations.Underline ? TextDecorations.Underline : null;
        }

        private void BtnStrikethrough_Click(object sender, RoutedEventArgs e)
        {
            txtBox.TextDecorations = txtBox.TextDecorations != TextDecorations.Strikethrough ? TextDecorations.Strikethrough : null;    
        }

        private void BtnBullet_Click(object sender, RoutedEventArgs e)
        {
            var lines = txtBox.Text.Split(Environment.NewLine);

            if (lines.Length == 0)
            {
                return;
            }

            var result = string.Empty;

            foreach (var line in lines)
            {
                result += line.StartsWith('•') ? line.TrimStart('•').Trim() : "• " + line;

                if (line != lines.LastOrDefault())
                {
                    result += Environment.NewLine;
                }
                else
                {
                    break;
                }
            }

            txtBox.Text = result;
        }
    }
}