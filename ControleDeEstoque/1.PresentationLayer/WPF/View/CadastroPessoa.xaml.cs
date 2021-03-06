﻿using DAL.ModeloDeDados;
using Orientacao.Application.ApplicationImplementation;
using System;
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
using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Lógica interna para CadastroPessoa.xaml
    /// </summary>
    public partial class CadastroPessoa : Window
    {
        private readonly PessoaApplication application = new PessoaApplication();
        private Pessoa pessoa;
        private string operacao;

        public CadastroPessoa()
        {
            InitializeComponent();
            txtEmail.IsEnabled = false;
            txtNome.IsEnabled = false;
            txtCelular.IsEnabled = false;
            dpNascim.IsEnabled = false;

        }

        #region AtivaCampos
        private void AtivaCampos()
        {
            txtEmail.IsEnabled = true;
            txtNome.IsEnabled = true;
            txtCelular.IsEnabled = true;
            dpNascim.IsEnabled = true;
        }
        #endregion  

        #region Limpar campos
        private void LimpaCampos()
        {
            txtNome.Clear();
            txtCelular.Clear();
            txtEmail.Clear();
            dpNascim.SelectedDate = null;
        }
        #endregion

        #region Alternar Botões
        private void AlternarBotoes(int op)
        {
            btnEditar.IsEnabled = false;
            btnInserir.IsEnabled = false;
            btnExcluir.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnBuscar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            if (op == 1)
            {
                //ativar opções iniciais
                btnInserir.IsEnabled = true;
                btnBuscar.IsEnabled = true;
            }
            if (op == 2)
            {
                //inserir um valor
                btnSalvar.IsEnabled = true;
                btnCancelar.IsEnabled = true;
            }
            if (op == 3)
            {
                
                btnEditar.IsEnabled = true;
                btnExcluir.IsEnabled = true;
            }

        }
        #endregion

        #region Salvar Pessoa
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            AtivaCampos();
            
            if (this.operacao == "inserir")
            {
                pessoa = new Pessoa();
                pessoa.nomePessoa = txtNome.Text;
                pessoa.celPessoa = Convert.ToDecimal(txtCelular.Text);
                pessoa.email = txtEmail.Text;
                pessoa.dtaNascimento = Convert.ToDateTime(dpNascim.Text);
                application.SalvarPessoa(pessoa);
                AlternarBotoes(1);
                dgListaPessoa.ItemsSource = application.BuscarTodos();
                editarGrid();
            }
            if (this.operacao == "alterar")
            {
                    if (dgListaPessoa.SelectedCells.ToList() != null && txtNome.Text != "")
                    {
                        Pessoa p = (Pessoa)dgListaPessoa.SelectedItem;
                        if (p.idPessoa != 0)
                        {
                            pessoa = application.BuscarPessoa(x => x.idPessoa == p.idPessoa);
                            pessoa.nomePessoa = txtNome.Text;
                            pessoa.celPessoa = Convert.ToDecimal(txtCelular.Text);
                            pessoa.email = txtEmail.Text;
                            pessoa.dtaNascimento = Convert.ToDateTime(dpNascim.Text);

                        application.SalvarPessoa(pessoa);
                        }
                    }
                
                dgListaPessoa.ItemsSource = application.BuscarTodos();
                editarGrid();

            }
            
        }
        #endregion

        #region Loaded Grid Lista Pessoas
        private void dgListaPessoa_Loaded(object sender, RoutedEventArgs e)
        {
            dgListaPessoa.ItemsSource = application.BuscarTodos();
            dgListaPessoa.Columns[0].IsReadOnly = true;
            dgListaPessoa.Columns[0].Header = "ID";
            dgListaPessoa.Columns[1].Header = "Pessoa";
            dgListaPessoa.Columns[2].Header = "Telefone";
            dgListaPessoa.Columns[2].MinWidth = 96;
            dgListaPessoa.Columns[2].MaxWidth = 96;
            dgListaPessoa.Columns[3].Header = "Nascimento";
            dgListaPessoa.Columns[3].MinWidth = 96;
            dgListaPessoa.Columns[3].MaxWidth = 96;
            dgListaPessoa.Columns[4].Header = "Email";
            dgListaPessoa.Columns[5].Visibility = Visibility.Hidden;
            dgListaPessoa.Columns[6].Visibility = Visibility.Hidden;
            AlternarBotoes(1);
        }
        #endregion

        #region Inserir
        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            AtivaCampos();
            this.operacao = "inserir";
            AlternarBotoes(2);
        }
        #endregion

        #region Editar Pessoa
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            AtivaCampos();
            this.operacao = "alterar";
            AlternarBotoes(2);

        }
        #endregion

        #region Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            AlternarBotoes(1);
            LimpaCampos();
        }

        #endregion

        #region Excluir Pessoa
        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            Pessoa p = (Pessoa)dgListaPessoa.SelectedItem;
            pessoa = application.BuscarPessoa(x => x.idPessoa == p.idPessoa);
            application.ExcluirPessoa(pessoa);
            dgListaPessoa.ItemsSource = application.BuscarTodos();
            editarGrid();
            AlternarBotoes(1);
        }
        #endregion

        #region MouseDoubleClick Grid ListaPessoa
        private void dgListaPessoa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AtivaCampos();
            if (dgListaPessoa.SelectedIndex >= 0)
            {
                
                Pessoa p = (Pessoa)dgListaPessoa.SelectedItem;
                txtNome.Text = p.nomePessoa;
                txtCelular.Text = p.celPessoa.ToString();
                txtEmail.Text = p.email;
                dpNascim.Text = p.dtaNascimento.ToString();
                this.AlternarBotoes(3);

            }
        }
        #endregion

        #region Método que bloqueia a edição se a pessoa só der um clique
        private void dgListaPessoa_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dgListaPessoa.Columns[0].IsReadOnly = true;
            dgListaPessoa.Columns[1].IsReadOnly = true;
            dgListaPessoa.Columns[2].IsReadOnly = true;
            dgListaPessoa.Columns[3].IsReadOnly = true;
            dgListaPessoa.Columns[4].IsReadOnly = true;
            dgListaPessoa.Columns[5].IsReadOnly = true;
            dgListaPessoa.Columns[6].IsReadOnly = true;
        }
        #endregion

        #region Buscar Pessoa
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            AtivaCampos();
            pessoa = new Pessoa();
            dpNascim.IsEnabled = false;
            //buscar por nome
            if (txtNome.Text.Trim().Count() > 0)
            {
                pessoa.nomePessoa = txtNome.Text;
                dgListaPessoa.ItemsSource = application.BuscarPor(x => x.nomePessoa.Contains(pessoa.nomePessoa));
                
            }        
            //buscar por celular
            if (txtCelular.Text.Trim().Count() > 0)
            {
                pessoa.celPessoa = Convert.ToDecimal(txtCelular.Text);
                dgListaPessoa.ItemsSource = application.BuscarPor(x => x.celPessoa.ToString().Contains(pessoa.celPessoa.ToString()));
                
            }
            //buscar por Email
            if (txtEmail.Text.Trim().Count() > 0)
            {
                pessoa.email = txtEmail.Text;
                dgListaPessoa.ItemsSource = application.BuscarPor(x => x.email.Contains(pessoa.email));
                
            }

            editarGrid();
        }
        #endregion

        #region EditarGrid
        private void editarGrid()
        {
            dgListaPessoa.Columns[0].IsReadOnly = true;
            dgListaPessoa.Columns[0].Header = "ID";
            dgListaPessoa.Columns[1].Header = "Pessoa";
            dgListaPessoa.Columns[2].Header = "Telefone";
            dgListaPessoa.Columns[2].MinWidth = 96;
            dgListaPessoa.Columns[2].MaxWidth = 96;
            dgListaPessoa.Columns[3].Header = "Nascimento";
            dgListaPessoa.Columns[3].MinWidth = 96;
            dgListaPessoa.Columns[3].MaxWidth = 96;
            dgListaPessoa.Columns[4].Header = "Email";
            dgListaPessoa.Columns[5].Visibility = Visibility.Hidden;
            dgListaPessoa.Columns[6].Visibility = Visibility.Hidden;
        }

        #endregion

        #region Formata data
        private void dgListaPessoa_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "dtaNascimento")
            {
                DataGridTextColumn column = e.Column as DataGridTextColumn;
                Binding binding = column.Binding as Binding;
                binding.StringFormat = "dd/MM/yyyy";
            }
        }
        #endregion
    }
}
