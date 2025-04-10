using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();


    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }


    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }

    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops!", ex.Message, "OK");
        }


    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue.ToLower();

            lst_produtos.IsRefreshing = true;

            lista.Clear(); 

            List<Produto> tmp = await App.Db.GetAll(); 

            var filtrados = tmp.Where(p => p.Descricao.ToLower().Contains(q)).ToList(); // Filtra os produtos pelo nome

            filtrados.ForEach(i => lista.Add(i)); // Atualiza a ObservableCollection com os produtos filtrados
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;

        }
    }


    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total � {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem certeza?", $"Remover {p.Descricao}?", "Sim", "N�o");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }

        catch (Exception ex)
        {
            DisplayAlert("Ops!", ex.Message, "OK");

        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");

        }
        finally
        {
            lst_produtos.IsRefreshing = false;

        }

    }

    private async void picker_filtro_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        string categoriaSelecionada = picker_filtro_categoria.SelectedItem?.ToString();

        if (categoriaSelecionada == "Todos")
        {
            lst_produtos.ItemsSource = await App.Db.GetAll();
        }
        else
        {
            lst_produtos.ItemsSource = await App.Db.FiltrarPorCategoria(categoriaSelecionada);
        }
    }

    private async void ToolbarItem_Clicked_Relatorio(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioCategoria());
    }




}