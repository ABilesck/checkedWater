using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace prjCheckedWater2.Resources.model
{
    class Usuario
    {
        public static Usuario UsuarioLogado = null;



        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("cep")]
        public string CEP { get; set; }

        [JsonProperty("nivelDeAcesso")]
        public int NivelDeAcesso { get; set; }

        public Usuario()
        {
            Id = 0;
            Nome = "";
            Senha = "";
            Telefone = "";
            Email = "";
            CEP = "";
            NivelDeAcesso = -1;
        }
    }
}