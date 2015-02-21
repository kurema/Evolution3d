using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution1
{
    public class CreatureAI
    {
        public const int SituationCount = 4;
        public class Command
        {
            public CommandSet CommandSet;
            public Argument[] Arguments={new Argument(),new Argument()};

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

            public override string ToString()
            {
                return this.CommandSet.Name + " " + Arguments[0].ToString() + (this.CommandSet.ArgumentCount == 2 ? " " + Arguments[1].ToString() : "") + "\n";
            }

            public class Argument
            {
                public ArgumentType Type;

                //参照の種類。
                //0:計算結果 1:計算結果、積分 2:計算、微分 3:入力 4:入力、積分 5:入力、微分
                public int AddressType;

                public double ImmediateValue;

                //負値の場合は、環境変数を指す。
                public int Address;

                public void AddAddress(int value)
                {
                    Address += value;
                    Address = Math.Max(value, 0);
                }

                public override string ToString()
                {
                    if (Type == ArgumentType.Address)
                    {
                        return (new string[]{ "LINE[", "INTEGRAL:LINE[", "DIFFERENTIATION:LINE[", "LINE[", "INTEGRAL:LINE[", "DIFFERENTIATION:LINE[" }[AddressType]) + Address + "]";

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
                            Random rnd = new Random();
                            Address = rnd.Next( Result[AddressType].Count() - 1);
                        }
                        return Result[AddressType][Address];
                    }
                    else if (Type == ArgumentType.Address && AddressType >= 3)
                    {
                        if (Input[AddressType-3].Count() <= Address)
                        {
                            Random rnd = new Random();
                            Address = rnd.Next(Input[AddressType-3].Count() - 1);
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
        }

        static public CommandSet[] CommandSets =
        {
            new CommandSet(){Function=(arg1,arg2)=>{return arg1+arg2;},ArgumentCount=2,Name="ADD"},
            new CommandSet(){Function=(arg1,arg2)=>{return arg1-arg2;},ArgumentCount=2,Name="SUB"},
            new CommandSet(){Function=(arg1,arg2)=>{return arg1*arg2;},ArgumentCount=2,Name="MUL"},
            new CommandSet(){Function=(arg1,arg2)=>
            {return Math.Abs(arg2)<=Math.Pow(10,-10)?( arg1*arg2>0? Math.Pow(10,10):-Math.Pow(10,10)):arg1/arg2;}
            ,ArgumentCount=2,Name="DIV"},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Log(arg1);},ArgumentCount=1,Name="LOG"},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Abs(arg1);},ArgumentCount=1,Name="ABS"},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Sin(arg1);},ArgumentCount=1,Name="SIN"},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Cos(arg1);},ArgumentCount=1,Name="COS"},
            new CommandSet(){Function=(arg1,arg2)=>{return Math.Pow(arg1,arg2);},ArgumentCount=2,Name="POW"},
            new CommandSet(){Function=(arg1,arg2)=>{return arg1;},ArgumentCount=1,Name="VALUE"},
        };


        //計算結果を格納
        //通常値、積分値、微分値
        public List<double>[][] Result = new List<double>[SituationCount][]{
            new List<double>[]{new List<double>(), new List<double>(), new List<double>()},
            new List<double>[]{new List<double>(), new List<double>(), new List<double>()},
            new List<double>[]{new List<double>(), new List<double>(), new List<double>()},
            new List<double>[]{new List<double>(), new List<double>(), new List<double>()}
        };
        public List<double>[] InputHistory = {new List<double>(), new List<double>(), new List<double>() };


        //命令列を定義。
        //状況により用いる命令列は異なる。
        public List<Command>[] CommandList 
            = { 
                  new List<Command>(),
                  new List<Command>(),
                  new List<Command>(),
                  new List<Command>(),
              };

        public double[] Execute(double[] Input,int Situation, ref int[] OutputAddress)
        {
            //以下の4Whileはバグがなければ実行されないはず。
            while (Result[Situation][0].Count() > Input.Count())
            {
                Result[Situation][0].Add(0);
                Result[Situation][1].Add(0);
                Result[Situation][2].Add(0);
            }
            while (Result[Situation][0].Count() < Input.Count())
            {
                Result[Situation][0].RemoveAt(Input.Count() - 1);
                Result[Situation][1].RemoveAt(Input.Count() - 1);
                Result[Situation][2].RemoveAt(Input.Count() - 1);
            }

            while (InputHistory[0].Count() > Input.Count())
            {
                InputHistory[0].Add(0);
                InputHistory[1].Add(0);
                InputHistory[2].Add(0);
            }
            while (InputHistory[0].Count() < Input.Count())
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
                OutputAddress[i] = Math.Max(Result.Count()-1, OutputAddress[i]);
                ret[i] = Result[Situation][0][OutputAddress[i]];
            }
            return ret;
        }

        public string CommandListToString(int Situation)
        {
            string s="";
            foreach(Command c in CommandList[Situation]){
                s += c.ToString();
            }
            return s;
        }

        //遺伝的アルゴリズム関連
        public void InsertRandomCommand(){
            Random rnd = new Random();
            int targetSit = rnd.Next(CommandList.Count());
            int targetPlace = rnd.Next(CommandList[targetSit].Count());
            //要チェック
            CommandList[targetSit].Insert(targetPlace, new Command()
            {
                CommandSet = CommandSets[rnd.Next(CommandSets.Count())]
            });
            CommandList[targetSit][targetPlace].Arguments[0]=new Command.Argument{
                    Type = rnd.Next(2) == 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                    AddressType=rnd.Next(6),
                    ImmediateValue=Math.Pow(1000,rnd.NextDouble()),
                    Address=rnd.Next(CommandList[targetSit].Count())
                };
            CommandList[targetSit][targetPlace].Arguments[1] = new Command.Argument
            {
                Type = rnd.Next(2) == 0 ? Command.Argument.ArgumentType.Address : Command.Argument.ArgumentType.Immediate,
                AddressType = rnd.Next(6),
                ImmediateValue = Math.Pow(1000, rnd.NextDouble()),
                Address = rnd.Next(CommandList[targetSit].Count())
            };
        }

        public void RemoveRandomCommand()
        {
            Random rnd = new Random();
            int targetSit = rnd.Next(CommandList.Count());
            int targetPlace = rnd.Next(CommandList[targetSit].Count());
            CommandList[targetSit].RemoveAt(targetPlace);
        }

        public void ChangeRandomArgument()
        {
            Random rnd = new Random();
            int targetSit = rnd.Next(CommandList.Count());
            int targetPlace = rnd.Next(CommandList[targetSit].Count());
            Command.Argument ag = CommandList[targetSit][targetPlace].Arguments[rnd.Next(2)];

            ag.ImmediateValue = Math.Pow(1000, rnd.NextDouble());
            ag.Address = rnd.Next(CommandList[targetSit].Count());
        }
    }
}
