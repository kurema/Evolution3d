using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution1
{
    public class CreatureAI
    {
        public class Command
        {
            public CommandSet CommandSet;
            public Argument[] Arguments={new Argument(),new Argument()};

            public double Execute(List<double>[] Result, int AddressOffset=0)
            {
                return CommandSet.Function(
                    Arguments[0].Value(Result,AddressOffset),
                    Arguments[1].Value(Result, AddressOffset)
                    );
            }

            public void AddAddress(int value)
            {
                Arguments[0].AddAddress(value);
                Arguments[1].AddAddress(value);
            }

            public class Argument
            {
                public ArgumentType Type;
                public AddressType AType;
                public double ImmediateValue;
                public int Address;
                //負値の場合は、環境変数を指す。

                public void AddAddress(int value)
                {
                    Address += value;
                    Address = Math.Max(value, 0);
                }

                public double Value(List<double>[] Result, int AddressOffset=0)
                {
                    int tempAddress = Address+AddressOffset >= Result.Length ? Result.Length - 1 : Address+AddressOffset;
                    switch (AType)
                    {
                        case AddressType.Basic:
                            return Result[0][Address];
                        case AddressType.Integral:
                            return Result[1][Address];
                        case AddressType.Differentiation:
                            return Result[2][Address];
                        default:
                            return 0;
                    }
                }
                public enum ArgumentType
                {
                    Address, Immediate
                }
                public enum AddressType
                {
                    Basic,Integral,Differentiation
                }
            }
        }

        public class CommandSet
        {
            public String Name;
            public Func<double, double, double> Function;
            public Int16 ArgumentCount;
        }

        static public CommandSet[] Commands =
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
            new CommandSet(){Function=(arg1,arg2)=>{return arg1;},ArgumentCount=1,Name="POW"},
        };


        //計算結果を格納
        //通常値、積分値、微分値
        public List<double>[] Result = {new List<double>(),new List<double>(),new List<double>()};

        //命令列を定義。
        //状況により用いる命令列は異なる。
        public List<Command>[] CommandList 
            = { 
                  new List<Command>(),
                  new List<Command>(),
                  new List<Command>(),
                  new List<Command>(),
              };

        private int LastInputValueCount = 0;

        public double[] Execute(double[] Input,int Situation, ref int[] OutputAddress)
        {
            int AddressDif =Input.Count()- LastInputValueCount;
            int count=0;

            //以下の二分岐はバグがなければ実行されないはず。
            if(Result[0].Count>Input.Count()+CommandList.Count()){
                int temp = Result[0].Count() - (Result[0].Count -( Input.Count() + CommandList.Count()));
                for (int i = 0; Result[0].Count > Input.Count() + CommandList.Count(); i++)
                {
                    Result[0].RemoveAt(temp);
                    Result[1].RemoveAt(temp);
                    Result[2].RemoveAt(temp);
                }
            }
            if(Result[0].Count<Input.Count()+CommandList.Count()){
                for(int i=0;Result[0].Count<Input.Count()+CommandList.Count();i++){
                    Result[0].Add(0);
                    Result[1].Add(0);
                    Result[2].Add(0);
                }
            }

            for (int i = 0; i < Input.Count(); i++)
            {
                Result[2][count] = Input[i] - Result[0][count];
                Result[0][count] = Input[i];
                Result[1][count] += Input[i];
                count++;
            }
            for (int i = 0; i < CommandList[Situation].Count(); i++)
            {
                double value = CommandList[Situation][i].Execute(this.Result,AddressDif);
                Result[2][count] = value - Result[0][count];
                Result[0][count] = value;
                Result[1][count] += value;
                count++;
            }
            double[] ret =
                new double[OutputAddress.Count()];
            for (int i = 0; i < OutputAddress.Count(); i++)
            {
                OutputAddress[i] = Math.Max(Result.Count()-1, OutputAddress[i]);
                ret[i] = Result[0][OutputAddress[i]];
            }
            lastInputValueCount = Input.Count();
            return ret;
        }
    }
}
