// See https://aka.ms/new-console-template for more information
using System.Text;

using WaterSortingSolution;

Console.WriteLine("Hello from 波皮皮虾!");
Console.WriteLine(">>> 水排序谜题 <<<");
await Task.Delay(300);
Console.WriteLine("* 请先在程序所在根目录的 “水排序谜题.txt” 文件内输入关卡内容！(没有此文件则需手动创建) ");
Console.WriteLine();
await Task.Delay(300);

string msg = """
    * 水排序谜题.txt 内容规则如下：

    【空, 红, 橙, 黄, 绿, 墨, 青, 蓝, 粉, 紫, 棕, 褐, 灰】

    1、每行表示一个瓶子，按顺序填写（左到右、上到下）
    2、空瓶子写 “空”
    3、颜色描述之间用分隔符分开（-,./;，。；、任意一种）
    4、从瓶口到瓶底顺序描述（自上而下）
    """;

Console.WriteLine(msg);

await Task.Delay(300);

Console.WriteLine("准备就绪后按回车开始解谜！");
Console.WriteLine();
Console.ReadKey();

// 注册编码
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
string[] linesAnsi = await File.ReadAllLinesAsync("水排序谜题.txt", Encoding.GetEncoding("GB2312"));
string[] linesUtf8 = await File.ReadAllLinesAsync("水排序谜题.txt", Encoding.UTF8);

var bottles = new List<Bottle>();

int id = 1;

foreach (string[]? lines in new[] { linesAnsi, linesUtf8 })
{
    try
    {
        foreach (string? line in lines.Where(x => !String.IsNullOrWhiteSpace(x)))
        {
            if (line.Trim() == "空")
            {
                var bottle0 = new Bottle($"{id++}");
                bottles.Add(bottle0);

                continue;
            }

            string[] water = line.Split(' ', '-', ',', '.', '/', ';', '，', '。', '；', '、');
            var bottle = new Bottle($"{id++}", water.Select(x => (Water)Enum.Parse(typeof(Water), x)));
            bottles.Add(bottle);
        }

        break;
    }
    catch (ArgumentException)
    {
        id = 1;
        bottles.Clear();

        continue;
    }
}

//var bottle1 = new Bottle("1");
//var bottle2 = new Bottle("2");
//var bottle3 = new Bottle("3", Water.粉, Water.墨, Water.棕, Water.绿);
//var bottle4 = new Bottle("4", Water.紫, Water.墨, Water.红, Water.红);
//var bottle5 = new Bottle("5", Water.墨, Water.橙, Water.青, Water.蓝);
//var bottle6 = new Bottle("6", Water.黄, Water.墨, Water.粉, Water.蓝);
//var bottle7 = new Bottle("7", Water.褐, Water.绿, Water.棕, Water.紫);
//var bottle8 = new Bottle("8", Water.青, Water.灰, Water.红, Water.橙);
//var bottle9 = new Bottle("9", Water.褐, Water.青, Water.棕, Water.橙);
//var bottle10 = new Bottle("10", Water.青, Water.紫, Water.黄, Water.橙);
//var bottle11 = new Bottle("11", Water.灰, Water.蓝, Water.黄, Water.褐);
//var bottle12 = new Bottle("12", Water.绿, Water.褐, Water.紫, Water.棕);
//var bottle13 = new Bottle("13", Water.红, Water.粉, Water.灰, Water.蓝);
//var bottle14 = new Bottle("14", Water.绿, Water.黄, Water.灰, Water.粉);

//Console.WriteLine(">>> 手动测试开始");

//bool r81 = bottle8.PourOutTo(bottle1);

//bool r31 = bottle3.PourOutTo(bottle1);

//bool r91 = bottle9.PourOutTo(bottle1);

//bool r12 = bottle1.PourOutTo(bottle2);

//Console.WriteLine(">>> 手动测试结束");

//return;

var solver = new Solver(bottles);

solver.Solve();

Console.WriteLine("按任意键确认退出...");
Console.ReadLine();
