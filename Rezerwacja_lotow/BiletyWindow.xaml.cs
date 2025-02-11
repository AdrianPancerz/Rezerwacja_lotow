using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using PathIO = System.IO.Path;  

namespace Rezerwacja_lotow
{
    public partial class BiletyWindow : Window
    {
        private int idZalogowanegoPasazera;

        public BiletyWindow(int idPasazera)
        {
            InitializeComponent();
            idZalogowanegoPasazera = idPasazera;
            ZaładujBilety();
        }

        private void ZaładujBilety()
        {
            using (var db = new ObslugaBazy())
            {
                var bilety = db.Bilet
                               .Where(b => b.Rezerwacja.ID_Pasazera == idZalogowanegoPasazera)
                               .Include(b => b.Lot)
                               .ToList();

                foreach (var bilet in bilety)
                {
                    Border biletkontener = new Border
                    {
                        BorderBrush = Brushes.White,
                        BorderThickness = new Thickness(2),
                        Margin = new Thickness(10),
                        Padding = new Thickness(10),
                        CornerRadius = new CornerRadius(15),
                        Background = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("C:\\Users\\Adrian\\Desktop\\projekt\\bile.png")),
                            Stretch = Stretch.UniformToFill
                        },
                        Width = 600,
                        Height = 220
                    };

                    StackPanel biletContent = new StackPanel { Orientation = Orientation.Vertical };

                    TextBlock numerBiletuText = new TextBlock
                    {
                        Text = $"Numer Biletu: {bilet.Numer_Biletu}",
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(40, 30, 0, 5)
                    };

                    TextBlock miejsceText = new TextBlock
                    {
                        Text = $"Numer siedzenia: {bilet.Miejsce}",
                        FontSize = 14,
                        FontFamily = new FontFamily("Cascadia Mono"),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(50, 10, 0, 5)
                    };

                    TextBlock miastoPoczatkoweText = new TextBlock
                    {
                        Text = $"Lot z : {bilet.Miasto_Poczatkowe} do: {bilet.Miasto_Celowe}",
                        FontSize = 14,
                        FontFamily = new FontFamily("Cascadia Mono"),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(50, 10, 0, 5)
                    };

                    TextBlock dataWylotuText = new TextBlock
                    {
                        Text = $"Wylot: {bilet.Data_Wylotu:yyyy-MM-dd HH:mm} Przylot: {bilet.Data_Przylotu:yyyy-MM-dd HH:mm}",
                        FontSize = 14,
                        FontFamily = new FontFamily("Cascadia Mono"),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(50, 10, 0, 5)
                    };

                    biletContent.Children.Add(numerBiletuText);
                    biletContent.Children.Add(miejsceText);
                    biletContent.Children.Add(miastoPoczatkoweText);
                    biletContent.Children.Add(dataWylotuText);

                    Button pobierzBilet = new Button
                    {
                        Content = "Pobierz Bilet",
                        Width = 110,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(10),
                        Background = Brushes.SteelBlue,
                        Foreground = Brushes.White,
                        Padding = new Thickness(5)
                    };

                    pobierzBilet.Click += (s, e) => PobierzBilet(bilet, biletkontener, pobierzBilet);
                    biletContent.Children.Add(pobierzBilet);
                    biletkontener.Child = biletContent;
                    Biletytabela.Children.Add(biletkontener);
                }
            }
        }
        private void PowrotBtn_Click1(object sender, RoutedEventArgs e)
        {
            MenuZalogowany powrot = new MenuZalogowany(idZalogowanegoPasazera);
            powrot.Show();
            this.Close();
        }



        
        private void PobierzBilet(Bilet bilet, UIElement element, Button buttonToHide)
        {
            try
            {
                
                buttonToHide.Visibility = Visibility.Collapsed;

                
                int margin = 3;

               
                int elementWidth = (int)Math.Ceiling(element.RenderSize.Width);
                int elementHeight = (int)Math.Ceiling(element.RenderSize.Height);

               
                RenderTargetBitmap renderTarget = new RenderTargetBitmap(elementWidth, elementHeight, 96, 96, PixelFormats.Pbgra32);
                element.Measure(new Size(element.RenderSize.Width, element.RenderSize.Height));
                element.Arrange(new Rect(element.RenderSize));
                renderTarget.Render(element);

              
                int outputWidth = elementWidth + margin * 2;
                int outputHeight = elementHeight + margin * 2;

                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(Brushes.White, null, new Rect(0, 0, outputWidth, outputHeight));
                    context.DrawImage(renderTarget, new Rect(margin, margin, elementWidth, elementHeight));
                }

                RenderTargetBitmap finalBitmap = new RenderTargetBitmap(outputWidth, outputHeight, 96, 96, PixelFormats.Pbgra32);
                finalBitmap.Render(visual);

                
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(finalBitmap));

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = PathIO.Combine(desktopPath, $"Bilet_{bilet.Numer_Biletu}.png");

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                MessageBox.Show($"Bilet został zapisany na pulpicie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania obrazu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                
                buttonToHide.Visibility = Visibility.Visible;
            }
        }

        private void PowrotBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
