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
                Console.Clear();
                Console.WriteLine("--- Sistema de Pedidos ---");
                Console.WriteLine("1) Cadastrar Cliente");
                Console.WriteLine("2) Listar Clientes");
                Console.WriteLine("3) Listar Menu");
                Console.WriteLine("4) Fazer Pedido");
                Console.WriteLine("5) Processar Pedido");
                Console.WriteLine("6) Cancelar Pedido");
                Console.WriteLine("7) Refazer Cancelamento");
                Console.WriteLine("0) Sair");
                Console.Write("Opção: ");

                if (!int.TryParse(Console.ReadLine(), out int opc)) continue;

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        if (contador >= clientes.Length)
                        {
                            Console.WriteLine("O limite de clientes foi atingido.");
                            break;
                        }
                        Console.Write("Digite o nome do Cliente: ");
                        clientes[contador] = new Cliente { Id = contador + 1, Nome = Console.ReadLine() };
                        Console.WriteLine($"Cliente cadastrado, seu ID é: = {clientes[contador].Id}");
                        contador++;
                        break;

                    case 2:
                        Console.Clear();
                        if (contador == 0)
                        {
                            Console.WriteLine("Nenhum cliente cadastro no momento.");
                            break;
                        }
                        List<int> fazendo = new List<int>();
                        foreach (var o in pedidos)
                        {
                            if (!fazendo.Contains(o.IdCliente))
                                fazendo.Add(o.IdCliente);
                        }
                        List<int> prontos = new List<int>();
                        foreach (var o in processados)
                        {
                            if (!prontos.Contains(o.IdCliente))
                                prontos.Add(o.IdCliente);
                        }

                        Console.WriteLine("--- Lista de Clientes ---");
                        for (int i = 0; i < contador; i++)
                            Console.WriteLine($"ID {clientes[i].Id} – {clientes[i].Nome}");

                        Console.WriteLine("--- Clientes com Pedido em Andamento ---");
                        if (fazendo.Count == 0) Console.WriteLine("Nenhum.");
                        else
                            foreach (int id in fazendo)
                                Console.WriteLine($"ID {id} – {clientes[id - 1].Nome}");

                        Console.WriteLine("--- Clientes com Pedido Pronto ---");
                        bool pronto = false;
                        foreach (int id in prontos)
                        {
                            if (!fazendo.Contains(id))
                            {
                                Console.WriteLine($"ID {id} – {clientes[id - 1].Nome}");
                                pronto = true;
                            }
                        }
                        if (!pronto) Console.WriteLine("Nenhum.");

                        Console.WriteLine("--- Clientes sem Pedidos ---");
                        bool sem = false;
                        for (int i = 0; i < contador; i++)
                        {
                            int id = clientes[i].Id;
                            if (!fazendo.Contains(id) && !prontos.Contains(id))
                            {
                                Console.WriteLine($"ID {id} – {clientes[i].Nome}");
                                sem = true;
                            }
                        }
                        if (!sem) Console.WriteLine("Nenhum.");
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("-- Menu Principal --");
                        for (int i = 0; i < opcoes.Length; i++)
                            Console.WriteLine($"{i + 1}) {opcoes[i]} – R$ {precos[i]:F2}");
                        Console.WriteLine("-- Opções de Hambúrguer --");
                        Console.WriteLine("1) x-salada\n2) x-bacon\n3) x-hotdog\n4) x-burguer\n5) x-frango\n6) x-tudo");
                        Console.WriteLine("\n-- Opções de Refrigerante --");
                        Console.WriteLine("1) Guaraná\n2) Coca-Cola\n3) Pepsi\n4) Sprite\n5) Fanta Uva\n6) Fanta Laranja");
                        break;


                    case 4:
                        Console.Clear();
                        Console.WriteLine("-- Fazer Pedido --");
                        Console.WriteLine("--- Clientes Cadastrados ---");
                        for (int i = 0; i < contador; i++)
                            Console.WriteLine($"ID {clientes[i].Id} – {clientes[i].Nome}");
                        Console.Write("ID do Cliente: ");
                        if (!int.TryParse(Console.ReadLine(), out int idcli) || idcli < 1 || idcli > contador)
                        {
                            Console.WriteLine("Cliente inexistente.");
                            break;
                        }
                        Pedido novoPedido = new Pedido { IdCliente = idcli };
                        bool novo = true;
                        while (novo)
                        {
                            Console.WriteLine("Selecione o pedido (número):");
                            for (int i = 0; i < opcoes.Length; i++)
                                Console.WriteLine($"{i + 1}) {opcoes[i]}");
                            if (!int.TryParse(Console.ReadLine(), out int categoria) || categoria < 1 || categoria > opcoes.Length) continue;

                            string escolhido = opcoes[categoria - 1];
                            if (escolhido == "Hambúrguer")
                            {
                                Console.WriteLine("1) x-salada\n2) x-bacon\n3) x-hotdog\n4) x-burguer\n5) x-frango\n6) x-tudo");
                                if (int.TryParse(Console.ReadLine(), out int hamburguer) && hamburguer >= 1 && hamburguer <= 6)
                                {
                                    string[] op = { "x-salada", "x-bacon", "x-hotdog", "x-burguer", "x-frango", "x-tudo" };
                                    escolhido = op[hamburguer - 1];
                                }
                            }
                            else if (escolhido == "Refrigerante")
                            {
                                Console.WriteLine("1) Guaraná\n2) Coca-Cola\n3) Pepsi\n4) Sprite\n5) Fanta Uva\n6) Fanta Laranja");
                                if (int.TryParse(Console.ReadLine(), out int refri) && refri >= 1 && refri <= 6)
                                {
                                    string[] op = { "Guaraná", "Coca-Cola", "Pepsi", "Sprite", "Fanta Uva", "Fanta Laranja" };
                                    escolhido = op[refri - 1];
                                }
                            }

                            Console.Write("Quantidade: ");
                            int q = int.TryParse(Console.ReadLine(), out int quant) ? quant : 1;
                            novoPedido.Items.Add(new Item_Pedido { Nome = escolhido, Quantidade = q });
                            while (true)
                            {
                                Console.Write("Deseja adicionar mais algum item? (s/n): ");
                                string resposta = Console.ReadLine().Trim().ToLower();
                                if (resposta == "s")
                                {
                                    novo = true;
                                    break;
                                }
                                else if (resposta == "n")
                                {
                                    novo = false;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Digite apenas 's' ou 'n' ");
                                }
                            }
                        }
                        pedidos.Enqueue(novoPedido);
                        Console.WriteLine("Pedido adicionado com sucesso.");
                        break;


                    case 5:
                        Console.Clear();
                        if (pedidos.Count == 0)
                        {
                            Console.WriteLine("Não existe pedido na fila");
                            break;
                        }
                        Console.WriteLine("-- Processar Pedido --");
                        Pedido[] array = pedidos.ToArray();
                        for (int i = 0; i < array.Length; i++)
                            Console.WriteLine($"{i + 1}) Cliente {array[i].IdCliente} - Itens: {string.Join(", ", array[i].Items.ConvertAll(it => $"{it.Quantidade}x {it.Nome}"))}");
                        Console.Write("Escolha índice: ");
                        if (int.TryParse(Console.ReadLine(), out int sel) && sel >= 1 && sel <= array.Length)
                        {
                            Pedido proc = array[sel - 1];
                            var temp = new Queue<Pedido>();
                            for (int i = 0; i < array.Length; i++)
                                if (i != sel - 1) temp.Enqueue(array[i]);

                            pedidos = temp;
                            processados.Add(proc);
                            Console.WriteLine("Pedido foi processado");
                        }
                        break;

                    case 6:
                        Console.Clear();
                        if (pedidos.Count == 0)
                        {
                            Console.WriteLine("Não existe pedido para ser cancelado");
                            break;
                        }
                        Console.WriteLine("-- Cancelar Pedido --");
                        array = pedidos.ToArray();
                        for (int i = 0; i < array.Length; i++)
                            Console.WriteLine($"{i + 1}) Cliente {array[i].IdCliente} - Itens: {string.Join(", ", array[i].Items.ConvertAll(it => $"{it.Quantidade}x {it.Nome}"))}");
                        Console.Write("Escolha índice: ");
                        if (int.TryParse(Console.ReadLine(), out sel) && sel >= 1 && sel <= array.Length)
                        {
                            Pedido canc = array[sel - 1];
                            var temp2 = new Queue<Pedido>();
                            for (int i = 0; i < array.Length; i++)
                                if (i != sel - 1) temp2.Enqueue(array[i]);
                            pedidos = temp2;
                            cancelados.Push(canc);
                            Console.WriteLine("Pedido foi cancelado");
                        }
                        break;
                    case 7:
                        Console.Clear();
                        if (cancelados.Count == 0)
                        {
                            Console.WriteLine("Não existe pedido cancelado para refazer");
                            break;
                        }
                        Pedido toRef = cancelados.Pop();
                        pedidos.Enqueue(toRef);
                        Console.WriteLine("Pedido para ser refeito adicionado");
                        break;
                    case 0:
                        return;
                }
                Console.WriteLine("Aperte qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}
