using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AZ3D;
using System.IO;

namespace My3DTransform
{
    class Mono3D : Game
    {

        enum RenderMode { Line, Triangle, Both }
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;
            Texture2D test;
            KeyboardState prevState = new KeyboardState();
            MouseState prevMState = new MouseState();
            RenderMode renderMode = RenderMode.Both;
            public VertexPositionColor[] vertices = new VertexPositionColor[0];
            public VertexPositionColor[] triVertices = new VertexPositionColor[0];

            VertexPositionTexture[] floorVerts;
            BasicEffect effect;

        /// <summary>
        /// FOr flver file, the rotation order is Y->Z->X. And Y is the parent bone!
        /// </summary>


        SpriteFont font;


        Vector3D[] vlist = new Vector3D[8];
        Transform3D t = new Transform3D();

        Transform3D t2 = new Transform3D();


        static Transform3D[] tList = { };
        public bool showCircle = false;

        float mouseX, mouseY;

            float cameraX = 0;
            float cameraY = 4;
            float cameraZ = 2;

            float offsetX = 0;
            float offsetY = 0;
            float offsetZ = 0;

            float centerX = 0;
            float centerY = 0;
            float centerZ = 0;

            public float lightX = 1;
            public float lightY = 1;
            public float lightZ = 1;



        [STAThread]
        static void Main(string[] args)
        {
            /* Application.EnableVisualStyles();
             Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new Form1());*/
            //argments = args;

            Console.WriteLine("Hello!");
            Mono3D n = new Mono3D();
            n.Run();
        }

        public Mono3D()
        {
            Window.Title = "3D Gimbal Lock Visualizer by Forsakensilver";
            Window.AllowUserResizing = true;
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        public static void TryReadBoneJson()
        {
            try
            {
                var sr = new StreamReader("bones.json");
                string res = sr.ReadToEnd();
                sr.Close();

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

               var bones = serializer.Deserialize<List<Bone>>(res);

                int boneLength = bones.Count;

                tList = new Transform3D[boneLength];
                for (int i = 0;i < boneLength; i++)
                {

                    Bone b = bones[i];
                    var nt = new Transform3D();
                    if (b.ParentIndex >= 0 && b.ParentIndex != i) { nt.parent = tList[b.ParentIndex]; } else { nt.parent = null; }


                    nt.position = new Vector3D(b.Translation);
                    nt.rotation = new Vector3D(b.Rotation);
                    nt.rotation = nt.rotation * (float)((1 / Math.PI) * 180f);
                    Console.WriteLine( "[" + i + "]" + b.Name + " Parent index: " + b.ParentIndex);
                    tList[i] = nt;
                }

           
                System.Windows.Forms.MessageBox.Show("Bone loaded!", "Info");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }

        }


        public void basicShowVertex()
        {
            List<VertexPositionColor> ans = new List<VertexPositionColor>();
            List<VertexPositionColor> triangles = new List<VertexPositionColor>();

      


            t.vlist = vlist;


            Vector3D[] nlist = null;


            nlist = t.getGlobalVlist();
            //X,Z,Y
            //ans.Add(new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(1,1,1), Microsoft.Xna.Framework.Color.Purple));
            //ans.Add(new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(1,2,1), Microsoft.Xna.Framework.Color.Purple));


            ans.Add(basicVertexXZY(nlist[0],Color.White));
            ans.Add(basicVertexXZY(nlist[1], Color.White));

            ans.Add(basicVertexXZY(nlist[1], Color.White));
            ans.Add(basicVertexXZY(nlist[2], Color.White));

            ans.Add(basicVertexXZY(nlist[2], Color.White));
            ans.Add(basicVertexXZY(nlist[3], Color.White));

            ans.Add(basicVertexXZY(nlist[3], Color.White));
            ans.Add(basicVertexXZY(nlist[0], Color.White));



            ans.Add(basicVertexXZY(nlist[4], Color.White));
            ans.Add(basicVertexXZY(nlist[5], Color.White));

            ans.Add(basicVertexXZY(nlist[5], Color.White));
            ans.Add(basicVertexXZY(nlist[6], Color.White));

            ans.Add(basicVertexXZY(nlist[6], Color.White));
            ans.Add(basicVertexXZY(nlist[7], Color.White));

            ans.Add(basicVertexXZY(nlist[7], Color.White));
            ans.Add(basicVertexXZY(nlist[4], Color.White));



            ans.Add(basicVertexXZY(nlist[0], Color.White));
            ans.Add(basicVertexXZY(nlist[4], Color.White));

            ans.Add(basicVertexXZY(nlist[1], Color.White));
            ans.Add(basicVertexXZY(nlist[5], Color.White));

            ans.Add(basicVertexXZY(nlist[2], Color.White));
            ans.Add(basicVertexXZY(nlist[6], Color.White));

            ans.Add(basicVertexXZY(nlist[3], Color.White));
            ans.Add(basicVertexXZY(nlist[7], Color.White));




            nlist = t2.getGlobalVlist();
            //X,Z,Y
            //ans.Add(new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(1,1,1), Microsoft.Xna.Framework.Color.Purple));
            //ans.Add(new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(1,2,1), Microsoft.Xna.Framework.Color.Purple));


            ans.Add(basicVertexXZY(nlist[0], Color.Gray));
            ans.Add(basicVertexXZY(nlist[1], Color.Gray));

            ans.Add(basicVertexXZY(nlist[1], Color.Gray));
            ans.Add(basicVertexXZY(nlist[2], Color.Gray));

            ans.Add(basicVertexXZY(nlist[2], Color.Gray));
            ans.Add(basicVertexXZY(nlist[3], Color.Gray));

            ans.Add(basicVertexXZY(nlist[3], Color.Gray));
            ans.Add(basicVertexXZY(nlist[0], Color.Gray));



            ans.Add(basicVertexXZY(nlist[4], Color.Gray));
            ans.Add(basicVertexXZY(nlist[5], Color.Gray));

            ans.Add(basicVertexXZY(nlist[5], Color.Gray));
            ans.Add(basicVertexXZY(nlist[6], Color.Gray));

            ans.Add(basicVertexXZY(nlist[6], Color.Gray));
            ans.Add(basicVertexXZY(nlist[7], Color.Gray));

            ans.Add(basicVertexXZY(nlist[7], Color.Gray));
            ans.Add(basicVertexXZY(nlist[4], Color.Gray));



            ans.Add(basicVertexXZY(nlist[0], Color.Gray));
            ans.Add(basicVertexXZY(nlist[4], Color.Gray));

            ans.Add(basicVertexXZY(nlist[1], Color.Gray));
            ans.Add(basicVertexXZY(nlist[5], Color.Gray));

            ans.Add(basicVertexXZY(nlist[2], Color.Gray));
            ans.Add(basicVertexXZY(nlist[6], Color.Gray));

            ans.Add(basicVertexXZY(nlist[3], Color.Gray));
            ans.Add(basicVertexXZY(nlist[7], Color.Gray));

            if(showCircle)
            {

                nlist = t.getRotCircleX();
                drawRing(ans,nlist,Color.Red);


                nlist = t.getRotCircleY();
                drawRing(ans, nlist, Color.Yellow);

                nlist = t.getRotCircleZ();
                drawRing(ans, nlist, Color.Blue);

            }
           /// System.Windows.Forms.MessageBox.Show("VT started", "Info");
            foreach (var vt  in tList)
            {
                vt.vlist = vlist;


                nlist = vt.getGlobalVlist();
                //X,Z,Y
                //ans.Add(new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(1,1,1), Microsoft.Xna.Framework.Color.Purple));
                //ans.Add(new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(1,2,1), Microsoft.Xna.Framework.Color.Purple));


                ans.Add(basicVertexXZY(nlist[0], Color.White));
                ans.Add(basicVertexXZY(nlist[1], Color.White));

                ans.Add(basicVertexXZY(nlist[1], Color.White));
                ans.Add(basicVertexXZY(nlist[2], Color.White));

                ans.Add(basicVertexXZY(nlist[2], Color.White));
                ans.Add(basicVertexXZY(nlist[3], Color.White));

                ans.Add(basicVertexXZY(nlist[3], Color.White));
                ans.Add(basicVertexXZY(nlist[0], Color.White));



                ans.Add(basicVertexXZY(nlist[4], Color.White));
                ans.Add(basicVertexXZY(nlist[5], Color.White));

                ans.Add(basicVertexXZY(nlist[5], Color.White));
                ans.Add(basicVertexXZY(nlist[6], Color.White));

                ans.Add(basicVertexXZY(nlist[6], Color.White));
                ans.Add(basicVertexXZY(nlist[7], Color.White));

                ans.Add(basicVertexXZY(nlist[7], Color.White));
                ans.Add(basicVertexXZY(nlist[4], Color.White));



                ans.Add(basicVertexXZY(nlist[0], Color.White));
                ans.Add(basicVertexXZY(nlist[4], Color.White));

                ans.Add(basicVertexXZY(nlist[1], Color.White));
                ans.Add(basicVertexXZY(nlist[5], Color.White));

                ans.Add(basicVertexXZY(nlist[2], Color.White));
                ans.Add(basicVertexXZY(nlist[6], Color.White));

                ans.Add(basicVertexXZY(nlist[3], Color.White));
                ans.Add(basicVertexXZY(nlist[7], Color.White));

            }
           // System.Windows.Forms.MessageBox.Show("VT completed", "Info");

            vertices = ans.ToArray();

            triVertices = triangles.ToArray();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            floorVerts = new VertexPositionTexture[6];
            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);
            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;



            t.rotation = new Vector3D(0, 90, 0);
            t.position = new Vector3D(1, 1, 1);

            t2.position.Y = 2;
            t2.parent = t;



            /* vlist[0] = new Vector3D(0, 0, 0);
             vlist[1] = new Vector3D(0, 0, 1);
             vlist[2] = new Vector3D(0, 1, 1);
             vlist[3] = new Vector3D(0, 1, 0);
             vlist[4] = new Vector3D(1, 0, 0);
             vlist[5] = new Vector3D(1, 0, 1);
             vlist[6] = new Vector3D(1, 1, 1);
             vlist[7] = new Vector3D(1, 1, 0);*/

            vlist[0] = new Vector3D(0, 0, 0);
            vlist[1] = new Vector3D(0, 0, 1);
            vlist[2] = new Vector3D(0, 1, 1);
            vlist[3] = new Vector3D(0, 1, 0);
            vlist[4] = new Vector3D(1, 0, 0);
            vlist[5] = new Vector3D(1, 0, 1);
            vlist[6] = new Vector3D(1, 1, 1);
            vlist[7] = new Vector3D(1, 1, 0);

            float scaleFactor =1f;
            foreach (var v in vlist)
            {
                v.X -= 0.5f; v.Y -= 0.5f; v.Z -= 0.5f;

                v.X *= scaleFactor;
                v.Y *= scaleFactor;
                v.Z *= scaleFactor;
            }


            t2.vlist = vlist;

            //TryReadBoneJson();

            basicShowVertex();

            effect = new BasicEffect(graphics.GraphicsDevice);
            effect.VertexColorEnabled = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("default"); // Use the name of your sprite font file here instead of 'Score'.




            /*  string path = @"data\img\27.png";

              System.Drawing.Bitmap btt = new System.Drawing.Bitmap(path);
              test = Texture2D.FromStream(this.GraphicsDevice, File.OpenRead(path));
              test = getTextureFromBitmap(btt, this.GraphicsDevice);*/
            // TODO: use this.Content to load your game content here
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState state = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Application.Exit();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;



            //Marine_Yes00.mp3



            if (mState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && IsActive)
            {
                float mdx = mState.X - prevMState.X;
                float mdy = mState.Y - prevMState.Y;
                {
                    System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);
                    System.Numerics.Vector3 p2 = Vector3D.RotatePoint(p, 0, 0, -mdx * 0.01f);
                    cameraX = p2.X;
                    cameraY = p2.Y;
                    cameraZ = p2.Z;
                }
                {
                    System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);

                    float nX = cameraY;
                    float nY = -cameraX;


                    System.Numerics.Vector3 p2 = Vector3D.RotateLine(p, new System.Numerics.Vector3(0, 0, 0),
                        new System.Numerics.Vector3(nX, nY, 0), mdy * 0.01f);


                    cameraX = p2.X;
                    cameraY = p2.Y;
                    cameraZ = p2.Z;

                }



            }

            if (mState.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && IsActive)
            {
                float mdx = mState.X - prevMState.X;
                float mdy = mState.Y - prevMState.Y;

                // offsetZ += mdy * 3 * delta;

                Vector3D upV = new Vector3D(0, 0, 1);
                Vector3D forwardV = new Vector3D(cameraX, cameraY, cameraZ);
                Vector3D rightV = Vector3D.crossPorduct(upV, forwardV).normalize();
                Vector3D camUpV = Vector3D.crossPorduct(forwardV, rightV).normalize();

                Vector3D offsetV = new Vector3D(offsetX, offsetY, offsetZ);
                offsetV = offsetV - new Vector3D(rightV.X * mdx * 0.01f, rightV.Y * mdx * 0.01f, rightV.Z * mdx * 0.01f);
                offsetV = offsetV + new Vector3D(camUpV.X * mdy * 0.01f, camUpV.Y * mdy * 0.01f, camUpV.Z * mdy * 0.01f);

                offsetX = offsetV.X;
                offsetY = offsetV.Y;
                offsetZ = offsetV.Z;
                //offsetX -= mdx* 1 * delta * rightV.X;
                //offsetY -= mdx * 1 * delta * rightV.Y;
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                renderMode = RenderMode.Line;
            }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2))
            {
                renderMode = RenderMode.Triangle;
            }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F3))
            {
                renderMode = RenderMode.Both;
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B) && !prevState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
            {
                t.position = new Vector3D();
                t.rotation = new Vector3D();
                basicShowVertex();
               // Program.boneDisplay = !Program.boneDisplay;
              //  Program.updateVertices();
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M) && !prevState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M))
            {
               // Program.dummyDisplay = !Program.dummyDisplay;
               // Program.updateVertices();
            }

            //1.73 Added focus detect
            if (mState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && this.IsActive)
            {
                float mdx = mState.X - prevMState.X;
                float mdy = mState.Y - prevMState.Y;

                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)) { t.position.X += mdx / 100f; }
                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S)) { t.position.Y += mdx / 100f; }
                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)) { t.position.Z += mdx / 100f; }


                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z)) { t.rotation.X += mdx; }
                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X)) { t.rotation.Y += mdx; }
                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C)) { t.rotation.Z += mdx; }
                


                basicShowVertex();
            }

            if (mState.ScrollWheelValue - prevMState.ScrollWheelValue > 0)
            {
                //mouseY -= (50 * delta);
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);



                cameraX = p.X - 0.5f * (float)(p.X / p.Length());
                cameraY = p.Y - 0.5f * (float)(p.Y / p.Length());
                cameraZ = p.Z - 0.5f * (float)(p.Z / p.Length());
            }

            if (mState.ScrollWheelValue - prevMState.ScrollWheelValue < 0)
            {
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);


                cameraX = p.X + 0.5f * (float)(p.X / p.Length());
                cameraY = p.Y + 0.5f * (float)(p.Y / p.Length());
                cameraZ = p.Z + 0.5f * (float)(p.Z / p.Length());
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);
                System.Numerics.Vector3 p2 = Vector3D.RotatePoint(p, 0, 0, 5 * delta);
                cameraX = p2.X;
                cameraY = p2.Y;
                cameraZ = p2.Z;
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);
                System.Numerics.Vector3 p2 = Vector3D.RotatePoint(p, 0, 0, -5 * delta);
                cameraX = p2.X;
                cameraY = p2.Y;
                cameraZ = p2.Z;
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                //mouseY -= (50 * delta);
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);


                System.Numerics.Vector3 p2 = Vector3D.RotateLine(p, new System.Numerics.Vector3(0, 0, 0),
                    new System.Numerics.Vector3(cameraY, -cameraX, 0), 3 * delta);
                cameraX = p2.X;
                cameraY = p2.Y;
                cameraZ = p2.Z;
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);


                System.Numerics.Vector3 p2 = Vector3D.RotateLine(p, new System.Numerics.Vector3(0, 0, 0),
                    new System.Numerics.Vector3(cameraY, -cameraX, 0), -3 * delta);
                cameraX = p2.X;
                cameraY = p2.Y;
                cameraZ = p2.Z;
            }


            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemComma))
            {
                //mouseY -= (50 * delta);
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);



                cameraX = p.X - 3 * delta * (float)(p.X / p.Length());
                cameraY = p.Y - 3 * delta * (float)(p.Y / p.Length());
                cameraZ = p.Z - 3 * delta * (float)(p.Z / p.Length());
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemPeriod))
            {
                System.Numerics.Vector3 p = new System.Numerics.Vector3(cameraX, cameraY, cameraZ);


                cameraX = p.X + 3 * delta * (float)(p.X / p.Length());
                cameraY = p.Y + 3 * delta * (float)(p.Y / p.Length());
                cameraZ = p.Z + 3 * delta * (float)(p.Z / p.Length());
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad8))
            {
                offsetZ += 3 * delta;

            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {

                offsetZ -= 3 * delta; ;
            }

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad4))
            {
                Vector3D upV = new Vector3D(0, 0, 1);
                Vector3D forwardV = new Vector3D(cameraX, cameraY, cameraZ);
                Vector3D rightV = Vector3D.crossPorduct(upV, forwardV).normalize();

                offsetX -= 3 * delta * rightV.X;
                offsetY -= 3 * delta * rightV.Y;
                //offsetZ -= 3 * delta; ;
            }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad6))
            {
                Vector3D upV = new Vector3D(0, 0, 1);
                Vector3D forwardV = new Vector3D(cameraX, cameraY, cameraZ);
                Vector3D rightV = Vector3D.crossPorduct(upV, forwardV).normalize();

                offsetX += 3 * delta * rightV.X;
                offsetY += 3 * delta * rightV.Y;
                //offsetZ -= 3 * delta; ;
            }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad5))
            {

                Vector3D forwardV = new Vector3D(cameraX, cameraY, cameraZ).normalize();


                offsetX -= 3 * delta * forwardV.X;
                offsetY -= 3 * delta * forwardV.Y;
                offsetZ -= 3 * delta * forwardV.Z;
                //offsetZ -= 3 * delta; ;
            }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.NumPad0))
            {
                Vector3D forwardV = new Vector3D(cameraX, cameraY, cameraZ).normalize();

                offsetX += 3 * delta * forwardV.X;
                offsetY += 3 * delta * forwardV.Y;
                offsetZ += 3 * delta * forwardV.Z;
                //offsetZ -= 3 * delta; ;
            }

            //new Vector3(cameraX + offsetX, cameraY + offsetY, cameraZ + offsetZ)

            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F) && !prevState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F))
            {
                showCircle = !showCircle;
                basicShowVertex();
                //Program.updateVertices();
            }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1)){ t.rotOrder = RotationOrder.XYZ; foreach (var vt in tList) { vt.rotOrder = RotationOrder.XYZ; }  basicShowVertex(); }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2)) { t.rotOrder = RotationOrder.XZY; foreach (var vt in tList) { vt.rotOrder = RotationOrder.XZY; } basicShowVertex(); }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3)) { t.rotOrder = RotationOrder.YXZ; foreach (var vt in tList) { vt.rotOrder = RotationOrder.YXZ; } basicShowVertex(); }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4)) { t.rotOrder = RotationOrder.YZX; foreach (var vt in tList) { vt.rotOrder = RotationOrder.YZX; } basicShowVertex(); }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D5)) { t.rotOrder = RotationOrder.ZXY; foreach (var vt in tList) { vt.rotOrder = RotationOrder.ZXY; } basicShowVertex(); }
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D6)) { t.rotOrder = RotationOrder.ZYX; foreach (var vt in tList) { vt.rotOrder = RotationOrder.ZYX; } basicShowVertex(); }
            //mouseX = Mouse.GetState().Position.X;
            //mouseY = Mouse.GetState().Position.Y;
            // TODO: Add your update logic here

            prevState = state;
            prevMState = mState;
            base.Update(gameTime);
        }



        public void drawText()
        {
            spriteBatch.DrawString(font, "T Pos:" + t.position.X + "," + t.position.Y + "," + t.position.Z, new Vector2(30, 30), Color.White);
            spriteBatch.DrawString(font, "T Rot:" + t.rotation.X + "," + t.rotation.Y + "," + t.rotation.Z, new Vector2(30, 60), Color.White);
            spriteBatch.DrawString(font, "T Rot order:" + t.rotOrder, new Vector2(30, 90), Color.White);
            spriteBatch.DrawString(font, "Right click + ASD : translate \n Right click + ZXC : Rotate \n Num 1-6 rotation order change \n F : toggle rotation circle\n B : reset all value", new Vector2(30, 150), Color.White);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            // Rectangle screenRectangle = new Rectangle((int)mouseX, (int)mouseY, 50, 50);
            //  spriteBatch.Draw(test, screenRectangle, Color.White);
            DrawGround();
            drawText();
            //effect.EnableDefaultLighting();
            /*var vertices = new VertexPositionColor[4];
             vertices[0].Position = new Vector3(100, 100, 0);
             vertices[0].Color = Color.Black;
             vertices[1].Position = new Vector3(200, 100, 0);
             vertices[1].Color = Color.Red;
             vertices[2].Position = new Vector3(200, 200, 0);
             vertices[2].Color = Color.Black;
             vertices[3].Position = new Vector3(100, 200, 0);
             vertices[3].Color = Color.Red;
             */
            /*if (renderMode == RenderMode.Triangle)
            {
                effect.LightingEnabled = true;
                effect.VertexColorEnabled = false;

            }
            else
            {
                effect.LightingEnabled = false;
                effect.VertexColorEnabled = true;
                
            }*/
           
            if (vertices.Length > 0 || triVertices.Length > 0)
            {
                if (renderMode == RenderMode.Line && vertices.Length > 0)
                {
                    GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Length / 2);

                }
                else if (renderMode == RenderMode.Triangle && triVertices.Length > 0)
                {

                    graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triVertices, 0, triVertices.Length / 3);
                }
                else 
                {


                    if (vertices.Length > 0) { GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, vertices.Length / 2); }
                    if (triVertices.Length > 0) { graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triVertices, 0, triVertices.Length / 3); }
                    
                   
                }
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }



        void DrawGround()
        {
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            // var cameraPosition = new Vector3(0 + mouseX, 40 + mouseY, 20);
            var cameraPosition = new Vector3(cameraX + offsetX, cameraY + offsetY, cameraZ + offsetZ);
            var cameraLookAtVector = new Vector3(centerX + offsetX, centerY + offsetY, centerZ + offsetZ);
            var cameraUpVector = Vector3.UnitZ;

            DepthStencilState depthBufferState = new DepthStencilState();
            depthBufferState.DepthBufferEnable = true;
            depthBufferState.DepthBufferFunction = CompareFunction.LessEqual;
            GraphicsDevice.DepthStencilState = depthBufferState;



            effect.View = Matrix.CreateLookAt(
                cameraPosition, cameraLookAtVector, cameraUpVector);
            effect.VertexColorEnabled = true;
            float aspectRatio =
                graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 0.1f;
            float farClipPlane = 200;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);


            /* foreach (var pass in effect.CurrentTechnique.Passes)
             {
                 pass.Apply();

                 graphics.GraphicsDevice.DrawUserPrimitives(
                     // We’ll be rendering two trinalges
                     PrimitiveType.TriangleList,
                     // The array of verts that we want to render
                     floorVerts,
                     // The offset, which is 0 since we want to start 
                     // at the beginning of the floorVerts array
                     0,
                     // The number of triangles to draw
                     2);
             }*/


            VertexPositionColor[] lines;
            lines = new VertexPositionColor[6];

            lines[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);

            lines[1] = new VertexPositionColor(new Vector3(10, 0, 0), Color.Red);

            lines[2] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Blue);

            lines[3] = new VertexPositionColor(new Vector3(0, 10, 0), Color.Blue);

            lines[4] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Yellow);

            lines[5] = new VertexPositionColor(new Vector3(0, 0, 10), Color.Yellow);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)

            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, lines, 0, 3);

            }


        }

        public static VertexPositionColor basicVertex(Vector3D v, Color c)
        {
            return new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(v.X, v.Y, v.Z),c);
        }

        public static VertexPositionColor basicVertexXZY(Vector3D v, Color c)
        {
            return new VertexPositionColor(new Microsoft.Xna.Framework.Vector3(v.X, v.Z, v.Y), c);
        }

        public static void drawRing(List<VertexPositionColor> ans, Vector3D[] vlist, Color c)
        {
            for (int i =0; i < vlist.Length - 1;i++)
            {
                ans.Add(basicVertexXZY(vlist[i], c));
                ans.Add(basicVertexXZY(vlist[i+1], c));


            }
            ans.Add(basicVertexXZY(vlist[vlist.Length - 1], c));
            ans.Add(basicVertexXZY(vlist[0], c));

        }


        
        
    }
}
