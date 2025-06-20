using System;
using System.Collections.Generic;

namespace Restaurante
{
    class Item_Pedido
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }

    class Pedido
    {
        public int IdCliente { get; set; }
        public List<Item_Pedido> Items { get; set; } = new List<Item_Pedido>();
        public string Status { get; set; } = "Pendente";
    }

    class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cliente[] clientes = new Cliente[100];
            int contador = 0;

            Queue<Pedido> pedidos = new Queue<Pedido>();
            Stack<Pedido> cancelados = new Stack<Pedido>();
            List<Pedido> processados = new List<Pedido>();

            string[] opcoes = { "Hambúrguer", "Batata Frita", "Refrigerante", "Salada" };
            decimal[] precos = { 15.00m, 7.50m, 5.00m, 9.00m };

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("--- Sistema de Pedidos ---");
                    Console.WriteLine("1) Cadastrar Cliente");
                    Console.WriteLine("2) Listar Clientes");
                    Console.WriteLine("3) Listar Menu");
                    Console.WriteLine("4) Fazer Pedido");
                    Console.WriteLine("5) Processar Pedido");
                    Console.WriteLine("6) Cancelar Pedido");
                    Console.WriteLine("7) Refazer Pedido Cancelado");
                    Console.WriteLine("8) Status do Pedido");
                    Console.WriteLine("0) Sair");
                    Console.Write("Opção: ");

                    string line = Console.ReadLine();
                    int opc;
                    if (!int.TryParse(line, out opc))
                        throw new ApplicationException("Opção invalida");

                    switch (opc)
                    {
                        case 1:
                            Console.Clear();
                            if (contador >= clientes.Length)
                            {
                                Console.WriteLine("O limite de clientes foi atingido");
                                break;
                            }
                            Console.Write("Digite o nome do Cliente: ");
                            string nome = Console.ReadLine();
                            if (nome == null || nome.Length == 0)
                                throw new ApplicationException("O nome do cliente não pode ser vazio");
                            clientes[contador] = new Cliente { Id = contador + 1, Nome = nome };
                            Console.WriteLine("Cliente cadastrado, seu ID é: " + clientes[contador].Id);
                            contador++;
                            break;

                        case 2:
                            Console.Clear();
                            if (contador == 0)
                            {
                                Console.WriteLine("Nenhum cliente cadastrado no momento");
                                break;
                            }
                            Console.WriteLine("--- Lista de Clientes e Total Gasto ---");
                            for (int i = 0; i < contador; i++)
                            {
                                int id = clientes[i].Id;
                                decimal totalGasto = 0m;
                                for (int j = 0; j < processados.Count; j++)
                                {
                                    Pedido p = processados[j];
                                    if (p.IdCliente == id)
                                    {
                                        foreach (var it in p.Items)
                                        {
                                            bool Hamb = false;
                                            if (it.Nome.Length >= 2 && it.Nome[0] == 'x' && it.Nome[1] == '-')
                                                Hamb = true;
                                            if (Hamb)
                                                totalGasto += it.Quantidade * precos[0];
                                            else if (it.Nome == "Batata Frita")
                                                totalGasto += it.Quantidade * precos[1];
                                            else if (
                                                it.Nome == "Guaraná" ||
                                                it.Nome == "Coca-Cola" ||
                                                it.Nome == "Pepsi" ||
                                                it.Nome == "Sprite" ||
                                                it.Nome == "Fanta Uva" ||
                                                it.Nome == "Fanta Laranja")
                                                totalGasto += it.Quantidade * precos[2];
                                            else if (it.Nome == "Salada")
                                                totalGasto += it.Quantidade * precos[3];
                                        }
                                    }
                                }
                                Console.WriteLine("ID " + id + " – " + clientes[i].Nome + " – R$ " + totalGasto.ToString("F2"));
                            }
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine("-- Menu Principal --");
                            for (int i = 0; i < opcoes.Length; i++)
                                Console.WriteLine((i + 1) + ") " + opcoes[i] + " – R$ " + precos[i].ToString("F2"));
                            Console.WriteLine("-- Opções de Hambúrguer --");
                            Console.WriteLine("1) x-salada\n2) x-bacon\n3) x-hotdog\n4) x-burguer\n5) x-frango\n6) x-tudo");
                            Console.WriteLine("\n-- Opções de Refrigerante --");
                            Console.WriteLine("1) Guaraná\n2) Coca-Cola\n3) Pepsi\n4) Sprite\n5) Fanta Uva\n6) Fanta Laranja");
                            break;

                        case 4:
                            Console.Clear();
                            Console.WriteLine("-- Fazer Pedido --");
                            if (contador == 0)
                                throw new ApplicationException("Não há clientes cadastrados para fazer pedido.");
                            Console.WriteLine("--- Clientes Cadastrados ---");
                            for (int i = 0; i < contador; i++)
                                Console.WriteLine("ID " + clientes[i].Id + " – " + clientes[i].Nome);
                            Console.Write("ID do Cliente: ");
                            if (!int.TryParse(Console.ReadLine(), out int idcli) || idcli < 1 || idcli > contador)
                                throw new ApplicationException("Cliente inexistente");

                            Pedido novoPedido = new Pedido { IdCliente = idcli, Status = "Pendente" };
                            bool adicionando = true;
                            while (adicionando)
                            {
                                Console.WriteLine("Selecione o pedido (número):");
                                for (int i = 0; i < opcoes.Length; i++)
                                    Console.WriteLine((i + 1) + ") " + opcoes[i]);
                                if (!int.TryParse(Console.ReadLine(), out int categoria) || categoria < 1 || categoria > opcoes.Length)
                                    throw new ApplicationException("Número invalida");

                                string escolhido = opcoes[categoria - 1];
                                if (escolhido == "Hambúrguer")
                                {
                                    Console.WriteLine("1) x-salada\n2) x-bacon\n3) x-hotdog\n4) x-burguer\n5) x-frango\n6) x-tudo");
                                    if (int.TryParse(Console.ReadLine(), out int hamb) && hamb >= 1 && hamb <= 6)
                                    {
                                        string[] ops = { "x-salada", "x-bacon", "x-hotdog", "x-burguer", "x-frango", "x-tudo" };
                                        escolhido = ops[hamb - 1];
                                    }
                                    else throw new ApplicationException("Opção invalida");
                                }
                                else if (escolhido == "Refrigerante")
                                {
                                    Console.WriteLine("1) Guaraná\n2) Coca-Cola\n3) Pepsi\n4) Sprite\n5) Fanta Uva\n6) Fanta Laranja");
                                    if (int.TryParse(Console.ReadLine(), out int refri) && refri >= 1 && refri <= 6)
                                    {
                                        string[] ops = { "Guaraná", "Coca-Cola", "Pepsi", "Sprite", "Fanta Uva", "Fanta Laranja" };
                                        escolhido = ops[refri - 1];
                                    }
                                    else throw new ApplicationException("Opção invalida");
                                }

                                Console.Write("Quantidade: ");
                                if (!int.TryParse(Console.ReadLine(), out int q) || q < 1)
                                    throw new ApplicationException("Quantidade invalida");

                                novoPedido.Items.Add(new Item_Pedido { Nome = escolhido, Quantidade = q });
                                Console.Write("Deseja adicionar mais algum item? (s/n): ");
                                string resp = Console.ReadLine();
                                if (resp != "s")
                                    adicionando = false;
                            }

                            pedidos.Enqueue(novoPedido);
                            Console.WriteLine("Pedido adicionado com sucesso.");
                            break;

                        case 5:
                            Console.Clear();
                            if (pedidos.Count == 0)
                                throw new ApplicationException("Não existe pedido na fila.");

                            Console.WriteLine("-- Processar Pedido --");
                            Pedido[] listaP = pedidos.ToArray();
                            for (int i = 0; i < listaP.Length; i++)
                            {
                                string itensDesc = "";
                                for (int k = 0; k < listaP[i].Items.Count; k++)
                                {
                                    var it = listaP[i].Items[k];
                                    itensDesc += it.Quantidade + "x " + it.Nome;
                                    if (k < listaP[i].Items.Count - 1)
                                        itensDesc += ", ";
                                }
                                Console.WriteLine((i + 1) + ") Cliente " + listaP[i].IdCliente + " - Itens: " + itensDesc);
                            }
                            Console.Write("Escolha o pedido: ");
                            if (!int.TryParse(Console.ReadLine(), out int selP) || selP < 1 || selP > listaP.Length)
                                throw new ApplicationException("Pedido invalido");

                            Pedido proc = listaP[selP - 1];
                            proc.Status = "Processado";
                            pedidos.Clear();
                            for (int i = 0; i < listaP.Length; i++)
                                if (i != selP - 1)
                                    pedidos.Enqueue(listaP[i]);
                            processados.Add(proc);
                            Console.WriteLine("Pedido foi processado");
                            break;

                        case 6:
                            Console.Clear();
                            if (pedidos.Count == 0)
                                throw new ApplicationException("Não existe pedido para ser cancelado");

                            Console.WriteLine("-- Cancelar Pedido --");
                            Pedido[] listaC = pedidos.ToArray();
                            for (int i = 0; i < listaC.Length; i++)
                            {
                                string itensDesc = "";
                                for (int k = 0; k < listaC[i].Items.Count; k++)
                                {
                                    var it = listaC[i].Items[k];
                                    itensDesc += it.Quantidade + "x " + it.Nome;
                                    if (k < listaC[i].Items.Count - 1)
                                        itensDesc += ", ";
                                }
                                Console.WriteLine((i + 1) + ") Cliente " + listaC[i].IdCliente + " - Itens: " + itensDesc);
                            }
                            Console.Write("Escolha o pedido: ");
                            if (!int.TryParse(Console.ReadLine(), out int selC) || selC < 1 || selC > listaC.Length)
                                throw new ApplicationException("Pedido inexistente");

                            Pedido canc = listaC[selC - 1];
                            canc.Status = "Cancelado";
                            pedidos.Clear();
                            for (int i = 0; i < listaC.Length; i++)
                                if (i != selC - 1)
                                    pedidos.Enqueue(listaC[i]);
                            cancelados.Push(canc);
                            Console.WriteLine("Pedido foi cancelado");
                            break;

                        case 7:
                            Console.Clear();
                            if (cancelados.Count == 0)
                                throw new ApplicationException("Não existe pedido cancelado para refazer");

                            Pedido toRef = cancelados.Pop();
                            toRef.Status = "Pendente";
                            pedidos.Enqueue(toRef);
                            Console.WriteLine("Pedido para ser refeito adicionado");
                            break;

                        case 8:
                            Console.Clear();
                            if (contador == 0)
                            {
                                Console.WriteLine("Nenhum cliente cadastrado no momento");
                                break;
                            }
                            List<int> fazendo = new List<int>();
                            foreach (var p in pedidos)
                                if (!fazendo.Contains(p.IdCliente))
                                    fazendo.Add(p.IdCliente);
                            List<int> prontos = new List<int>();
                            for (int i = 0; i < processados.Count; i++)
                            {
                                int idc = processados[i].IdCliente;
                                bool jaTem = false;
                                foreach (int x in prontos)
                                    if (x == idc) { jaTem = true; break; }
                                if (!jaTem)
                                    prontos.Add(idc);
                            }

                            Console.WriteLine("--- Clientes com Pedido Pendente ---");
                            if (fazendo.Count == 0)
                                Console.WriteLine("Nenhum");
                            else
                                foreach (int id in fazendo)
                                    Console.WriteLine("ID " + id + " – " + clientes[id - 1].Nome);

                            Console.WriteLine("\n--- Clientes com Pedido Processado ---");
                            if (prontos.Count == 0)
                                Console.WriteLine("Nenhum");
                            else
                                foreach (int id in prontos)
                                    Console.WriteLine("ID " + id + " – " + clientes[id - 1].Nome);

                            Console.WriteLine("\n--- Clientes sem Pedidos ---");
                            bool algum = false;
                            for (int i = 0; i < contador; i++)
                            {
                                int idc = clientes[i].Id;
                                bool emAndamento = false;
                                foreach (int x in fazendo)
                                    if (x == idc) { emAndamento = true; break; }
                                bool jaProcessado = false;
                                foreach (int x in prontos)
                                    if (x == idc) { jaProcessado = true; break; }
                                if (!emAndamento && !jaProcessado)
                                {
                                    Console.WriteLine("ID " + idc + " – " + clientes[i].Nome);
                                    algum = true;
                                }
                            }
                            if (!algum)
                                Console.WriteLine("Nenhum");
                            break;

                        case 0:
                            return;

                        default:
                            Console.WriteLine("Opção não reconhecida");
                            break;
                    }
                }
                catch (ApplicationException aex)
                {
                    Console.WriteLine("Erro: " + aex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro: " + ex.Message);
                }

                Console.WriteLine("Aperte qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}
