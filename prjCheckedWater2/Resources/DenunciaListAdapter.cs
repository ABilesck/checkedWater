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
using prjCheckedWater2.model;

namespace prjCheckedWater2.Resources
{
    class DenunciaListAdapter : BaseAdapter
    {
        private readonly Activity context;
        private readonly List<Denuncia> denuncias;

        public DenunciaListAdapter(Activity context, List<Denuncia> denuncias)
        {
            this.context = context;
            this.denuncias = denuncias;
        }

        public override int Count
        {
            get
            {
                return this.denuncias.Count;
            }
        }

        public override long GetItemId(int position)
        {
            int id = Convert.ToInt32(denuncias[position].ID);
            return id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.ListViewLayoutDenuncia, parent, false);

            var lvtxtTitulo = view.FindViewById<TextView>(Resource.Id.txtViewTitulo);
            var lvtxtDescricao = view.FindViewById<TextView>(Resource.Id.txtViewDescricao);
            var lvtxtPositivos = view.FindViewById<TextView>(Resource.Id.txtViewVotosPositivos);
            var lvtxtNegativos = view.FindViewById<TextView>(Resource.Id.txtViewVotosNegativos);


            lvtxtTitulo.Text = this.denuncias[position].Titulo;
            lvtxtDescricao.Text = this.denuncias[position].Descricao;
            lvtxtPositivos.Text = this.denuncias[position].VotosPositivos.ToString();
            lvtxtNegativos.Text = this.denuncias[position].VotosNegativos.ToString();

            return view;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }
    }
}