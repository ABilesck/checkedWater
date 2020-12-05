using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using prjCheckedWater2.Resources.model;

namespace prjCheckedWater2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class CriarContaActivity : Activity
    {

        EditText txtEmail;
        EditText txtSenha;
        EditText txtNome;
        EditText txtTelefone;
        EditText txtCep;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.criarConta);

            txtEmail = FindViewById<EditText>(Resource.Id.txtEmail);
            txtSenha = FindViewById<EditText>(Resource.Id.txtSenha);
            txtNome = FindViewById<EditText>(Resource.Id.txtNome);
            txtTelefone = FindViewById<EditText>(Resource.Id.txtTelefone);
            txtCep= FindViewById<EditText>(Resource.Id.txtCEP);

            var botao = FindViewById<Button>(Resource.Id.btnCriar);

            botao.Click += Botao_Click;
        }

        private async void Botao_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario novoUsuario = new Usuario();
                novoUsuario.Nome = txtNome.Text;

                string json = JsonConvert.SerializeObject(novoUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string uri = "https://grugol.000webhostapp.com/API/usuario/criar.php";

                HttpClient client = new HttpClient();
                var result = await client.PostAsync(uri, content);

                //Se ocorrer um erro lança uma exceção
                result.EnsureSuccessStatusCode();

                Toast.MakeText(this, "Cadastrado com sucesso!", ToastLength.Long).Show();
                
                Usuario.UsuarioLogado = novoUsuario;
                StartActivity(typeof(MainActivity));
            }
            catch (HttpRequestException ex)
            {
                AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                AlertDialog alerta = builder.Create();

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
                AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                AlertDialog alerta = builder.Create();

                alerta.SetTitle("Erro!");
                alerta.SetIcon(Android.Resource.Drawable.StatNotifyError);
                alerta.SetMessage($"Ocorreu um erro inesperado\n{ex.Message}");
                alerta.SetButton("OK", (s, ev) =>
                {

                });
                alerta.Show();
            }
        }
    }
}