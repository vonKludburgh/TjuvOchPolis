using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TjuvOchPolis
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
            
            Console.ReadLine();
        }






        static void Header()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--------------------------------------------------");
            Console.Write("                  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("TJUV ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("OCH ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("POLIS");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void Interface(int sw, int swa, int sp, int st, int rw, int rwa, int rp, int rt, string bbb)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("               STOLEN");
            Console.WriteLine("Watch: " + sw + " Wallet: " + swa + " Phone: " + sp + " Keys: " + st);
            Console.WriteLine("               RETRIEVED");
            Console.WriteLine("Watch: " + rw + " Wallet: " + rwa + " Phone: " + rp + " Keys: " + rt);
            Console.WriteLine(bbb);
            Console.WriteLine("--------------------------------------------------");
        }

        static void Prison(List<Person> prisonlist)
        {
            int aaa = 0;
            Console.WriteLine("List of thieves in prison");
            foreach (Person x in prisonlist)
            {
                Console.WriteLine("Prisoner time in jail: " + x.PrisonTime);
                aaa++;
            }
            Console.WriteLine("Currently in prison: " + aaa);
            aaa = 0;
        }

        static void Start()
        {
            Random rand = new Random();
            List<Person> aaa = new List<Person>();
            List<Person> PrisonList = new List<Person>();

            int MapY = 50, MapX = 20, Citizen = 20, Thief = 20, Police = 20, RWatch = 0, RWallet = 0,
                RPhone = 0, RThing = 0, SWatch = 0, SWallet = 0, SPhone = 0, SThing = 0, Dice = 0;
            string bbb = "";
            bool Encounter = false;

            // Create people
            People(Citizen, Police, Thief, aaa, rand, MapX, MapY);

            // Game loop
            GameLoop(aaa, Encounter, bbb, MapX, MapY, PrisonList, Dice, rand, SWatch, SWallet, SPhone, SThing, RWatch, RWallet, RPhone, RThing);
        }

        static void People(int citizen, int police, int thief, List<Person> aaa, Random rand, int mapx, int mapy)
        {
            for (int i = 0; i < citizen; i++)
            {
                aaa.Add(new Citizen(rand.Next(-1, 1 + 1), rand.Next(-1, 1 + 1), rand.Next(1, mapx - 1), rand.Next(1, mapy - 1)));
            }
            for (int i = 0; i < police; i++)
            {
                aaa.Add(new Police(rand.Next(-1, 1 + 1), rand.Next(-1, 1 + 1), rand.Next(1, mapx - 1), rand.Next(1, mapy - 1)));
            }
            for (int i = 0; i < thief; i++)
            {
                aaa.Add(new Thief(rand.Next(-1, 1 + 1), rand.Next(-1, 1 + 1), rand.Next(1, mapx - 1), rand.Next(1, mapy - 1)));
            }

            // fix 0,0 velocity
            foreach (Person x in aaa)
            {
                if (x.VelX == 0 && x.VelY == 0)
                {
                    x.VelX = 1;
                    x.VelY = -1;
                }
            }
        }

        static void GameLoop(List<Person> aaa, bool Encounter, string bbb, int MapX, int MapY, List<Person> PrisonList, int Dice, Random rand, int SWatch, int SWallet, int SPhone, int SThing, int RWatch, int RWallet, int RPhone, int RThing)
        {
            while (true)
            {
                Encounter = false;
                bbb = "";

                Header();

                for (int i = 0; i < MapX; i++)
                {
                    for (int j = 0; j < MapY; j++)
                    {

                        foreach (Person x in aaa)
                        {
                            if (x.X == i && x.Y == j && x is Citizen)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("M");
                                j++;
                            }
                            if (x.X == i && x.Y == j && x is Police)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("P");
                                j++;
                            }
                            if (x.X == i && x.Y == j && x is Thief && x.Prison == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("T");
                                j++;
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" ");

                    }
                    Console.WriteLine();
                }

                // Walk out of map, walk people and collision
                foreach (Person x in aaa)
                {
                    // Walk + Walk out of map
                    if (x.Prison == false)
                    {
                        x.X += x.VelX;
                        x.Y += x.VelY;

                        if (x.Y > MapY - 1) x.Y = 0;
                        if (x.Y < 0) x.Y = MapY - 1;
                        if (x.X < 0) x.X = MapX - 1;
                        if (x.X > MapX - 1) x.X = 0;
                    }

                    // In prison
                    if (x.Prison == true)
                    {
                        x.X = x.Y = -1000;
                        x.PrisonTime -= 1;
                        if (x.PrisonTime == 0)
                        {
                            x.Prison = false;
                            x.X = x.Y = 0;
                            PrisonList.Remove(x);
                            bbb = "Thief got out of jail!";
                        }
                    }

                    // Collision
                    foreach (Person y in aaa)
                    {
                        if (x.X == y.X && x.Y == y.Y)
                        {
                            if (x is Citizen && y is Thief)
                            {
                                Dice = rand.Next(0, 4);

                                if (Dice == 0 && x.Belongings.Watch > 0)
                                {
                                    x.Belongings.Watch -= 1;
                                    y.StolenGoods.Watch += 1;
                                    SWatch++;
                                    Encounter = true;
                                }
                                else if (Dice == 1 && x.Belongings.Wallet > 0)
                                {
                                    x.Belongings.Wallet -= 1;
                                    y.StolenGoods.Wallet += 1;
                                    SWallet++;
                                    Encounter = true;
                                }
                                else if (Dice == 2 && x.Belongings.Phone > 0)
                                {
                                    x.Belongings.Phone -= 1;
                                    y.StolenGoods.Phone += 1;
                                    SPhone++;
                                    Encounter = true;
                                }
                                else if (Dice == 3 && x.Belongings.Keys > 0)
                                {
                                    x.Belongings.Keys -= 1;
                                    y.StolenGoods.Keys += 1;
                                    SThing++;
                                    Encounter = true;
                                }
                                else
                                {
                                    if (x.Belongings.Watch > 0)
                                    {
                                        x.Belongings.Watch -= 1;
                                        y.StolenGoods.Watch += 1;
                                        SWatch++;
                                        Encounter = true;
                                    }
                                    else if (x.Belongings.Wallet > 0)
                                    {
                                        x.Belongings.Wallet -= 1;
                                        y.StolenGoods.Wallet += 1;
                                        SWallet++;
                                        Encounter = true;
                                    }
                                    else if (x.Belongings.Phone > 0)
                                    {
                                        x.Belongings.Phone -= 1;
                                        y.StolenGoods.Phone += 1;
                                        SPhone++;
                                        Encounter = true;
                                    }
                                    else if (x.Belongings.Keys > 0)
                                    {
                                        x.Belongings.Keys -= 1;
                                        y.StolenGoods.Keys += 1;
                                        SThing++;
                                        Encounter = true;
                                    }

                                }

                                bbb = "CITIZEN and THIEF Collide";
                            }
                            if (x is Thief && y is Police)
                            {
                                if (x.StolenGoods.Watch > 0 || x.StolenGoods.Wallet > 0 || x.StolenGoods.Phone > 0 || x.StolenGoods.Keys > 0)
                                {
                                    y.Confiscated.Watch += x.StolenGoods.Watch;
                                    RWatch += x.StolenGoods.Watch;
                                    SWatch -= x.StolenGoods.Watch;
                                    x.StolenGoods.Watch = 0;

                                    y.Confiscated.Wallet += x.StolenGoods.Wallet;
                                    RWallet += x.StolenGoods.Wallet;
                                    SWallet -= x.StolenGoods.Wallet;
                                    x.StolenGoods.Wallet = 0;

                                    y.Confiscated.Phone += x.StolenGoods.Phone;
                                    RPhone += x.StolenGoods.Phone;
                                    SPhone -= x.StolenGoods.Phone;
                                    x.StolenGoods.Phone = 0;

                                    y.Confiscated.Keys += x.StolenGoods.Keys;
                                    RThing += x.StolenGoods.Keys;
                                    SThing -= x.StolenGoods.Keys;
                                    x.StolenGoods.Keys = 0;

                                    x.Prison = true;
                                    x.PrisonTime = 30;
                                    PrisonList.Add(x);

                                    Encounter = true;
                                }
                                bbb = "POLICE and THIEF collide";
                            }
                        }
                    }
                }
                Interface(SWatch, SWallet, SPhone, SThing, RWatch, RWallet, RPhone, RThing, bbb);

                Prison(PrisonList);

                if (Encounter)
                {
                    Thread.Sleep(2000);
                }
                else
                {
                    Thread.Sleep(500);
                }

                Console.Clear();
            }
        }
    }
    }
