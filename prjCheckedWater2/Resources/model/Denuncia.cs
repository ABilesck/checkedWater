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

namespace prjCheckedWater2.model
{
    class Denuncia
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("votosPositivos")]
        public int VotosPositivos { get; set; }

        [JsonProperty("votosNegativos")]
        public int VotosNegativos { get; set; }

        [JsonProperty("aprovadoPorAdmin")]
        public bool AprovadoPorAdmin { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        public Denuncia()
        {
            ID = 0;
            Titulo = "titulo";
            Descricao = "descricao";
            VotosPositivos = 1;
            VotosNegativos = 0;
            AprovadoPorAdmin = false;
            Latitude = 0;
            Longitude = 0;
        }
    }
}