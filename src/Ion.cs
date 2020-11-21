using System;
using System.Runtime.InteropServices;

public class Ion
{
    public enum TypeCode
    {
        Int = 0,   //!< signed integers
        Uint = 1,  //!< unsigned integers
        Float = 2, //!< floating point numbers
        Handle = 3 //!< opaque pointer type (void *)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Type
    {
        public TypeCode code;
        public Byte bits;
        public Byte lanes;

        public Type(TypeCode code_, Byte bits_, Byte lanes_)
        {
            code = code_;
            bits = bits_;
            lanes = lanes_;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BuilderCompileOption
    {
        public string output_directory;
    }

    private static class NativeMethods
    {
        [DllImport("ion-core.dll")]
        internal static extern Int32 ion_port_create(IntPtr ptr, String key, Type type, Int32 dim);

        [DllImport("ion-core.dll")]
        internal static extern Int32 ion_port_destroy(IntPtr obj);

        [DllImport("ion-core.dll")]
        internal static extern Int32 ion_param_create(IntPtr ptr, String key, String val);

        [DllImport("ion-core.dll")]
        internal static extern Int32 ion_param_destroy(IntPtr ptr);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_node_create(IntPtr ptr);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_node_destroy(IntPtr obj);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_node_get_port(IntPtr obj, String key, IntPtr port_ptr);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_node_set_port(IntPtr obj, IntPtr ports_ptr, Int32 ports_num);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_node_set_param(IntPtr obj, IntPtr params_ptr, Int32 params_num);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_create(IntPtr ptr);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_destroy(IntPtr obj);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_set_target(IntPtr obj, String target);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_with_bb_module(IntPtr obj, String module_name);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_add_node(IntPtr obj, String key, IntPtr node_ptr);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_compile(IntPtr obj, String function_name, BuilderCompileOption option);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_save(IntPtr obj, String file_name);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_load(IntPtr obj, String file_name);

        // [DllImport("ion-core.dll")]
        // internal extern static Int32 ion_builder_bb_metadata(IntPtr obj, IntPtr ptr, Int32 n, IntPtr ret_n);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_run(IntPtr obj, IntPtr pm);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_buffer_create(IntPtr ptr, Type type, IntPtr sizes, Int32 dim);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_buffer_destroy(IntPtr obj);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_buffer_write(IntPtr obj, IntPtr ptr, Int32 size);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_buffer_read(IntPtr obj, IntPtr ptr, Int32 size);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_create(IntPtr ptr);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_destroy(IntPtr obj);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_i8(IntPtr obj, IntPtr p, SByte v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_i16(IntPtr obj, IntPtr p, Int16 v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_i32(IntPtr obj, IntPtr p, Int32 v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_i64(IntPtr obj, IntPtr p, Int64 v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u1(IntPtr obj, IntPtr p, bool v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u8(IntPtr obj, IntPtr p, Byte v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u16(IntPtr obj, IntPtr p, UInt16 v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u32(IntPtr obj, IntPtr p, UInt32 v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u64(IntPtr obj, IntPtr p, UInt64 v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_f32(IntPtr obj, IntPtr p, float v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_f64(IntPtr obj, IntPtr p, double v);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_buffer(IntPtr obj, IntPtr p, IntPtr buffer);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_buffer_array(IntPtr obj, IntPtr p, IntPtr buffers, Int32 buffers_num);

    }

    public class Port
    {
        public IntPtr obj;

        public Port(IntPtr obj_)
        {
            obj = obj_;
        }

        public Port(String key, Type type, Int32 dim)
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_port_create(ptr, key, type, dim);
            if (ret != 0) {
                throw new InvalidOperationException();
            }
            obj = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);
        }

        ~Port()
        {
            if (obj !=IntPtr.Zero)
            {
                NativeMethods.ion_port_destroy(obj);
            }
        }
    }

    public class Param
    {
        public IntPtr obj;

        public Param(String key, String val)
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_param_create(ptr, key, val);
            if (ret != 0) {
                throw new InvalidOperationException();
            }
            obj = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);
        }

        ~Param()
        {
            if (obj != IntPtr.Zero) {
                NativeMethods.ion_param_destroy(obj);
            }
        }
    }

    public class Node
    {
        public IntPtr obj;

        public Node(IntPtr obj_)
        {
            obj = obj_;
        }

        public Node()
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_node_create(ptr);
            if (ret != 0) {
                throw new InvalidOperationException();
            }
            obj = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);
        }

        ~Node()
        {
            if (obj != IntPtr.Zero) {
                NativeMethods.ion_node_destroy(obj);
            }
        }

        public Port this[String key] {
            // set {}
            get {
                IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
                Int32 ret = NativeMethods.ion_node_get_port(obj, key, ptr);
                if (ret != 0) {
                    throw new InvalidOperationException();
                }
                var port = new Port(Marshal.ReadIntPtr(ptr));
                Marshal.FreeHGlobal(ptr);
                return port;
            }
        }

        public Node SetPort(params Port[] ports)
        {
            IntPtr ptr = Marshal.AllocHGlobal(ports.Length * IntPtr.Size);
            for (int i=0; i<ports.Length; ++i)
            {
                Marshal.WriteIntPtr(ptr + i * IntPtr.Size, ports[i].obj);
            }
            Int32 ret = NativeMethods.ion_node_set_port(obj, ptr, ports.Length);
            if (ret != 0)
            {
                throw new InvalidCastException();
            }
            Marshal.FreeHGlobal(ptr);
            return this;
        }
        public Node SetParam(params Param[] ps)
        {
            IntPtr ptr = Marshal.AllocHGlobal(ps.Length * IntPtr.Size);
            for (int i = 0; i < ps.Length; ++i)
            {
                Marshal.WriteIntPtr(ptr + i * IntPtr.Size, ps[i].obj);
            }
            Int32 ret = NativeMethods.ion_node_set_param(obj, ptr, ps.Length);
            if (ret != 0)
            {
                throw new InvalidCastException();
            }
            Marshal.FreeHGlobal(ptr);
            return this;
        }
    }

    public class Builder
    {
        public IntPtr obj;

        public Builder()
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_builder_create(ptr);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
            obj = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);
        }

        ~Builder()
        {
            if (obj != IntPtr.Zero)
            {
                NativeMethods.ion_builder_destroy(obj);
            }
        }

        public Builder SetTarget(String target)
        {
            Int32 ret = NativeMethods.ion_builder_set_target(obj, target);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
            return this;
        }

        public Builder WithBBModule(String path)
        {
            Int32 ret = NativeMethods.ion_builder_with_bb_module(obj, path);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
            return this;
        }

        public Node Add(String key)
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_builder_add_node(obj, key, ptr);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
            var node = new Node(Marshal.ReadIntPtr(ptr));
            Marshal.FreeHGlobal(ptr);
            return node;
        }

        public void Compile(String func_name, BuilderCompileOption option)
        {
            Int32 ret = NativeMethods.ion_builder_compile(obj, func_name, option);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
        }
        public void Save(String file_name)
        {
            Int32 ret = NativeMethods.ion_builder_save(obj, file_name);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
        }
        public void Load(String file_name)
        {
            Int32 ret = NativeMethods.ion_builder_load(obj, file_name);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Run(PortMap pm)
        {
            Int32 ret = NativeMethods.ion_builder_run(obj, pm.obj);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
    public class Buffer
    {
        public IntPtr obj;

        public Buffer(Type type, params int[] sizes) {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            IntPtr sizes_ptr = Marshal.AllocHGlobal(sizeof(Int32) * sizes.Length);
            for (int i=0; i<sizes.Length; ++i)
            {
                Marshal.WriteInt32(sizes_ptr + i * sizeof(Int32), sizes[i]);
            }
            Int32 ret = NativeMethods.ion_buffer_create(ptr, type, sizes_ptr, sizes.Length);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }

            obj = Marshal.ReadIntPtr(ptr);

            Marshal.FreeHGlobal(sizes_ptr);
            Marshal.FreeHGlobal(ptr);
        }

        ~Buffer()
        {
            if (obj != IntPtr.Zero)
            {
                NativeMethods.ion_builder_destroy(obj);
            }
        }

        public void Write(byte[] data)
        {
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            Int32 ret = NativeMethods.ion_buffer_write(obj, ptr, data.Length);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
            Marshal.FreeHGlobal(ptr);
        }

        public void Read(byte[] data)
        {
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);          
            Int32 ret = NativeMethods.ion_buffer_read(obj, ptr, data.Length);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
            Marshal.Copy(ptr, data, 0, data.Length);
            Marshal.FreeHGlobal(ptr);
        }
    }

    public class PortMap
    {
        public IntPtr obj;

        public PortMap()
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_port_map_create(ptr);
            if (ret != 0)
            {
                throw new InvalidOperationException();
            }
            obj = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);  
        }

        ~PortMap()
        {
            if (obj != IntPtr.Zero) {            
                Int32 ret = NativeMethods.ion_port_map_destroy(obj);      
            }
        }

        public void Set(Port p, SByte v)
        {
            if (NativeMethods.ion_port_map_set_i8(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, Int16 v)
        {
            if (NativeMethods.ion_port_map_set_i16(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, Int32 v)
        {
            if (NativeMethods.ion_port_map_set_i32(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, Int64 v)
        {
            if (NativeMethods.ion_port_map_set_i64(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, Byte v)
        {
            if (NativeMethods.ion_port_map_set_u8(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, UInt16 v)
        {
            if (NativeMethods.ion_port_map_set_u16(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, UInt32 v)
        {
            if (NativeMethods.ion_port_map_set_u32(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, UInt64 v)
        {
            if (NativeMethods.ion_port_map_set_u64(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, float v)
        {
            if (NativeMethods.ion_port_map_set_f32(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, double v)
        {
            if (NativeMethods.ion_port_map_set_f64(obj, p.obj, v) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, Buffer buffer)
        {
            if (NativeMethods.ion_port_map_set_buffer(obj, p.obj, buffer.obj) != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void Set(Port p, Buffer[] buffers)
        {
            IntPtr ptr = Marshal.AllocHGlobal(buffers.Length * IntPtr.Size);
            for (int i = 0; i < buffers.Length; ++i)
            {
                Marshal.WriteIntPtr(ptr + i * IntPtr.Size, buffers[i].obj);
            }
            if (NativeMethods.ion_port_map_set_buffer_array(obj, p.obj, ptr, buffers.Length) != 0)
            {
                throw new InvalidOperationException();
            }
            Marshal.FreeHGlobal(ptr);
        }
    }
}
