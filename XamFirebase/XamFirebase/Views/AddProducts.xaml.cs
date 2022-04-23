using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamFirebase.Models;
using XamFirebase.ViewModels;

namespace XamFirebase.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProducts : ContentPage
    {
        VMProducts vmProduct;
        public AddProducts()
        {
            InitializeComponent();
            vmProduct = new VMProducts();            
            this.BindingContext = vmProduct;
            VMProducts proValues = new VMProducts();            
            var data = proValues.GetAllProducts();
            var ultPago = dateUltimoPago.Date.ToString("dd/MM/yyyy");            
            //--------------------------//            
        }              
        private async void lstProducts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (lstProducts.SelectedItem != null)
                {
                    Products product = (Products)e.SelectedItem;
                    if (product != null)
                    {

                        var display = await DisplayActionSheet(product.placa, "Cancelar",
                            null, new string[] { "Editar", "Borrar" });
                        if (display.Equals("Editar"))
                        {

                            vmProduct.setProduct(product);
                        }
                        else if (display.Equals("Borrar"))
                        {
                            vmProduct.setProduct(product);
                            await vmProduct.trnProducts("DELETE");
                        }
                        DateTime start = dateUltimoPago.Date;
                        DateTime end = DateTime.Now;
                        TimeSpan dif = end - start;
                        //--------------------------//
                        DateTime start2 = product.fechaIngreso.AddHours(product.horaIngreso.Hours);
                        DateTime end2 = product.fechaSalida.AddHours(product.horaSalida.Hours);
                        TimeSpan difCobro = end2 - start2;                        
                        //-------------------------//
                        var tarifaPlena = 7500;
                        var tarifaHora = 2000;
                        //Validar no mensual        
                        if (product.mensualidad != "si")
                        {
                            if (difCobro.TotalHours >= 3 && difCobro.TotalHours <= 12)
                            {
                                txtUltimoPago.Text = "El valor de la tarifa es: " + tarifaPlena;
                            }
                            else if (difCobro.TotalHours > 12)
                            {
                                var calcPlena = (difCobro.TotalHours / 12);
                                int intPlena = (int)calcPlena;
                                var calc = (difCobro.TotalHours - (intPlena * 12));
                                var calc2 = calc * tarifaHora;
                                txtUltimoPago.Text = "El valor de la tarfia es: " + ((intPlena * tarifaPlena)+calc2);                                
                            }
                            else if (difCobro.TotalHours < 3)
                            {
                                txtUltimoPago.Text = "El valor de la tarifa es: " + tarifaHora * difCobro.TotalHours;
                            }
                        }
                        else
                        {

                            if (dif.Days > 30)
                            {
                                txtUltimoPago.Text = "Estan en mora";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            lstProducts.SelectedItem = null;
        }
    }
}