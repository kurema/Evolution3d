using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution1
{
    public class CreatureAI
    {
        public Random CommonRandom = new Random();
        public const int SituationCount = 1;
        public class Command
        {
            public CommandSet CommandSet;
            public Argument[] Arguments=new Argument[2];

            public double Execute(List<double>[] Result, List<double>[] Input)
            {
                return CommandSet.Function(
                    Arguments[0].Value(Result,Input),
                    Arguments[1].Value(Result, Input)
                    );
            }

            public void AddAddress(int value)
            {
                Arguments[0].AddAddress(value);
                Arguments[1].AddAddress(value);
            }

            public void FixRemovedAddress(int value)
            {
                Arguments[0].FixRemovedAddress(value);
                Arguments[1].FixRemovedAddress(value);
            }

            public override string ToString()
            {
                return this.CommandSet.Name + " " + Arguments[0].ToString() + (this.CommandSet.ArgumentCount == 2 ? " " + Arguments[1].ToString() : "") + "\n";
            }

            public class Argument
            {
                public Random CommonRandom;
                public ArgumentType Type;

                public Argument(ref Random rnd)
                {
                    CommonRandom = new Random(rnd.Next(int.MaxValue));
                }
                public Argument()
                {
                    CommonRandom = new Random();
                }

                //参照の種類。
                //0:計算結果 1:計算結果、積分 2:計算、微分 3:入力 4:入力、積分 5:入力、微分
                public int AddressType;

                public double ImmediateValue;

                //負値の場合は、環境変数を指す。
                public int Address;

                public void AddAddress(int value)
                {
                    Address += value;
                }

                public void FixRemovedAddress(int Value)
                {
                    if (Address > Value)
                    {
                        Address -= 1;
                    }
                    else if (Address == Value)
                    {
                        Address = CommonRandom.Next(Value);
                    }
                }

                public override string ToString()
                {
                    if (Type == ArgumentType.Address)
                    {
                        return (new string[] { "LINE[", "INT:LINE[", "DIF:LINE[", "IN[", "INT:IN[", "DIF:IN[" }[AddressType]) + Address + "]";

                    }
                    else
                    {
                        return ImmediateValue.ToString();
                    }
                }

                public double Value(List<double>[] Result, List<double>[] Input)
                {
                    if (Type == ArgumentType.Address && AddressType<3 )
                    {
                        if (Result[AddressType].Count() <= Address)
                        {
                            Address = CommonRandom.Next( Result[AddressType].Count() - 1);
                        }
                        return Result[AddressType][Address];
                    }
                    else if (Type == ArgumentType.Address && AddressType >= 3)
                    {
                        if (Input[AddressType-3].Count() <= Address)
                        {
                            Address = CommonRandom.Next(Input[AddressType - 3].Count() - 1);
                        }
                        return Input[AddressType-3][Address];
                    }
                    else
                    {
                        return ImmediateValue;
                    }
                }
                public enum ArgumentType
                {
                    Address, Immediate
                }
            }
        }

        public class CommandSet
        {
            public String Name;
            public Func<double, double, double> Function;
            public Int16 ArgumentCount;
            public Guid CommandID;
        }

        public CreatureAI()
        {
        }
        public CreatureAI(CreatureAI baseCreature)
        {
            CommonRandom = new Random(baseCreature.CommonRandom.Next(int.MaxValue));
            CommandList =new List<Command>[baseCreature.CommandList.Count()];
            for (int i = 0; i < baseCreature.CommandList.Count(); i++)
            {
                CommandList[i] = new List<Command>(baseCreature.CommandList[i]);
            }

        }
        public CreatureAI(CreatureData.AIData baseAI)
        {
            Dictionary<Guid, CommandSet> csdic = new Dictionary<Guid, CommandSet>();
            foreach (CommandSet cs in CommandSets)
            {
                csdic[cs.CommandID] = cs;
            }
            for (int i = 0; SituationCount > i; i++)
            {
                for (int j = 0; baseAI.commands[i].Count() > j; j++)
                {
                    CommandList[i].Add(new Command() { Arguments = baseAI.commands[i][j].arguments, CommandSet = csdic[baseAI.commands[i][j].commandID]});
                }
            }
        }

        static public CommandSet[] CommandSets =
        {

            new CommandSet(){Function=(arg1,arg2)=>{return arg1+arg2;},ArgumentCount=2,Name="ADD",CommandID=new Guid("847F8E90-D40D-42CA-8808-18D5BEBA96FA")},
            new CommandSet(){Function=(arg1,arg2)=>{return arg1-arg2;},ArgumentCount=2,Name="SUB",CommandID=new Guid("B342E909-C4D5-4265-98FC-F4F4648F04BD")},
            new CommandSet(){Function=(arg1,arg2)=>{return arg1*arg2;},ArgumentCount=2,Name="MUL",CommandID=new Guid("961E1C3A-8E95-49FB-8ABC-FF4688C1FB07")},
            new CommandSet(){Function=(arg1,arg2)=>
            {return Math.Abs(arg2)<=Math.Pow(10,-10)?( arg1*arg2>0? Math.Pow(10,10):-Math.Pow(10,10)):arg1/arg2;}
            ,ArgumentCount=2,Name="DIV",CommandID=new Guid("1F2677DC-FFA1-490C-83A2-283673774162")},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Abs(arg1)==0?0: Math.Log(Math.Abs(arg1));},ArgumentCount=1,Name="LOG",CommandID=new Guid("B1F0EB4C-F2FE-40A7-BBC6-FE9425BD4095")},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Abs(arg1);},ArgumentCount=1,Name="ABS",CommandID=new Guid("BD420764-D3F6-48D3-9DD1-1860C8F1CEDC")},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Sin(arg1);},ArgumentCount=1,Name="SIN",CommandID=new Guid("047ACA12-907C-4CFD-9C76-61C8393A155E")},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Cos(arg1);},ArgumentCount=1,Name="COS",CommandID=new Guid("70A275B8-27B6-482B-81ED-C00EBE64C9DD")},
//            new CommandSet(){Function=(arg1,arg2)=>{return Math.Pow(arg1,arg2);},ArgumentCount=2,Name="POW",CommandID=new Guid("C52FCB3E-BC7F-46CF-A77C-AE21BEB337EF")},
            new CommandSet(){Function=(arg1,arg2)=>{return arg1;},ArgumentCount=1,Name="VALUE",CommandID=new Guid("02B73F08-4F80-4118-BB85-574CAFD80FA8")},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Max(arg1,arg2);},ArgumentCount=1,Name="MAX",CommandID=new Guid("ABCF0C34-97CD-4CE1-A05B-D4FC5B319F25")},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Min(arg1,arg2);},ArgumentCount=1,Name="MIN",CommandID=new Guid("F490776F-5626-4584-92C3-F8135673ABA4")},
        };


        //計算結果を格納
        //通常値、積分値、微分値
        public List<double>[][] Result = new List<double>[SituationCount][]{
            new List<double>[]{new List<double>(), new List<double>(), new List<double>()},
            //new List<double>[]{new List<double>(), new List<double>(), new List<double>()},
            //new List<double>[]{new List<double>(), new List<double>(), new List<double>()},
            //new List<double>[]{new List<double>(), new List<double>(), new List<double>()}
        };
        public List<double>[] InputHistory = {new List<double>(), new List<double>(), new List<double>() };


        //命令列を定義。
        //状況により用いる命令列は異なる。
        public List<Command>[] CommandList 
            = { 
                  new List<Command>(),
                  //new List<Command>(),
                  //new List<Command>(),
                  //new List<Command>(),
              };

        public double[] Execute(double[] Input,int Situation, ref int[] OutputAddress)
        {
            //以下の処理はCommandListを書き換えるので注意。
            while (CommandList[Situation].Count == 0)
                AddRandomCommand();

            
            while (Result[Situation][0].Count() < CommandList[Situation].Count())
            {
                Result[Situation][0].Add(0);
                Result[Situation][1].Add(0);
                Result[Situation][2].Add(0);
            }
            while (Result[Situation][0].Count() > CommandList[Situation].Count())
            {
                Result[Situation][0].RemoveAt(Input.Count() - 1);
                Result[Situation][1].RemoveAt(Input.Count() - 1);
                Result[Situation][2].RemoveAt(Input.Count() - 1);
            }

            while (InputHistory[0].Count() < Input.Count())
            {
                InputHistory[0].Add(0);
                InputHistory[1].Add(0);
                InputHistory[2].Add(0);
            }
            while (InputHistory[0].Count() > Input.Count())
            {
                InputHistory[0].RemoveAt(Input.Count() - 1);
                InputHistory[1].RemoveAt(Input.Count() - 1);
                InputHistory[2].RemoveAt(Input.Count() - 1);
            }

            for (int i = 0; i < Input.Count(); i++)
            {
                InputHistory[2][i] = Input[i] - InputHistory[0][i];
                InputHistory[0][i] = Input[i];
                InputHistory[1][i] += Input[i];
            }
            for (int i = 0; i < CommandList[Situation].Count(); i++)
            {
                double value = CommandList[Situation][i].Execute(this.Result[Situation],this.InputHistory);
                Result[Situation][2][i] = value - Result[Situation][0][i];
                Result[Situation][0][i] = value;
                Result[Situation][1][i] += value;
            }
            double[] ret =
                new double[OutputAddress.Count()];
            for (int i = 0; i < OutputAddress.Count(); i++)
            {
                OutputAddress[i] %= Result[Situation][0].Count();
                ret[i] = Result[Situation][0][OutputAddress[i]];
                ret[i] = double.IsNaN(ret[i])||double.IsInfinity(ret[i]) ? 0 : ret[i];
            }
            return ret;
        }

        public string CommandListToString(int Situation)
        {
            string s="";
            for(int i=0;i<CommandList[Situation].Count();i++){
                s +=　String.Format( "{0,4}",i) + " "+ CommandList[Situation][i].ToString();
            }
            return s;
        }

#region 遺伝的アルゴリズム関連
        public void InsertRandomCommand(ref int[] OutputAddress)
        {
            int targetSit = CommonRandom.Next(CommandList.Count());
            int targetPlace = CommonRandom.Next(CommandList[targetSit].Count());

            CommandList[targetSit].Insert(targetPlace, new Command()
            {
                CommandSet = CommandSets[CommonRandom.Next(CommandSets.Count())]
            });
            CommandList[targetSit][targetPlace].Arguments[0]=new Command.Argument(ref CommonRandom){
                    Type = CommonRandom.Next(4) > 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                    AddressType=CommonRandom.Next(6),
                    ImmediateValue=Math.Pow(1000,CommonRandom.NextDouble()),
                    Address=CommonRandom.Next(CommandList[targetSit].Count())
                    //Address = CommonRandom.Next(targetPlace)
                };
            CommandList[targetSit][targetPlace].Arguments[1] = new Command.Argument(ref CommonRandom)
            {
                Type = CommonRandom.Next(4) > 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                AddressType = CommonRandom.Next(6),
                ImmediateValue = Math.Pow(1000, CommonRandom.NextDouble()),
//                Address = CommonRandom.Next(CommandList[targetSit].Count())
                Address = CommonRandom.Next(targetPlace)
            };
            for (int i = targetPlace+2; i < CommandList[targetSit].Count(); i++)
            {
                CommandList[targetSit][i].AddAddress(1);
            }
            for (int i = 0; i < OutputAddress.Count(); i++)
            {
                if (OutputAddress[i] > targetPlace)
                {
                    OutputAddress[i]++;
                }
            }
        }
        public void ChangeRandomCommand()
        {
            int targetSit = CommonRandom.Next(CommandList.Count());
            int targetPlace = CommonRandom.Next(CommandList[targetSit].Count());

            CommandList[targetSit][targetPlace]=new Command()
            {
                CommandSet = CommandSets[CommonRandom.Next(CommandSets.Count())]
            };
            CommandList[targetSit][targetPlace].Arguments[0] = new Command.Argument(ref CommonRandom)
            {
                Type = CommonRandom.Next(4) > 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                AddressType = CommonRandom.Next(6),
                ImmediateValue = Math.Pow(1000, CommonRandom.NextDouble()),
                                    Address=CommonRandom.Next(CommandList[targetSit].Count())
                //Address = CommonRandom.Next(targetPlace)
            };
            CommandList[targetSit][targetPlace].Arguments[1] = new Command.Argument(ref CommonRandom)
            {
                Type = CommonRandom.Next(4) > 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                AddressType = CommonRandom.Next(6),
                ImmediateValue = Math.Pow(1000, CommonRandom.NextDouble()),
                                Address = CommonRandom.Next(CommandList[targetSit].Count())
                //Address = CommonRandom.Next(targetPlace)
            };
        }
        
        public void AddRandomCommand()
        {
            int targetSit = CommonRandom.Next(CommandList.Count());
            int targetPlace = CommandList[targetSit].Count();

            CommandList[targetSit].Insert(targetPlace, new Command()
            {
                CommandSet = CommandSets[CommonRandom.Next(CommandSets.Count())]
            });
            CommandList[targetSit][targetPlace].Arguments[0] = new Command.Argument(ref CommonRandom)
            {
                Type = CommonRandom.Next(4) > 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                AddressType = CommonRandom.Next(6),
                ImmediateValue = Math.Pow(1000, CommonRandom.NextDouble()),
                //                    Address=CommonRandom.Next(CommandList[targetSit].Count())
                Address = CommonRandom.Next(targetPlace)
            };
            CommandList[targetSit][targetPlace].Arguments[1] = new Command.Argument(ref CommonRandom)
            {
                Type = CommonRandom.Next(4) > 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                AddressType = CommonRandom.Next(6),
                ImmediateValue = Math.Pow(1000, CommonRandom.NextDouble()),
                //                Address = CommonRandom.Next(CommandList[targetSit].Count())
                Address = CommonRandom.Next(targetPlace)
            };
            for (int i = targetPlace + 2; i < CommandList[targetSit].Count(); i++)
            {
                CommandList[targetSit][i].AddAddress(1);
            }
        }

        public void RemoveRandomCommand(ref int[] OutputAddress)
        {
            int targetSit = CommonRandom.Next(CommandList.Count());
            int targetPlace = CommonRandom.Next(CommandList[targetSit].Count());
            for (int i = targetPlace ; i < CommandList[targetSit].Count(); i++)
            {
                CommandList[targetSit][i].FixRemovedAddress(targetPlace);
            }
            CommandList[targetSit].RemoveAt(targetPlace);
            for (int i = 0; i < OutputAddress.Count(); i++)
            {
                if (OutputAddress[i] > targetPlace)
                {
                    OutputAddress[i]--;
                    OutputAddress[i] = Math.Max(OutputAddress[i], 0);
                }
            }
        }

        public void ChangeRandomArgument()
        {
            int targetSit = CommonRandom.Next(CommandList.Count());
            int targetPlace = CommonRandom.Next(CommandList[targetSit].Count());
            Command.Argument ag = CommandList[targetSit][targetPlace].Arguments[CommonRandom.Next(2)];

            ag.ImmediateValue = Math.Pow(1000, CommonRandom.NextDouble());
            ag.Address = CommonRandom.Next(CommandList[targetSit].Count());
            //ag.Address = CommonRandom.Next(targetPlace);
        }
#endregion
    }
}
