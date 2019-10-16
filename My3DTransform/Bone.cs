using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace My3DTransform
{
    class Bone
    {
        public string Name;
        //
        // 摘要:
        //     Index of the parent in this FLVER's bone collection, or -1 for none.
        public short ParentIndex;
        //
        // 摘要:
        //     Index of the first child in this FLVER's bone collection, or -1 for none.
        public short ChildIndex;
        //
        // 摘要:
        //     Index of the next child of this bone's parent, or -1 for none.
        public short NextSiblingIndex;
        //
        // 摘要:
        //     Index of the previous child of this bone's parent, or -1 for none.
        public short PreviousSiblingIndex;
        //
        // 摘要:
        //     Translation of this bone.
        public Vector3 Translation;
        //
        // 摘要:
        //     Rotation of this bone; euler radians.
        public Vector3 Rotation;
        //
        // 摘要:
        //     Scale of this bone.
        public Vector3 Scale;
        //
        // 摘要:
        //     Minimum extent of the vertices weighted to this bone.
        public Vector3 BoundingBoxMin;
        //
        // 摘要:
        //     Maximum extent of the vertices weighted to this bone.
        public Vector3 BoundingBoxMax;
        //
        // 摘要:
        //     Unknown; only 0 or 1 before Sekiro.
        public int Unk3C;
    }
}
