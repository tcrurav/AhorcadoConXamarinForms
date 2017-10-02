using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AhorcadoConXamarinForms
{
    public partial class MainPage : ContentPage
    {
        readonly string[] PALABRAS = { "PALABROTA", "TOCINO", "ESPAÑA" };
        readonly string[] LETRAS = {
            "A", "B", "C", "D", "E", "F", "G",
            "H", "I", "J", "K", "L", "M", "N",
            "Ñ", "O", "P", "Q", "R", "S", "T",
            "U", "V", "W", "X", "Y", "Z", "*",
        };
        string palabraAAdivinar;
        string palabraAdivinadaPorAhora;
        int numFallos;
        int numAciertos;

        public MainPage()
        {
            InitializeComponent();

            Inicializar();
        }

        private void Inicializar()
        {
            numFallos = 0;
            numAciertos = 0;

            imageAhorcado.Source = ImageSource.FromResource("AhorcadoConXamarinForms.img.0.png");

            InicializarPalabraSecreta();

            InicializarBotonera();
        }

        private void InicializarBotonera()
        {
            for(var fila = 0; fila < 4; fila++)
            {
                for(var columna = 0; columna < 7; columna++)
                {
                    Button boton = new Button
                    {
                        Text = LETRAS[fila * 7 + columna]
                    };
                    gridBotonera.Children.Add(boton, columna, fila);

                    boton.Clicked += Boton_ClickedAsync;
                }
            }
        }

        private async void Boton_ClickedAsync(object sender, EventArgs e)
        {
            Button botonPulsado = (Button)sender;
            string letraPulsada = botonPulsado.Text;
            if (letraAcertada(letraPulsada))
            {
                labelPalabraSecreta.Text = palabraAdivinadaPorAhora;
                botonPulsado.BackgroundColor = Color.Green;
                
                if (numAciertos == palabraAAdivinar.Length)
                {
                    await DisplayAlert("Has ganado", "Pulsa Ok para jugar otra vez.", "Ok");
                    Inicializar();
                }
            } else
            {
                botonPulsado.BackgroundColor = Color.Red;
                imageAhorcado.Source = ImageSource.FromResource("AhorcadoConXamarinForms.img." + numFallos + ".png");
                numFallos++;
                if(numFallos == 6)
                {
                    await DisplayAlert("Has perdido", "Pulsa Ok para jugar otra vez.", "Ok");
                    Inicializar();
                }
            }

        }

        private bool letraAcertada(string letraPulsada)
        {
            bool hayAcierto = false;

            for(var i = 0; i < palabraAAdivinar.Length; i++)
            {
                if(palabraAAdivinar.Substring(i, 1) == letraPulsada)
                {
                    palabraAdivinadaPorAhora =
                        palabraAdivinadaPorAhora.Substring(0, i * 2)+
                        letraPulsada +
                        palabraAdivinadaPorAhora.Substring(i * 2 + 1);
                    hayAcierto = true;
                    numAciertos++;
                }
            }

            return hayAcierto;
        }

        private void InicializarPalabraSecreta()
        {
            Random aleatorio = new Random();
            int numeroPalabraAAdivinar = aleatorio.Next(0, PALABRAS.Length);
            palabraAAdivinar = PALABRAS[numeroPalabraAAdivinar];

            palabraAdivinadaPorAhora = "";

            for(var i = 0; i < palabraAAdivinar.Length; i++)
            {
                palabraAdivinadaPorAhora += "- ";
            }

            labelPalabraSecreta.Text = palabraAdivinadaPorAhora;
        }


    }
}
