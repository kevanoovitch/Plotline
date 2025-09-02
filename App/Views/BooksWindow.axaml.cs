using Avalonia.Controls;
using Avalonia.Input;
using PlotLineApp.Models;

namespace PlotLineApp.Views
{
    public partial class BooksWindow : Window
    {
        public BooksWindow()
        {
            InitializeComponent();

        }

         public async void Book_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender is Border border && border.DataContext is Book book)
            {
                var data = new DataObject();
                data.Set("book", book);

                await DragDrop.DoDragDrop(e, data, DragDropEffects.Copy);
            }    
        }
    }
    
   
}