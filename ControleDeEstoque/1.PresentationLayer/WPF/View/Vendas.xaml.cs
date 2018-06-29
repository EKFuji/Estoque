﻿using DAL.ModeloDeDados;
using Orientacao.Application.ApplicationImplementation;
using Orientacao.Application.FinalizarVConnection;
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
    /// Lógica interna para Vendas.xaml
    /// </summary>
    public partial class Vendas : Window
    {
        private string operacao;
        private readonly VendaApplication vendaApplication = new VendaApplication();
        private readonly CarrinhoApplication carrinhoApplication = new CarrinhoApplication();
        private readonly ProdutoApplication produtoApplication = new ProdutoApplication();
        private readonly FuncionarioApplication funcionarioApp = new FuncionarioApplication();
        private readonly FinalizarVenda finalizarVenda = new FinalizarVenda();
        private Venda venda = new Venda();
        private Carrinho carrinho;
        private Produto produto = new Produto();
        private List<Carrinho> carrinhos = new List<Carrinho>();
        private List<Venda> vendas = new List<Venda>();
        decimal valor;
        decimal valorFinal;


        public Vendas()
        {
            InitializeComponent();
            DesativaCampos();
            AlterarBotoes(1);
        }

        #region btn Adicionar qtde
        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            int adicionar = 0;
            if(txtQuantidade.Text != "")
            {
                adicionar = Convert.ToInt32(txtQuantidade.Text);
                adicionar += 1;
                txtQuantidade.Text = Convert.ToString(adicionar);
                valor = adicionar * produto.valorProduto;
                txtValorTot.Text = Convert.ToString(valor);
            }

        }
        #endregion

        #region btn Reduzir qtde
        private void btnReduzir_Click(object sender, RoutedEventArgs e)
        {
            int redu = 0;
            if (txtQuantidade.Text != "" && Convert.ToInt32(txtQuantidade.Text) > 0)
            {
                
                redu = Convert.ToInt32(txtQuantidade.Text);
                redu -= 1;
                txtQuantidade.Text = Convert.ToString(redu);
                valor = valor - produto.valorProduto;
                txtValorTot.Text = Convert.ToString(valor);
            }
        }
        #endregion

        #region Ativa Campos Venda
        public void AtivaCamposV()
        {
            dpData.IsEnabled = true;
            boxFuncPessoa.IsEnabled = true;
        }
        #endregion

        #region Ativa os Campos Carrinho
        private void AtivaCamposC()
        {
            boxProduto.IsEnabled = true;
            txtQuantidade.IsEnabled = true;
            btnAdicionar.IsEnabled = true;
            btnReduzir.IsEnabled = true;
        }
        #endregion

        #region Desativa Campos
        private void DesativaCampos()
        {
            dpData.IsEnabled = false;
            boxFuncPessoa.IsEnabled = false;
            boxProduto.IsEnabled = false;
            txtValorUnit.IsEnabled = false;
            txtValorTot.IsEnabled = false;
            txtQuantidade.IsEnabled = false;
            btnAdicionar.IsEnabled = false;
            btnReduzir.IsEnabled = false;
        }
        #endregion

        #region Desativa Campos Carrinho
        private void DesativaCamposC()
        {
            boxProduto.IsEnabled = false;
            txtQuantidade.IsEnabled = false;
            btnAdicionar.IsEnabled = false;
            btnReduzir.IsEnabled = false;
        }
        #endregion

        #region Desativa Campos Venda
        private void DesativaCamposV()
        {
            dpData.IsEnabled = false;
            boxFuncPessoa.IsEnabled = false;
        }
        #endregion

        #region Desativa Botões Carrinho
        private void DesativaBtnC()
        {
            btnEditar.IsEnabled = false;
            btnInserir.IsEnabled = false;
            btnExcluir.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnFinalizar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
        }
        #endregion

        #region Aletera os Botões
        private void AlterarBotoes(int op)
        {
            btnEditar.IsEnabled = false;
            btnInserir.IsEnabled = false;
            btnExcluir.IsEnabled = false;
            btnCancelar.IsEnabled = false;
            btnFinalizar.IsEnabled = false;
            btnSalvar.IsEnabled = false;
            if (op == 1)
            {
                //ativar opções iniciais
                btnInserir.IsEnabled = true;
                btnFinalizar.IsEnabled = true;
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

        #region Limpa Campos
        private void LimpaCampos()
        {
            if (dgListaV.IsLoaded == true)
            {
                dpData.SelectedDate = null;
                boxFuncPessoa.SelectedIndex = -1;
            }
            else if(dgListaC.IsLoaded == true)
            {
                boxProduto.SelectedIndex = -1;
                txtValorUnit.Clear();
                txtValorTot.Clear();
                txtQuantidade.Text = "0";
            }

        }
        #endregion

        #region Inserir
        private void btnInserir_Click(object sender, RoutedEventArgs e)
        {
            if (dgListaV.IsLoaded == true)
            {
                AtivaCamposV();
                DesativaCamposC();
            }
            else if(dgListaC.IsLoaded == true)
            {
                AtivaCamposC();
                DesativaCamposV();
            }
            this.operacao = "inserir";
            AlterarBotoes(2);
        }
        #endregion

        #region Salvar Venda
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            carrinho = new Carrinho();
            if(this.operacao == "inserir" && dgListaV.IsLoaded == true)
            {
                venda = new Venda();
                venda.FK_idFuncionario = (int)boxFuncPessoa.SelectedValue;
                venda.dtaVenda = Convert.ToDateTime(dpData.Text);
                vendaApplication.Salvar(venda);
                dgListaV.ItemsSource = vendaApplication.BuscarTodos();
                AlterarBotoes(1);
            }
            else if (this.operacao == "inserir" && dgListaC.IsLoaded == true)
            {
                
                carrinho.idProduto = (int)boxProduto.SelectedValue;
                produto.idProduto = carrinho.idProduto;
                produto = produtoApplication.BuscarProduto(x => x.idProduto == produto.idProduto);
                carrinho.valorParcial = valor;
                valorFinal += carrinho.valorParcial;
                carrinho.qtdeItensVenda = Convert.ToInt32(txtQuantidade.Text);
                carrinho.idVenda = venda.idVenda;
                carrinhos.Add(carrinho);
                dgListaC.ItemsSource = carrinhos.ToList();
                
                carrinhoApplication.SalvarCarrinho(carrinho);
                AlterarBotoes(1);
            }

            if(this.operacao == "alterar" && dgListaV.IsLoaded == true)
            {
                if(dgListaV.SelectedCells.ToList() != null)
                {
                    Venda v = (Venda)dgListaV.SelectedItem;
                    if(v.idVenda != 0)
                    {
                        vendaApplication.BuscarVenda(x => x.idVenda == v.idVenda);
                        venda.valorTotal = Convert.ToInt32(txtValorTot.Text);
                        venda.FK_idFuncionario = (int)boxFuncPessoa.SelectedValue;
                        venda.dtaVenda = Convert.ToDateTime(dpData.Text);
                        vendaApplication.Salvar(venda);
                        dgListaV.ItemsSource = vendaApplication.BuscarTodos();
                        
                    }
                }
            }

            else if (this.operacao == "alterar" && dgListaC.IsLoaded == true)
            {
                if(dgListaV.SelectedCells.ToList() != null)
                {
                    Carrinho c = (Carrinho)dgListaV.SelectedItem;
                    if(c.idCarrinho != 0)
                    {
                        
                        carrinhoApplication.BuscarCarrinho(x => x.idCarrinho == c.idCarrinho);
                        carrinho.idProduto = (int)boxProduto.SelectedValue;
                        produto.idProduto = carrinho.idProduto;
                        produto = produtoApplication.BuscarProduto(x => x.idProduto == produto.idProduto);
                        carrinho.valorParcial = valor;
                        carrinho.qtdeItensVenda = Convert.ToInt32(txtQuantidade.Text);
                        carrinhoApplication.SalvarCarrinho(carrinho);
                        carrinhoApplication.BuscarTodos();
                    }
                }
            }

        }

        #endregion

        #region Editar
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (dgListaV.IsLoaded == true)
            {
                AtivaCamposC();
               
            }
            else if(dgListaC.IsLoaded == true)
            {
                AtivaCamposC();
            }
            this.operacao = "alterar";
            AlterarBotoes(2);
        }
        #endregion

        #region Cancelar
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            AlterarBotoes(1);
            LimpaCampos();
            DesativaCampos();
        }

        #endregion

        #region Excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if(dgListaV.IsLoaded == true)
            {
                Venda v = (Venda)dgListaV.SelectedItem;
                vendaApplication.ExcluirVenda(venda);
                dgListaV.ItemsSource = vendaApplication.BuscarTodos();

                AlterarBotoes(1);
            }
            else if (dgListaC.IsLoaded == true)
            {
                Carrinho c = (Carrinho)dgListaV.SelectedItem;
                carrinhoApplication.ExcluirCarrinho(carrinho);
            }
        }
        #endregion

        #region Funcinario Grid Box MouseUP
        private void boxFuncPessoa_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Funcionario func = new Funcionario();
            Pessoa pessoa = new Pessoa();
            try
            {
                pessoa.idPessoa = (int)boxFuncPessoa.SelectedValue;
                func = funcionarioApp.BuscarFuncionario(x => x.FK_idPessoa == pessoa.idPessoa);
                dgListaV.ItemsSource = vendaApplication.BuscarPor(x => x.FK_idFuncionario == func.idFuncionario);
            }
            catch (Exception)
            {
                MessageBox.Show("Selecione o funcionario antes de Clickar nele!");
            }
        }
        #endregion

        #region Carregar boxFuncionario
        private void boxFuncPessoa_Loaded(object sender, RoutedEventArgs e)
        {
            boxFuncPessoa.ItemsSource = funcionarioApp.BuscarTodos();
        }
        #endregion

        #region Carregar boxProduto
        private void boxProduto_Loaded(object sender, RoutedEventArgs e)
        {
            boxProduto.ItemsSource = produtoApplication.BuscarTodos();
        }
        #endregion

        #region Produto Box MouseUP
        private void boxProduto_MouseUp(object sender, MouseButtonEventArgs e)
        {
            produto = new Produto();
            try
            {
                produto.idProduto = (int)boxProduto.SelectedValue;
                produto = produtoApplication.BuscarProduto(x => x.idProduto == produto.idProduto);
                txtValorUnit.Text = Convert.ToString(produto.valorProduto);
                txtQuantidade.Text = "0";
                txtValorTot.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("Selecione o produto antes de Clickar nele!");
            }

        }
        #endregion

        #region MouseUp Venda
        private void TabItem_MouseUp_Venda(object sender, MouseButtonEventArgs e)
        {
            DesativaCamposC();
            AlterarBotoes(1);
        }
        #endregion

        #region MouseUp Carrinho
        private void TabItem_MouseUp_Carrinho(object sender, MouseButtonEventArgs e)
        {
            DesativaCamposV();
            AlterarBotoes(1);
        }
        #endregion

        #region btn Finalizar Compra
        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            venda.valorTotal = valorFinal;
            vendaApplication.Salvar(venda);
            carrinhoApplication.FinalizarCompra();
            dgListaV.ItemsSource = vendaApplication.BuscarTodos();
            var car = carrinhoApplication.BuscarPor( x => x.idVenda == venda.idVenda);
            dgListaC.ItemsSource = car;
        }
        #endregion
    }
}
