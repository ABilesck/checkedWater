using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using prjCheckedWater2.model;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using prjCheckedWater2.Resources;
using prjCheckedWater2.Resources.model;

namespace prjCheckedWater2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        EditText txtTitulo;
        EditText txtDescricao;

        ListView lvDadosDenuncia;
        List<Denuncia> denuncias = new List<Denuncia>();
        List<Voto> votos = new List<Voto>();

        Button btnVotoPositivo;
        Button btnVotoNegativo;
        Button btnVisualizarMapa;
        Button btnDenunciar;

        long denunciaSelecionada = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Platform.Init(this, savedInstanceState);

            lvDadosDenuncia = FindViewById<ListView>(Resource.Id.lvDados);

            txtTitulo = FindViewById<EditText>(Resource.Id.txtTitulo);
            txtDescricao = FindViewById<EditText>(Resource.Id.txtDescricao);

            btnDenunciar = FindViewById<Button>(Resource.Id.btnDenunciar);
            btnVotoPositivo = FindViewById<Button>(Resource.Id.btnVotoPositivo);
            btnVotoNegativo = FindViewById<Button>(Resource.Id.btnVotoNegativo);
            btnVisualizarMapa = FindViewById<Button>(Resource.Id.btnVisualizarMapa);

            CarregarDados();

            btnDenunciar.Click += btnDenunciar_click;

            btnVotoPositivo.Click += delegate
            {
                Votar(1);
            };

            btnVotoNegativo.Click += delegate
            {
                Votar(0);
            };

            txtTitulo.Click += TxtTitulo_Click;

            lvDadosDenuncia.ItemClick += (s, e) =>
            {
                denunciaSelecionada = e.Id;
                foreach (Voto voto in votos)
                {
                    if(voto.IdDenuncia == e.Id 
                        && voto.IdUsuario == Usuario.UsuarioLogado.Id)
                    {
                        HabilitarVotos(false);
                        btnVisualizarMapa.Enabled = true;
                        return;
                    }
                }
                HabilitarVotos(true);
                btnVisualizarMapa.Enabled = true;
            };

            btnVisualizarMapa.Click += VisualizarMapa_click;

        }

        private void TxtTitulo_Click(object sender, EventArgs e)
        {
            denunciaSelecionada = 0;
            HabilitarVotos(false);
            btnVisualizarMapa.Enabled = false;
        }

        private void HabilitarVotos(bool v)
        {
            btnVotoPositivo.Enabled = v;
            btnVotoNegativo.Enabled = v;
        }

        private async void btnDenunciar_click(object sender, EventArgs e)
        {
            try
            {
                btnDenunciar.Enabled = false;
                var coods = await Geolocation.GetLocationAsync();
                double lat = coods.Latitude;
                double lon = coods.Longitude;

                Denuncia denuncia = new Denuncia
                {
                    Latitude = lat,
                    Longitude = lon
                };

                denuncia.Titulo = txtTitulo.Text;
                denuncia.Descricao = txtDescricao.Text;

                LimparCampos();
                

                HttpClient client = new HttpClient();

                string json = JsonConvert.SerializeObject(denuncia);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = "https://grugol.000webhostapp.com/API/denuncia/criarNova.php";
                var result = await client.PostAsync(uri, content);

                // Se ocorrer um erro lança uma exceção
                result.EnsureSuccessStatusCode();

                Toast.MakeText(this, "Denúncia feita com sucesso!", ToastLength.Short).Show();
                CarregarDados();
                btnDenunciar.Enabled = true;
            }
            catch (HttpRequestException ex)
            {
                btnDenunciar.Enabled = true;
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alerta = builder.Create();

                alerta.SetTitle("Erro!");
                alerta.SetIcon(Android.Resource.Drawable.StatNotifyError);
                alerta.SetMessage($"Ocorreu um erro de conexão ao carregar os dados\n{ex.Message}");
                alerta.SetButton("OK", (s, ev) =>
                {

                });
                alerta.Show();
                //Toast.MakeText(this, $"Ocorreu um erro de conexão ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                btnDenunciar.Enabled = true;
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alerta = builder.Create();

                alerta.SetTitle("Erro!");
                alerta.SetIcon(Android.Resource.Drawable.StatNotifyError);
                alerta.SetMessage($"Ocorreu um erro inesperado ao carregar os dados\n{ex.Message}");
                alerta.SetButton("OK", (s, ev) =>
                {

                });
                alerta.Show();
                //Toast.MakeText(this, $"Ocorreu um erro inesperado ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
            }

        }

        private async void Votar(int tipo)
        {
            try
            {
                Voto voto = new Voto()
                {
                    IdUsuario = Usuario.UsuarioLogado.Id,
                    IdDenuncia = (int)denunciaSelecionada,
                    tipoVoto = tipo
                };

                HttpClient client = new HttpClient();

                string json = JsonConvert.SerializeObject(voto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = "https://grugol.000webhostapp.com/API/votos/votar.php";
                var result = await client.PostAsync(uri, content);

                // Se ocorrer um erro lança uma exceção
                result.EnsureSuccessStatusCode();

                Toast.MakeText(this, "Voto realizado com sucesso!", ToastLength.Short).Show();
                TxtTitulo_Click(null, null);
                CarregarDados();
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
                //Toast.MakeText(this, $"Ocorreu um erro de conexão ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
            }
            catch (Exception ex)
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
                //Toast.MakeText(this, $"Ocorreu um erro inesperado ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
            }
        }

        private async void CarregarDados()
        {
            try
            {
                HttpClient client = new HttpClient();
                // envia a requisição GET
                var uri = "https://grugol.000webhostapp.com/API/denuncia/LerTodos.php";
                var result = await client.GetStringAsync(uri);

                CarregarVotos();

                // processa a resposta
                var posts = JsonConvert.DeserializeObject<List<Denuncia>>(result);
                denuncias = posts;
                var adapter = new DenunciaListAdapter(this, denuncias);
                lvDadosDenuncia.Adapter = adapter;
            }
            catch (JsonSerializationException)
            {
                Toast.MakeText(this, "Nenhuma denuncia foi encontrada!", ToastLength.Long).Show();
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
                //Toast.MakeText(this, $"Ocorreu um erro de conexão ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
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
                //Toast.MakeText(this, $"Ocorreu um erro inesperado ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
            }
            
        }

        private async void CarregarVotos()
        {
            try
            {
                HttpClient client = new HttpClient();

                var uri2 = "https://grugol.000webhostapp.com/API/votos/LerPorDenuncia.php";
                var result2 = await client.GetStringAsync(uri2);
                var resultVotos = JsonConvert.DeserializeObject<List<Voto>>(result2);
                votos = resultVotos;

            }
            catch (JsonSerializationException)
            {
                
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
                //Toast.MakeText(this, $"Ocorreu um erro de conexão ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
            }
            catch (Exception ex)
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
                //Toast.MakeText(this, $"Ocorreu um erro inesperado ao carregar os dados\n{ex.Message}", ToastLength.Long).Show();
            }
        }

        private void LimparCampos()
        {
            txtTitulo.Text = "";
            txtDescricao.Text = "";
        }

        private async void VisualizarMapa_click(object sender, EventArgs e)
        {
            try
            {
                foreach (Denuncia denuncia in denuncias)
                {
                    if(denunciaSelecionada == denuncia.ID)
                    {
                        var location = new Location(denuncia.Latitude, denuncia.Longitude);
                        var options = new MapLaunchOptions
                        {
                            Name = denuncia.Titulo
                        };
                        await Map.OpenAsync(location, options);
                    }
                }
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, $"Erro ao abrir o mapa\n{ex.Message}", ToastLength.Long).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults); base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

