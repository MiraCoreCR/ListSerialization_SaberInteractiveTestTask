// MiraCore, Copyright 2022.
// Доброго времени суток, проверяющий. Это задание выполнил Александр Томишинец (@MiraCore). На задание было затрачено:
// 1 час, 54 минуты и 47 секунд. Можно было и немного быстрее, но я пил чай. 
// Плюс ещё некоторое время на оформление кода, разнесение по файлам, заливку на GitHub и т.п. малозначимые для Вас операции.

using System.IO;

namespace ListSerialization_SaberInteractiveTestTask
{
    // Подготовил для Вас пример, который был в Вашем задании. Откомментируете нужное.
    internal class Program
    {
        static void Main(string[] args)
        {
            ListWithRandomRefs list = new ListWithRandomRefs();

            var node1 = list.AddNewNode("1");
            var node2 = list.AddNewNode("2");

            node2.Random = node2;

            var node3 = list.AddNewNode("3");
            var node4 = list.AddNewNode("4", node1);
            var node5 = list.AddNewNode("5");

            node3.Random = node5;

            FileStream fileStream = new FileStream("out.txt", FileMode.OpenOrCreate);

            list.Serialize(fileStream);

            //ListWithRandomRefs listRandom = new ListWithRandomRefs();
            //listRandom.Deserialize(fileStream);
        }
    }
}
