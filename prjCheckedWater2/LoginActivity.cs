using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using prjCheckedWater2.model;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using prjCheckedWater2.Resources;
using prjCheckedWater2.Resources.model;

namespace prjCheckedWater2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class LoginActivity : Activity
    {

        List<Usuario> usuarios = new List<Usuario>();

        EditText txtEmail;
        EditText txtSenha;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.login);

            CarregarDados();

            txtEmail = FindViewById<EditText>(Resource.Id.txtLoginEmail);
            txtSenha = FindViewById<EditText>(Resource.Id.txtloginSenha);

            var botao = FindViewById<Button>(Resource.Id.btnLogar);
            var link = FindViewById<TextView>(Resource.Id.lblCriarConta);

            botao.Click += Login;

            link.Click += Link_Click;
        }

        private void Link_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(CriarContaActivity));
        }

        async void CarregarDados()
        {
            try
            {
                HttpClient client = new HttpClient();

                string uri = "https://grugol.000webhostapp.com/API/usuario/LerTodos.php";
                var result = await client.GetStringAsync(uri);

                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(result);
            }
            catch (HttpRequestException ex)
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alerta = builder.Create();

                alerta.SetTitle("Erro!");
                alerta.SetIcon(Android.Resource.Drawable.StatNotifyError);
                alerta.SetMessage($"Ocorreu um erro de conexão ao carregar os dados\n{ex.Message}");
                alerta.SetButton("OK", (s, ev) =>
                {

                });
                alerta.Show();
            }
            catch(Exception ex)
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alerta = builder.Create();

                alerta.SetTitle("Erro!");
                alerta.SetIcon(Android.Resource.Drawable.StatNotifyError);
                alerta.SetMessage($"Ocorreu um erro inesperado ao carregar os dados\n{ex.Message}");
                alerta.SetButton("OK", (s, ev) =>
                {

                });
                alerta.Show();
            }
        }

        private void Login(object sender, EventArgs e)
        {
            CarregarDados();

            foreach (Usuario usuario in usuarios)
            {
                if(usuario.Email.Equals(txtEmail.Text) && usuario.Senha.Equals(txtSenha.Text))
                {
                    Usuario.UsuarioLogado = usuario;
                    StartActivity(typeof(MainActivity));
                }
            }
            if(Usuario.UsuarioLogado == null)
                Toast.MakeText(this, "Credenciais incorretas!", ToastLength.Long).Show();
        }
    }
}