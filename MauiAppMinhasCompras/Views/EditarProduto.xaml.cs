using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    private Produto _produto;

    public List<string> Categorias { get; set; }

    public EditarProduto()
    {
        InitializeComponent();

        Categorias = new List<string>
        {
            "Alimentos", "Higiene", "Limpeza", "Bebidas", "Outros"
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Produto produto)
        {
            _produto = produto;

            picker_categoria.ItemsSource = Categorias;
            picker_categoria.SelectedItem = produto.Categoria;

            txt_descricao.Text = produto.Descricao;
            txt_quantidade.Text = produto.Quantidade.ToString();
            txt_preco.Text = produto.Preco.ToString("F2");
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            _produto.Descricao = txt_descricao.Text;
            _produto.Quantidade = Convert.ToDouble(txt_quantidade.Text);
            _produto.Preco = Convert.ToDouble(txt_preco.Text);
            _produto.Categoria = picker_categoria.SelectedItem?.ToString();

            await App.Db.Update(_produto);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops!", ex.Message, "OK");
        }
    }
}
