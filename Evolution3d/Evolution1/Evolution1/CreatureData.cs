using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution1
{
    public class CreatureData
    {
        public AIData AI;
        public BodyData Body;
        public CreatureType Type;
        public Guid ID=new Guid("1CDFBBB9-0ECC-4894-9871-C761F510C183");
        public Guid Parent = new Guid("1CDFBBB9-0ECC-4894-9871-C761F510C183");

        public enum CreatureType
        {
            eater,eaten
        }

        public CreatureData()
        {
        }
        public CreatureData(Creature original,CreatureType type){
            AI = new AIData(original.AI);
            Body = new BodyData(original.MainGene);
            Type = type;
            ID = original.ID;
            Parent = original.Parent;
        }

        public class AIData
        {
            public List<List<command>> commands = new List<List<command>>();

            public AIData()
            {
            }

            public AIData(CreatureAI ai)
            {
                for (int i = 0; i < ai.CommandList.Count(); i++)
                {
                    commands.Add(new List<command>());
                    for (int j = 0; j < ai.CommandList[i].Count(); j++)
                    {
                        commands[i].Add(new command(){
                            arguments=ai.CommandList[i][j].Arguments,
                            commandID=ai.CommandList[i][j].CommandSet.CommandID
                        });
                    }
                }
            }

            public class command
            {
                public Guid commandID;
                public CreatureAI.Command.Argument[] arguments;
            }
        }

        public class BodyData
        {
            public int[] outputAddress = new int[0];
            public Guid bodyID;
            public List<Joint> joints=new List<Joint>();
            public List<BodyGene.Connection> connections=new List<BodyGene.Connection>();

            public class Joint
            {
                public double weight;
                public double size;
                public double target;

                public double X;
                public double Y;
                public double Z;
                public BodyGene.BodyPart.ObjectType type;
            }

            public BodyData()
            {
            }

            public BodyData(BodyGene baseGene)
            {
                connections = baseGene.Connections;
                foreach (BodyGene.BodyPart bp in baseGene.Joints)
                {
                    joints.Add(new Joint() { weight = bp.Weight, size = bp.Size, target = bp.Target, X = bp.OrigX, Y = bp.OrigY, Z = bp.OrigZ, type = bp.Type });
                }
                outputAddress = baseGene.OutputAddress;
                bodyID = baseGene.BodyGeneID;
            }
        }
    }
}
