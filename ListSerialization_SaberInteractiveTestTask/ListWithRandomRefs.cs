using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ListSerialization_SaberInteractiveTestTask
{
    // Сам двусвязный список. Для удобства, я добавил необходимые мне методы, чтобы не связывать всё это руками. Особо не 
    // изощрялся, но тут ещё просятся методы Clear и DeleteNode. 
    public class ListWithRandomRefs
    {
        public Node Head;
        public Node Tail;
        public int Count = 0;

        // Метод добавления новой ноды, ничего особо интресного.
        public Node AddNewNode(string data, Node randomNode = null)
        {
            Node node = new Node();

            if (this.Head == null)
            {
                node.Previous = null;
                node.Next = null;
                node.Random = null;

                this.Head = node;
                this.Tail = node;
            }

            else
            {
                this.Tail.Next = node;
                node.Previous = this.Tail;
                this.Tail = node;
                node.Next = null;
            }

            node.Data = data;
            node.Random = randomNode;
            this.Count++;

            return node;
        }

        // Этот метод нужен, чтобы искать ноды по индексу.
        private Node GetNodeByIndex(int index)
        {
            int counter = 0;

            for (Node IteratorNode = Head; IteratorNode != null; IteratorNode = IteratorNode.Next)
            {
                if (counter == index)
                {
                    return IteratorNode;
                }

                counter++;
            }

            return null;
        }

        // Я видел несколько примеров, когда люди "пишут" в файл рандомную ноду полностью, когда встречают её, тол есть у ним, потенциально, может быть
        // N копий этой самой ноды, где N - количество элементов списка. И ладно если это все лишь число/строка (в качестве наполнения, т.е. переменная Data),
        // а если это большая структура или ещё что-либо? Размеры файла могут быть просто огромными.
        // Поскольку эта нода, согласно Вашему же рисунку, в этом списку, то логично просто выписать последовательно все ноды в документик, раздать им индексы,
        // а потом просто подсовывать нужный индекс, если ссылка Random не пуста.
        // Да, кстати, я, изначально, хотел на Срр написать, но достаточно давно не работал на нём, поэтому решил написать на С# который уже месяца 4 использую
        // на текущей работе. Просто чтобы быстрее закончить. На досуге, наверное, всё-таки выполню задачку это на плюсах, ибо интересно. Но особых различный 
        // быть не должно.
        private int GetNodeIndexFromHead(Node node)
        {
            int currentNodeNumber = 0;

            for (Node iteratorNode = Head; iteratorNode != null; iteratorNode = iteratorNode.Next)
            {
                if (iteratorNode == node)
                {
                    return currentNodeNumber;
                }

                currentNodeNumber++;
            }

            return -1;
        }

        // Сериализация. Вы говорили, что сложность не должна быть выше N в квадрате. Я, честно говоря, не любитель расчёт сложностей 
        // подобным образом, считаю по длительности выполнения инструкции CPU, но выскажу свои сообращения.
        // В этом методе 1 for, который напрямую завязан на длину списка, соответственно сложность уже N, где N - это та самая длина
        // того самого списка. Далее внутри этого for'a есть вызов метода, в которой тоже for, зависвящий от длины списка.
        // Но полностью пройдут все итерации только в том случае, когда будет поиск последней ноды. В любом случае, квадратичная сложность,
        // как мне кажется, не достигается.
        public void Serialize(FileStream stream)
        {
            string allNodesInString = "";
            int currentNodeNumber = 0;

            for (Node iteratorNode = Head; iteratorNode != null; iteratorNode = iteratorNode.Next)
            {
                allNodesInString += currentNodeNumber + "," + iteratorNode.Data + "," + GetNodeIndexFromHead(iteratorNode.Random) + "\n";
                currentNodeNumber++;
            }

            allNodesInString = allNodesInString.Remove(allNodesInString.Length - 1);

            using (stream)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(allNodesInString);

                stream.Write(buffer, 0, buffer.Length);

                Console.WriteLine("List serialized");
            }
        }

        // Десериализация. Касательно сложности, та же история. 2N, ибо 2 цикла, равных длине списка, а далее зависит от рандомного индекса.
        public void Deserialize(FileStream stream)
        {
            string textFromFile = "";

            using (stream)
            {
                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                textFromFile = Encoding.UTF8.GetString(buffer);
            }

            List<string> strings = textFromFile.Split("\n", StringSplitOptions.None).ToList();
            List<int> listOfRandomPointers = new List<int>();


            foreach (var item in strings)
            {
                string[] data = item.Split(",", StringSplitOptions.None);

                AddNewNode(data[1]);
                listOfRandomPointers.Add(int.Parse(data[2]));
            }

            int pointerIndex = 0;
            for (Node iteratorNode = Head; iteratorNode != null; iteratorNode = iteratorNode.Next)
            {
                if (listOfRandomPointers[pointerIndex] == -1)
                {
                    pointerIndex++;
                    continue;
                }

                iteratorNode.Random = GetNodeByIndex(listOfRandomPointers[pointerIndex]);

                pointerIndex++;
            }
        }
    }
}
