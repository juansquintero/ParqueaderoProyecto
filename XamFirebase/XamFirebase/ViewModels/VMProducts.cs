using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamFirebase.Models;

namespace XamFirebase.ViewModels
{
    public class VMProducts : INotifyPropertyChanged
    {
        FirebaseClient fClient;

        private Products _product { get; set; }

        public Products product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        private bool _showButton { get; set; }
        public bool showButton
        {
            get { return _showButton; }
            set
            {
                _showButton = value;
                OnPropertyChanged();
            }
        }
        private bool _isBusy { get; set; }
        public bool isBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                showButton = !value;
            }
        }

        private ICommand _btnSaveProduct { get; set; }
        public ICommand btnSaveProduct
        {
            get { return _btnSaveProduct; }
            set
            {
                _btnSaveProduct = value;
                OnPropertyChanged();
            }
        }

        private string _lblMessage { get; set; }
        public string lblMessage
        {
            get { return _lblMessage; }
            set
            {
                _lblMessage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Products> _lstProducts { get; set; }

        public ObservableCollection<Products> lstProducts
        {
            get { return _lstProducts; }
            set
            {
                _lstProducts = value;
                OnPropertyChanged();
            }
        }

        private string _btnSaveText { get; set; }
        public string btnSaveText
        {
            get { return _btnSaveText; }
            set
            {
                _btnSaveText = value;
                OnPropertyChanged();
            }
        }


        readonly string productResource = "Products";
        public VMProducts()
        {
            try
            {
                lstProducts = new ObservableCollection<Products>();
                btnSaveProduct = new Command(async () =>
                {
                    isBusy = true;
                    await trnProducts("ADD");
                });
                var callList = new Command(async () => await GetAllProducts());
                callList.Execute(null);

            }
            catch (Exception ex)
            {
                lblMessage = "Error occurred " + ex.Message.ToString();
            }
        }

        public bool connectFirebase()
        {
            try
            {
                if (fClient == null)
                    fClient = new FirebaseClient("https://parkingproyect-bf01f-default-rtdb.firebaseio.com");
                return true;
            }
            catch (Exception ex)
            {
                lblMessage = "Erro al conectarse a la base de datos Error:" + ex.Message.ToString();
                return false;
            }

        }

        public async Task trnProducts(string action)
        {
            try
            {
                if (product == null || String.IsNullOrWhiteSpace(product.placa) || String.IsNullOrWhiteSpace(product.tipo) || String.IsNullOrWhiteSpace(product.tipoVehiculo) || String.IsNullOrWhiteSpace(product.observaciones) || String.IsNullOrWhiteSpace(product.mensualidad))
                {
                    lblMessage = "Ingrese los datos del vehiculo";
                    isBusy = false;
                    return;
                }

                if (connectFirebase())
                {
                    Products products = new Products();
                    products.placa = product.placa;
                    products.tipo = product.tipo;
                    products.tipoVehiculo = product.tipoVehiculo;
                    products.modelo = product.modelo;
                    products.celular = product.celular;
                    products.fechaIngreso = product.fechaIngreso;
                    products.fechaSalida = product.fechaSalida;
                    products.horaIngreso = product.horaIngreso;
                    products.horaSalida = product.horaSalida;
                    products.observaciones = product.observaciones;
                    products.mensualidad = product.mensualidad;
                    products.fechaUltimoPago = product.fechaUltimoPago;
                    if (btnSaveText == "GUARDAR" && action.Equals("ADD"))
                    {
                        products.vehiculeId = Guid.NewGuid();

                        await fClient.Child(productResource).PostAsync(JsonConvert.SerializeObject(products));
                        await GetAllProducts();
                        lblMessage = "Se ha guardado el vehiculo";
                    }
                    else if (btnSaveText == "Actualizar" && action.Equals("ADD"))
                    {
                        products.vehiculeId = product.vehiculeId;

                        var updateProduct = (await fClient.Child(productResource).OnceAsync<Products>()).FirstOrDefault(x => x.Object.vehiculeId == products.vehiculeId);

                        if (updateProduct == null)
                        {
                            lblMessage = "No se puede encontrar el vehiculo";
                            isBusy = false;
                            return;
                        }
                        await fClient
                          .Child(productResource + "/" + updateProduct.Key).PatchAsync(JsonConvert.SerializeObject(products));
                        await GetAllProducts();
                        lblMessage = "El vehiculo se ha actualizado";
                    }
                    else if (action.Equals("DELETE"))
                    {
                        var deleteProduct = (await fClient
               .Child(productResource)
               .OnceAsync<Products>()).FirstOrDefault(d => d.Object.vehiculeId == product.vehiculeId);

                        if (deleteProduct == null)
                        {
                            lblMessage = "No se encuentra el vehiculo";
                            isBusy = false;
                            return;
                        }

                        await fClient
                          .Child(productResource + "/" + deleteProduct.Key).DeleteAsync();

                        await GetAllProducts();
                        lblMessage = "El veehiculo se ha eliminado";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage = "Ha ocurrido un error no se puede guardar el producto Error:" + ex.Message.ToString();

            }
            isBusy = false;
        }        
        public async Task GetAllProducts()
        {
            Clear();
            isBusy = true;
            try
            {
                lstProducts = new ObservableCollection<Products>();
                if (connectFirebase())
                {
                    var lst = (await fClient.Child(productResource).OnceAsync<Products>()).Select(x =>
                    new Products
                    {
                        vehiculeId = x.Object.vehiculeId,                        
                        placa = x.Object.placa,
                        tipo = x.Object.tipo,
                        tipoVehiculo = x.Object.tipoVehiculo,
                        modelo = x.Object.modelo,
                        celular = x.Object.celular,
                        fechaIngreso = x.Object.fechaIngreso,
                        fechaSalida = x.Object.fechaSalida,
                        horaIngreso = x.Object.horaIngreso,
                        horaSalida = x.Object.horaSalida,
                        observaciones = x.Object.observaciones,
                        mensualidad = x.Object.mensualidad,
                        fechaUltimoPago = x.Object.fechaUltimoPago,
                }).ToList();

                    lstProducts = new ObservableCollection<Products>(lst);
                }
            }
            catch (Exception ex)
            {
                lblMessage = "Error en la consulta del vehiculo. Error:" + ex.Message.ToString();
            }
            isBusy = false;
        }

        public void setProduct(Products edt)
        {
            product = new Products();            
            product.placa = edt.placa;
            product.tipo = edt.tipo;
            product.tipoVehiculo = edt.tipoVehiculo;
            product.modelo = edt.modelo;
            product.celular = edt.celular;
            product.fechaIngreso = edt.fechaIngreso;
            product.fechaSalida = edt.fechaSalida;
            product.horaIngreso = edt.horaIngreso;
            product.horaSalida = edt.horaSalida;
            product.observaciones = edt.observaciones;
            product.mensualidad = edt.mensualidad;
            product.fechaUltimoPago = edt.fechaUltimoPago;
            btnSaveText = "Actualizar";
            product.vehiculeId = edt.vehiculeId;
        }

        public void Clear()
        {

            product = new Products();
            product.placa = "";
            product.tipo = "";
            product.tipoVehiculo = "";
            product.modelo = 0;
            product.celular = 0;
            product.fechaIngreso = DateTime.MinValue;
            product.fechaSalida = DateTime.MinValue;
            product.horaIngreso = TimeSpan.Zero;
            product.horaSalida = product.horaSalida;
            product.observaciones = "";
            product.mensualidad = product.mensualidad;
            product.fechaUltimoPago = DateTime.MinValue;
            isBusy = false;
            product.vehiculeId = Guid.Empty;
            btnSaveText = "GUARDAR";
            lblMessage = "";
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
