namespace Restaurante
{

    using System;
    using System.Collections.Generic;

    // Classe que representa um cliente
    class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    // Classe que representa um pedido
    class Order
    {
        public int CustomerId { get; set; }
        public string ItemNome { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1) VETOR de clientes
            // Definimos um vetor de tamanho fixo para armazenar até 100 clientes
            Cliente[] clientes = new Cliente[100];
            int clienteCount = 0;  // controla quantos clientes já foram cadastrados

            // 2) FILA de pedidos
            // A fila garante que o primeiro pedido que entrar seja o primeiro a ser processado (FIFO)
            Queue<Order> orderQueue = new Queue<Order>();

            // 3) PILHA de pedidos cancelados
            // A pilha garante que o último pedido cancelado seja o primeiro a ser refeito (LIFO)
            Stack<Order> PedidosCancelados = new Stack<Order>();

            List<Order> PedidosProcessados = new List<Order>();

            // 4) MATRIZ do menu
            // Vamos usar uma matriz de strings para os nomes e uma matriz de decimais para os preços
            string[,] menuItens = {
            { "Hambúrguer", "Batata Frita", "Refrigerante", "Salada" }
        };
            decimal[,] menuPrecos = {
            { 15.00m,       7.50m,         5.00m,         9.00m    }
        };
            int menuCount = menuItens.GetLength(1);

            while (true)
            {
                Console.WriteLine("\n--- Sistema de Pedidos ---");
                Console.WriteLine("1) Cadastrar Cliente");
                Console.WriteLine("2) Listar Clientes");
                Console.WriteLine("3) Listar Menu");
                Console.WriteLine("4) Fazer Pedido");
                Console.WriteLine("5) Processar Próximo Pedido");
                Console.WriteLine("6) Cancelar Pedido Atual");
                Console.WriteLine("7) Refeita Pedido Cancelado");
                Console.WriteLine("0) Sair");
                Console.Write("Opção: ");

                int x = 0;

                try
                {
                    x = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Valor inválido. Por favor, digite um número:" + e.Message);
                    continue;
                }

                switch (x)
                {
                    case 1:
                        // Cadastrar novo cliente no próximo slot do vetor
                        if (clienteCount >= clientes.Length)
                        {
                            Console.WriteLine("Limites de clientes atingidos, espere o restaurante esvaziar.");
                            break;
                        }
                        Console.Write("Nome do Cliente: ");
                        string nome = Console.ReadLine();
                        clientes[clienteCount] = new Cliente { Id = clienteCount + 1, Nome = nome };
                        Console.WriteLine($"Cliente cadastrado com o ID = {clientes[clienteCount].Id}");
                        clienteCount++;
                        break;

                    case 2:
                        // LISTAR CLIENTES POR CATEGORIA
                        //  - Clientes cadastrados: todos os IDs de 1 a clienteCount
                        //  - Clientes com pedidos em andamento: todos que aparecem em orderQueue
                        //  - Clientes que ainda não pediram: cadastrados, mas não em orderQueue nem em processedOrders
                        //  - Clientes com pedidos prontos: que aparecem em processedOrders, mas que não têm pedidos em aberto agora

                        if (clienteCount == 0)
                        {
                            Console.WriteLine("Não há clientes cadastrados.");
                            break;
                        }

                        // Extrair listas de IDs dos pedidos em andamento e prontos
                        var clientesComPedidosEmAndamento = orderQueue
                            .Select(o => o.CustomerId)
                            .Distinct()
                            .ToList();

                        var clientesComPedidosProntos = PedidosProcessados
                            .Select(o => o.CustomerId)
                            .Distinct()
                            .ToList();

                        Console.WriteLine("=== Clientes Cadastrados ===");
                        for (int i = 0; i < clienteCount; i++)
                        {
                            Console.WriteLine($"ID {clientes[i].Id} – {clientes[i].Nome}");
                        }

                        Console.WriteLine("\n=== Clientes com Pedido em Andamento ===");
                        if (clientesComPedidosEmAndamento.Count == 0)
                        {
                            Console.WriteLine("Nenhum.");
                        }
                        else
                        {
                            foreach (var cid in clientesComPedidosEmAndamento)
                            {
                                var c = clientes[cid - 1];
                                Console.WriteLine($"ID {c.Id} – {c.Nome}");
                            }
                        }

                        Console.WriteLine("\n=== Clientes com Pedido Pronto ===");
                        if (clientesComPedidosProntos.Count == 0)
                        {
                            Console.WriteLine("Nenhum.");
                        }
                        else
                        {
                            foreach (var cid in clientesComPedidosProntos)
                            {
                                // Certifique-se de não repetir clientes que ainda têm pedidos em andamento:
                                if (!clientesComPedidosEmAndamento.Contains(cid))
                                {
                                    var c = clientes[cid - 1];
                                    Console.WriteLine($"ID {c.Id} – {c.Nome}");
                                }
                            }
                        }

                        Console.WriteLine("\n=== Clientes que ainda não pediram ===");
                        bool algumSemPedir = false;
                        for (int i = 0; i < clienteCount; i++)
                        {
                            int cid = clientes[i].Id;
                            bool pediuEmAndamento = clientesComPedidosEmAndamento.Contains(cid);
                            bool jáProcessou = clientesComPedidosProntos.Contains(cid) && !pediuEmAndamento;
                            if (!pediuEmAndamento && !jáProcessou)
                            {
                                Console.WriteLine($"ID {clientes[i].Id} – {clientes[i].Nome}");
                                algumSemPedir = true;
                            }
                        }
                        if (!algumSemPedir)
                        {
                            Console.WriteLine("Nenhum.");
                        }
                        break;

                    case 3:
                        // Exibir matriz de itens e preços
                        Console.WriteLine("Menu:");
                        for (int i = 0; i < menuCount; i++)
                            Console.WriteLine($"{i + 1}) {menuItens[0, i]} – R$ {menuPrecos[0, i]:F2}");
                        break;

                    case 4:
                        // Fazer pedido: enfileirar na fila de pedidos
                        Console.Write("ID do Cliente: ");
                        if (!int.TryParse(Console.ReadLine(), out int custId) || custId < 1 || custId > clienteCount)
                        {
                            Console.WriteLine("Cliente inválido.");
                            break;
                        }
                        Console.Write("Item (número): ");
                        if (!int.TryParse(Console.ReadLine(), out int itemIdx) || itemIdx < 1 || itemIdx > menuCount)
                        {
                            Console.WriteLine("Item inválido.");
                            break;
                        }
                        var pedido = new Order
                        {
                            CustomerId = custId,
                            ItemNome = menuItens[0, itemIdx - 1]
                        };
                        orderQueue.Enqueue(pedido);
                        Console.WriteLine($"Pedido enfileirado: Cliente {custId} – {pedido.ItemNome}");
                        break;

                    case 5:
                        // Processar próximo pedido da fila (desenfileirar)
                        if (orderQueue.Count == 0)
                        {
                            Console.WriteLine("Nenhum pedido na fila.");
                            break;
                        }
                        var proximo = orderQueue.Dequeue();
                        Console.WriteLine($"Processando pedido: Cliente {proximo.CustomerId} – {proximo.ItemNome}");
                        break;

                    case 6:
                        // Cancelar o pedido atual: tiramos o da fila e empilhamos na pilha de cancelados
                        if (orderQueue.Count == 0)
                        {
                            Console.WriteLine("Nenhum pedido na fila para cancelar.");
                            break;
                        }
                        var cancelar = orderQueue.Dequeue();
                        PedidosCancelados.Push(cancelar);
                        Console.WriteLine($"Pedido cancelado: Cliente {cancelar.CustomerId} – {cancelar.ItemNome}");
                        break;

                    case 7:
                        // Repetir último pedido cancelado: desempilha e enfileira de volta
                        if (PedidosCancelados.Count == 0)
                        {
                            Console.WriteLine("Nenhum pedido cancelado para refazer.");
                            break;
                        }
                        var refazer = PedidosCancelados.Pop();
                        orderQueue.Enqueue(refazer);
                        Console.WriteLine($"Refeito pedido cancelado: Cliente {refazer.CustomerId} – {refazer.ItemNome}");
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Entrada inválida.");
                        break;
                }
            }
        }
    }

}
