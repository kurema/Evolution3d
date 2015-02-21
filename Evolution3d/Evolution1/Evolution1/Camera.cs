using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Evolution1
{
    public class Camera
    {

		public float Radius;
		public float CloseUpSpeed;

		public Vector3 LookAtPosition;
		public Vector3 CamRot;
		public Vector3 UpVector;

		public Matrix Projection;

		public Matrix View
		{
			get
			{
				return Matrix.CreateLookAt(
					this.CameraPosition,
					this.LookAtPosition,
					this.UpVector
				);
			}
		}

		public Vector3 CameraPosition { get; set; }

		public Camera(
			Vector3 camPosition,
			Vector3 lookAt,
			Vector3 upVector,
			Matrix projection,
			float closeUpSpeed
		)
		{
			this.CloseUpSpeed = closeUpSpeed;

			this.LookAtPosition = lookAt;
			this.CameraPosition = camPosition;
			this.UpVector = upVector;

			Vector3 Direction = lookAt - camPosition;
			this.Radius = Direction.Length();
			this.CamRot = ForceToAxis(Direction);

			this.Projection = projection;
		}

        public Creature Target;
        public bool TargetMode = false;
        public Vector3 TargetRot = new Vector3(0.4f, 0.4f, 0);
        public float TargetRotSpeed = 0.02f;


		/// <summary>
		/// <para> 方向ベクトルからXY軸角を生成 </para>
		/// <para> 標準化不要 </para>
		/// </summary>
		/// <param name="normal">方向ベクトル</param>
		/// <returns>ラジアン値のXY軸ベクトル</returns>
        public static Vector3 ForceToAxis(Vector3 vector)
		{
			float sqrt = (float)Math.Sqrt(vector.Z * vector.Z + vector.X * vector.X);

			float rotY = -(float)Math.Atan2(vector.Z, vector.X);
			float rotX = -(float)Math.Atan2(vector.Y, sqrt);

			return new Vector3(rotX, rotY, 0.0f);
		}


		/// <summary>
		/// <para> XY軸角から方向ベクトルを生成 </para>
		/// <para> X=90°:(0,1,0) </para>
		/// <para> Y=0° :(1,0,0) </para>
		/// </summary>
		public static Vector3 AxisToForce(float radX, float radY)
		{
			float cosY = (float)Math.Cos((double)radY);
			float cosX = (float)Math.Cos((double)radX);
			float sinY = (float)Math.Sin((double)radY);
			float sinX = (float)Math.Sin((double)radX);

			return new Vector3(cosY * cosX, sinX, sinY * cosX);
		}
	}
}
