using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Phy = StillDesign.PhysX;

namespace Evolution1
{
    /// <summary>
    /// 基底 Game クラスから派生した、ゲームのメイン クラスです。
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model ModelCube;
        Model ModelCube2;
        Model ModelCube3;
        Model ModelCube4;
        Model ModelSphere;
        Model ModelSphere2;
        Model ModelSphere3;
        Model ModelSphere4;
        Camera Camera;
        Random rnd = new Random();

        List<Creature> Eaters = new List<Creature>();
        List<Creature> Eatens = new List<Creature>();
        List<CreatureAdditonalInfo> EatersInfo = new List<CreatureAdditonalInfo>();
        List<CreatureAdditonalInfo> EatensInfo = new List<CreatureAdditonalInfo>();
        /// <summary>
        /// スプライトでテキストを描画するためのフォント
        /// </summary>
        private SpriteFont font = null;

        private string Log = "";
        private bool LogSilentMode = false;

        //PhysX関連
        Phy.Core PhyCore;
        Phy.Scene PhyScene;
        Vector3 GroundScale = new Vector3(1000, 1, 1000);
        //Phy.Actor GroundActor;

        //Box関係
        Phy.Actor[] BoxActors;
        const int BoxFigure = 3;
        Vector3 BoxScale = new Vector3(3, 3, 3);

        BasicEffect LineBasicEffect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 1290;
            this.graphics.PreferredBackBufferHeight = 730;
        }

        string textMessage = "";
        List<latestMessage> LatestMessages = new List<latestMessage>();
        struct latestMessage
        {
            public string Text;
            public DateTime Time;
        }
        private void addLatestMessage(string text)
        {
            LatestMessages.Add(new latestMessage() { Text = text, Time = DateTime.Now });
            if (!LogSilentMode)
                Log += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\n" + text + "\n\n";
            //if(MessageMode!=MessageModes.Silent)MessageMode = MessageModes.Normal;
        }
        private void addLatestMessage(string text, string deatail)
        {
            LatestMessages.Add(new latestMessage() { Text = text, Time = DateTime.Now });
            if (!LogSilentMode)
                Log += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\n" + text + "\n(" + deatail + ")\n\n";
            //if (MessageMode != MessageModes.Silent) MessageMode=MessageModes.Normal;
        }
        private void AddLog(string text)
        {
            if (!LogSilentMode)
                Log += DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Log\n" + text + "\n\n";
        }

        private string GuidToString(Guid id)
        {
            if (id == new Guid("E38D30A9-34F8-4894-93A3-CB75BFD3C033"))
            {
                return "God";
            }
            else if (id == new Guid("1CDFBBB9-0ECC-4894-9871-C761F510C183"))
            {
                return "Unknown";
            }
            else
            {
                return id.ToString();
            }
        }

        private void report()
        {
            string report = "Report\n*Eaters\n  There is " + Eaters.Count() + " predators.\n";
            foreach (Creature ct in Eaters)
            {
                report += "   ID:" + ct.ID + " Parent:" + ct.Parent + " Body:" + ct.MainGene.BodyGeneID + " at " + ct.MainGene.Joints[0].PhyActor.GlobalPosition + "\n";
            }
            report += "\n  There is " + Eatens.Count() + " preys.\n";
            foreach (Creature ct in Eatens)
            {
                report += "   ID:" + ct.ID + " Parent:" + ct.Parent + " Body:" + ct.MainGene.BodyGeneID + " at " + ct.MainGene.Joints[0].PhyActor.GlobalPosition + "\n";
            }
            AddLog(report);
        }
        private string getLatestMessage()
        {
            DateTime dn = DateTime.Now;
            string ret = "";
            for (int i = 0; i < LatestMessages.Count(); i++)
            {
                latestMessage lm = LatestMessages[i];
                if ((dn - lm.Time) < new TimeSpan(0, 0, 10))
                {
                    ret += lm.Text + "\n";
                }
                else
                {
                    LatestMessages.Remove(lm);
                    i--;
                }
                if (LatestMessages.Count() == 0)
                {
                    resetMessageMode();
                }
            }
            return ret;
        }

        Dictionary<Guid, int> CreatureCount = new Dictionary<Guid, int>();

        public Creature AddCreature(float OffsetX, float OffsetY, float OffsetZ)
        {
            return AddCreature(new Creature(ref rnd), OffsetX, OffsetY, OffsetZ);
        }

        public Creature AddCreature(Creature Creature, float OffsetX, float OffsetY, float OffsetZ)
        {
            if (CreatureCount.ContainsKey(Creature.MainGene.BodyGeneID))
            {
                CreatureCount[Creature.MainGene.BodyGeneID]++;
            }
            else
            {
                CreatureCount[Creature.MainGene.BodyGeneID] = 1;
            }
            foreach (BodyGene.BodyPart bp in Creature.MainGene.Joints)
            {
                Phy.ShapeDescription spShapeDesc = bp.Type == BodyGene.BodyPart.ObjectType.Sphere ? (Phy.ShapeDescription)new Phy.SphereShapeDescription((float)bp.Size) : new Phy.BoxShapeDescription((float)bp.Size, (float)bp.Size, (float)bp.Size);
                Phy.ActorDescription actorDesc = new Phy.ActorDescription()
                {
                    Name = String.Format("Object {0}", bp.Guid.ToString()),
                    BodyDescription = new Phy.BodyDescription((float)bp.Weight),
                    GlobalPose = Phy.MathPrimitives.Matrix.Translation((float)bp.OrigX + OffsetX, (float)bp.OrigY + OffsetY, (float)bp.OrigZ + OffsetZ),
                    Shapes = { spShapeDesc },
                };
                bp.PhyActor = this.PhyScene.CreateActor(actorDesc);
            }
            AddLog("Creature Added\nName: " + GuidToString(Creature.ID) + "\nParent: " + GuidToString(Creature.Parent) + "\nBody ID: " + Creature.MainGene.BodyGeneID.ToString() + " #" + CreatureCount[Creature.MainGene.BodyGeneID]);
            return Creature;
        }

        public Creature DuplicateCreature(Creature Base, float OffsetX, float OffsetY, float OffsetZ)
        {
            return AddCreature(new Creature(rnd, Base), OffsetX, OffsetY, OffsetZ);
        }
        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います。
        /// ここで、必要なサービスを照会して、関連するグラフィック以外のコンテンツを
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポーネントが列挙されるとともに、初期化されます。
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジックを追加します。

            base.Initialize();
            LogSilentMode = true;

            #region Camera initialize.
            {
                Viewport viewport = this.GraphicsDevice.Viewport;

                Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.PiOver4,
                    viewport.AspectRatio,
                    1, 10000
                    );

                this.Camera = new Camera(
                    new Vector3(0, 30, 100),
                    new Vector3(0, 20, 0),
                    Vector3.Up,
                    projection,
                    0.1f);
            }
            #endregion

            #region PhysX initialize.
            {
                Phy.CoreDescription coreDesc = new Phy.CoreDescription();

                UserOutput output = new UserOutput();
                this.PhyCore = new Phy.Core(coreDesc, output);
                Phy.SceneDescription sceneDesc = new Phy.SceneDescription()
                {
                    Gravity = new Phy.MathPrimitives.Vector3(0.0f, -9.81f * 5, 0.0f),
                    GroundPlaneEnabled = true
                };

                this.PhyScene = this.PhyCore.CreateScene(sceneDesc);
                PhyScene.Materials[0].StaticFriction = 0.95f;
                PhyScene.Materials[0].DynamicFriction = 0.8f;
                this.PhyCore.SetParameter(Phy.PhysicsParameter.ContinuousCollisionDetection, false);
            }
            #endregion

            //#region Create ground.
            //{
            //    Phy.BoxShapeDescription boxShapeDesc =
            //        new Phy.BoxShapeDescription(GroundScale.X, GroundScale.Y, GroundScale.Z);
            //    Phy.ActorDescription actorDesc = new Phy.ActorDescription()
            //    {
            //        GlobalPose = Phy.MathPrimitives.Matrix.Translation(new Phy.MathPrimitives.Vector3(0, 5, 0)),
            //        Shapes = { boxShapeDesc }
            //    };

            //    this.GroundActor = this.PhyScene.CreateActor(actorDesc);
            //}
            //#endregion

            //BOX関係
            #region Create boxs.
            {
                this.BoxActors = new Phy.Actor[BoxFigure];
                for (int x = 0; x < BoxFigure; x++)
                {
                    // 箱形状の作成
                    Phy.BoxShapeDescription boxShapeDesc
                            = new Phy.BoxShapeDescription(BoxScale.X, BoxScale.Y, BoxScale.Z);

                    // 質量属性等の記述
                    Phy.ActorDescription actorDesc = new Phy.ActorDescription()
                    {
                        Name = String.Format("Box {0}", x),
                        BodyDescription = new Phy.BodyDescription(100.0f),
                        GlobalPose = Phy.MathPrimitives.Matrix.Translation(0, 50 + 3 * x, 0),
                        Shapes = { boxShapeDesc },
                    };

                    // シーンに登録及びアクターの生成
                    this.BoxActors[x] = this.PhyScene.CreateActor(actorDesc);
                }
            }
            #endregion

            #region CreatureRelated
            {
                //クリーチャー
                for (int i = 0; i < 20; i++)
                {
                    float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    try
                    {
                        Eaters.Add(AddCreature(xvalue, 500, zvalue));
                        EatersInfo.Add(new CreatureAdditonalInfo() { BornTime = 0, OriginalPlace = new Vector3(xvalue, 0, zvalue), HitPoint = 100 });
                    }
                    catch
                    {
                        AddLog("Failed to add Creature.(Initializetion Eater)");
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    try
                    {
                        Eatens.Add(AddCreature(xvalue, 500, zvalue));
                        EatensInfo.Add(new CreatureAdditonalInfo() { BornTime = 0, OriginalPlace = new Vector3(xvalue, 500, zvalue), HitPoint = 100 });
                    }
                    catch
                    {
                        AddLog("Failed to add Creature.(Initializetion Eaten)");
                    }
                }
            }
            #endregion

            LineBasicEffect = new BasicEffect(GraphicsDevice);
            LogSilentMode = false;

            addLatestMessage("Push H for help.", "Hello, World!");

        }


        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// 読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            // 新規の SpriteBatch を作成します。これはテクスチャーの描画に使用できます。
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = this.Content.Load<SpriteFont>("SpriteFont1");

            this.ModelCube = this.Content.Load<Model>("Cube");
            this.ModelCube2 = this.Content.Load<Model>("Cube2");
            this.ModelCube3 = this.Content.Load<Model>("Cube3");
            this.ModelCube4 = this.Content.Load<Model>("Cube4");
            this.ModelSphere = this.Content.Load<Model>("Sphere");
            this.ModelSphere2 = this.Content.Load<Model>("Sphere2");
            this.ModelSphere3 = this.Content.Load<Model>("Sphere3");
            this.ModelSphere4 = this.Content.Load<Model>("Sphere4");


            // TODO: this.Content クラスを使用して、ゲームのコンテンツを読み込みます。
        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// アンロードします。
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。
            report();
            SaveTextFile("./Creatures/log." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + ".txt", Log);
        }

        private static double GetLength(BodyGene.BodyPart a, BodyGene.BodyPart b)
        {
            return Vector3.Distance(a.PhyActor.GlobalPosition.As<Vector3>(), b.PhyActor.GlobalPosition.As<Vector3>());
            //return Math.Sqrt((a.PhyActor.GlobalPosition.X - b.PhyActor.GlobalPosition.X) * (a.PhyActor.GlobalPosition.X - b.PhyActor.GlobalPosition.X) +
            //    (a.PhyActor.GlobalPosition.Y - b.PhyActor.GlobalPosition.Y) * (a.PhyActor.GlobalPosition.Y - b.PhyActor.GlobalPosition.Y) +
            //    (a.PhyActor.GlobalPosition.Z - b.PhyActor.GlobalPosition.Z) * (a.PhyActor.GlobalPosition.Z - b.PhyActor.GlobalPosition.Z));
        }

        private static double GetLength(Creature a, Creature b)
        {
            return GetLength(a.MainGene.Joints[0], b.MainGene.Joints[1]);
        }

        private int TargetCnt = 0;
        private bool TargetEater = true;
        private bool PrevKeyPushed = false;

        private void SetClosestTarget()
        {
            Camera.TargetMode = true;
            MessageMode = MessageModes.Target;

            bool isEater = true;
            int cnt = 0;
            double minDist = 10000;
            for (int i = 0; i < Eaters.Count(); i++)
            {
                Creature ct = Eaters[i];
                if (minDist > Vector3.Distance(ct.MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>(), Camera.CameraPosition))
                {
                    minDist = Vector3.Distance(ct.MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>(), Camera.CameraPosition);
                    isEater = true;
                    cnt = i;
                }
            }
            for (int i = 0; i < Eatens.Count(); i++)
            {
                Creature ct = Eatens[i];
                if (minDist > Vector3.Distance(ct.MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>(), Camera.CameraPosition))
                {
                    minDist = Vector3.Distance(ct.MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>(), Camera.CameraPosition);
                    isEater = false;
                    cnt = i;
                }
            }
            Camera.Target = isEater ? Eaters[cnt] : Eatens[cnt];
            TargetCnt = cnt;
            TargetEater = isEater;
            SetTarget();
        }

        private void SetTarget()
        {
            Camera.Target = TargetEater ? Eaters[Math.Min(TargetCnt, Eaters.Count() - 1)] : Eatens[Math.Min(TargetCnt, Eatens.Count() - 1)];
        }

        private MouseState currentMouse = Mouse.GetState();
        private MouseState prevMouse = Mouse.GetState();

        private class CreatureAdditonalInfo
        {
            public int HitPoint = 100;
            public Vector3 OriginalPlace;
            public double BornTime;
        }

        private double LastSaveTime;

        private MessageModes MessageMode = MessageModes.Normal;

        private enum MessageModes
        {
            Normal, Help, Target, FileSelect, Silent
        }
        private void resetMessageMode()
        {
            if (MessageMode != MessageModes.Silent)
            {
                if (Camera.TargetMode)
                {
                    MessageMode = MessageModes.Target;
                }
                else
                {
                    MessageMode = MessageModes.Normal;
                }
            }
        }

        /// <summary>
        /// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
        /// ゲーム ロジックを、実行します。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            GamePadState gpState = GamePad.GetState(PlayerIndex.One);
            KeyboardState kbState = Keyboard.GetState();
            #region Input Related
            {
                // ゲームの終了条件をチェックします。
                if (gpState.Buttons.Back == ButtonState.Pressed || kbState.IsKeyDown(Keys.Escape))
                    this.Exit();
                if (!PrevKeyPushed)
                {
                    if (Camera.TargetMode)
                    {
                        if (kbState.IsKeyDown(Keys.T))
                        {
                            Camera.TargetMode = false;
                            MessageMode = MessageModes.Normal;
                            PrevKeyPushed = true;
                        }
                        if (kbState.IsKeyDown(Keys.R))
                        {
                            if (TargetCnt == 0)
                            {
                                TargetEater = !TargetEater;
                                TargetCnt = (TargetEater ? Eaters.Count() - 1 : Eatens.Count() - 1);
                            }
                            else
                            {
                                TargetCnt--;
                            }
                            SetTarget();
                            PrevKeyPushed = true;
                        }
                        if (kbState.IsKeyDown(Keys.Y))
                        {
                            if (TargetCnt == (TargetEater ? Eaters.Count() - 1 : Eatens.Count() - 1))
                            {
                                TargetEater = !TargetEater;
                                TargetCnt = 0;
                            }
                            else
                            {
                                TargetCnt++;
                            }
                            SetTarget();
                            PrevKeyPushed = true;
                        }
                        if (kbState.IsKeyDown(Keys.K))
                        {
                            if ((TargetEater ? Eaters.Count() : Eatens.Count()) > 1)
                            {
                                addLatestMessage("You Killed a poor " + (TargetEater ? "predator" : "prey") + ".");
                                RemoveCreature(TargetEater, TargetCnt);
                            }
                            else
                            {
                                addLatestMessage("You can't Kill this " + (TargetEater ? "predator" : "prey") + ".\nYou've killed too much.");
                            }
                            PrevKeyPushed = true;
                        }
                        if (kbState.IsKeyDown(Keys.J))
                        {
                            SaveCreatureAI(TargetEater, TargetCnt);
                            PrevKeyPushed = true;
                        }
                        if (kbState.IsKeyDown(Keys.F))
                        {
                            SaveCreature(TargetEater, TargetCnt);
                            PrevKeyPushed = true;
                        }
                    }
                    else
                    {
                        if (kbState.IsKeyDown(Keys.T))
                        {
                            SetClosestTarget();
                        }
                        if (kbState.IsKeyDown(Keys.L))
                        {
                            if (MessageMode == MessageModes.FileSelect)
                            {
                                resetMessageMode();
                            }
                            else
                            {
                                MessageMode = MessageModes.FileSelect;
                                int count = 0;
                                if (0 == CreatureFiles.Count())
                                {
                                    MessageMode = MessageModes.Normal;
                                    addLatestMessage("File not found.", "No file in Creature folder.");
                                }
                                else
                                {
                                    while (System.IO.Path.GetExtension(CreatureFiles[CreatureFileCnt]).ToLower() != ".xml")
                                    {
                                        CreatureFileCnt = (CreatureFileCnt + 1) % CreatureFiles.Count();
                                        if (count >= CreatureFiles.Count())
                                        {
                                            MessageMode = MessageModes.Normal;
                                            addLatestMessage("File not found.", "No xml file in Creature folder.");
                                            break;
                                        }
                                        count++;
                                    }
                                }
                            }
                        }
                        if (kbState.IsKeyDown(Keys.F))
                        {
                            SaveState();
                            LastSaveTime = gameTime.TotalGameTime.TotalSeconds;
                        }
                        if (kbState.IsKeyDown(Keys.P))
                        {
                            try
                            {
                                Creature temp = AddCreature(Camera.LookAtPosition.X, 500, Camera.LookAtPosition.Z);
                                Eaters.Add(temp);
                                EatersInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(Camera.LookAtPosition.X, 0, Camera.LookAtPosition.Z) });
                                addLatestMessage("New predator is caged.");
                            }
                            catch
                            {
                                AddLog("Failed to add Creature(New predator is caged).");
                            }
                        }
                        if (kbState.IsKeyDown(Keys.O))
                        {
                            try
                            {
                                Creature temp = AddCreature(Camera.LookAtPosition.X, 500, Camera.LookAtPosition.Z);
                                Eatens.Add(temp);
                                EatensInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(Camera.LookAtPosition.X, 0, Camera.LookAtPosition.Z) });
                                addLatestMessage("New prey is caged.", "1");
                            }
                            catch
                            {
                                AddLog("Failed to add Creature.(New prey is caged. 1)");
                            }
                        }
                    }
                    if (kbState.IsKeyDown(Keys.U))
                    {
                        if (MessageMode == MessageModes.Silent)
                        {
                            addLatestMessage("Escaped silent mode.");
                            MessageMode = MessageModes.Normal;
                        }
                        else
                        {
                            MessageMode = MessageModes.Silent;
                        }
                    }
                    if (MessageMode == MessageModes.FileSelect)
                    {
                        if (kbState.IsKeyDown(Keys.Enter) && (kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift)))
                        {
                            LoadCreatureForRecording(CreatureFiles[CreatureFileCnt], gameTime.TotalGameTime.TotalSeconds);
                            resetMessageMode();
                        }
                        else if (kbState.IsKeyDown(Keys.Enter))
                        {
                            LoadCreature(CreatureFiles[CreatureFileCnt], gameTime.TotalGameTime.TotalSeconds);
                            resetMessageMode();
                        }
                        if (kbState.IsKeyDown(Keys.Up))
                        {
                            do
                            {
                                CreatureFileCnt = (CreatureFileCnt - 1 + CreatureFiles.Count()) % CreatureFiles.Count();
                            }
                            while (System.IO.Path.GetExtension(CreatureFiles[CreatureFileCnt]).ToLower() != ".xml");
                        }
                        if (kbState.IsKeyDown(Keys.Down))
                        {
                            do
                            {
                                CreatureFileCnt = (CreatureFileCnt + 1) % CreatureFiles.Count();
                            }
                            while (System.IO.Path.GetExtension(CreatureFiles[CreatureFileCnt]).ToLower() != ".xml");
                        }
                    }

                    if (kbState.IsKeyDown(Keys.H))
                    {
                        if (MessageMode != MessageModes.Help)
                        {
                            MessageMode = MessageModes.Help;
                        }
                        else
                        {
                            resetMessageMode();
                        }
                    }
                    if (kbState.IsKeyDown(Keys.I))
                    {
                        DisplayMode = !DisplayMode;
                    }
                    if (kbState.IsKeyDown(Keys.F11))
                    {
                        this.graphics.ToggleFullScreen();
                    }
                    if (kbState.IsKeyDown(Keys.F12))
                    {
                        RecordingMode = !RecordingMode;
                        addLatestMessage((RecordingMode ? "Entered " : "Escaped ") + "Recording Mode");
                        if (RecordingMode) MessageMode = MessageModes.Silent;
                        else MessageMode = MessageModes.Normal;
                    }
                }
                PrevKeyPushed = !(kbState.IsKeyUp(Keys.U) && kbState.IsKeyUp(Keys.I) && kbState.IsKeyUp(Keys.P) && kbState.IsKeyUp(Keys.O) && kbState.IsKeyUp(Keys.K) && kbState.IsKeyUp(Keys.T) && kbState.IsKeyUp(Keys.R) && kbState.IsKeyUp(Keys.Y) && kbState.IsKeyUp(Keys.J) && kbState.IsKeyUp(Keys.F) && kbState.IsKeyUp(Keys.L) && kbState.IsKeyUp(Keys.H) && kbState.IsKeyUp(Keys.Down) && kbState.IsKeyUp(Keys.Up) && kbState.IsKeyUp(Keys.F11) && kbState.IsKeyUp(Keys.F12));


                //Camera
                // マウス情報の取得
                prevMouse = currentMouse;
                currentMouse = Mouse.GetState();

                // ホイールにより距離を変更
                this.Camera.Radius -= (currentMouse.ScrollWheelValue - prevMouse.ScrollWheelValue) * this.Camera.CloseUpSpeed;

                if (this.Camera.TargetMode)
                {
                    this.Camera.TargetRot.Y += this.Camera.TargetRotSpeed;
                    if (currentMouse.RightButton == ButtonState.Pressed)
                        this.Camera.TargetRotSpeed += MathHelper.ToRadians(currentMouse.X - prevMouse.X) * 0.005f;
                    this.Camera.CameraPosition = this.Camera.Target.MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>() + Camera.AxisToForce(this.Camera.TargetRot.X, this.Camera.TargetRot.Y) * this.Camera.Radius;
                    this.Camera.LookAtPosition = this.Camera.Target.MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>();
                }
                else
                {

                    // 左ボタンで回転
                    if (currentMouse.RightButton == ButtonState.Pressed)
                    {
                        this.Camera.CamRot.Y += MathHelper.ToRadians(currentMouse.X - prevMouse.X);
                        this.Camera.CamRot.X += MathHelper.ToRadians(currentMouse.Y - prevMouse.Y);

                        this.Camera.CamRot.X = MathHelper.ToRadians(MathHelper.Clamp(MathHelper.ToDegrees(this.Camera.CamRot.X), -89, 89));
                        if (this.Camera.CamRot.Y < -MathHelper.TwoPi) this.Camera.CamRot.Y += MathHelper.TwoPi;
                        if (this.Camera.CamRot.Y > +MathHelper.TwoPi) this.Camera.CamRot.Y -= MathHelper.TwoPi;

                        if (this.IsActive)
                        {
                            Mouse.SetPosition((currentMouse.X + 600 - 1) % 600 + 1, (currentMouse.Y + 600 - 1) % 600 + 1);
                            currentMouse = Mouse.GetState();
                        }
                    }
                    else if (currentMouse.LeftButton == ButtonState.Pressed)
                    {
                        this.Camera.LookAtPosition.X += (float)((currentMouse.X - prevMouse.X) * Math.Cos(this.Camera.CamRot.Y) - (currentMouse.Y - prevMouse.Y) * Math.Sin(this.Camera.CamRot.Y));
                        this.Camera.LookAtPosition.Z += (float)((currentMouse.X - prevMouse.X) * Math.Sin(this.Camera.CamRot.Y) + (currentMouse.Y - prevMouse.Y) * Math.Cos(this.Camera.CamRot.Y));

                        if (this.IsActive)
                        {
                            Mouse.SetPosition((currentMouse.X + 600 - 1) % 600 + 1, (currentMouse.Y + 600 - 1) % 600 + 1);
                            currentMouse = Mouse.GetState();
                        }
                    }
                    const float moveLen = 3;
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        this.Camera.LookAtPosition.X -= moveLen * (float)Math.Cos(this.Camera.CamRot.Y);
                        this.Camera.LookAtPosition.Z -= moveLen * (float)Math.Sin(this.Camera.CamRot.Y);
                    }
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        this.Camera.LookAtPosition.X += moveLen * (float)Math.Cos(this.Camera.CamRot.Y);
                        this.Camera.LookAtPosition.Z += moveLen * (float)Math.Sin(this.Camera.CamRot.Y);
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        this.Camera.LookAtPosition.X += moveLen * (float)Math.Sin(this.Camera.CamRot.Y);
                        this.Camera.LookAtPosition.Z -= moveLen * (float)Math.Cos(this.Camera.CamRot.Y);
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        this.Camera.LookAtPosition.X -= moveLen * (float)Math.Sin(this.Camera.CamRot.Y);
                        this.Camera.LookAtPosition.Z += moveLen * (float)Math.Cos(this.Camera.CamRot.Y);
                    }
                    if (kbState.IsKeyDown(Keys.E))
                    {
                        this.Camera.CamRot.Y += 0.01f;
                    }
                    if (kbState.IsKeyDown(Keys.Q))
                    {
                        this.Camera.CamRot.Y -= 0.01f;
                    }

                    this.Camera.CameraPosition = this.Camera.LookAtPosition + Evolution1.Camera.AxisToForce(this.Camera.CamRot.X, this.Camera.CamRot.Y) * this.Camera.Radius;
                }
            }
            #endregion

            #region メッセージ関連
            switch (MessageMode)
            {
                case MessageModes.Normal:
                    textMessage = getLatestMessage();
                    break;
                case MessageModes.Target:
                    textMessage = "" + (TargetEater ? "Predator #" : "Prey #") + TargetCnt + "\n";
                    textMessage += "Name: " + GuidToString(Camera.Target.ID) + "\nParent: " + GuidToString(Camera.Target.Parent) + "\n";
                    textMessage += "Race: " + Camera.Target.MainGene.BodyGeneID + "\nRace population: " + CreatureCount[Camera.Target.MainGene.BodyGeneID] + "\n";
                    textMessage += "(" + Math.Floor(Camera.Target.MainGene.Joints[0].PhyActor.GlobalPosition.X) + " , "
                        + Math.Floor(Camera.Target.MainGene.Joints[0].PhyActor.GlobalPosition.Y) + " , "
                        + Math.Floor(Camera.Target.MainGene.Joints[0].PhyActor.GlobalPosition.Z) + ")\n";
                    break;
                case MessageModes.Help:
                    if (Camera.TargetMode)
                    {
                        textMessage = "Help\n Mouse\n  click right button and drag: change spped of rotation.\n Keyboard\n  Y: Go to next creature.\n  R: Go to previous creature.\n  F: Save creature as XML\n  T: Quit target mode.\n  K: kill.\n  F: Save this.\n  J: Save AI of this.\n  I:Enter caliculation mode.\n  Esc: Close\n  H:Show/close this help";
                    }
                    else
                    {
                        textMessage = "Help\n Mouse\n  click right button and drag: rotate the point of view.\n  click left button and drag: move POV.\n Keyboard\n  WASD: Move\n  QE: Rotate\n  F: Save state as XML\n  PO:Add new Predator(P)/Prey(O).\n  T: Enter target mode.\n  U: Enter silent mode.\n  I:Enter caliculation mode.\n  Esc: Close\n  H:Show/close this help";
                    }
                    break;
                case MessageModes.FileSelect:
                    textMessage = "Do you want to load following file?\n" + CreatureFiles[CreatureFileCnt] + "\n\nHelp\n Up:Previous file.\n Down:Next file.\n L:Quit.\n Enter:Load.";
                    break;
                case MessageModes.Silent:
                    textMessage = "";
                    break;
            }
            #endregion

            if (gameTime.TotalGameTime.TotalSeconds - LastSaveTime > 600 && !RecordingMode)
            {
                SaveState();
                LastSaveTime = gameTime.TotalGameTime.TotalSeconds;
                report();

                SaveTextFile("./Creatures/log." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + ".txt", Log);
                Log = "";
            }


            this.PhyScene.Simulate((float)gameTime.ElapsedGameTime.TotalSeconds);
            //this.PhyScene.Simulate(1.0f/60.0f);
            this.PhyScene.FlushStream();
            this.PhyScene.FetchResults(Phy.SimulationStatus.RigidBodyFinished, true);

            double[] disEaters = new double[Eaters.Count()];
            double[] disEatens = new double[Eatens.Count()];
            Phy.MathPrimitives.Vector3[] vecEaters = new Phy.MathPrimitives.Vector3[Eaters.Count()];
            Phy.MathPrimitives.Vector3[] vecEatens = new Phy.MathPrimitives.Vector3[Eatens.Count()];

            for (int j = 0; j < Eatens.Count(); j++)
            {
                disEatens[j] = 15000;
            }
            for (int i = 0; i < Eaters.Count(); i++)
            {
                disEaters[i] = 15000;
                for (int j = 0; j < Eatens.Count(); j++)
                {
                    double temp = GetLength(Eaters[i], Eatens[j]);
                    if (disEaters[i] > temp)
                    {
                        disEaters[i] = temp;
                        vecEaters[i] = Eaters[i].MainGene.Joints[0].PhyActor.GlobalPosition -
                            Eatens[j].MainGene.Joints[0].PhyActor.GlobalPosition;
                    }
                    if (disEatens[j] > temp)
                    {
                        disEatens[j] = temp;
                        vecEatens[j] = Eatens[j].MainGene.Joints[0].PhyActor.GlobalPosition -
                            Eaters[i].MainGene.Joints[0].PhyActor.GlobalPosition;
                    }
                }

                base.Update(gameTime);
            }

            for (int cnt = 0; cnt < Eaters.Count(); cnt++)
            {
                if (disEaters[cnt] < 100)
                {
                    if (Eaters.Count() < 100 && CreatureCount[Eaters[cnt].MainGene.BodyGeneID] < 10)
                    {
                        float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                        float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                        try
                        {
                            Eaters.Add(DuplicateCreature(Eaters[cnt], xvalue, 500, zvalue));
                            EatersInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });

                            addLatestMessage("A predator ate a prey.");
                        }
                        catch
                        {
                            AddLog("Failed to add Creature.(A predator ate a prey)");
                        }

                        List<Phy.MathPrimitives.Vector3> tempVec = vecEaters.ToList();
                        tempVec.Add(new Phy.MathPrimitives.Vector3());
                        vecEaters = tempVec.ToArray();

                        List<double> tempdis = disEaters.ToList();
                        tempdis.Add(1000);
                        disEaters = tempdis.ToArray();

                    }

                }
                else if (disEaters[cnt] > 3000)
                {
                    addLatestMessage("A predator escaped.");
                    float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    if (rnd.Next(3) > 0)
                    {
                        rnd.Next();
                        try
                        {
                            Eaters.Add(DuplicateCreature(Eaters[cnt], xvalue, 500, zvalue));
                            EatersInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });
                        }
                        catch
                        {
                            AddLog("Failed to add Creature.(Duplicate escaped predator.)");
                        }
                        List<Phy.MathPrimitives.Vector3> tempVec = vecEaters.ToList();
                        tempVec.Add(new Phy.MathPrimitives.Vector3());
                        vecEaters = tempVec.ToArray();

                        List<double> tempdis = disEaters.ToList();
                        tempdis.Add(1000);
                        disEaters = tempdis.ToArray();
                    }
                    else
                    {
                        Eaters.Add(AddCreature(xvalue, 500, zvalue));
                        EatersInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });

                        List<Phy.MathPrimitives.Vector3> tempVec = vecEaters.ToList();
                        tempVec.Add(new Phy.MathPrimitives.Vector3());
                        vecEaters = tempVec.ToArray();

                        List<double> tempdis = disEaters.ToList();
                        tempdis.Add(1000);
                        disEaters = tempdis.ToArray();
                    }
                    RemoveCreature(true, cnt);

                    List<Phy.MathPrimitives.Vector3> tempVect = vecEaters.ToList();
                    tempVect.RemoveAt(cnt);
                    vecEaters = tempVect.ToArray();

                    List<double> tempdist = disEaters.ToList();
                    tempdist.RemoveAt(cnt);
                    disEaters = tempdist.ToArray();

                    cnt--;
                }
            }
            for (int cnt = 0; cnt < Eatens.Count(); cnt++)
            {
                if (disEatens[cnt] < 100)
                {
                    RemoveCreature(false, cnt);
                    float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    try
                    {
                        Creature temp = AddCreature(xvalue, 500, zvalue);
                        Eatens.Add(temp);
                        EatensInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });
                        addLatestMessage("New prey is caged.", "2");
                    }
                    catch
                    {
                        AddLog("Failed to add Creature.(New prey is caged. 2)");
                    }
                }
                else if (disEatens[cnt] > 10000)
                {
                    SaveCreature("./Creatures/Eaten.Winner." + Eatens[cnt].MainGene.BodyGeneID + "." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + ".xml", Eatens[cnt], false);
                    RemoveCreature(false, cnt);

                    float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                    Eatens.Add(AddCreature(xvalue, 500, zvalue));
                    EatensInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });
                }
            }

            //Creature系の処理
            for (int cnt = 0; cnt < Eaters.Count(); cnt++)
            {
                Creature ct = Eaters[cnt];
                List<double> ld = ct.MainGene.Infos;

                ct.MainGene.Infos[0] = gameTime.TotalGameTime.TotalSeconds;
                ct.MainGene.Infos[1] = vecEaters[cnt].X;
                ct.MainGene.Infos[2] = vecEaters[cnt].Y;
                ct.MainGene.Infos[3] = vecEaters[cnt].Z;
                const int offset = 4;
                ct.MainGene.Infos[offset] = ct.MainGene.Joints[0].PhyActor.GlobalPosition.X;
                ct.MainGene.Infos[1 + offset] = ct.MainGene.Joints[0].PhyActor.GlobalPosition.Y;
                ct.MainGene.Infos[2 + offset] = ct.MainGene.Joints[0].PhyActor.GlobalPosition.Z;
                for (int i = 1; i < ct.MainGene.Joints.Count(); i++)
                {
                    ct.MainGene.Infos[i * 3 + offset] = ct.MainGene.Joints[i].PhyActor.GlobalPosition.X - ct.MainGene.Joints[0].PhyActor.GlobalPosition.X;
                    ct.MainGene.Infos[i * 3 + 1 + offset] = ct.MainGene.Joints[i].PhyActor.GlobalPosition.Y - ct.MainGene.Joints[0].PhyActor.GlobalPosition.Y;
                    ct.MainGene.Infos[i * 3 + 2 + offset] = ct.MainGene.Joints[i].PhyActor.GlobalPosition.Z - ct.MainGene.Joints[0].PhyActor.GlobalPosition.Z;
                    //ld.Add(ct.MainGene.Joints[i].OrigX - ct.MainGene.Joints[0].OrigX);
                }
            }

            //AI計算
            double[][] aiForces = new double[Eaters.Count()][];
            System.Threading.Tasks.Parallel.For(0, Eaters.Count(), i =>
            {
                Creature ct = Eaters[i];
                aiForces[i] = ct.AI.Execute(ct.MainGene.Infos.ToArray(), 0, ref ct.MainGene.OutputAddress);
            });

            for (int cnt = 0; cnt < Eaters.Count(); cnt++)
            {
                Creature ct = Eaters[cnt];

                int j = 0;
                foreach (BodyGene.Connection cn in ct.MainGene.Connections)
                {
                    //ひも張力計算
                    Phy.MathPrimitives.Vector3 vec1 = ct.MainGene.Joints[cn.Target1].PhyActor.GlobalPosition;
                    Phy.MathPrimitives.Vector3 vec2 = ct.MainGene.Joints[cn.Target2].PhyActor.GlobalPosition;
                    float dist = (float)Math.Sqrt((vec1.X - vec2.X) * (vec1.X - vec2.X) + (vec1.Y - vec2.Y) * (vec1.Y - vec2.Y) + (vec1.Z - vec2.Z) * (vec1.Z - vec2.Z));
                    float force = (float)(1 / dist * 1 * (dist - cn.NaturalLength) * (dist - cn.NaturalLength) * (dist > cn.NaturalLength ? 1 : -1) + (aiForces[cnt][j * 3] % 5));
                    float forceX = (float)(aiForces[cnt][j * 3 + 1] / dist % 20);
                    float forceY = (float)(aiForces[cnt][j * 3 + 2] / dist % 1000);

                    //System.Diagnostics.Debug.WriteLine(force);
                    //float force = (float)(1 / dist * 0.01 * (dist - cn.NaturalLength) * (dist - cn.NaturalLength));
                    //float force = (float)(1 / dist * 1 * (dist - cn.NaturalLength) );
                    //ct.MainGene.Joints[cn.Target1].PhyActor.AddForce(new Phy.MathPrimitives.Vector3(-(vec1 - vec2).X * force, -(vec1 - vec2).Y * force, -(vec1 - vec2).Z * force), Phy.ForceMode.Force);
                    //ct.MainGene.Joints[cn.Target2].PhyActor.AddForce(new Phy.MathPrimitives.Vector3((vec1 - vec2).X * force, (vec1 - vec2).Y * force, (vec1 - vec2).Z * force), Phy.ForceMode.Force);
                    ct.MainGene.Joints[cn.Target1].PhyActor.AddForce(new Phy.MathPrimitives.Vector3(-(vec1 - vec2).X * force - (vec1 - vec2).Z * forceX, -(vec1 - vec2).Y * force - forceY, -(vec1 - vec2).Z * force - (vec1 - vec2).X * forceX), Phy.ForceMode.Force);
                    ct.MainGene.Joints[cn.Target2].PhyActor.AddForce(new Phy.MathPrimitives.Vector3((vec1 - vec2).X * force + (vec1 - vec2).Z * forceX, (vec1 - vec2).Y * force + forceY, (vec1 - vec2).Z * force + (vec1 - vec2).X * forceX), Phy.ForceMode.Force);
                    j++;

                }
                //for (int i = 1; i < ct.MainGene.Joints.Count(); i++)
                //{
                //    ct.MainGene.Joints[i].PhyActor.AddForce(new Phy.MathPrimitives.Vector3((float)(aiForce[3 * i-3]%10), 0, 0), Phy.ForceMode.Force);
                //    ct.MainGene.Joints[i].PhyActor.AddForce(new Phy.MathPrimitives.Vector3(0, 0, (float)(aiForce[3 * i-1]%10)), Phy.ForceMode.Force);
                //    ct.MainGene.Joints[i].PhyActor.AddForce(new Phy.MathPrimitives.Vector3(0, (float)(aiForce[3 * i - 2] % 10),0), Phy.ForceMode.Force);
                //}

                if (gameTime.TotalGameTime.TotalSeconds - EatersInfo[cnt].BornTime > 60 && !RecordingMode)
                {
                    if (Vector3.Distance(EatersInfo[cnt].OriginalPlace, Eaters[cnt].MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>()) < 50)
                    {
                        addLatestMessage("A unqualified predator is killed.");

                        float zvalue = -(float)rnd.NextDouble() * 8000 - 4000;
                        float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                        RemoveCreature(true, cnt);
                        if (Eaters.Count() < 60)
                        {
                            try
                            {
                                Creature temp = AddCreature(xvalue, 500, zvalue);
                                Eaters.Add(temp);
                                EatersInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });

                                List<double[]> tempAIF2 = aiForces.ToList();
                                tempAIF2.Add(new double[temp.MainGene.OutputAddress.Count()]);
                                aiForces = tempAIF2.ToArray();
                            }
                            catch
                            {
                                AddLog("Failed to add Creature.(add predator instead of unqualified)");
                            }
                        }

                        List<double[]> tempAIF = aiForces.ToList();
                        tempAIF.RemoveAt(cnt);
                        aiForces = tempAIF.ToArray();

                        cnt--;
                    }
                    else
                    {
                        EatersInfo[cnt].BornTime = gameTime.TotalGameTime.TotalSeconds;
                        EatersInfo[cnt].OriginalPlace = Eaters[cnt].MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>();
                    }

                }
            }

            //Creature系の処理
            for (int cnt = 0; cnt < Eatens.Count(); cnt++)
            {
                Creature ct = Eatens[cnt];
                List<double> ld = ct.MainGene.Infos;

                ct.MainGene.Infos[0] = gameTime.TotalGameTime.TotalSeconds;
                ct.MainGene.Infos[1] = vecEatens[cnt].X;
                ct.MainGene.Infos[2] = vecEatens[cnt].Y;
                ct.MainGene.Infos[3] = vecEatens[cnt].Z;
                const int offset = 4;
                ct.MainGene.Infos[offset] = ct.MainGene.Joints[0].PhyActor.GlobalPosition.X;
                ct.MainGene.Infos[1 + offset] = ct.MainGene.Joints[0].PhyActor.GlobalPosition.Y;
                ct.MainGene.Infos[2 + offset] = ct.MainGene.Joints[0].PhyActor.GlobalPosition.Z;
                for (int i = 1; i < ct.MainGene.Joints.Count(); i++)
                {
                    ct.MainGene.Infos[i * 3 + offset] = ct.MainGene.Joints[i].PhyActor.GlobalPosition.X - ct.MainGene.Joints[0].PhyActor.GlobalPosition.X;
                    ct.MainGene.Infos[i * 3 + 1 + offset] = ct.MainGene.Joints[i].PhyActor.GlobalPosition.Y - ct.MainGene.Joints[0].PhyActor.GlobalPosition.Y;
                    ct.MainGene.Infos[i * 3 + 2 + offset] = ct.MainGene.Joints[i].PhyActor.GlobalPosition.Z - ct.MainGene.Joints[0].PhyActor.GlobalPosition.Z;
                    //ld.Add(ct.MainGene.Joints[i].OrigX - ct.MainGene.Joints[0].OrigX);
                }
            }

            //AI計算
            aiForces = new double[Eatens.Count()][];
            System.Threading.Tasks.Parallel.For(0, Eatens.Count(), i =>
            {
                Creature ct = Eatens[i];
                aiForces[i] = ct.AI.Execute(ct.MainGene.Infos.ToArray(), 0, ref ct.MainGene.OutputAddress);
            });

            for (int cnt = 0; cnt < Eatens.Count(); cnt++)
            {
                Creature ct = Eatens[cnt];
                int j = 0;
                foreach (BodyGene.Connection cn in ct.MainGene.Connections)
                {
                    //ひも張力計算
                    Phy.MathPrimitives.Vector3 vec1 = ct.MainGene.Joints[cn.Target1].PhyActor.GlobalPosition;
                    Phy.MathPrimitives.Vector3 vec2 = ct.MainGene.Joints[cn.Target2].PhyActor.GlobalPosition;
                    float dist = (float)Math.Sqrt((vec1.X - vec2.X) * (vec1.X - vec2.X) + (vec1.Y - vec2.Y) * (vec1.Y - vec2.Y) + (vec1.Z - vec2.Z) * (vec1.Z - vec2.Z));
                    float force = (float)(1 / dist * 1 * (dist - cn.NaturalLength) * (dist - cn.NaturalLength) * (dist > cn.NaturalLength ? 1 : -1) + (aiForces[cnt][j * 3] % 5));
                    float forceX = (float)(aiForces[cnt][j * 3 + 1] / dist % 20);
                    float forceY = (float)(aiForces[cnt][j * 3 + 2] / dist % 1000);
                    ct.MainGene.Joints[cn.Target1].PhyActor.AddForce(new Phy.MathPrimitives.Vector3(-(vec1 - vec2).X * force - (vec1 - vec2).Z * forceX, -(vec1 - vec2).Y * force - forceY, -(vec1 - vec2).Z * force - (vec1 - vec2).X * forceX), Phy.ForceMode.Force);
                    ct.MainGene.Joints[cn.Target2].PhyActor.AddForce(new Phy.MathPrimitives.Vector3((vec1 - vec2).X * force + (vec1 - vec2).Z * forceX, (vec1 - vec2).Y * force + forceY, (vec1 - vec2).Z * force + (vec1 - vec2).X * forceX), Phy.ForceMode.Force);
                    j++;

                }
                if (gameTime.TotalGameTime.TotalSeconds - EatensInfo[cnt].BornTime > 60 && !RecordingMode)
                {
                    if (Vector3.Distance(EatensInfo[cnt].OriginalPlace, Eatens[cnt].MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>()) < 50)
                    {
                        addLatestMessage("A prey is dead of hungerstarve.");

                        RemoveCreature(false, cnt);

                        List<double[]> tempAIF = aiForces.ToList();
                        tempAIF.RemoveAt(cnt);
                        aiForces = tempAIF.ToArray();
                        cnt--;

                        if (Eatens.Count() < 30)
                        {
                            float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                            float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                            try
                            {
                                Creature temp = AddCreature(xvalue, 500, zvalue);
                                Eatens.Add(temp);
                                EatensInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });
                                addLatestMessage("New prey is caged.", "3");

                                List<double[]> tempAIF2 = aiForces.ToList();
                                tempAIF2.Add(new double[temp.MainGene.OutputAddress.Count()]);
                                aiForces = tempAIF2.ToArray();
                            }
                            catch
                            {
                                AddLog("Failed to add Creature.(New prey is caged. 3)");
                            }
                        }
                    }
                    else
                    {
                        if (Eatens.Count() < 100 && CreatureCount[Eatens[cnt].MainGene.BodyGeneID] < 5)
                        {
                            try
                            {
                                float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                                float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                                Creature temp = DuplicateCreature(Eatens[cnt], xvalue, 500, zvalue);
                                Eatens.Add(temp);
                                EatensInfo.Add(new CreatureAdditonalInfo() { BornTime = gameTime.TotalGameTime.TotalSeconds, OriginalPlace = new Vector3(xvalue, 0, zvalue) });
                                addLatestMessage("A prey bred a child.");

                                List<double[]> tempAIF2 = aiForces.ToList();
                                tempAIF2.Add(new double[temp.MainGene.OutputAddress.Count()]);
                                aiForces = tempAIF2.ToArray();

                                List<Phy.MathPrimitives.Vector3> tempVec = vecEatens.ToList();
                                tempVec.Add(new Phy.MathPrimitives.Vector3());
                                vecEatens = tempVec.ToArray();

                                List<double> tempdis = disEatens.ToList();
                                tempdis.Add(1000);
                                disEatens = tempdis.ToArray();
                            }
                            catch
                            {
                                AddLog("Failed to add Creature.(A prey bred a child.)");
                            }

                        }
                        EatensInfo[cnt].BornTime = gameTime.TotalGameTime.TotalSeconds;
                        EatensInfo[cnt].OriginalPlace = Eatens[cnt].MainGene.Joints[0].PhyActor.GlobalPosition.As<Vector3>();
                    }

                }
            }
            lastUpdateTime = gameTime;
        }
        string[] CreatureFiles = System.IO.Directory.GetFiles("./Creatures/");
        int CreatureFileCnt = 0;
        bool DisplayMode = true;
        bool RecordingMode = false;
        GameTime lastUpdateTime = new GameTime();

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(233, 205, 173));
            if (DisplayMode)
            {
                // TODO: ここに描画コードを追加します。
                Matrix Scale;
                //Scale = Matrix.CreateScale(GroundScale);
                //DrawModel(this.ModelCube, Scale * this.GroundActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                for (int i = -10; i <= 10; i++)
                {
                    for (int j = -10; j <= 10; j++)
                    {
                        DrawModel(this.ModelCube, Matrix.CreateScale(GroundScale) * Matrix.CreateTranslation(Camera.CameraPosition.X - (Camera.CameraPosition.X % GroundScale.X) + GroundScale.X * i, -1, Camera.CameraPosition.Z - (Camera.CameraPosition.Z % GroundScale.Z) + GroundScale.Z * j), this.Camera.View, this.Camera.Projection);
                    }
                }

                Scale = Matrix.CreateScale(BoxScale);   // 箱の大きさ
                foreach (Phy.Actor act in this.BoxActors)
                {
                    DrawModel(this.ModelCube, Scale * act.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                }


                //System.Diagnostics.Debug.WriteLine("_______________________________");
                foreach (Creature ct in Eaters)
                {
                    foreach (BodyGene.Connection cn in ct.MainGene.Connections)
                    {
                        DrawLine(ct.MainGene.Joints[cn.Target1].PhyActor.GlobalPosition.As<Vector3>(), ct.MainGene.Joints[cn.Target2].PhyActor.GlobalPosition.As<Vector3>(), this.Camera.View, this.Camera.Projection);
                    }

                    DrawModel(this.ModelSphere4, Matrix.CreateScale((float)(ct.MainGene.Joints[0]).Size * 2) * (ct.MainGene.Joints[0]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                    //DrawModel(this.ModelCube3, Matrix.CreateScale((float)(ct.MainGene.Joints[0]).Size) * (ct.MainGene.Joints[0]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                    for (int i = 1; i < ct.MainGene.Joints.Count(); i++)
                    {
                        //System.Diagnostics.Debug.WriteLine(ct.MainGene.Joints[i].PhyActor.GlobalPosition);
                        switch (ct.MainGene.Joints[i].Type)
                        {
                            case BodyGene.BodyPart.ObjectType.Sphere:
                                DrawModel(this.ModelSphere3, Matrix.CreateScale((float)(ct.MainGene.Joints[i]).Size * 2) * (ct.MainGene.Joints[i]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                                break;
                            case BodyGene.BodyPart.ObjectType.Cube:
                                DrawModel(this.ModelCube4, Matrix.CreateScale((float)(ct.MainGene.Joints[i]).Size) * (ct.MainGene.Joints[i]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                                break;
                            default:
                                break;
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine("....................");
                    //foreach (BodyGene.BodyPart bp in ct.MainGene.Joints)
                    //{
                    //    DrawModel(this.ModelSphere, Matrix.CreateScale((float)bp.Size * 2) * bp.PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                    //}
                }

                foreach (Creature ct in Eatens)
                {
                    foreach (BodyGene.Connection cn in ct.MainGene.Connections)
                    {
                        DrawLine(ct.MainGene.Joints[cn.Target1].PhyActor.GlobalPosition.As<Vector3>(), ct.MainGene.Joints[cn.Target2].PhyActor.GlobalPosition.As<Vector3>(), this.Camera.View, this.Camera.Projection);
                    }

                    DrawModel(this.ModelSphere2, Matrix.CreateScale((float)(ct.MainGene.Joints[0]).Size * 2) * (ct.MainGene.Joints[0]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                    //DrawModel(this.ModelCube3, Matrix.CreateScale((float)(ct.MainGene.Joints[0]).Size) * (ct.MainGene.Joints[0]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                    for (int i = 1; i < ct.MainGene.Joints.Count(); i++)
                    {
                        //System.Diagnostics.Debug.WriteLine(ct.MainGene.Joints[i].PhyActor.GlobalPosition);
                        switch (ct.MainGene.Joints[i].Type)
                        {
                            case BodyGene.BodyPart.ObjectType.Sphere:
                                DrawModel(this.ModelSphere, Matrix.CreateScale((float)(ct.MainGene.Joints[i]).Size * 2) * (ct.MainGene.Joints[i]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                                break;
                            case BodyGene.BodyPart.ObjectType.Cube:
                                DrawModel(this.ModelCube2, Matrix.CreateScale((float)(ct.MainGene.Joints[i]).Size) * (ct.MainGene.Joints[i]).PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                                break;
                            default:
                                break;
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine("....................");
                    //foreach (BodyGene.BodyPart bp in ct.MainGene.Joints)
                    //{
                    //    DrawModel(this.ModelSphere, Matrix.CreateScale((float)bp.Size * 2) * bp.PhyActor.GlobalPose.As<Matrix>(), this.Camera.View, this.Camera.Projection);
                    //}
                }
                base.Draw(gameTime);
            }
            else
            {
                textMessage = (1 / Math.Max(lastUpdateTime.ElapsedGameTime.TotalSeconds, 0.0000001)) + "\n" + textMessage;
            }

            DrawMessage();
        }

        void SaveTextFile(string fileName, string contents)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName, true, System.Text.Encoding.UTF8))
            {
                writer.Write(contents);
            }
        }

        void SaveCreatureAI(bool isEater, int count)
        {
            if ((isEater ? Eaters : Eatens).Count() > count)
            {
                Creature ct = (isEater ? Eaters : Eatens)[count];
                string outtxt = "Creature Commands\n\n";

                outtxt += "Creature Info\n    ID: " + GuidToString(ct.ID) + "\n    Parent: " + GuidToString(ct.Parent) + "\n";
                outtxt += "\n___________________________________________________\n\n";

                outtxt += "Latest Input\n";
                for (int i = 0; i < ct.MainGene.Infos.Count(); i++)
                {
                    outtxt += String.Format("{0,3}", i) + " " + ct.MainGene.Infos[i] + "\n";
                }
                outtxt += "\n___________________________________________________\n\n";

                outtxt += "Latest Result\n";
                for (int i = 0; i < ct.AI.Result[0][0].Count(); i++)
                {
                    outtxt += String.Format("{0,3}", i) + " " + ct.AI.Result[0][0][i] + "\n";
                }
                outtxt += "\n___________________________________________________\n\n";

                outtxt += "Output Address\n";
                for (int i = 0; i < ct.MainGene.OutputAddress.Count(); i++)
                {
                    outtxt += String.Format("{0,3}", i) + " " + ct.MainGene.OutputAddress[i] + " " + ct.AI.Result[0][0][ct.MainGene.OutputAddress[i]] + "\n";
                }
                outtxt += "\n___________________________________________________\n\n";


                for (int i = 0; i < CreatureAI.SituationCount; i++)
                {
                    outtxt += "Situation " + i + "\n\n";
                    outtxt += ct.AI.CommandListToString(i);
                    outtxt += "\n___________________________________________________\n\n";
                }
                string fileName = "./Creatures/" + (isEater ? "Eater" : "Eaten") + "." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + "." + count + ".AI.txt";
                if (!System.IO.File.Exists(fileName))
                {
                    addLatestMessage("Saved AI of the creatures as " + fileName);
                    SaveTextFile(fileName, outtxt);
                }
            }
        }

        void SaveState()
        {
            SaveState("./Creatures/Creature." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + ".xml");
        }

        void SaveState(string fileName)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<CreatureData>));

            List<CreatureData> ctds = new List<CreatureData>();
            foreach (Creature ct in Eaters)
            {
                ctds.Add(new CreatureData(ct, CreatureData.CreatureType.eater));
            }
            foreach (Creature ct in Eatens)
            {
                ctds.Add(new CreatureData(ct, CreatureData.CreatureType.eaten));
            }
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                serializer.Serialize(fs, ctds);
                addLatestMessage("Saved all creatures as " + fileName);
            }
        }

        void SaveCreature(bool IsEater, int count)
        {
            if ((IsEater ? Eaters : Eatens).Count() > count)
            {
                string fileName = "./Creatures/" + (IsEater ? "Eater" : "Eaten") + "." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + "." + count + ".xml";
                SaveCreature(fileName, (IsEater ? Eaters : Eatens)[count], IsEater);
            }
        }

        void SaveCreature(string fileName, Creature ct, bool IsEater)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<CreatureData>));

            List<CreatureData> ctds = new List<CreatureData>();
            ctds.Add(new CreatureData(ct, IsEater ? CreatureData.CreatureType.eater : CreatureData.CreatureType.eaten));
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                serializer.Serialize(fs, ctds);
                addLatestMessage("Saved a creature as " + fileName);
            }
        }

        void LoadCreatureForRecording(string fileName, double time)
        {
            Random rnd = new Random();
            LoadCreature(fileName, time);
            for (int i = 0; i < Eaters.Count(); i++)
            {
                if (rnd.Next(3) > 0)
                {
                    RemoveCreature(true, i);
                    i--;
                }
            }
            for (int i = 0; i < Eatens.Count(); i++)
            {
                if (rnd.Next(3) > 0)
                {
                    RemoveCreature(false, i);
                    i--;
                }
            }
        }



        void LoadCreature(string fileName, double time)
        {
            LogSilentMode = true;
            List<CreatureData> cds;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<CreatureData>));
            if (System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
                {
                    cds = (List<CreatureData>)serializer.Deserialize(fs);
                }
                if (cds.Count() == 0)
                {
                }
                else if (cds.Count() == 1)
                {
                    try
                    {
                        (cds[0].Type == CreatureData.CreatureType.eater ? Eaters : Eatens).Add(AddCreature(new Creature(ref rnd, cds[0]), Camera.LookAtPosition.X, 500, Camera.LookAtPosition.Z));
                        (cds[0].Type == CreatureData.CreatureType.eater ? EatersInfo : EatensInfo).Add(new CreatureAdditonalInfo() { BornTime = time, OriginalPlace = new Vector3(Camera.LookAtPosition.X, 0, Camera.LookAtPosition.Z), HitPoint = 100 });
                        addLatestMessage("Loaded a creature from " + fileName);
                    }
                    catch
                    {
                        LogSilentMode = false;
                        AddLog("Failed to add Creature.(Loaded a creature from " + fileName + ")");
                    }
                    LogSilentMode = false;
                }
                else
                {
                    while (Eaters.Count() > 0)
                    {
                        RemoveCreature(true, 0);
                    }
                    while (Eatens.Count() > 0)
                    {
                        RemoveCreature(false, 0);
                    }

                    foreach (CreatureData cd in cds)
                    {
                        float zvalue = (float)rnd.NextDouble() * 8000 - 4000;
                        float xvalue = (float)rnd.NextDouble() * 8000 - 4000;
                        try
                        {
                            (cd.Type == CreatureData.CreatureType.eater ? Eaters : Eatens).Add(AddCreature(new Creature(ref rnd, cd), xvalue, 500, zvalue));
                            (cd.Type == CreatureData.CreatureType.eater ? EatersInfo : EatensInfo).Add(new CreatureAdditonalInfo() { BornTime = time, OriginalPlace = new Vector3(xvalue, 0, zvalue), HitPoint = 100 });
                        }
                        catch
                        {
                            LogSilentMode = false;
                            AddLog("Failed to add Creature.(Loaded creatures from " + fileName + ")");
                        }
                    }
                    LogSilentMode = false;
                    addLatestMessage("Loaded creatures from " + fileName);
                }
            }
        }

        /// <summary>
        /// モデルを表示します。
        /// </summary>
        /// <param name="model">モデル</param>
        /// <param name="world">変換行列</param>
        /// <param name="view">ビュー行列</param>
        /// <param name="projection">プロジェクション行列</param>
        void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            // 親トランスフォームのコピー
            // 複数メッシュがあった場合などは、指定された行列をかけないといけない場合がある。
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            // メッシュごとの描写
            foreach (ModelMesh mesh in model.Meshes)
            {
                // メッシュに割り当てられてるエフェクトの設定
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = view;
                    effect.Projection = projection;

                }

                // このメッシュを描写
                mesh.Draw();
            }
        }

        void DrawLine(Vector3 From, Vector3 To, Matrix view, Matrix projection)
        {
            LineBasicEffect.View = view;
            LineBasicEffect.Projection = projection;

            LineBasicEffect.CurrentTechnique.Passes[0].Apply();
            VertexPositionColor[] vertices = new VertexPositionColor[] { new VertexPositionColor(From, Color.Green), new VertexPositionColor(To, Color.Green) };
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
        }

        void DrawMessage()
        {
            // スプライトの描画準備
            this.spriteBatch.Begin();
            // テキストをスプライトとして描画する
            this.spriteBatch.DrawString(this.font, textMessage, Vector2.Zero, Color.White);
            // スプライトの一括描画
            this.spriteBatch.End();

            DepthStencilState depthBufferState;
            depthBufferState = new DepthStencilState();
            depthBufferState.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = depthBufferState;
        }

        void RemoveCreature(bool IsEaters, int cnt)
        {
            if (IsEaters)
            {
                if (Eaters.Count() > cnt)
                {
                    if (TargetEater && TargetCnt == cnt && Camera.TargetMode)
                    {
                        TargetCnt = 0;
                        Camera.TargetMode = false;
                        MessageMode = MessageModes.Normal;
                        Camera.LookAtPosition = new Vector3(0, 20, 0);
                    }
                    else if (TargetCnt > cnt)
                    {
                        TargetCnt--;
                    }
                    foreach (BodyGene.BodyPart bg in Eaters[cnt].MainGene.Joints)
                    {
                        bg.PhyActor.Dispose();
                    }
                    EatersInfo.RemoveAt(cnt);
                    AddLog("Predator Removed\nBody ID: " + Eaters[cnt].MainGene.BodyGeneID.ToString() + " #" + CreatureCount[Eaters[cnt].MainGene.BodyGeneID]);

                    CreatureCount[Eaters[cnt].MainGene.BodyGeneID]--;
                    Eaters.RemoveAt(cnt);
                }
            }
            else
            {
                if (Eatens.Count() > cnt)
                {
                    if ((!TargetEater) && TargetCnt == cnt && Camera.TargetMode)
                    {
                        TargetCnt = 0;
                        Camera.TargetMode = false;
                        MessageMode = MessageModes.Normal;
                    }
                    else if ((!TargetEater) && TargetCnt > cnt)
                    {
                        TargetCnt--;
                    }
                    foreach (BodyGene.BodyPart bg in Eatens[cnt].MainGene.Joints)
                    {
                        bg.PhyActor.Dispose();
                    }
                    EatensInfo.RemoveAt(cnt);
                    AddLog("Prey Removed\nBody ID: " + Eatens[cnt].MainGene.BodyGeneID.ToString() + " #" + CreatureCount[Eatens[cnt].MainGene.BodyGeneID]);

                    CreatureCount[Eatens[cnt].MainGene.BodyGeneID]--;
                    Eatens.RemoveAt(cnt);
                }
            }
        }
    }

    public class Creature
    {

        public Random CommonRandom = new Random();
        public CreatureAI AI = new CreatureAI();
        public BodyGene MainGene = new BodyGene();
        public Guid Parent = new Guid("E38D30A9-34F8-4894-93A3-CB75BFD3C033");
        public Guid ID = Guid.NewGuid();

        public Creature(Random rnd, Creature baseCreature)
        {
            CommonRandom = new Random(rnd.Next(System.Int16.MaxValue));
            Parent = baseCreature.ID;

            BodyGene tempBG;
            tempBG = new BodyGene(baseCreature.MainGene);
            tempBG.CommonRandom = new Random(CommonRandom.Next(System.Int16.MaxValue));
            if (CommonRandom.Next(100) == 37)
            {
                tempBG.AddJoints();
            }
            MainGene = tempBG;


            CreatureAI tempAi = new CreatureAI(baseCreature.AI);
            tempAi.CommonRandom = new Random(CommonRandom.Next(System.Int16.MaxValue));
            for (int i = 0; i < 50; i++)
            {
                tempAi.ChangeRandomCommand();
            }
            for (int i = 0; i < 50; i++)
            {
                tempAi.ChangeRandomArgument();
            }
            AI = tempAi;
        }

        public Creature(ref Random rnd, CreatureData baseCreature)
        {
            CommonRandom = new Random(rnd.Next(System.Int16.MaxValue));

            MainGene = new BodyGene(baseCreature.Body);
            MainGene.CommonRandom = new Random(CommonRandom.Next(System.Int16.MaxValue));

            AI = new CreatureAI(baseCreature.AI);
            AI.CommonRandom = new Random(CommonRandom.Next(System.Int16.MaxValue));

            Parent = baseCreature.Parent;
            ID = baseCreature.ID;
        }

        public Creature()
            : this(new Random().Next())
        {
        }

        public Creature(int randomSeed)
        {
            CommonRandom = new Random(randomSeed);
            AI.CommonRandom = new Random(CommonRandom.Next(System.Int16.MaxValue));
            MainGene.CommonRandom = new Random(CommonRandom.Next(System.Int16.MaxValue));

            for (int i = 0; i < 1000; i++)
            {
                AI.InsertRandomCommand(ref MainGene.OutputAddress);
                //AI.AddRandomCommand();
            }

            int j = CommonRandom.Next(15) + 4;
            for (int i = 0; i < j; i++)
            {
                MainGene.AddJoints();
            }
        }

        public Creature(ref Random rnd)
            : this(rnd.Next(System.Int16.MaxValue))
        {
        }
    }

    public class Setting
    {

    }
}
