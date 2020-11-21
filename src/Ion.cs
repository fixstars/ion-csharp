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
        TypeCode code;
        Byte bits;
        Byte lanes;

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
        string output_directory;
    }

    private static class NativeMethods
    {
        [DllImport("ion-core.dll")]
        internal static extern Int32 ion_port_create(IntPtr ptr, String key, Type type);

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
        internal extern static Int32 ion_node_set_port(IntPtr obj, IntPtr ports_ptr, Int32);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_node_set_param(IntPtr obj, IntPtr params_ptr, Int32);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_create(IntPtr ptr);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_destroy(IntPtr obj);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_set_target(IntPtr obj, String target);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_with_bb_module(IntPtr obj, String module_name);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_add_node(IntPtr obj, String key, IntPtr node_ptr)

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_builder_compile(IntPtr obj, String function_name, IntPtr option);

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
        internal extern static Int32 ion_port_map_set_i8(IntPtr obj, ion_port_t, SByte);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_i16(IntPtr obj, IntPtr p, Int16);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_i32(IntPtr obj, IntPtr p, Int32);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_i64(IntPtr obj, IntPtr p, Int64);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u1(IntPtr obj, IntPtr p, bool);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u8(IntPtr obj, IntPtr p, Byte);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u16(IntPtr obj, IntPtr p, UInt16);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u32(IntPtr obj, IntPtr p, UInt32);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_u64(IntPtr obj, IntPtr p, UInt64);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_f32(IntPtr obj, IntPtr p, float);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_f64(IntPtr obj, IntPtr p, double);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_buffer(IntPtr obj, IntPtr p, ion_buffer_t);

        [DllImport("ion-core.dll")]
        internal extern static Int32 ion_port_map_set_buffer_array(IntPtr obj, IntPtr p, ion_buffer_t *, int);

    }

    public class Port
    {
        private IntPtr obj;

        public Port(IntPtr obj_)
        {
            obj = obj_
        }

        public Port(String key, Type type)
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_port_create(ptr, key, type);
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
        private IntPtr obj;

        public Param(String key, String val)
        {
            IntPtr ptr = Marshal.AllocHGlobal(IntPtr.Size);
            Int32 ret = NativeMethods.ion_param_create(ptr, key, type);
            if (ret != 0) {
                throw new InvalidOperationException();
            }
            obj = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);
        }

        ~Param()
        {
            if (obj != InPtr.Zero) {
                NativeMethods.ion_param_destroy(obj);
            }
        }
    }

    public class Node
    {
        private IntPtr obj;

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
            if (obj != InPtr.Zero) {
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
                Port port(Marshal.ReadIntPtr(ptr));
                Marshal.FreeHGlobal(ptr);
                return port;
            }
        }
    }
}
