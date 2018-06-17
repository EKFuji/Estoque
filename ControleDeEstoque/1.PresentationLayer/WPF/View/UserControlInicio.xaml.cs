﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Interação lógica para UserControlInicio.xam
    /// </summary>
    public partial class UserControlInicio : UserControl
    {
        public UserControlInicio()
        {
            InitializeComponent();
        }


        private void btnCategoria_Click(object sender, RoutedEventArgs e)
        {
            CadastroCategoria cadasCategoria = new CadastroCategoria();
            cadasCategoria.ShowDialog();
        }

        private void btnProduto_Click(object sender, RoutedEventArgs e)
        {
            CadastroPessoa cadasPessoa = new CadastroPessoa();
            cadasPessoa.ShowDialog();
        }

        private void btnPessoa_Click(object sender, RoutedEventArgs e)
        {
            CadastroPessoa cadasPessoa = new CadastroPessoa();
            cadasPessoa.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CadastroUsuario cadasUser = new CadastroUsuario();
            cadasUser.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CadastroFuncionario cadasFunc = new CadastroFuncionario();
            cadasFunc.ShowDialog();
        }
    }
}
