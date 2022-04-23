using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XamFirebase.Models
{
    public class Products : INotifyPropertyChanged
    {
        private Guid _vehiculeId { get; set; }

        public Guid vehiculeId
        {
            get { return _vehiculeId; }
            set
            {
                _vehiculeId = value; OnPropertyChanged();
            }
        }
        private string _placa { get; set; }

        public string placa
        {
            get { return _placa; }
            set
            {
                _placa = value; OnPropertyChanged();
            }
        }
        private string _tipo { get; set; }

        public string tipo
        {
            get { return _tipo; }
            set
            {
                _tipo = value; OnPropertyChanged();
            }
        }
        private string _tipoVehiculo { get; set; }

        public string tipoVehiculo
        {
            get { return _tipoVehiculo; }
            set
            {
                _tipoVehiculo = value; OnPropertyChanged();
            }
        }
        private int _modelo { get; set; }

        public int modelo
        {
            get { return _modelo; }
            set
            {
                _modelo = value; OnPropertyChanged();
            }
        }
        private double _celular { get; set; }

        public double celular
        {
            get { return _celular; }
            set
            {
                _celular = value; OnPropertyChanged();
            }
        }

        private DateTime _fechaIngreso { get; set; }

        public DateTime fechaIngreso
        {
            get { return _fechaIngreso; }
            set
            {
                _fechaIngreso = value; OnPropertyChanged();
            }
        }
        private DateTime _fechaSalida { get; set; }

        public DateTime fechaSalida
        {
            get { return _fechaSalida; }
            set
            {
                _fechaSalida = value; OnPropertyChanged();
            }
        }
        private TimeSpan _horaIngreso { get; set; }

        public TimeSpan horaIngreso
        {
            get { return _horaIngreso; }
            set
            {
                _horaIngreso = value; OnPropertyChanged();
            }
        }
        private TimeSpan _horaSalidar { get; set; }

        public TimeSpan horaSalida
        {
            get { return _horaSalidar; }
            set
            {
                _horaSalidar = value; OnPropertyChanged();
            }
        }
        private string _observaciones { get; set; }

        public string observaciones
        {
            get { return _observaciones; }
            set
            {
                _observaciones = value; OnPropertyChanged();
            }
        }
        private string _mensualidad { get; set; }

        public string mensualidad
        {
            get { return _mensualidad; }
            set
            {
                _mensualidad = value; OnPropertyChanged();
            }
        }
        private DateTime _fechaUltimoPago { get; set; }

        public DateTime fechaUltimoPago
        {
            get { return _fechaUltimoPago; }
            set
            {
                _fechaUltimoPago = value; OnPropertyChanged();
            }
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
