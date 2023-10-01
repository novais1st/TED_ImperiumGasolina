using System;
using System.Collections.Generic;

namespace PostoGasolinaConsoleApp
{
    class Program
    {
        class Produto
        {
            public string Nome { get; set; }
            public decimal Preco { get; set; }
            public bool PermiteQuantidade { get; set; }
            public decimal Quantidade { get; set; }
        }

        class Cliente
        {
            public string Nome { get; set; }
            public string CPF { get; set; }
            public string Email { get; set; }
        }

        static void Main(string[] args)
        {
            List<Produto> produtos = new List<Produto>
            {
                new Produto { Nome = "Gasolina Comum", Preco = 5.50m, PermiteQuantidade = true },
                new Produto { Nome = "Gasolina Aditivada", Preco = 6.00m, PermiteQuantidade = true },
                new Produto { Nome = "Etanol", Preco = 4.00m, PermiteQuantidade = true },
                new Produto { Nome = "Diesel", Preco = 4.80m, PermiteQuantidade = true },
                new Produto { Nome = "Óleo Lubrificante", Preco = 20.00m, PermiteQuantidade = false },
                new Produto { Nome = "Lavagem Simples", Preco = 15.00m, PermiteQuantidade = false },
                new Produto { Nome = "Lavagem Completa", Preco = 25.00m, PermiteQuantidade = false },
                new Produto { Nome = "Bebidas (Refrigerante)", Preco = 3.50m, PermiteQuantidade = false },
                new Produto { Nome = "Lanches (Sanduíche)", Preco = 7.00m, PermiteQuantidade = false },
                new Produto { Nome = "Revista", Preco = 2.50m, PermiteQuantidade = false }
            };

            decimal precoTotal = 0;
            List<Produto> produtosEscolhidos = new List<Produto>();
            List<Produto> produtosQuantidade = new List<Produto>();
            Cliente cliente = null;

            while (true)
            {   
                Console.Clear();
                CriarLogo();
                ExibirProdutos(produtos);
                ExibirTotal(precoTotal);
                ExibirCarrinho(produtosEscolhidos.Skip(Math.Max(0, produtosEscolhidos.Count - 9)).ToList(),
                               produtosQuantidade.Skip(Math.Max(0, produtosQuantidade.Count - 9)).ToList());

                int escolha = ObterEscolhaUsuario(produtos.Count);

               

                if (escolha == 0)
                {
                    break;
                }

                if (escolha >= 1 && escolha <= produtos.Count)
                {
                    Produto produtoEscolhido = produtos[escolha - 1];

                    if (produtoEscolhido.PermiteQuantidade)
                    {
                        decimal quantidade = ObterQuantidadeProduto();
                        produtoEscolhido.Quantidade = quantidade;
                        produtosQuantidade.Add(produtoEscolhido);

                        Console.WriteLine($"\nVocê escolheu: {produtoEscolhido.Nome} - {quantidade} litros - R${produtoEscolhido.Preco:F2} por litro");
                        precoTotal += (produtoEscolhido.Preco * quantidade);
                    }
                    else
                    {
                        produtosEscolhidos.Add(produtoEscolhido);
                        Console.WriteLine($"\nVocê escolheu: {produtoEscolhido.Nome} - R${produtoEscolhido.Preco:F2}");
                        precoTotal += produtoEscolhido.Preco;
                    }
                }
                else
                {
                    Console.WriteLine("\nOpção inválida. Por favor, insira um número de produto válido.");
                }
            }
            static void ExibirCarrinho(List<Produto> produtosEscolhidos, List<Produto> produtosQuantidade)
            {
                Console.WriteLine("\nCarrinho de Compras:");

                foreach (var produto in produtosEscolhidos)
                {
                    Console.WriteLine($"{produto.Nome} - R${produto.Preco:F2}");
                }

                foreach (var produto in produtosQuantidade)
                {
                    Console.WriteLine($"{produto.Nome} - {produto.Quantidade} litros - R${produto.Preco:F2} por litro");
                }

                Console.WriteLine();
            }

            bool clienteCadastrado = PerguntarCadastro();

            if (clienteCadastrado)
            {
                cliente = ObterInformacoesCliente();
                AplicarDesconto(ref precoTotal);
            }

            ExibirOpcoesPagamento();

            int formaPagamento = ObterEscolhaUsuario(3);

            ProcessarPagamento(precoTotal, clienteCadastrado, formaPagamento);

            Console.WriteLine("\nPressione Enter para sair.");
            Console.ReadLine();
        }

        static void ExibirProdutos(List<Produto> produtos)
        {
            Console.WriteLine("Produtos disponíveis no posto Imperium Gasolina:");
            for (int i = 0; i < produtos.Count; i++)
            {
                if (produtos[i].PermiteQuantidade)
                {
                    Console.WriteLine($"{i + 1}. {produtos[i].Nome} - R${produtos[i].Preco:F2} por litro");
                }
                else
                {
                    Console.WriteLine($"{i + 1}. {produtos[i].Nome} - R${produtos[i].Preco:F2}");
                }
            }
        }

        static void ExibirTotal(decimal precoTotal)
        {
            Console.WriteLine($"\nTotal a Pagar: R${precoTotal:F2}\n");
        }

        static int ObterEscolhaUsuario(int maxOpcao)
        {
            int escolha;
            do
            {
                Console.Write("Digite o número do produto desejado (ou 0 para sair): ");
            } while (!int.TryParse(Console.ReadLine(), out escolha) || (escolha < 0 || escolha > maxOpcao));

            return escolha;
        }

        static decimal ObterQuantidadeProduto()
        {
            decimal quantidade;
            do
            {
                Console.Write("Digite a quantidade desejada (em litros): ");
            } while (!decimal.TryParse(Console.ReadLine(), out quantidade) || quantidade <= 0);

            return quantidade;
        }

        static bool PerguntarCadastro()
        {
            Console.Write("\nDeseja se cadastrar? (S para Sim, N para Não): ");
            string respostaCadastro = Console.ReadLine();
            return respostaCadastro.Equals("S", StringComparison.OrdinalIgnoreCase);
        }

        static Cliente ObterInformacoesCliente()
        {
            Console.WriteLine("\nPor favor, insira suas informações:");
            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("CPF: ");
            string cpf = Console.ReadLine();

            Console.Write("E-mail: ");
            string email = Console.ReadLine();

            return new Cliente { Nome = nome, CPF = cpf, Email = email };
        }

        static void AplicarDesconto(ref decimal precoTotal)
        {
            decimal desconto = 15;
            decimal valorDesconto = (precoTotal * desconto) / 100;
            precoTotal -= valorDesconto;
            Console.WriteLine($"\nCadastro realizado com sucesso! Desconto de {desconto}% aplicado. Novo Total: R${precoTotal:F2}");
        }


        static void CriarLogo()
        {
            Console.WriteLine("********************************************************************");
            Console.WriteLine("************************ Imperium Gasolina *************************");
            Console.WriteLine("********************************************************************\n");
        }
        static void ExibirOpcoesPagamento()
        {
            Console.WriteLine("\nOpções de pagamento:");
            Console.WriteLine("1. Débito");
            Console.WriteLine("2. Crédito");
            Console.WriteLine("3. Dinheiro");
        }

        static void ProcessarPagamento(decimal precoTotal, bool clienteCadastrado, int formaPagamento)
        {
            switch (formaPagamento)
            {
                case 1:
                    Console.WriteLine("\nPagamento em Débito bem-sucedido!");
                    break;
                case 2:
                    Console.WriteLine("\nPagamento em Crédito bem-sucedido!");
                    break;
                case 3:
                    if (clienteCadastrado)
                    {
                        Console.Write("Digite o valor pago em dinheiro: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal valorPago))
                        {
                            if (valorPago >= precoTotal)
                            {
                                decimal troco = valorPago - precoTotal;
                                Console.WriteLine($"\nPagamento em Dinheiro bem-sucedido! Troco: R${troco:F2}");
                            }
                            else
                            {
                                Console.WriteLine("\nValor pago insuficiente.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nValor inválido. Por favor, insira um valor numérico.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nVocê escolheu pagamento em dinheiro, mas não se cadastrou para obter o desconto. Total a pagar: R$" + precoTotal);
                    }
                    break;
                default:
                    Console.WriteLine("\nOpção de pagamento inválida.");
                    break;
            }
        }
    }
}
