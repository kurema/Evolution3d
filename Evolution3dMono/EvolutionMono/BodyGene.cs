using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Phy=StillDesign.PhysX;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Evolution1
{
    public class BodyGene
    {
        public Random CommonRandom = new Random();
        public List<BodyPart> Joints = new List<BodyPart>();
        public List<Connection> Connections = new List<Connection>();
        public int[] OutputAddress = new int[0];
        public List<double> Infos = new List<double>();
        public Guid BodyGeneID = Guid.NewGuid();


        public class BodyPart
        {
            public double Weight;
            public double Size;
            //参照値
            public double Target;

            public double OrigX;
            public double OrigY;
            public double OrigZ;

            public Guid Guid = Guid.NewGuid();

            public Phy.Actor PhyActor;

            public ObjectType Type;

            public enum ObjectType
            {
                Sphere,Cube,Wheel
            }
        }

        public class Connection
        {
            public int Target1;
            public int Target2;
            public float NaturalLength;
        }

        public BodyGene()
        {
            for (int i = 0; i < 4; i++)
            {
                Infos.Add(0);
            }
        }

        public BodyGene(BodyGene baseGene):this()
        {
            Connections =　new List<Connection>(baseGene.Connections);
            foreach (BodyPart bp in baseGene.Joints)
            {
                Joints.Add(new BodyPart() { Weight = bp.Weight, Size = bp.Size, Target = bp.Target, OrigX = bp.OrigX, OrigY = bp.OrigY, OrigZ = bp.OrigZ, Guid = Guid.NewGuid(), Type = bp.Type });
                Infos.Add(bp.OrigX);
                Infos.Add(bp.OrigY);
                Infos.Add(bp.OrigZ);
            }
            OutputAddress = baseGene.OutputAddress;
            CommonRandom = new Random(baseGene.CommonRandom.Next(int.MaxValue));
            BodyGeneID = baseGene.BodyGeneID;
        }

        public BodyGene(CreatureData.BodyData baseGene)
            : this()
        {
            Connections = baseGene.connections;
            foreach (CreatureData.BodyData.Joint bp in baseGene.joints)
            {
                Joints.Add(new BodyPart() { Weight = bp.weight, Size = bp.size, Target = bp.target, OrigX = bp.X, OrigY = bp.Y, OrigZ = bp.Z, Guid = Guid.NewGuid(), Type = bp.type });
                Infos.Add(bp.X);
                Infos.Add(bp.Y);
                Infos.Add(bp.Z);
            }
            OutputAddress = baseGene.outputAddress;
            BodyGeneID = baseGene.bodyID;
        }


        public BodyPart AddJoints()
        {
            BodyGeneID = Guid.NewGuid();
            if (Joints.Count() == 0)
            {
                BodyPart bp = new BodyPart() { Weight = 3, Size = 10, OrigX = 0.0, OrigY = 0.0, OrigZ = 0.0, Type = BodyPart.ObjectType.Sphere };
                Joints.Add(bp);
                Infos.Add(0);
                Infos.Add(0);
                Infos.Add(0);
                return bp;
            }
            else
            {
                int jcnt=Joints.Count();
                int cncnt=(int)Math.Min(Math.Floor(Math.Pow(CommonRandom.NextDouble()*1.1,10)),jcnt-1);
                //int cncnt = (int)Math.Min(Math.Floor(Math.Pow(CommonRandom.NextDouble()*1.1, 10)), jcnt - 1);
                int[] fnd = new int[cncnt + 1];
                int tgt;
                double tempX=0, tempY=0, tempZ=0;
                double tempSize = CommonRandom.NextDouble() * 3 + 1;
                for (int i = 0; i <= cncnt; i++)
                {
                    do
                    {
                        tgt = CommonRandom.Next(jcnt);
                    } while (fnd.Contains(tgt + 1));
                    if (i == 0)
                    {
                        bool conflict;
                        do
                        {
                            conflict = false;
                            tempX = Joints[tgt].OrigX + (CommonRandom.NextDouble() * 10 + tempSize * 2 + 10)*(CommonRandom.Next(2)==0?-1:1);
                            tempY = Joints[tgt].OrigY + CommonRandom.NextDouble() * 2 - 1;
                            tempZ = Joints[tgt].OrigZ + CommonRandom.NextDouble() * 30-15;
                            for (int j = 0; j < jcnt; j++)
                            {
                                conflict = conflict|| ((Joints[j].OrigX - tempX) * (Joints[j].OrigX - tempX) + (Joints[j].OrigY - tempY) * (Joints[j].OrigY - tempY) + (Joints[j].OrigZ - tempZ) * (Joints[j].OrigZ - tempZ) < (tempSize + Joints[j].Size + 1) * (tempSize + Joints[j].Size + 1));
                            }
                        } while (conflict);
                    }
                    float tempNaturalLen=(float)Math.Sqrt((Joints[tgt].OrigX - tempX) * (Joints[tgt].OrigX - tempX) + (Joints[tgt].OrigY - tempY) * (Joints[tgt].OrigY - tempY) + (Joints[tgt].OrigZ - tempZ) * (Joints[tgt].OrigZ - tempZ));
                    //Infos.Add(tempNaturalLen);
                    Connections.Add(new Connection() { Target1 = tgt, Target2 = jcnt, NaturalLength =tempNaturalLen  });
                    fnd[i] = tgt + 1;

                    List<int> tempList = OutputAddress.ToList();
                    tempList.Add(CommonRandom.Next(1000000));
                    tempList.Add(CommonRandom.Next(1000000));
                    tempList.Add(CommonRandom.Next(1000000));
                    OutputAddress = tempList.ToArray();
                }
                Infos.Add(tempX);
                Infos.Add(tempY);
                Infos.Add(tempZ);
                //Infos.Add(0);

                //List<int> tempList = OutputAddress.ToList();
                //tempList.Add(CommonRandom.Next(1000000));
                //tempList.Add(CommonRandom.Next(1000000));
                //tempList.Add(CommonRandom.Next(1000000));
                //OutputAddress = tempList.ToArray();
                BodyPart bp=new BodyPart() { Weight = tempSize * tempSize * tempSize / 10, Size = tempSize, OrigX = tempX, OrigY = tempY, OrigZ = tempZ,Type=(new BodyPart.ObjectType[2]{BodyPart.ObjectType.Sphere,BodyPart.ObjectType.Cube})[CommonRandom.Next(2)] };
                Joints.Add(bp);
                return bp;
            }
        }
    }
}
