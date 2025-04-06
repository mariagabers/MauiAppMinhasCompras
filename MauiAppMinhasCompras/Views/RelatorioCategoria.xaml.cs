using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    public RelatorioCategoria()
    {
        InitializeComponent();
        CarregarRelatorio();
    }

    private async void CarregarRelatorio()
    {
        var relatorio = await App.Db.GetGastosPorCategoria();
        lst_relatorio.ItemsSource = relatorio;
    }
}
