using System.IO;

namespace ListSerialization_SaberInteractiveTestTask
{
    // Подготовил для Вас пример, который был в Вашем задании. Откомментируете нужное.s
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
